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

namespace VrBrowserTestCore
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser? browser;
        CVRSystem? VrSystem = null;
        uint HmdId;

        bool UrlLoaded = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void BuLoadUrl_Click(object sender, EventArgs e)
        {
            UrlLoaded = false;
            browser?.LoadUrl(textBox1.Text);
            Settings.Default.URL = textBox1.Text;
            Settings.Default.Save();
        }

        private void RiCss_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.CSS = RiCss.Text;
            Settings.Default.Save();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox1.Text = Settings.Default.URL;
            RiCss.Text = Settings.Default.CSS;
            NuCurvature.Value = (decimal)Settings.Default.Curvature;
            NuSize.Value = (decimal)Settings.Default.Size;
            NuOffset.Value = (decimal)Settings.Default.Offset;
            ChAudioEnable.Checked = Settings.Default.AudioEnable;
            CoColorFormat.SelectedIndex = Settings.Default.ColorFormat;
            TryToStartMainProcess();
        }

        private void TiStartVr_Tick(object sender, EventArgs e)
        {
            TryToStartMainProcess();
        }

        private void Browser_FrameLoadEnd(object? sender, FrameLoadEndEventArgs e)
        {
            UrlLoaded = true;
            InjectCSS();
        }

        private void InjectCSS()
        {
            var css = Properties.Settings.Default.CSS;
            if (!string.IsNullOrEmpty(css))
            {
                string script = $@"
                    (function() {{
                        var style = document.createElement('style');
                        style.type = 'text/css';
                        style.innerHTML = `{css}`;
                        document.head.appendChild(style);
                    }})();
                ";
                browser.ExecuteScriptAsync(script);
            }
        }

        private static bool AssociateOverlayToDevice(ulong overlayHandle, uint deviceIndex)
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
                m11 = -Settings.Default.Offset
            };

            OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(overlayHandle, deviceIndex, ref transform);
            return true;
        }

        static int LoadTextureFromBitmap(Bitmap bitmap)
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
            switch (Settings.Default.ColorFormat)
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
            if (browser == null || browser.IsDisposed) return;
            if (!UrlLoaded) return;
            var bitmap = await browser.ScreenshotAsync();
            if (bitmap != null)
            {
                try
                {
                    this.Invoke(
                        new Action(() =>
                        {
                            ShowFrameFileFromBitmap(bitmap);
                        }));
                }
                catch { }
            }
            await Task.Delay(16);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VrStarted)
            {
                VrStarted = false;
                Thread.Sleep(1000);
                // Cleanup
                OpenVR.Overlay.HideOverlay(OverlayHandle);
                OpenVR.Overlay.DestroyOverlay(OverlayHandle);
            }
            Cef.Shutdown();
            OpenVR.Shutdown();
            if (PreviewGameWindow != null)
            {
                PreviewGameWindow.Close();
                PreviewGameWindow.Dispose();
                GL.DeleteBuffer(VertexBufferObject);
                GL.DeleteVertexArray(VertexArrayObject);
            }
        }

        // Overlay
        bool VrStarted = false;
        ulong OverlayHandle = 0;
        GameWindow? PreviewGameWindow = null;

        // OpenGL
        private int VertexArrayObject;
        private int VertexBufferObject;


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
                overlayErr = OpenVR.Overlay.SetOverlayCurvature(overlayHandle, Settings.Default.Curvature);
                if (overlayErr != EVROverlayError.None)
                    throw new Exception($"SetOverlayCurvature " + overlayErr.ToString());

                overlayErr = OpenVR.Overlay.SetOverlayWidthInMeters(overlayHandle, Settings.Default.Size);
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

            NativeWindowSettings nwSettings = new()
            {
                Size = new OpenTK.Mathematics.Vector2i(1, 1),
                Title = "VR Browser Preview",
                StartVisible = false,
                WindowBorder = OpenTK.Windowing.Common.WindowBorder.Hidden,
                Location = location,
                WindowState = OpenTK.Windowing.Common.WindowState.Minimized,
                Flags = ContextFlags.Offscreen
            };
            PreviewGameWindow = new GameWindow(GameWindowSettings.Default, nwSettings) { };

            GL.ClearColor(new Color4(0.1f, 0, 0, 0.5f));

            // Create vertex array and vertex buffer objects
            GL.GenVertexArrays(1, out VertexArrayObject);
            GL.BindVertexArray(VertexArrayObject);

            GL.GenBuffers(1, out VertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Define the vertices of a quad
            float[] vertices = {
                -1.0f, -1.0f, 0.0f, 1.0f, 1.0f,
                1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
                1.0f,  1.0f, 0.0f, 0.0f, 0.0f,
                -1.0f, -1.0f, 0.0f, 1.0f, 1.0f,
                1.0f,  1.0f, 0.0f, 0.0f, 0.0f,
                -1.0f,  1.0f, 0.0f, 1.0f, 0.0f
            };

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VertexArrayObject);
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
                    ["enable-audio"] = (Settings.Default.AudioEnable) ? "1" : "0",
                },
                WindowlessRenderingEnabled = true,
            };
            if (Settings.Default.AudioEnable)
            {
                settings.CefCommandLineArgs.Remove("mute-audio");
            }
            Cef.Initialize(settings);


            if (string.IsNullOrEmpty(Settings.Default.URL))
            {
                browser = new ChromiumWebBrowser()
                {
                    Size = new Size(1920, 1080)
                };
            }
            else
            {
                browser = new ChromiumWebBrowser(Settings.Default.URL)
                {
                    Size = new Size(1920, 1080)
                };
            }

            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            Task t = new(() =>
            {
                while (true)
                {
                    UpdateOverlayWithBrowserContent();
                }
            });
            t.Start();
        }

        private void NuSize_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Size = (float)NuSize.Value;
            Settings.Default.Save();
            try
            {
                if (!VrStarted) return;
                OpenVR.Overlay.SetOverlayWidthInMeters(OverlayHandle, Settings.Default.Size);
            }
            catch { }
        }

        private void NuOffset_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Offset = (float)NuOffset.Value;
            Settings.Default.Save();
            try
            {
                if (!VrStarted) return;
                AssociateOverlayToDevice(OverlayHandle, HmdId);
            }
            catch { }
        }

        private void NuCurvature_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Curvature = (float)NuCurvature.Value;
            Settings.Default.Save();
            try
            {
                if (!VrStarted) return;
                OpenVR.Overlay.SetOverlayCurvature(OverlayHandle, Settings.Default.Curvature);
            }
            catch { }
        }

        private void ChAudioEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AudioEnable = (bool)ChAudioEnable.Checked;
            Settings.Default.Save();
        }

        private void CoColorFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.ColorFormat = CoColorFormat.SelectedIndex;
            Settings.Default.Save();
        }
    }
}