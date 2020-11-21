using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
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
            string[] ports = SerialPort.GetPortNames();
            // Display each port name to the console.
            foreach (string port in ports)
            {
                Console.WriteLine(port);
                Com_ports_selete.Items.Add(port);
            }

            if (Properties.Settings.Default.Setting_mode == 1)
            {
                usbmode.Checked = true;
                SEmode.Checked = false;
                BT_mode.Checked = false;
            }
            else if(Properties.Settings.Default.Setting_mode == 2)
            {
                usbmode.Checked = false;
                SEmode.Checked = true;
                BT_mode.Checked = false;
            }
            else if (Properties.Settings.Default.Setting_mode == 3)
            {
                usbmode.Checked = false;
                SEmode.Checked = false;
                BT_mode.Checked = true;
            }
            load_config_usb();
            serial_usb_enable();
            funtion_custrom_mode();
            load_save_camera_custorm();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        bool init = false;
        private FilterInfoCollection videoDevices;
        public void LoadConfig()
        {
            coMaxCamera.Items.Clear();
            coSyncMaster.Items.Clear();
            for (int i = 1; i <= 10; i++)
            {
                coMaxCamera.Items.Add(i);
                coSyncMaster.Items.Add(i);
            }
            try { coMaxCamera.SelectedIndex = (Properties.Settings.Default.MaxCamera - 1); } catch { coMaxCamera.SelectedIndex = 5 - 1; }
            try { coSyncMaster.SelectedIndex = (Properties.Settings.Default.MasterConfig - 1); } catch { coMaxCamera.SelectedIndex = (coMaxCamera.SelectedIndex / 2) + 1; }

 
            coSFXtype.Items.Clear();
            coSFXtype.Items.Add("Freeze");
            coSFXtype.Items.Add("Forward");
            coSFXtype.Items.Add("Reward");
            coSFXtype.Items.Add("Random");
            coSFXtype.Items.Add("Custom");
            if (coSFXtype.SelectedIndex != 0)
            {
                coSFXtype.SelectedIndex = Properties.Settings.Default.SFXType;
            }
            else
            {
                coSFXtype.SelectedIndex = 0;
            }

     

            coSFXtime.Items.Clear();
            for(int i=1;i<=3000;i++)
            {
                coSFXtime.Items.Add(i);
            }
            if (coSFXtype.SelectedIndex > 0)
            {
                coSFXtime.SelectedIndex = Properties.Settings.Default.SFXTime - 1;
                coSFXtime.Enabled = true;
            }
            else
            { 
                coSFXtime.SelectedIndex = -1;
                coSFXtime.Enabled = false;
            }

            coLength.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                string str = "     " + i.ToString() + " sec";
                coLength.Items.Add(str);
            }
            coLength.SelectedIndex = Properties.Settings.Default.Length - 1;
            //coLength.SelectedIndex = Properties.Settings.Default.Length;

            coLoop.Items.Clear();
            for (int i = 1; i <= 10; i++)
            {
                string str = "     " + i.ToString() + " round";
                coLoop.Items.Add(str);
            }
            coLoop.SelectedIndex =  Properties.Settings.Default.Loop - 1;

            coRenderType.Items.Clear();
            coRenderType.Items.Add("     MP4");
            coRenderType.Items.Add("     GIF");
            if (Properties.Settings.Default.Type >= 0) coRenderType.SelectedIndex = Properties.Settings.Default.Type;
            else coRenderType.SelectedIndex = 0;
  

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

            if (Properties.Settings.Default.CountDownEnable)
            {
                chkCountDown.Checked = true;
            }
            if (Properties.Settings.Default.PreviewBeforeFire)
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

                    if (coCameraDevice.Items.Count >= Properties.Settings.Default.WebcamDevice + 1)
                    {
                        coCameraDevice.Enabled = true;
                        coCameraDevice.SelectedIndex = Properties.Settings.Default.WebcamDevice;
                    }
                }
                else
                {
                    chkWebCam.Checked = false;
                    coCameraDevice.Enabled = false;
                    Properties.Settings.Default.PreviewBeforeFire = false;
                }
            }
            if (Properties.Settings.Default.PreviewEnable)
            {
                chkPreview.Checked = true;
            }

            init = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void coMaxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.MaxCamera = coMaxCamera.SelectedIndex + 1;
                Properties.Settings.Default.Save();
            }
        }

        private void coSyncMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.MasterConfig = coSyncMaster.SelectedIndex + 1;
                Properties.Settings.Default.Save();
            }
        }

        private void chkCountDown_CheckedChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.CountDownEnable = chkCountDown.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void chkWebCam_CheckedChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.PreviewBeforeFire = chkWebCam.Checked;
                coCameraDevice.Enabled = chkWebCam.Checked;
                Properties.Settings.Default.Save();
                
                if(chkWebCam.Checked)
                {
                    coCameraDevice.Enabled = true;
                    videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    if (videoDevices.Count != 0)
                    {
                        coCameraDevice.Items.Clear();
                        foreach (FilterInfo device in videoDevices)
                        {
                            coCameraDevice.Items.Add(device.Name);
                        }

                        if (coCameraDevice.Items.Count >= Properties.Settings.Default.WebcamDevice + 1)
                        {
                            coCameraDevice.Enabled = true;
                            coCameraDevice.SelectedIndex = Properties.Settings.Default.WebcamDevice;
                        }
                    }
                    else
                    {
                        chkWebCam.Checked = false;
                        coCameraDevice.Enabled = false;
                        Properties.Settings.Default.PreviewBeforeFire = false;
                    }
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.PreviewEnable = chkPreview.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void funtion_custrom_mode()
        {
            if (coSFXtype.SelectedIndex == 4)
            {
                group_setting.Enabled = true;
                coSFXtime.Enabled = false;
            }
            else
            {
                group_setting.Enabled = false;
            }
        }

        private void coSFXtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.SFXType = coSFXtype.SelectedIndex;
                Properties.Settings.Default.Save();

                if (coSFXtype.SelectedIndex > 0)
                {
                    coSFXtime.Enabled = true;
                    coSFXtime.SelectedIndex = Properties.Settings.Default.SFXTime - 1;
                }
                else
                {
                    coSFXtime.Enabled = false;
                    coSFXtime.SelectedIndex = -1;
                }


                funtion_custrom_mode();
            }
        }

        private void coSFXtime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.SFXTime = coSFXtime.SelectedIndex + 1;
                Properties.Settings.Default.Save();
            }
        }

        private void coLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.Length = coLength.SelectedIndex + 1;
                Properties.Settings.Default.Save();
            }
        }

        private void coLoop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.Loop = coLoop.SelectedIndex + 1;
                Properties.Settings.Default.Save();
            }
        }

        private void coRenderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.Type = coRenderType.SelectedIndex;
                Properties.Settings.Default.Save();
            }
        }

        private void coRenderResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.Resolution = coRenderResolution.SelectedIndex;
                Properties.Settings.Default.Save();
            }
        }

        private void FormSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void coCameraDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                Properties.Settings.Default.WebcamDevice = coCameraDevice.SelectedIndex;
                Properties.Settings.Default.Save();
            }
        }

        private void load_config_usb()
        {
            if (Properties.Settings.Default.Setting_mode == 1)
            {
                usbmode.Checked = true;
                serial_usb_enable();
            }
            else if(Properties.Settings.Default.Setting_mode == 2)
            {
                SEmode.Checked = true;
                serial_usb_enable();
            }
            else if(Properties.Settings.Default.Setting_mode == 3)
            {
                BT_mode.Checked = true;
                serial_usb_enable();
            }
        }

        private void serial_usb_enable()
        {
            if (usbmode.Checked)
            {
                Com_ports.Enabled = false;
                coSFXtype.Enabled = true;
                Properties.Settings.Default.Setting_mode = 1;
                Properties.Settings.Default.Save();
            }
            else if (SEmode.Checked)
            {
                Com_ports.Enabled = true;
                coSFXtype.Enabled = false;
                Properties.Settings.Default.Setting_mode = 2;
                Properties.Settings.Default.Save();
            }
            else if(BT_mode.Checked)
            {
                Com_ports.Enabled = true;
                coSFXtype.Enabled = true;
                Properties.Settings.Default.Setting_mode = 3;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.COM_Ports != "")
            {
                Com_ports_selete.Text = Properties.Settings.Default.COM_Ports;
            }
        }
   
       

        private void settingmode_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Setting_mode = usbmode.Checked;
            //MessageBox.Show(usbmode.Checked.ToString());
            //Properties.Settings.Default.Save();
            serial_usb_enable();
        }

        private void Com_ports_selete_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Com_port = Com_ports_selete.Items[Com_ports_selete.SelectedIndex].ToString();
            Properties.Settings.Default.COM_Ports = Com_port;
            Properties.Settings.Default.Save();
        }

        private void Setting_mode_Enter(object sender, EventArgs e)
        {
        }

        private void SFX_Group_Enter(object sender, EventArgs e)
        {

        }

        private void BT_mode_CheckedChanged(object sender, EventArgs e)
        {
            serial_usb_enable();
        }

        private void load_save_camera_custorm()
        {
            camera_number_1.Value = Properties.Settings.Default.Camera1_interval;
            camera_number_2.Value = Properties.Settings.Default.Camera2_interval;
            camera_number_3.Value = Properties.Settings.Default.Camera3_interval;
            camera_number_4.Value = Properties.Settings.Default.Camera4_interval;
            camera_number_5.Value = Properties.Settings.Default.Camera5_interval;
            camera_number_6.Value = Properties.Settings.Default.Camera6_interval;
            camera_number_7.Value = Properties.Settings.Default.Camera7_interval;
            camera_number_8.Value = Properties.Settings.Default.Camera8_interval;
            camera_number_9.Value = Properties.Settings.Default.Camera9_interval;
            camera_number_10.Value = Properties.Settings.Default.Camera10_interval;
        }

        private void camera_number_1_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_1 = Convert.ToInt32(camera_number_1.Value);
            Properties.Settings.Default.Camera1_interval = camera_set_1;
            Properties.Settings.Default.Save();
        }

        private void camera_number_2_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_2 = Convert.ToInt32(camera_number_2.Value);
            Properties.Settings.Default.Camera2_interval = camera_set_2;
            Properties.Settings.Default.Save();
        }

        private void camera_number_3_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_3 = Convert.ToInt32(camera_number_3.Value);
            Properties.Settings.Default.Camera3_interval = camera_set_3;
            Properties.Settings.Default.Save();
        }

        private void camera_number_4_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_4 = Convert.ToInt32(camera_number_4.Value);
            Properties.Settings.Default.Camera4_interval = camera_set_4;
            Properties.Settings.Default.Save();
        }

        private void camera_number_5_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_5 = Convert.ToInt32(camera_number_5.Value);
            Properties.Settings.Default.Camera5_interval = camera_set_5;
            Properties.Settings.Default.Save();
        }

        private void camera_number_6_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_6 = Convert.ToInt32(camera_number_6.Value);
            Properties.Settings.Default.Camera6_interval = camera_set_6;
            Properties.Settings.Default.Save();
        }

        private void camera_number_7_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_7 = Convert.ToInt32(camera_number_7.Value);
            Properties.Settings.Default.Camera7_interval = camera_set_7;
            Properties.Settings.Default.Save();
        }

        private void camera_number_8_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_8 = Convert.ToInt32(camera_number_8.Value);
            Properties.Settings.Default.Camera8_interval = camera_set_8;
            Properties.Settings.Default.Save();
        }

        private void camera_number_9_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_9 = Convert.ToInt32(camera_number_9.Value);
            Properties.Settings.Default.Camera9_interval = camera_set_9;
            Properties.Settings.Default.Save();
        }

        private void camera_number_10_ValueChanged(object sender, EventArgs e)
        {
            int camera_set_10 = Convert.ToInt32(camera_number_10.Value);
            Properties.Settings.Default.Camera10_interval = camera_set_10;
            Properties.Settings.Default.Save();
        }
    }
}
