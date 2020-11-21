namespace MATRIX3DPHOTO
{
    partial class FormPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreview));
            this.ax = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.ax)).BeginInit();
            this.SuspendLayout();
            // 
            // ax
            // 
            this.ax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ax.Enabled = true;
            this.ax.Location = new System.Drawing.Point(0, 0);
            this.ax.Name = "ax";
            this.ax.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ax.OcxState")));
            this.ax.Size = new System.Drawing.Size(814, 481);
            this.ax.TabIndex = 0;
            this.ax.ClickEvent += new AxWMPLib._WMPOCXEvents_ClickEventHandler(this.ax_ClickEvent);
            // 
            // FormPreview
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(814, 481);
            this.Controls.Add(this.ax);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Preview";
            ((System.ComponentModel.ISupportInitialize)(this.ax)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxWMPLib.AxWindowsMediaPlayer ax;
    }
}