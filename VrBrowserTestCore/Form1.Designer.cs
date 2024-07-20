namespace VrBrowserTestCore
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TeUrl = new TextBox();
            BuReload = new Button();
            RiCss = new RichTextBox();
            label1 = new Label();
            label3 = new Label();
            NuCurvature = new NumericUpDown();
            NuOffset = new NumericUpDown();
            label4 = new Label();
            NuSize = new NumericUpDown();
            label5 = new Label();
            TiStartVr = new System.Windows.Forms.Timer(components);
            CoColorFormat = new ComboBox();
            tabControl1 = new TabControl();
            TaVrSettings = new TabPage();
            BuApplyVrSettings = new Button();
            label2 = new Label();
            TaAbout = new TabPage();
            RiAbout = new RichTextBox();
            TaBrowser = new TabPage();
            ChAudioEnabled = new CheckBox();
            BuTabDelete = new Button();
            ImButtons = new ImageList(components);
            GrCss = new GroupBox();
            BuTabAdd = new Button();
            ((System.ComponentModel.ISupportInitialize)NuCurvature).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NuOffset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NuSize).BeginInit();
            tabControl1.SuspendLayout();
            TaVrSettings.SuspendLayout();
            TaAbout.SuspendLayout();
            TaBrowser.SuspendLayout();
            GrCss.SuspendLayout();
            SuspendLayout();
            // 
            // TeUrl
            // 
            TeUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TeUrl.Location = new Point(71, 6);
            TeUrl.Name = "TeUrl";
            TeUrl.Size = new Size(456, 23);
            TeUrl.TabIndex = 0;
            TeUrl.Tag = "{00000000-0000-0000-0000-000000000000}";
            TeUrl.TextChanged += TextBoxUrl_TextChanged;
            // 
            // BuReload
            // 
            BuReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BuReload.Location = new Point(533, 6);
            BuReload.Name = "BuReload";
            BuReload.Size = new Size(75, 23);
            BuReload.TabIndex = 1;
            BuReload.Tag = "00000000-0000-0000-0000-000000000000";
            BuReload.Text = "Reload";
            BuReload.UseVisualStyleBackColor = true;
            BuReload.Click += BuReload_Click;
            // 
            // RiCss
            // 
            RiCss.Dock = DockStyle.Fill;
            RiCss.Location = new Point(3, 19);
            RiCss.Name = "RiCss";
            RiCss.Size = new Size(596, 343);
            RiCss.TabIndex = 2;
            RiCss.Tag = "{00000000-0000-0000-0000-000000000000}";
            RiCss.Text = ".ObsPage__widgetContainer--Ra4Ps {\ndisplay: none;\n}";
            RiCss.TextChanged += RichTextBoxCss_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 9);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 3;
            label1.Tag = "00000000-0000-0000-0000-000000000000";
            label1.Text = "URL:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 66);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 5;
            label3.Text = "Curvature";
            // 
            // NuCurvature
            // 
            NuCurvature.DecimalPlaces = 2;
            NuCurvature.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            NuCurvature.Location = new Point(83, 64);
            NuCurvature.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NuCurvature.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            NuCurvature.Name = "NuCurvature";
            NuCurvature.Size = new Size(120, 23);
            NuCurvature.TabIndex = 6;
            NuCurvature.ValueChanged += NuCurvature_ValueChanged;
            // 
            // NuOffset
            // 
            NuOffset.DecimalPlaces = 2;
            NuOffset.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            NuOffset.Location = new Point(83, 35);
            NuOffset.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            NuOffset.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
            NuOffset.Name = "NuOffset";
            NuOffset.Size = new Size(120, 23);
            NuOffset.TabIndex = 8;
            NuOffset.Value = new decimal(new int[] { 45, 0, 0, 131072 });
            NuOffset.ValueChanged += NuOffset_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 37);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 7;
            label4.Text = "Offset";
            // 
            // NuSize
            // 
            NuSize.DecimalPlaces = 2;
            NuSize.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            NuSize.Location = new Point(83, 6);
            NuSize.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            NuSize.Name = "NuSize";
            NuSize.Size = new Size(120, 23);
            NuSize.TabIndex = 10;
            NuSize.Value = new decimal(new int[] { 2, 0, 0, 0 });
            NuSize.ValueChanged += NuSize_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 8);
            label5.Name = "label5";
            label5.Size = new Size(27, 15);
            label5.TabIndex = 9;
            label5.Text = "Size";
            // 
            // TiStartVr
            // 
            TiStartVr.Enabled = true;
            TiStartVr.Interval = 3000;
            TiStartVr.Tick += TiStartVr_Tick;
            // 
            // CoColorFormat
            // 
            CoColorFormat.FormattingEnabled = true;
            CoColorFormat.Items.AddRange(new object[] { "Bgra", "Rgba" });
            CoColorFormat.Location = new Point(83, 93);
            CoColorFormat.Name = "CoColorFormat";
            CoColorFormat.Size = new Size(120, 23);
            CoColorFormat.TabIndex = 12;
            CoColorFormat.Text = "Bgra";
            CoColorFormat.SelectedIndexChanged += CoColorFormat_SelectedIndexChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(TaVrSettings);
            tabControl1.Controls.Add(TaAbout);
            tabControl1.Controls.Add(TaBrowser);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(622, 450);
            tabControl1.TabIndex = 13;
            // 
            // TaVrSettings
            // 
            TaVrSettings.Controls.Add(BuApplyVrSettings);
            TaVrSettings.Controls.Add(label2);
            TaVrSettings.Controls.Add(NuSize);
            TaVrSettings.Controls.Add(CoColorFormat);
            TaVrSettings.Controls.Add(label5);
            TaVrSettings.Controls.Add(NuOffset);
            TaVrSettings.Controls.Add(label3);
            TaVrSettings.Controls.Add(NuCurvature);
            TaVrSettings.Controls.Add(label4);
            TaVrSettings.Location = new Point(4, 24);
            TaVrSettings.Name = "TaVrSettings";
            TaVrSettings.Padding = new Padding(3);
            TaVrSettings.Size = new Size(614, 422);
            TaVrSettings.TabIndex = 1;
            TaVrSettings.Text = "VR Settings";
            TaVrSettings.UseVisualStyleBackColor = true;
            // 
            // BuApplyVrSettings
            // 
            BuApplyVrSettings.Location = new Point(6, 122);
            BuApplyVrSettings.Name = "BuApplyVrSettings";
            BuApplyVrSettings.Size = new Size(197, 32);
            BuApplyVrSettings.TabIndex = 14;
            BuApplyVrSettings.Text = "Apply";
            BuApplyVrSettings.UseVisualStyleBackColor = true;
            BuApplyVrSettings.Click += BuApplyVrSettings_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 96);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 13;
            label2.Text = "Color format";
            // 
            // TaAbout
            // 
            TaAbout.Controls.Add(RiAbout);
            TaAbout.Location = new Point(4, 24);
            TaAbout.Name = "TaAbout";
            TaAbout.Size = new Size(614, 422);
            TaAbout.TabIndex = 2;
            TaAbout.Text = "About";
            TaAbout.UseVisualStyleBackColor = true;
            // 
            // RiAbout
            // 
            RiAbout.Dock = DockStyle.Fill;
            RiAbout.Location = new Point(0, 0);
            RiAbout.Name = "RiAbout";
            RiAbout.ReadOnly = true;
            RiAbout.Size = new Size(614, 422);
            RiAbout.TabIndex = 0;
            RiAbout.Text = "Author: alextrof94 aka GoodVrGames\nGithub: https://github.com/alextrof94/VrBrowser\n\nIcons by Icons8 https://icons8.com/";
            // 
            // TaBrowser
            // 
            TaBrowser.Controls.Add(ChAudioEnabled);
            TaBrowser.Controls.Add(BuTabDelete);
            TaBrowser.Controls.Add(GrCss);
            TaBrowser.Controls.Add(TeUrl);
            TaBrowser.Controls.Add(label1);
            TaBrowser.Controls.Add(BuReload);
            TaBrowser.Location = new Point(4, 24);
            TaBrowser.Name = "TaBrowser";
            TaBrowser.Padding = new Padding(3);
            TaBrowser.Size = new Size(614, 422);
            TaBrowser.TabIndex = 0;
            TaBrowser.Text = "Main";
            TaBrowser.UseVisualStyleBackColor = true;
            // 
            // ChAudioEnabled
            // 
            ChAudioEnabled.AutoSize = true;
            ChAudioEnabled.Location = new Point(514, 35);
            ChAudioEnabled.Name = "ChAudioEnabled";
            ChAudioEnabled.Size = new Size(94, 19);
            ChAudioEnabled.TabIndex = 16;
            ChAudioEnabled.Tag = "{00000000-0000-0000-0000-000000000000}";
            ChAudioEnabled.Text = "Enable audio";
            ChAudioEnabled.UseVisualStyleBackColor = true;
            ChAudioEnabled.CheckedChanged += CheckBoxAudioEnabled_CheckedChanged;
            // 
            // BuTabDelete
            // 
            BuTabDelete.ImageIndex = 1;
            BuTabDelete.ImageList = ImButtons;
            BuTabDelete.Location = new Point(6, 6);
            BuTabDelete.Name = "BuTabDelete";
            BuTabDelete.Size = new Size(22, 22);
            BuTabDelete.TabIndex = 15;
            BuTabDelete.Tag = "00000000-0000-0000-0000-000000000000";
            BuTabDelete.UseVisualStyleBackColor = true;
            // 
            // ImButtons
            // 
            ImButtons.ColorDepth = ColorDepth.Depth8Bit;
            ImButtons.ImageStream = (ImageListStreamer)resources.GetObject("ImButtons.ImageStream");
            ImButtons.TransparentColor = Color.Transparent;
            ImButtons.Images.SetKeyName(0, "icons8-plus-16.png");
            ImButtons.Images.SetKeyName(1, "icons8-minus-16.png");
            ImButtons.Images.SetKeyName(2, "icons8-left-16.png");
            ImButtons.Images.SetKeyName(3, "icons8-right-16.png");
            // 
            // GrCss
            // 
            GrCss.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GrCss.Controls.Add(RiCss);
            GrCss.Location = new Point(6, 51);
            GrCss.Name = "GrCss";
            GrCss.Size = new Size(602, 365);
            GrCss.TabIndex = 4;
            GrCss.TabStop = false;
            GrCss.Tag = "00000000-0000-0000-0000-000000000000";
            GrCss.Text = "CSS";
            // 
            // BuTabAdd
            // 
            BuTabAdd.ImageIndex = 0;
            BuTabAdd.ImageList = ImButtons;
            BuTabAdd.Location = new Point(597, 1);
            BuTabAdd.Name = "BuTabAdd";
            BuTabAdd.Size = new Size(22, 22);
            BuTabAdd.TabIndex = 14;
            BuTabAdd.UseVisualStyleBackColor = true;
            BuTabAdd.Click += BuTabAdd_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(622, 450);
            Controls.Add(BuTabAdd);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "VrBrowser Overlay";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            ((System.ComponentModel.ISupportInitialize)NuCurvature).EndInit();
            ((System.ComponentModel.ISupportInitialize)NuOffset).EndInit();
            ((System.ComponentModel.ISupportInitialize)NuSize).EndInit();
            tabControl1.ResumeLayout(false);
            TaVrSettings.ResumeLayout(false);
            TaVrSettings.PerformLayout();
            TaAbout.ResumeLayout(false);
            TaBrowser.ResumeLayout(false);
            TaBrowser.PerformLayout();
            GrCss.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox TeUrl;
        private Button BuReload;
        private RichTextBox RiCss;
        private Label label1;
        private Label label3;
        private NumericUpDown NuCurvature;
        private NumericUpDown NuOffset;
        private Label label4;
        private NumericUpDown NuSize;
        private Label label5;
        private System.Windows.Forms.Timer TiStartVr;
        private ComboBox CoColorFormat;
        private TabControl tabControl1;
        private TabPage TaBrowser;
        private GroupBox GrCss;
        private TabPage TaVrSettings;
        private Label label2;
        private TabPage TaAbout;
        private RichTextBox RiAbout;
        private Button BuTabAdd;
        private Button BuTabDelete;
        private ImageList ImButtons;
        private CheckBox ChAudioEnabled;
        private Button BuApplyVrSettings;
    }
}
