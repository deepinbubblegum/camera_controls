namespace MATRIX3DPHOTO
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.coLoop = new System.Windows.Forms.ComboBox();
            this.coLength = new System.Windows.Forms.ComboBox();
            this.coSFXtype = new System.Windows.Forms.ComboBox();
            this.coRenderResolution = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.coRenderType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkCountDown = new System.Windows.Forms.CheckBox();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.chkWebCam = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.coSFXtime = new System.Windows.Forms.ComboBox();
            this.coMaxCamera = new System.Windows.Forms.ComboBox();
            this.coSyncMaster = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.coCameraDevice = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Setting_mode = new System.Windows.Forms.GroupBox();
            this.BT_mode = new System.Windows.Forms.RadioButton();
            this.Com_ports = new System.Windows.Forms.GroupBox();
            this.Com_ports_selete = new System.Windows.Forms.ComboBox();
            this.SEmode = new System.Windows.Forms.RadioButton();
            this.usbmode = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Camera1_interval = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.camera_number_1 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_6 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_2 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_7 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_3 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_8 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_4 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_9 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_5 = new System.Windows.Forms.NumericUpDown();
            this.camera_number_10 = new System.Windows.Forms.NumericUpDown();
            this.group_setting = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.Setting_mode.SuspendLayout();
            this.Com_ports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_10)).BeginInit();
            this.group_setting.SuspendLayout();
            this.SuspendLayout();
            // 
            // coLoop
            // 
            this.coLoop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coLoop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coLoop.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coLoop.ForeColor = System.Drawing.Color.White;
            this.coLoop.FormattingEnabled = true;
            this.coLoop.Location = new System.Drawing.Point(207, 497);
            this.coLoop.Name = "coLoop";
            this.coLoop.Size = new System.Drawing.Size(119, 31);
            this.coLoop.TabIndex = 48;
            this.coLoop.TabStop = false;
            this.coLoop.SelectedIndexChanged += new System.EventHandler(this.coLoop_SelectedIndexChanged);
            // 
            // coLength
            // 
            this.coLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coLength.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coLength.ForeColor = System.Drawing.Color.White;
            this.coLength.FormattingEnabled = true;
            this.coLength.Location = new System.Drawing.Point(207, 459);
            this.coLength.Name = "coLength";
            this.coLength.Size = new System.Drawing.Size(119, 31);
            this.coLength.TabIndex = 47;
            this.coLength.TabStop = false;
            this.coLength.SelectedIndexChanged += new System.EventHandler(this.coLength_SelectedIndexChanged);
            // 
            // coSFXtype
            // 
            this.coSFXtype.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coSFXtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coSFXtype.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coSFXtype.ForeColor = System.Drawing.Color.White;
            this.coSFXtype.FormattingEnabled = true;
            this.coSFXtype.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.coSFXtype.Location = new System.Drawing.Point(207, 349);
            this.coSFXtype.Name = "coSFXtype";
            this.coSFXtype.Size = new System.Drawing.Size(119, 31);
            this.coSFXtype.TabIndex = 46;
            this.coSFXtype.TabStop = false;
            this.coSFXtype.SelectedIndexChanged += new System.EventHandler(this.coSFXtype_SelectedIndexChanged);
            // 
            // coRenderResolution
            // 
            this.coRenderResolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coRenderResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coRenderResolution.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coRenderResolution.ForeColor = System.Drawing.Color.White;
            this.coRenderResolution.FormattingEnabled = true;
            this.coRenderResolution.Location = new System.Drawing.Point(87, 633);
            this.coRenderResolution.Name = "coRenderResolution";
            this.coRenderResolution.Size = new System.Drawing.Size(239, 31);
            this.coRenderResolution.TabIndex = 45;
            this.coRenderResolution.TabStop = false;
            this.coRenderResolution.SelectedIndexChanged += new System.EventHandler(this.coRenderResolution_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(80, 601);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(117, 29);
            this.label15.TabIndex = 44;
            this.label15.Text = "RESOLUTION :";
            // 
            // coRenderType
            // 
            this.coRenderType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coRenderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coRenderType.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coRenderType.ForeColor = System.Drawing.Color.White;
            this.coRenderType.FormattingEnabled = true;
            this.coRenderType.Location = new System.Drawing.Point(87, 567);
            this.coRenderType.Name = "coRenderType";
            this.coRenderType.Size = new System.Drawing.Size(239, 31);
            this.coRenderType.TabIndex = 43;
            this.coRenderType.TabStop = false;
            this.coRenderType.SelectedIndexChanged += new System.EventHandler(this.coRenderType_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(80, 535);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 29);
            this.label14.TabIndex = 42;
            this.label14.Text = "TYPE :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(80, 499);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 29);
            this.label13.TabIndex = 41;
            this.label13.Text = "LOOP :";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(80, 459);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 29);
            this.label12.TabIndex = 40;
            this.label12.Text = "LENGTH :";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label11.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(90, 347);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 29);
            this.label11.TabIndex = 39;
            this.label11.Text = "SFX STYLE :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label1.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(90, 385);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 29);
            this.label1.TabIndex = 49;
            this.label1.Text = "INTERVAL :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkCountDown
            // 
            this.chkCountDown.AutoSize = true;
            this.chkCountDown.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCountDown.Location = new System.Drawing.Point(87, 167);
            this.chkCountDown.Name = "chkCountDown";
            this.chkCountDown.Size = new System.Drawing.Size(134, 33);
            this.chkCountDown.TabIndex = 52;
            this.chkCountDown.Text = "COUNT DOWN";
            this.chkCountDown.UseVisualStyleBackColor = true;
            this.chkCountDown.CheckedChanged += new System.EventHandler(this.chkCountDown_CheckedChanged);
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreview.Location = new System.Drawing.Point(87, 242);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(184, 33);
            this.chkPreview.TabIndex = 53;
            this.chkPreview.Text = "LIVE FEED [Monitor2]";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkWebCam
            // 
            this.chkWebCam.AutoSize = true;
            this.chkWebCam.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWebCam.Location = new System.Drawing.Point(88, 205);
            this.chkWebCam.Name = "chkWebCam";
            this.chkWebCam.Size = new System.Drawing.Size(101, 33);
            this.chkWebCam.TabIndex = 54;
            this.chkWebCam.Text = "WEB Cam";
            this.chkWebCam.UseVisualStyleBackColor = true;
            this.chkWebCam.CheckedChanged += new System.EventHandler(this.chkWebCam_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(83, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 29);
            this.label3.TabIndex = 55;
            this.label3.Text = "VOL CAM :";
            // 
            // coSFXtime
            // 
            this.coSFXtime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coSFXtime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coSFXtime.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coSFXtime.ForeColor = System.Drawing.Color.White;
            this.coSFXtime.FormattingEnabled = true;
            this.coSFXtime.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.coSFXtime.Location = new System.Drawing.Point(207, 387);
            this.coSFXtime.Name = "coSFXtime";
            this.coSFXtime.Size = new System.Drawing.Size(119, 31);
            this.coSFXtime.TabIndex = 56;
            this.coSFXtime.TabStop = false;
            this.coSFXtime.SelectedIndexChanged += new System.EventHandler(this.coSFXtime_SelectedIndexChanged);
            // 
            // coMaxCamera
            // 
            this.coMaxCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coMaxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coMaxCamera.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coMaxCamera.ForeColor = System.Drawing.Color.White;
            this.coMaxCamera.FormattingEnabled = true;
            this.coMaxCamera.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.coMaxCamera.Location = new System.Drawing.Point(207, 84);
            this.coMaxCamera.Name = "coMaxCamera";
            this.coMaxCamera.Size = new System.Drawing.Size(119, 31);
            this.coMaxCamera.TabIndex = 57;
            this.coMaxCamera.TabStop = false;
            this.coMaxCamera.SelectedIndexChanged += new System.EventHandler(this.coMaxCamera_SelectedIndexChanged);
            // 
            // coSyncMaster
            // 
            this.coSyncMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coSyncMaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coSyncMaster.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coSyncMaster.ForeColor = System.Drawing.Color.White;
            this.coSyncMaster.FormattingEnabled = true;
            this.coSyncMaster.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.coSyncMaster.Location = new System.Drawing.Point(207, 122);
            this.coSyncMaster.Name = "coSyncMaster";
            this.coSyncMaster.Size = new System.Drawing.Size(119, 31);
            this.coSyncMaster.TabIndex = 59;
            this.coSyncMaster.TabStop = false;
            this.coSyncMaster.SelectedIndexChanged += new System.EventHandler(this.coSyncMaster_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(83, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 29);
            this.label4.TabIndex = 58;
            this.label4.Text = "MASTER :";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label2.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 58);
            this.label2.TabIndex = 51;
            this.label2.Text = "SFX EDITING:";
            // 
            // coCameraDevice
            // 
            this.coCameraDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.coCameraDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coCameraDevice.Enabled = false;
            this.coCameraDevice.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coCameraDevice.ForeColor = System.Drawing.Color.White;
            this.coCameraDevice.FormattingEnabled = true;
            this.coCameraDevice.Items.AddRange(new object[] {
            "No Device"});
            this.coCameraDevice.Location = new System.Drawing.Point(208, 203);
            this.coCameraDevice.Name = "coCameraDevice";
            this.coCameraDevice.Size = new System.Drawing.Size(119, 31);
            this.coCameraDevice.TabIndex = 65;
            this.coCameraDevice.TabStop = false;
            this.coCameraDevice.SelectedIndexChanged += new System.EventHandler(this.coCameraDevice_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.btnOK.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(1025, 528);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 150);
            this.btnOK.TabIndex = 66;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::MATRIX3DPHOTO.Properties.Resources._001;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(16, 64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 231);
            this.pictureBox1.TabIndex = 61;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox3.Location = new System.Drawing.Point(353, 64);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(655, 612);
            this.pictureBox3.TabIndex = 63;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
            this.pictureBox4.Location = new System.Drawing.Point(15, 306);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(324, 131);
            this.pictureBox4.TabIndex = 64;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(15, 447);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(324, 231);
            this.pictureBox2.TabIndex = 67;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(860, -20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 58);
            this.label5.TabIndex = 68;
            // 
            // Setting_mode
            // 
            this.Setting_mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Setting_mode.BackColor = System.Drawing.Color.Transparent;
            this.Setting_mode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Setting_mode.Controls.Add(this.BT_mode);
            this.Setting_mode.Controls.Add(this.Com_ports);
            this.Setting_mode.Controls.Add(this.SEmode);
            this.Setting_mode.Controls.Add(this.usbmode);
            this.Setting_mode.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 15.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Setting_mode.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Setting_mode.Location = new System.Drawing.Point(1022, 52);
            this.Setting_mode.Name = "Setting_mode";
            this.Setting_mode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Setting_mode.Size = new System.Drawing.Size(176, 182);
            this.Setting_mode.TabIndex = 69;
            this.Setting_mode.TabStop = false;
            this.Setting_mode.Text = "Setting Mode";
            this.Setting_mode.Enter += new System.EventHandler(this.Setting_mode_Enter);
            // 
            // BT_mode
            // 
            this.BT_mode.AutoSize = true;
            this.BT_mode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_mode.Location = new System.Drawing.Point(12, 73);
            this.BT_mode.Name = "BT_mode";
            this.BT_mode.Size = new System.Drawing.Size(166, 30);
            this.BT_mode.TabIndex = 71;
            this.BT_mode.Text = "Bluetooth Serial Mode";
            this.BT_mode.UseVisualStyleBackColor = true;
            this.BT_mode.CheckedChanged += new System.EventHandler(this.BT_mode_CheckedChanged);
            // 
            // Com_ports
            // 
            this.Com_ports.Controls.Add(this.Com_ports_selete);
            this.Com_ports.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Com_ports.Location = new System.Drawing.Point(6, 105);
            this.Com_ports.Name = "Com_ports";
            this.Com_ports.Size = new System.Drawing.Size(164, 67);
            this.Com_ports.TabIndex = 70;
            this.Com_ports.TabStop = false;
            this.Com_ports.Text = "COM Ports";
            // 
            // Com_ports_selete
            // 
            this.Com_ports_selete.FormattingEnabled = true;
            this.Com_ports_selete.Location = new System.Drawing.Point(6, 27);
            this.Com_ports_selete.Name = "Com_ports_selete";
            this.Com_ports_selete.Size = new System.Drawing.Size(152, 33);
            this.Com_ports_selete.TabIndex = 2;
            this.Com_ports_selete.SelectedIndexChanged += new System.EventHandler(this.Com_ports_selete_SelectedIndexChanged);
            // 
            // SEmode
            // 
            this.SEmode.AutoSize = true;
            this.SEmode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SEmode.Location = new System.Drawing.Point(12, 48);
            this.SEmode.Name = "SEmode";
            this.SEmode.Size = new System.Drawing.Size(132, 30);
            this.SEmode.TabIndex = 1;
            this.SEmode.Text = "USB Serial Mode";
            this.SEmode.UseVisualStyleBackColor = true;
            // 
            // usbmode
            // 
            this.usbmode.AutoSize = true;
            this.usbmode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usbmode.Location = new System.Drawing.Point(12, 22);
            this.usbmode.Name = "usbmode";
            this.usbmode.Size = new System.Drawing.Size(93, 30);
            this.usbmode.TabIndex = 0;
            this.usbmode.Text = "USB Mode";
            this.usbmode.UseVisualStyleBackColor = true;
            this.usbmode.CheckedChanged += new System.EventHandler(this.settingmode_CheckedChanged);
            // 
            // Camera1_interval
            // 
            this.Camera1_interval.AutoSize = true;
            this.Camera1_interval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.Camera1_interval.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Camera1_interval.Location = new System.Drawing.Point(29, 87);
            this.Camera1_interval.Name = "Camera1_interval";
            this.Camera1_interval.Size = new System.Drawing.Size(94, 29);
            this.Camera1_interval.TabIndex = 72;
            this.Camera1_interval.Text = "CAMERA 1 :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label6.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(29, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 29);
            this.label6.TabIndex = 74;
            this.label6.Text = "CAMERA 2 :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label7.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(29, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 29);
            this.label7.TabIndex = 72;
            this.label7.Text = "CAMERA 3 :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label8.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(29, 215);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 29);
            this.label8.TabIndex = 74;
            this.label8.Text = "CAMERA 4:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label9.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(29, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 29);
            this.label9.TabIndex = 72;
            this.label9.Text = "CAMERA 5 :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label10.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(347, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 29);
            this.label10.TabIndex = 72;
            this.label10.Text = "CAMERA 6 :";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label16.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(347, 172);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 29);
            this.label16.TabIndex = 72;
            this.label16.Text = "CAMERA 8 :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label17.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(347, 130);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(94, 29);
            this.label17.TabIndex = 74;
            this.label17.Text = "CAMERA 7 :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label18.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(347, 261);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(102, 29);
            this.label18.TabIndex = 72;
            this.label18.Text = "CAMERA 10 :";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.label19.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(347, 215);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(94, 29);
            this.label19.TabIndex = 74;
            this.label19.Text = "CAMERA 9 :";
            // 
            // camera_number_1
            // 
            this.camera_number_1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_1.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_1.Location = new System.Drawing.Point(129, 87);
            this.camera_number_1.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_1.Name = "camera_number_1";
            this.camera_number_1.Size = new System.Drawing.Size(119, 29);
            this.camera_number_1.TabIndex = 75;
            this.camera_number_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_1.ThousandsSeparator = true;
            this.camera_number_1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_1.ValueChanged += new System.EventHandler(this.camera_number_1_ValueChanged);
            // 
            // camera_number_6
            // 
            this.camera_number_6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_6.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_6.Location = new System.Drawing.Point(447, 87);
            this.camera_number_6.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_6.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_6.Name = "camera_number_6";
            this.camera_number_6.Size = new System.Drawing.Size(119, 29);
            this.camera_number_6.TabIndex = 75;
            this.camera_number_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_6.ThousandsSeparator = true;
            this.camera_number_6.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_6.ValueChanged += new System.EventHandler(this.camera_number_6_ValueChanged);
            // 
            // camera_number_2
            // 
            this.camera_number_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_2.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_2.Location = new System.Drawing.Point(129, 130);
            this.camera_number_2.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_2.Name = "camera_number_2";
            this.camera_number_2.Size = new System.Drawing.Size(119, 29);
            this.camera_number_2.TabIndex = 75;
            this.camera_number_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_2.ThousandsSeparator = true;
            this.camera_number_2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_2.ValueChanged += new System.EventHandler(this.camera_number_2_ValueChanged);
            // 
            // camera_number_7
            // 
            this.camera_number_7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_7.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_7.Location = new System.Drawing.Point(447, 130);
            this.camera_number_7.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_7.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_7.Name = "camera_number_7";
            this.camera_number_7.Size = new System.Drawing.Size(119, 29);
            this.camera_number_7.TabIndex = 75;
            this.camera_number_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_7.ThousandsSeparator = true;
            this.camera_number_7.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_7.ValueChanged += new System.EventHandler(this.camera_number_7_ValueChanged);
            // 
            // camera_number_3
            // 
            this.camera_number_3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_3.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_3.Location = new System.Drawing.Point(129, 175);
            this.camera_number_3.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_3.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_3.Name = "camera_number_3";
            this.camera_number_3.Size = new System.Drawing.Size(119, 29);
            this.camera_number_3.TabIndex = 75;
            this.camera_number_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_3.ThousandsSeparator = true;
            this.camera_number_3.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_3.ValueChanged += new System.EventHandler(this.camera_number_3_ValueChanged);
            // 
            // camera_number_8
            // 
            this.camera_number_8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_8.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_8.Location = new System.Drawing.Point(447, 175);
            this.camera_number_8.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_8.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_8.Name = "camera_number_8";
            this.camera_number_8.Size = new System.Drawing.Size(119, 29);
            this.camera_number_8.TabIndex = 75;
            this.camera_number_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_8.ThousandsSeparator = true;
            this.camera_number_8.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_8.ValueChanged += new System.EventHandler(this.camera_number_8_ValueChanged);
            // 
            // camera_number_4
            // 
            this.camera_number_4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_4.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_4.Location = new System.Drawing.Point(129, 215);
            this.camera_number_4.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_4.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_4.Name = "camera_number_4";
            this.camera_number_4.Size = new System.Drawing.Size(119, 29);
            this.camera_number_4.TabIndex = 75;
            this.camera_number_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_4.ThousandsSeparator = true;
            this.camera_number_4.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_4.ValueChanged += new System.EventHandler(this.camera_number_4_ValueChanged);
            // 
            // camera_number_9
            // 
            this.camera_number_9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_9.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_9.Location = new System.Drawing.Point(447, 215);
            this.camera_number_9.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_9.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_9.Name = "camera_number_9";
            this.camera_number_9.Size = new System.Drawing.Size(119, 29);
            this.camera_number_9.TabIndex = 75;
            this.camera_number_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_9.ThousandsSeparator = true;
            this.camera_number_9.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_9.ValueChanged += new System.EventHandler(this.camera_number_9_ValueChanged);
            // 
            // camera_number_5
            // 
            this.camera_number_5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_5.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_5.Location = new System.Drawing.Point(129, 261);
            this.camera_number_5.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_5.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_5.Name = "camera_number_5";
            this.camera_number_5.Size = new System.Drawing.Size(119, 29);
            this.camera_number_5.TabIndex = 75;
            this.camera_number_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_5.ThousandsSeparator = true;
            this.camera_number_5.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_5.ValueChanged += new System.EventHandler(this.camera_number_5_ValueChanged);
            // 
            // camera_number_10
            // 
            this.camera_number_10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.camera_number_10.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camera_number_10.Location = new System.Drawing.Point(447, 261);
            this.camera_number_10.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.camera_number_10.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.camera_number_10.Name = "camera_number_10";
            this.camera_number_10.Size = new System.Drawing.Size(119, 29);
            this.camera_number_10.TabIndex = 75;
            this.camera_number_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.camera_number_10.ThousandsSeparator = true;
            this.camera_number_10.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.camera_number_10.ValueChanged += new System.EventHandler(this.camera_number_10_ValueChanged);
            // 
            // group_setting
            // 
            this.group_setting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.group_setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(5)))), ((int)(((byte)(82)))));
            this.group_setting.Controls.Add(this.label2);
            this.group_setting.Controls.Add(this.label9);
            this.group_setting.Controls.Add(this.camera_number_10);
            this.group_setting.Controls.Add(this.Camera1_interval);
            this.group_setting.Controls.Add(this.camera_number_9);
            this.group_setting.Controls.Add(this.label10);
            this.group_setting.Controls.Add(this.camera_number_8);
            this.group_setting.Controls.Add(this.label7);
            this.group_setting.Controls.Add(this.camera_number_7);
            this.group_setting.Controls.Add(this.label6);
            this.group_setting.Controls.Add(this.camera_number_6);
            this.group_setting.Controls.Add(this.label16);
            this.group_setting.Controls.Add(this.camera_number_5);
            this.group_setting.Controls.Add(this.label17);
            this.group_setting.Controls.Add(this.camera_number_4);
            this.group_setting.Controls.Add(this.label18);
            this.group_setting.Controls.Add(this.camera_number_3);
            this.group_setting.Controls.Add(this.label8);
            this.group_setting.Controls.Add(this.camera_number_2);
            this.group_setting.Controls.Add(this.label19);
            this.group_setting.Controls.Add(this.camera_number_1);
            this.group_setting.Location = new System.Drawing.Point(376, 80);
            this.group_setting.Name = "group_setting";
            this.group_setting.Size = new System.Drawing.Size(616, 357);
            this.group_setting.TabIndex = 76;
            this.group_setting.TabStop = false;
            // 
            // FormSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1201, 737);
            this.Controls.Add(this.group_setting);
            this.Controls.Add(this.Setting_mode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.coRenderType);
            this.Controls.Add(this.coCameraDevice);
            this.Controls.Add(this.coSyncMaster);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.coMaxCamera);
            this.Controls.Add(this.coSFXtime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkWebCam);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.chkCountDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.coLoop);
            this.Controls.Add(this.coLength);
            this.Controls.Add(this.coSFXtype);
            this.Controls.Add(this.coRenderResolution);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("DB HelvethaicaMon X 77 BdCond", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "FormSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MATRIX 3D PHOTO > SETTING";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSetting_FormClosing);
            this.Load += new System.EventHandler(this.Form_SETTING_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.Setting_mode.ResumeLayout(false);
            this.Setting_mode.PerformLayout();
            this.Com_ports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_number_10)).EndInit();
            this.group_setting.ResumeLayout(false);
            this.group_setting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkCountDown;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.CheckBox chkWebCam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox coLoop;
        public System.Windows.Forms.ComboBox coLength;
        public System.Windows.Forms.ComboBox coSFXtype;
        public System.Windows.Forms.ComboBox coRenderResolution;
        public System.Windows.Forms.ComboBox coRenderType;
        public System.Windows.Forms.ComboBox coSFXtime;
        public System.Windows.Forms.ComboBox coMaxCamera;
        public System.Windows.Forms.ComboBox coSyncMaster;
        public System.Windows.Forms.ComboBox coCameraDevice;
        private System.Windows.Forms.GroupBox Setting_mode;
        private System.Windows.Forms.RadioButton usbmode;
        private System.Windows.Forms.ComboBox Com_ports_selete;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox Com_ports;
        private System.Windows.Forms.RadioButton SEmode;
        private System.Windows.Forms.RadioButton BT_mode;
        private System.Windows.Forms.Label Camera1_interval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown camera_number_1;
        private System.Windows.Forms.NumericUpDown camera_number_6;
        private System.Windows.Forms.NumericUpDown camera_number_2;
        private System.Windows.Forms.NumericUpDown camera_number_7;
        private System.Windows.Forms.NumericUpDown camera_number_3;
        private System.Windows.Forms.NumericUpDown camera_number_8;
        private System.Windows.Forms.NumericUpDown camera_number_4;
        private System.Windows.Forms.NumericUpDown camera_number_9;
        private System.Windows.Forms.NumericUpDown camera_number_5;
        private System.Windows.Forms.NumericUpDown camera_number_10;
        private System.Windows.Forms.GroupBox group_setting;
    }
}