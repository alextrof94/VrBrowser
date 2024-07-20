using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Valve.VR;
using CefSharp;
using CefSharp.OffScreen;
using System.Diagnostics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using VrBrowserTestCore.Properties;
using System.Windows.Forms;
using System.Linq;
using Newtonsoft.Json;

namespace VrBrowserTestCore
{
    public partial class Form1 : Form
    {
        private AppSettings ThisAppSettings = new();

        // Overlay
        CVRSystem? VrSystem = null;
        bool VrStarted = false;
        uint HmdId;
        ulong OverlayHandle = 0;
        GameWindow? PreviewGameWindow = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (File.Exists("AppSettings.json"))
            {
                string data = File.ReadAllText("AppSettings.json");
                ThisAppSettings = JsonConvert.DeserializeObject<AppSettings>(data);
            }
            else
            {
                Tab tab = new()
                {
                    Tag = new Guid()
                };
                ThisAppSettings.Tabs.Add(tab);
            }

            NuCurvature.Value = (decimal)ThisAppSettings.Curvature;
            NuSize.Value = (decimal)ThisAppSettings.Size;
            NuOffset.Value = (decimal)ThisAppSettings.Offset;
            CoColorFormat.SelectedIndex = ThisAppSettings.ColorFormat;

            LoadPages();
            TryToStartMainProcess();
        }

        private void TiStartVr_Tick(object sender, EventArgs e)
        {
            TryToStartMainProcess();
        }

        private void TryToStartMainProcess()
        {
            if (!InitVr(out OverlayHandle))
                return;
            InitGL();
            TiStartVr.Stop();

            var settings = new CefSettings()
            {
                CefCommandLineArgs = {
                    ["enable-media-stream"] = "1",
                    ["autoplay-policy"] = "no-user-gesture-required",
                    ["enable-audio"] = "1",
                },
                WindowlessRenderingEnabled = true,
            };
            settings.CefCommandLineArgs.Remove("mute-audio");
            Cef.Initialize(settings);

            foreach (var tab in ThisAppSettings.Tabs)
            {
                if (!string.IsNullOrEmpty(tab.Url))
                {
                    tab.Browser = new ChromiumWebBrowser(tab.Url)
                    {
                        Size = new Size(1920, 1080)
                    };

                    tab.Browser.FrameLoadEnd += Browser_FrameLoadEnd;
                }
            }

            Task t = new(() =>
            {
                while (true)
                {
                    UpdateOverlayWithBrowserContent();
                }
            });
            t.Start();
        }

        private void Browser_FrameLoadEnd(object? sender, FrameLoadEndEventArgs e)
        {
            foreach (var tab in ThisAppSettings.Tabs)
            {
                if (tab.Browser == sender)
                {
                    tab.UrlLoaded = true;
                    var browserHost = tab.Browser.GetBrowserHost();
                    browserHost?.SetAudioMuted(!tab.AudioEnabled);
                    if (!string.IsNullOrEmpty(tab.Css))
                    {
                        string script = $@"
                            (function() {{
                                var style = document.createElement('style');
                                style.type = 'text/css';
                                style.innerHTML = `{tab.Css}`;
                                document.head.appendChild(style);
                            }})();
                        ";
                        tab.Browser.ExecuteScriptAsync(script);
                    }
                    break;
                }
            }
        }

        private bool AssociateOverlayToDevice(ulong overlayHandle, uint deviceIndex)
        {
            HmdMatrix34_t transform = new()
            {
                m0 = -1,
                m1 = 0,
                m2 = 0,
                m3 = 0,
                m4 = 0,
                m5 = -1,
                m6 = 0,
                m7 = 0,
                m8 = 0,
                m9 = 0,
                m10 = 1,
                m11 = -ThisAppSettings.Offset
            };

            OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(overlayHandle, deviceIndex, ref transform);
            return true;
        }

