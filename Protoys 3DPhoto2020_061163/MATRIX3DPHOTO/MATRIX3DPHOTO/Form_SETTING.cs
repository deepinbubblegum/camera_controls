using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace MATRIX3DPHOTO
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Form_SETTING_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void Init()
        {
            
              
        }
        private FilterInfoCollection videoDevices;
        public void LoadConfig()
        {
            coMaxCamera.Items.Clear();
            coSyncMaster.Items.Clear();
            for (int i = 1; i <= 50; i++)
            {
                coMaxCamera.Items.Add(i);
                coSyncMaster.Items.Add(i);
            }
            try { coMaxCamera.SelectedIndex = (Properties.Settings.Default.MaxCamera - 1); } catch { coMaxCamera.SelectedIndex = 5 - 1; }
            try { coSyncMaster.SelectedIndex = (Properties.Settings.Default.MasterConfig - 1); } catch { coMaxCamera.SelectedIndex = (coMaxCamera.SelectedIndex / 2) + 1; }

            if (Properties.Settings.Default.WebcamDevice >= 0)
            {
                chkWebCam.Checked = true;
                coCameraDevice.Enabled = true;
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count != 0)
                {
                    coCameraDevice.Items.Clear();
                    foreach (FilterInfo device in videoDevices)
                    {
                        coCameraDevice.Items.Add(device.Name);
                    }
                }
            }
            else
            {
                coCameraDevice.Enabled = false;
            }

            coSFXtype.Items.Clear();
            coSFXtype.Items.Add("Freeze");
            coSFXtype.Items.Add("Forward");
            coSFXtype.Items.Add("Reward");
            coSFXtype.Items.Add("Random");
            coSFXtype.Items.Add("Custom");
            coSFXtype.SelectedIndex = 0;

            
            coSFXtime.Items.Clear();
            for(int i=0;i<=1000;i++)
            {
                coSFXtime.Items.Add(i);
            }
            if (coSFXtype.SelectedIndex != 0)
            {
                coSFXtime.SelectedIndex = Properties.Settings.Default.SFXTime;
                coSFXtime.Enabled = true;
            }
            else
            { 
                coSFXtime.SelectedIndex = 0;
                coSFXtime.Enabled = false;
            }

            coLength.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                string str = "     " + i.ToString() + " sec";
                coLength.Items.Add(str);
            }
            coLength.SelectedIndex = 1;
            //coLength.SelectedIndex = Properties.Settings.Default.Length;

            coLoop.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                string str = "     " + i.ToString() + " round";
                coLoop.Items.Add(str);
            }
            coLoop.SelectedIndex = 2;

            coRenderType.Items.Clear();
            coRenderType.Items.Add("     MP4");
            coRenderType.Items.Add("     GIF");
            if (Properties.Settings.Default.Type >= 0) coRenderType.SelectedIndex = Properties.Settings.Default.Type;
            else coRenderType.SelectedIndex = 0;
            //coRenderType.SelectedIndex = Properties.Settings.Default.Type;

            coRenderResolution.Items.Clear();
            coRenderResolution.Items.Add("     3:2  [RAW]");
            coRenderResolution.Items.Add("     16:9 [1080 H]");
            coRenderResolution.Items.Add("     16:9 [720 H]");
            coRenderResolution.Items.Add("     16:9 [1080 V]");
            coRenderResolution.Items.Add("     16:9 [720 V]");
            coRenderResolution.Items.Add("     1:1 [1080]");
            coRenderResolution.Items.Add("     1:1 [720]");

            if (Properties.Settings.Default.Resolution >= 0) coRenderResolution.SelectedIndex = Properties.Settings.Default.Resolution;
            else coRenderResolution.SelectedIndex = 1;

        }
    }
}
