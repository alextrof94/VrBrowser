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
            textBox1 = new TextBox();
            BuLoadUrl = new Button();
            RiCss = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            NuCurvature = new NumericUpDown();
            NuOffset = new NumericUpDown();
            label4 = new Label();
            NuSize = new NumericUpDown();
            label5 = new Label();
            TiStartVr = new System.Windows.Forms.Timer(components);
            ChAudioEnable = new CheckBox();
            CoColorFormat = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)NuCurvature).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NuOffset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NuSize).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(49, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(245, 23);
            textBox1.TabIndex = 0;
            // 
            // BuLoadUrl
            // 
            BuLoadUrl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BuLoadUrl.Location = new Point(921, 12);
            BuLoadUrl.Name = "BuLoadUrl";
            BuLoadUrl.Size = new Size(75, 23);
            BuLoadUrl.TabIndex = 1;
            BuLoadUrl.Text = "Загрузить";
            BuLoadUrl.UseVisualStyleBackColor = true;
            BuLoadUrl.Click += BuLoadUrl_Click;
            // 
            // RiCss
            // 
            RiCss.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RiCss.Location = new Point(12, 62);
            RiCss.Name = "RiCss";
            RiCss.Size = new Size(984, 347);
            RiCss.TabIndex = 2;
            RiCss.Text = ".ObsPage__widgetContainer--Ra4Ps {\ndisplay: none;\n}";
            RiCss.TextChanged += RiCss_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 3;
            label1.Text = "URL:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 4;
            label2.Text = "CSS:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(451, 417);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 5;
            label3.Text = "Curvature";
            // 
            // NuCurvature
            // 
            NuCurvature.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NuCurvature.DecimalPlaces = 2;
            NuCurvature.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            NuCurvature.Location = new Point(528, 415);
            NuCurvature.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NuCurvature.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            NuCurvature.Name = "NuCurvature";
            NuCurvature.Size = new Size(120, 23);
            NuCurvature.TabIndex = 6;
            NuCurvature.ValueChanged += NuCurvature_ValueChanged;
            // 
            // NuOffset
            // 
            NuOffset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NuOffset.DecimalPlaces = 2;
            NuOffset.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            NuOffset.Location = new Point(308, 415);
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
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(231, 417);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 7;
            label4.Text = "Offset";
            // 
            // NuSize
            // 
            NuSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NuSize.DecimalPlaces = 2;
            NuSize.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            NuSize.Location = new Point(89, 415);
            NuSize.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            NuSize.Name = "NuSize";
            NuSize.Size = new Size(120, 23);
            NuSize.TabIndex = 10;
            NuSize.Value = new decimal(new int[] { 2, 0, 0, 0 });
            NuSize.ValueChanged += NuSize_ValueChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(12, 417);
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
            // ChAudioEnable
            // 
            ChAudioEnable.AutoSize = true;
            ChAudioEnable.Location = new Point(829, 415);
            ChAudioEnable.Name = "ChAudioEnable";
            ChAudioEnable.Size = new Size(167, 19);
            ChAudioEnable.TabIndex = 11;
            ChAudioEnable.Text = "Enable audio (need restart)";
            ChAudioEnable.UseVisualStyleBackColor = true;
            ChAudioEnable.CheckedChanged += ChAudioEnable_CheckedChanged;
            // 
            // CoColorFormat
            // 
            CoColorFormat.FormattingEnabled = true;
            CoColorFormat.Items.AddRange(new object[] { "Bgra", "Rgba" });
            CoColorFormat.Location = new Point(654, 415);
            CoColorFormat.Name = "CoColorFormat";
            CoColorFormat.Size = new Size(169, 23);
            CoColorFormat.TabIndex = 12;
            CoColorFormat.SelectedIndexChanged += CoColorFormat_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1002, 450);
            Controls.Add(CoColorFormat);
            Controls.Add(ChAudioEnable);
            Controls.Add(NuSize);
            Controls.Add(label5);
            Controls.Add(NuOffset);
            Controls.Add(label4);
            Controls.Add(NuCurvature);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(RiCss);
            Controls.Add(BuLoadUrl);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "VrBrowser (by GoodVrGames aka alextrof94)";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            ((System.ComponentModel.ISupportInitialize)NuCurvature).EndInit();
            ((System.ComponentModel.ISupportInitialize)NuOffset).EndInit();
            ((System.ComponentModel.ISupportInitialize)NuSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button BuLoadUrl;
        private RichTextBox RiCss;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown NuCurvature;
        private NumericUpDown NuOffset;
        private Label label4;
        private NumericUpDown NuSize;
        private Label label5;
        private System.Windows.Forms.Timer TiStartVr;
        private CheckBox ChAudioEnable;
        private ComboBox CoColorFormat;
    }
}