        private int LoadTextureFromBitmap(Bitmap bitmap)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            // Load the image using System.Drawing
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            OpenTK.Graphics.OpenGL4.PixelFormat pixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
            switch (ThisAppSettings.ColorFormat)
            {
                case 1: pixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Rgba; break;
            }

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                data.Width,
                data.Height,
                0,
                pixelFormat,
                PixelType.UnsignedByte,
                data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            // Set texture parameters (optional)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            return textureId;
        }

        private void ShowFrameFileFromBitmap(Bitmap bitmap)
        {
            int textureId = LoadTextureFromBitmap(bitmap);

            Texture_t texture = new()
            {
                eColorSpace = EColorSpace.Auto,
                eType = ETextureType.OpenGL,
                handle = (IntPtr)textureId
            };
            OpenVR.Overlay.SetOverlayTexture(OverlayHandle, ref texture);


            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.DeleteTextures(1, ref textureId);
        }

        private async Task UpdateOverlayWithBrowserContent()
        {
            if (!VrStarted) return;

            Bitmap bitmap = new(1920, 1080);
            foreach (var tab in ThisAppSettings.Tabs)
            {
                if (tab.Browser == null || tab.Browser.IsDisposed) continue;
                if (!tab.UrlLoaded) continue;
                var screenshot = await tab.Browser.ScreenshotAsync();
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenshot, 0, 0, screenshot.Width, screenshot.Height);
                }
                screenshot.Dispose();
            }

            try
            {
                this.Invoke(new Action(() =>
                {
                    ShowFrameFileFromBitmap(bitmap);
                }));
            }
            catch { }
            bitmap.Dispose();

            await Task.Delay(16);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VrStarted)
            {
                VrStarted = false;
                Thread.Sleep(1000);
                OpenVR.Overlay.HideOverlay(OverlayHandle);
                OpenVR.Overlay.DestroyOverlay(OverlayHandle);
                OpenVR.Shutdown();
            }
            Cef.Shutdown();
            if (PreviewGameWindow != null)
            {
                PreviewGameWindow.Close();
                PreviewGameWindow.Dispose();
            }
        }



        static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        bool InitVr(out ulong overlayHandle)
        {
            overlayHandle = 0;
            try
            {
                if (!IsProcessRunning("vrserver") && !IsProcessRunning("vrcompositor"))
                    throw new Exception("");

                var systemErr = EVRInitError.None;
                VrSystem = OpenVR.Init(ref systemErr, EVRApplicationType.VRApplication_Overlay);
                if (systemErr != EVRInitError.None)
                    throw new Exception($"OpenVR.Init " + systemErr.ToString());

                uint[] indexes = new uint[1];
                VrSystem.GetSortedTrackedDeviceIndicesOfClass(ETrackedDeviceClass.HMD, indexes, 0);
                HmdId = indexes[0];

                // Init VR
                var overlayErr = EVROverlayError.None;
                overlayErr = OpenVR.Overlay.CreateOverlay("browser", "browser", ref overlayHandle);
                if (overlayErr != EVROverlayError.None)
                {
                    if (overlayErr == EVROverlayError.KeyInUse)
                    {
                        OpenVR.Overlay.FindOverlay("browser", ref overlayHandle);
                        OpenVR.Overlay.DestroyOverlay(overlayHandle);
                    }
                    throw new Exception($"CreateOverlay " + overlayErr.ToString());
                }
                OverlayHandle = overlayHandle;
                overlayErr = OpenVR.Overlay.SetOverlayCurvature(overlayHandle, ThisAppSettings.Curvature);
                if (overlayErr != EVROverlayError.None)
                    throw new Exception($"SetOverlayCurvature " + overlayErr.ToString());

                overlayErr = OpenVR.Overlay.SetOverlayWidthInMeters(overlayHandle, ThisAppSettings.Size);
                if (overlayErr != EVROverlayError.None)
                    throw new Exception($"SetOverlayWidthInMeters " + overlayErr.ToString());

                OpenVR.Overlay.SetOverlayColor(overlayHandle, 1, 1, 1);
                OpenVR.Overlay.SetOverlayAlpha(overlayHandle, 1);

                OpenVR.Overlay.ShowOverlay(overlayHandle);
                VRTextureBounds_t bounds;
                bounds.uMin = 0;
                bounds.uMax = 1;
                bounds.vMin = 0;
                bounds.vMax = 1;
                OpenVR.Overlay.SetOverlayTextureBounds(overlayHandle, ref bounds);

                AssociateOverlayToDevice(overlayHandle, HmdId);
                VrStarted = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VR is not initialized. " + ex.Message);
                return false;
            }
        }

        void InitGL()
        {
            var location = new OpenTK.Mathematics.Vector2i(0, -10000);

            GameWindowSettings gwSettings = new() { };
            NativeWindowSettings nwSettings = new()
            {
                Size = new OpenTK.Mathematics.Vector2i(1, 1),
                Title = "VR Browser Preview",
                WindowBorder = OpenTK.Windowing.Common.WindowBorder.Hidden,
                Location = location,
            };
            PreviewGameWindow = new GameWindow(gwSettings, nwSettings) { IsVisible = false };

            GL.ClearColor(new Color4(0.1f, 0, 0, 0.5f));
        }


        private void NuSize_ValueChanged(object sender, EventArgs e)
        {
            ThisAppSettings.Size = (float)NuSize.Value;
        }

        private void NuOffset_ValueChanged(object sender, EventArgs e)
        {
            ThisAppSettings.Offset = (float)NuOffset.Value;
        }

        private void NuCurvature_ValueChanged(object sender, EventArgs e)
        {
            ThisAppSettings.Curvature = (float)NuCurvature.Value;
        }

        private void CoColorFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThisAppSettings.ColorFormat = CoColorFormat.SelectedIndex;
        }






        private void LoadPages()
        {
            foreach (var tab in ThisAppSettings.Tabs)
            {
                if (tab.Tag != new Guid())
                {

                    tabControl1.TabPages.Add("New");
                    var tabPage = tabControl1.TabPages[^1];

                    tabPage.Tag = tab.Tag;
                    tabPage.Name = "TaBrowser" + tabPage.Tag.ToString();
                    tabPage.BackColor = Color.White;

                    foreach (Control c in TaBrowser.Controls)
                    {
                        var clonedControl = CloneControl(c, (Guid)tabPage.Tag);
                        tabPage.Controls.Add(clonedControl);
                    }

                }

                var tabPageForNaming = (FindControlByName(this, nameof(TaBrowser), tab.Tag) as TabPage);
                if (tab.Url.Contains("//"))
                {
                    string name = tab.Url[(tab.Url.IndexOf("//") + 2)..];
                    if (name.Contains('/'))
                    {
                        name = name[..name.IndexOf("/")];
                    }
                    tabPageForNaming.Text = name;
                }

                (FindControlByName(this, nameof(TeUrl), tab.Tag) as TextBox).Text = tab.Url;
                (FindControlByName(this, nameof(RiCss), tab.Tag) as RichTextBox).Text = tab.Css;
                (FindControlByName(this, nameof(ChAudioEnabled), tab.Tag) as CheckBox).Checked = tab.AudioEnabled;
            }
        }

        private void SaveAppSettings()
        {
            string str = JsonConvert.SerializeObject(ThisAppSettings);
            File.WriteAllText("AppSettings.json", str);
        }

        private Tab? GetTabByGuid(Guid tag)
        {
            foreach (var tab in ThisAppSettings.Tabs)
            {
                if (tab.Tag.ToString() == tag.ToString())
                    return tab;
            }
            return null;
        }

        private static Control? FindControlByName(Control parent, string name, Guid tag)
        {
            if (parent.Name.StartsWith(name) && (tag == new Guid() || parent.Name.EndsWith(tag.ToString())))
            {
                return parent;
            }

            foreach (Control control in parent.Controls)
            {
                var foundControl = FindControlByName(control, name, tag);
                if (foundControl != null)
                {
                    return foundControl;
                }
            }

            return null;
        }

        private void BuTabAdd_Click(object sender, EventArgs e)
        {
            Tab newTab = new();
            ThisAppSettings.Tabs.Add(newTab);

            tabControl1.TabPages.Add("New");
            var page = tabControl1.TabPages[^1];

            page.Tag = newTab.Tag;
            page.Name = "TaBrowser" + page.Tag.ToString();
            page.BackColor = Color.White;

            foreach (Control c in TaBrowser.Controls)
            {
                var clonedControl = CloneControl(c, (Guid)page.Tag);
                page.Controls.Add(clonedControl);
            }
        }

        private Control CloneControl(Control original, Guid tag)
        {
            var type = original.GetType();
            var clonedControl = Activator.CreateInstance(type) as Control;

            clonedControl.Size = original.Size;
            clonedControl.Location = original.Location;
            clonedControl.Text = original.Text;
            clonedControl.Tag = tag;
            clonedControl.Name = original.Name + tag.ToString();
            clonedControl.BackColor = original.BackColor;
            clonedControl.ForeColor = original.ForeColor;

            if (clonedControl is TextBox clonedTextBox)
            {
                clonedTextBox.Text = "";
                if (clonedTextBox.Name.StartsWith("TeUrl"))
                {
                    clonedTextBox.TextChanged += TextBoxUrl_TextChanged;
                }
            }

            if (clonedControl is RichTextBox clonedTichTextBox)
            {
                clonedTichTextBox.Text = "";
                if (clonedTichTextBox.Name.StartsWith("RiCss"))
                {
                    clonedTichTextBox.TextChanged += RichTextBoxCss_TextChanged;
                }
            }

            if (clonedControl is CheckBox clonedCheckBox)
            {
                if (clonedCheckBox.Name.StartsWith("ChAudioEnabled"))
                {
                    clonedCheckBox.CheckedChanged += CheckBoxAudioEnabled_CheckedChanged;
                }
            }

            if (original is Button button && clonedControl is Button clonedBtn)
            {
                ((Button)clonedControl).ImageList = button.ImageList;
                ((Button)clonedControl).ImageIndex = button.ImageIndex;
                if (clonedBtn.Name.StartsWith("BuTabDelete"))
                {
                    clonedBtn.Click += BuPageDelete_Click;
                }
                if (clonedBtn.Name.StartsWith("BuReload"))
                {
                    clonedBtn.Click += BuReload_Click;
                }
            }

            if (original is GroupBox gb && clonedControl is GroupBox clonedGb)
            {
                foreach (Control c in gb.Controls)
                {
                    var cc = CloneControl(c, tag);
                    clonedGb.Controls.Add(cc);
                }
            }

            return clonedControl;
        }

        private void CheckBoxAudioEnabled_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender == null) return;
            var control = (CheckBox)sender;

            var tab = GetTabByGuid(Guid.Parse(control.Tag.ToString()));
            if (tab == null) return;

            tab.AudioEnabled = control.Checked;
            if (tab.Browser != null && !tab.Browser.IsDisposed)
            {
                tab.Browser.GetBrowserHost().SetAudioMuted(!tab.AudioEnabled);
            }
            SaveAppSettings();
        }

        private void RichTextBoxCss_TextChanged(object? sender, EventArgs e)
        {
            if (sender == null) return;
            var control = (RichTextBox)sender;

            var tab = GetTabByGuid(Guid.Parse(control.Tag.ToString()));
            if (tab == null) return;

            tab.Css = control.Text;
        }

        private void TextBoxUrl_TextChanged(object? sender, EventArgs e)
        {
            if (sender == null) return;
            var control = (TextBox)sender;

            var tab = GetTabByGuid(Guid.Parse(control.Tag.ToString()));
            if (tab == null) return;

            tab.Url = control.Text;
        }

        private void BuReload_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            var control = (Button)sender;

            var tab = GetTabByGuid(Guid.Parse(control.Tag.ToString()));
            if (tab == null) return;

            var tabPage = FindControlByName(this, "TaBrowser", tab.Tag);
            if (tabPage == null) return;

            if (tab.Url.Contains("//"))
            {
                string name = tab.Url[(tab.Url.IndexOf("//") + 2)..];
                if (name.Contains('/'))
                {
                    name = name[..name.IndexOf("/")];
                }
                tabPage.Text = name;
            }

            SaveAppSettings();

            if (!string.IsNullOrEmpty(tab.Url))
            {
                if (tab.Browser == null)
                {
                    tab.Browser = new ChromiumWebBrowser()
                    {
                        Size = new Size(1920, 1080)
                    }; 
                    tab.Browser.FrameLoadEnd += Browser_FrameLoadEnd;
                }

                tab.Browser.LoadUrl(tab.Url);
            }
        }

        private void BuPageDelete_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            var control = (Button)sender;

            if (MessageBox.Show("Do you want to delete tab?", "Deleting tab", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            var tab = GetTabByGuid((Guid)control.Tag);
            if (tab == null) return;
            tab.Browser?.Dispose();
            ThisAppSettings.Tabs.Remove(tab);

            TabPage? toDelete = null;
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Tag?.ToString() == control.Tag?.ToString())
                {
                    toDelete = tabPage;
                    break;
                }
            }

            tabControl1.TabPages.Remove(toDelete);

            SaveAppSettings();
        }

        private void BuApplyVrSettings_Click(object sender, EventArgs e)
        {
            SaveAppSettings();
            try
            {
                if (!VrStarted) return;
                OpenVR.Overlay.SetOverlayWidthInMeters(OverlayHandle, ThisAppSettings.Size);
                AssociateOverlayToDevice(OverlayHandle, HmdId);
                OpenVR.Overlay.SetOverlayCurvature(OverlayHandle, ThisAppSettings.Curvature);
            }
            catch { }
        }
    }
}