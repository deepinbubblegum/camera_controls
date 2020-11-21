using AForge.Video;
using AForge.Video.DirectShow;
using EOSDigital.API;
using EOSDigital.SDK;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MATRIX3DPHOTO
{
    public partial class FromMatrix3DPhoto : Form
    {
        //Install Font =====================================================================
        //[DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        //public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
        //                                 string lpFileName);
        //[DllImport("gdi32.dll", EntryPoint = "RemoveFontResourceW", SetLastError = true)]
        //public static extern int RemoveFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
        //                                    string lpFileName);
        ////Install Font =====================================================================



        string DllPath = Application.StartupPath + @"/MATRIX3DPHOTO.dll";



        CanonAPI APIHandler;
        CanonSDK.MainForm[] CameraControl;
        static List<Camera> CamList;
        bool CameraConnectionChange = true;

        Bitmap Evf_Bmp;
        Bitmap Mrk_Bmp;
        Bitmap CountDown_3_Bmp;
        Bitmap CountDown_2_Bmp;
        Bitmap CountDown_1_Bmp;
        Bitmap CountDown_Action_Bmp;
        int[] CameraStatus;
        int[] CameraSlot;

        int MaxCamera = Convert.ToInt16(Properties.Settings.Default.MaxCamera);
        int TotalFile = Convert.ToInt16(Properties.Settings.Default.MaxCamera);
        string CameraSavePath = Application.StartupPath + "/temp/";
        string FilePath = Application.StartupPath + "/temp/";
        string TempPath = Application.StartupPath + "/temp/";
        string RenderPath = Application.StartupPath + "/render/";
        string BackupPath = Properties.Settings.Default.BackupPath + "/";
        string BackupVdoPath = Properties.Settings.Default.BackupVdoPath + "/";
        string OutputPath = Properties.Settings.Default.OutputPath + "/";

        bool NewFire = false;
        int CheckFile_Timeout = 0;

        bool formClosed = false;

        bool FireEnable = false;
        //bool RunThread = true;
        //Thread tAutoConnect;
        //Thread tCheckFile;
        public FromMatrix3DPhoto()
        {
            InitializeComponent();

            CameraSavePath.Replace(@"\", @"/");
            FilePath.Replace(@"\", @"/");
            TempPath.Replace(@"\", @"/");
            RenderPath.Replace(@"\", @"/");
            BackupPath.Replace(@"\", @"/");
            BackupVdoPath.Replace(@"\", @"/");

            //Install Font =====================================================================
            //string fontName = "DB HelvethaicaMon X 77 BdCond";
            //float fontSize = 12;
            //using (Font fontTester = new Font(fontName,fontSize,FontStyle.Regular,GraphicsUnit.Pixel))
            //{
            //    if (fontTester.Name != fontName)
            //    {
            //        if (File.Exists("DB HelvethaicaMon X Bd Cond v3.2.ttf"))
            //        {
            //            var result = AddFontResource(@"DB HelvethaicaMon X Bd Cond v3.2.ttf");
            //            this.Controls.Clear();
            //            InitializeComponent();
            //        }                 
            //    }
            //    else
            //    {
            //        InitializeComponent();
            //    }

            //}
        }

        private void CloseAllLiveView()
        {
            for (int i = 0; i < MaxCamera; i++)
            {
                if (CameraControl[i] != null)
                {
                    if (CameraControl[i].MainCamera.IsLiveViewOn)
                    {
                        CameraControl[i].MainCamera.StopLiveView();
                    }
                }
            }
            IsLiveViewOn = false;
        }

        bool IsLiveViewOn = false;
        void LiveViewProc(int n)
        {
            if (n >= MaxCamera) return;
            for (int i = 0; i < MaxCamera; i++)
            {
                if (CameraControl[i] != null)
                {
                    CameraControl[i].MainCamera.LiveViewUpdated += MainCamera_LiveViewUpdated;
                }
            }

            if (!CameraControl[n].MainCamera.IsLiveViewOn)
            {
                for (int i = 0; i < MaxCamera; i++)
                {
                    if (CameraControl[i] != null)
                    {
                        if (CameraControl[i].MainCamera.IsLiveViewOn)
                        {
                            CameraControl[i].MainCamera.StopLiveView();
                        }
                    }
                }
                CameraControl[n].MainCamera.StartLiveView();
                IsLiveViewOn = true;
            }
            else
            {
                IsLiveViewOn = false;
                if (CameraControl[n] != null)
                {
                    CameraControl[n].MainCamera.StopLiveView();
                }
                MainCamera_LiveViewUpdated(null, null);
            }
        }

        private void btnLiveView_Click(object sender, EventArgs e)
        {
            Label btn = sender as Label;
            int n = Convert.ToInt16(btn.Text) - 1;
            if (CameraSlot[n] < MaxCamera)
            {
                if (CameraControl[CameraSlot[n]] != null)
                {
                    if (CameraControl[CameraSlot[n]].isOpen() == true) LiveViewProc(CameraSlot[n]);
                }
            }

        }
        private void btnImgBox_Click(object sender, EventArgs e)
        {
            PictureBox img = sender as PictureBox;
            try
            {
                int n = Convert.ToInt16(img.Tag) - 1;
                if (CameraSlot[n] < MaxCamera)
                {
                    if (CameraControl[CameraSlot[n]] != null)
                    {
                        if (CameraControl[CameraSlot[n]].isOpen() == true)
                        {
                            if (IsLiveViewOn == false)
                            {
                                if (img.Image != null)
                                {
                                    Image temp;
                                    using (var bmpTemp = new Bitmap(img.Image))
                                    {
                                        temp = new Bitmap(bmpTemp);
                                    }
                                    imgBox.Image = temp;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

        }
        private void MainCamera_LiveViewUpdated(Camera sender, Stream img)
        {
            try
            {
                Bitmap result = new Bitmap(imgBox.Width, imgBox.Height);
                Graphics g = Graphics.FromImage(result);
                //g.Flush();
                //    if (IsLiveViewOn)
                //    {
                Evf_Bmp = new Bitmap(img);
                g.DrawImage(Evf_Bmp, 0, 0, imgBox.Width, imgBox.Height);
                g.Flush();
                //        g.DrawImage(Mrk_Bmp, 0, 0, imgBox.Width, imgBox.Height);
                //        g.Flush();
                //    }
                imgBox.Image = result;
                Evf_Bmp.Dispose();
                //    imgBox.Invalidate();
            }
            catch { }
        }

        private void ClearTempFolder()
        {
            if (Directory.Exists(CameraSavePath))
            {
                try
                {
                    Directory.Delete(CameraSavePath, true);
                    Directory.CreateDirectory(CameraSavePath);
                }
                catch { }
            }
            if (Directory.Exists(TempPath))
            {
                try
                {
                    Directory.Delete(TempPath, true);
                    Directory.CreateDirectory(TempPath);
                }
                catch { }
            }
            if (Directory.Exists(RenderPath))
            {
                try
                {
                    Directory.Delete(RenderPath, true);
                    Directory.CreateDirectory(RenderPath);
                }
                catch { }
            }
            if (!Directory.Exists(BackupPath))
            {
                Directory.CreateDirectory(BackupPath);
            }
            if (!Directory.Exists(BackupVdoPath))
            {
                Directory.CreateDirectory(BackupVdoPath);
            }

        }
        private void FromMatrix3DPhoto_Load(object sender, EventArgs e)
        {
            LoadConfig();
            ClearTempFolder();
            Init();
            NetworkRemote_Init();
            InitPreview();
            InitCountDown();
            InitKeyboardMonitor();
        }

        private void InitKeyboardMonitor()
        {
            KeyboardListener.s_KeyEventHandler += KeyboardListener_s_KeyEventHandler;
        }

        private void KeyboardListener_s_KeyEventHandler(object sender, EventArgs e)
        {
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;
            //this.Text = string.Format("Key = {0}  Msg = {1}  Text = {2}", eventArgs.m_Key, eventArgs.m_Msg, eventArgs.KeyData);
            //SpaceBar Fire ===============================
            if (Properties.Settings.Default.SpaceBarFire == true) if (eventArgs.m_Key == 32 && eventArgs.m_Msg == 256) btnFire_Click(sender, e);

            if (Properties.Settings.Default.RigthClickFire == true)
            {
                if (eventArgs.m_Key == 27 && eventArgs.m_Msg == 256)
                {
                    if (FireEnable == false)
                    {
                        if (count_down > 0)
                        {
                            count_down = 6;
                            tCountDown.Enabled = false;
                            FinalFrame.Stop();
                            imgBox.Image = null;
                            FireEnable = true;
                        }
                    }
                }
            }
        }

        private void pg_update()
        {
            if (progressBar.Value < progressBar.Maximum) progressBar.Value++;// else progressBar.Value = 0;
        }
        private void pg_done()
        {
            progressBar.Value = 0;
        }
        private void Init()
        {


            if (!Directory.Exists(CameraSavePath)) Directory.CreateDirectory(CameraSavePath);

            if (Directory.Exists(TempPath)) Directory.Delete(TempPath, true);

            try
            {
                if (File.Exists(Properties.Settings.Default.LiveViewMarkerPath))
                {
                    Mrk_Bmp = new Bitmap(Properties.Settings.Default.LiveViewMarkerPath);
                }
            }
            catch { }

            try
            {
                if (File.Exists(Properties.Settings.Default.CountDownPath + "/1.png") &&
                    File.Exists(Properties.Settings.Default.CountDownPath + "/2.png") &&
                    File.Exists(Properties.Settings.Default.CountDownPath + "/3.png") &&
                    File.Exists(Properties.Settings.Default.CountDownPath + "/Action.png"))
                {
                    CountDown_3_Bmp = new Bitmap(Properties.Settings.Default.CountDownPath + "/3.png");
                    CountDown_2_Bmp = new Bitmap(Properties.Settings.Default.CountDownPath + "/2.png");
                    CountDown_1_Bmp = new Bitmap(Properties.Settings.Default.CountDownPath + "/1.png");
                    CountDown_Action_Bmp = new Bitmap(Properties.Settings.Default.CountDownPath + "/Action.png");
                }
            }
            catch { }

            CameraControl = new CanonSDK.MainForm[50];
            CameraStatus = new int[50];
            CameraSlot = new int[50];



            //Load SDK =======================================================
            int x = 0, y = 0;

            //for (int i = 0; i < MaxCamera; i++)
            //{

            //    CameraControl[i] = new CanonSDK.MainForm();
            //    CameraControl[i].SDKEnable = true;

            //    CameraControl[i].WindowState = FormWindowState.Minimized;
            //    CameraControl[i].Show();

            //    CameraControl[i].tcpPort = 20000 + i;
            //    CameraControl[i].SavePath = CameraSavePath + i.ToString() + @"\";
            //    Directory.CreateDirectory(CameraControl[i].SavePath);
            //    CameraControl[i].Left = x * (CameraControl[i].Width - 15);
            //    CameraControl[i].Top = y * (CameraControl[i].Height - 5);
            //    //CameraControl[i].Hide();

            //    if (Properties.Settings.Default.ServiceMode == true)
            //    {
            //        CameraControl[i].Show();
            //    }

            //    if (y == 2)
            //    {
            //        y = 0;
            //        x++;
            //    }
            //    else
            //    {
            //        y++;
            //    }
            //    CameraControl[i].CID = i;
            //    CameraControl[i].Text = "[" + i.ToString() + "] CANON INTERFACE";
            //}

            APIHandler = new CanonAPI();
            APIHandler.CameraAdded += APIHandler_CameraAdded;
            ErrorHandler.SevereErrorHappened += ErrorHandler_SevereErrorHappened;
            ErrorHandler.NonSevereErrorHappened += ErrorHandler_NonSevereErrorHappened;
            Invoke((Action)delegate
            {
                RefreshCamera();
            });

            //Hardware Connect ================================================
            try
            {
                serialPort1.Open();
            }
            catch { }

            if (MaxCamera >= 1) btnC1.Enabled = true;
            if (MaxCamera >= 2) btnC2.Enabled = true;
            if (MaxCamera >= 3) btnC3.Enabled = true;
            if (MaxCamera >= 4) btnC4.Enabled = true;
            if (MaxCamera >= 5) btnC5.Enabled = true;
            if (MaxCamera >= 6) btnC6.Enabled = true;
            if (MaxCamera >= 7) btnC7.Enabled = true;
            if (MaxCamera >= 8) btnC8.Enabled = true;
            if (MaxCamera >= 9) btnC9.Enabled = true;
            if (MaxCamera >= 10) btnC10.Enabled = true;


            tCameraConnect.Enabled = true;
            tCheckFile.Enabled = true;
            tUpdateStatus.Enabled = true;
            tSync.Enabled = true;

            for (int i = 0; i < MaxCamera; i++)
            {
                CameraStatus[i] = 0xFF;
                CameraSlot[i] = 0xFF;
            }
        }

        private void MainCamera_StateChanged(Camera sender, StateEventID eventID, int parameter)
        {
            //MessageBox.Show("x");
            Invoke((Action)delegate
            {

                if (eventID == StateEventID.Shutdown)
                {
                    Camera s = sender as Camera;

                    for (int x = 0; x < MaxCamera; x++)
                    {
                        if (CameraControl[x] != null)
                        {
                            if (CameraControl[x].MainCamera.ID == s.ID || CameraControl[x].Text == "[0][0]")
                            {
                                // MessageBox.Show(x.ToString());
                                CameraControl[x].Close();
                                CameraControl[x] = null;
                            }
                        }
                    }
                }
                RefreshCamera();
            });
        }
        private void APIHandler_CameraAdded(CanonAPI sender)
        {
            CameraConnectionChange = true;
            Invoke((Action)delegate
            {
                CameraConnectionChange = false;
                RefreshCamera();

            });
        }

        private void ErrorHandler_NonSevereErrorHappened(object sender, ErrorCode ex)
        {
            Invoke((Action)delegate
            {
                RefreshCamera();
            });
        }

        private void ErrorHandler_SevereErrorHappened(object sender, Exception ex)
        {
            Invoke((Action)delegate
            {
                RefreshCamera();
            });
        }
        async void RefreshCamera()
        {
            //CameraListBox.Items.Clear();
            CamList = APIHandler.GetCameraList();
            foreach (Camera cam in CamList)
                //CameraListBox.Items.Add(cam.ID);


                for (int i = 0; i < CamList.Count; i++)
                {
                    bool NewCamera = true;
                    for (int x = 0; x < MaxCamera; x++)
                    {
                        if (CameraControl[x] != null)
                        {
                            if (CameraControl[x].MainCamera?.SessionOpen == true)
                            {
                                if (CameraControl[x].MainCamera.ID == CamList[i].ID) NewCamera = false;
                            }
                        }
                    }
                    if (NewCamera)
                    {
                        //MessageBox.Show("New Camera");
                        for (int x = 0; x < MaxCamera; x++)
                        {
                            if (CameraControl[x] == null)
                            {
                                CameraControl[x] = new CanonSDK.MainForm();
                                CameraControl[x].SDKEnable = true;

                                CameraControl[x].WindowState = FormWindowState.Minimized;
                                CameraControl[x].Show();

                                CameraControl[x].tcpPort = 20000 + x;
                                CameraControl[x].SavePath = CameraSavePath + x.ToString() + @"\";
                                Directory.CreateDirectory(CameraControl[x].SavePath);


                                if (Properties.Settings.Default.ServiceMode == true)
                                {
                                    CameraControl[x].Show();
                                }
                                else CameraControl[x].Hide();
                            }
                            //else MessageBox.Show("xxxx");
                            if (CameraControl[x] != null)
                            {
                                if (CameraControl[x].MainCamera?.SessionOpen != true)
                                {
                                    //MessageBox.Show(CamList[i].ID + " Connect to Slot " + x.ToString());
                                    await Task.Delay(100);
                                    CameraControl[x].OpenSession(CamList[i]);
                                    CameraControl[x].MainCamera.StateChanged += MainCamera_StateChanged;
                                    break;
                                }
                            }


                        }
                    }
                }

        }

        public bool IsFileLocked(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (File.Open(filePath, FileMode.Open)) { }
                }
                catch (IOException e)
                {
                    var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                    return errorCode == 32 || errorCode == 33;
                }
            }
            return false;
        }
        public string GetFileName(string path)
        {
            path += @"\";
            if (Directory.Exists(path))
            {
                string[] FileList = Directory.GetFiles(path, "*.jpg");
                if (FileList.Length >= 1)
                {
                    return FileList[0];//.Replace(path, string.Empty).Remove(0, 2).Replace(".JPG", string.Empty).Replace(".jpg", string.Empty).Replace(@"\", string.Empty);
                }
                else
                    return "";
            }
            else
            {
                return "";
            }
        }

        public bool CheckFolderExite(string path)
        {
            if (Directory.Exists(path)) return true;
            return false;
        }
        public bool CheckFileExite(string path)
        {
            if (File.Exists(path)) return true;
            return false;
        }
        private void LoadConfig()
        {
            //coSFXTime.Items.Clear();
            //coSFXTime.Items.Add("     Freeze");
            //for (int i = 1; i <= 1000; i++)
            //{
            //    string str = "     " + i.ToString() + " ms";
            //    coSFXTime.Items.Add(str);
            //}
            //coSFXTime.SelectedIndex = 0;
            ////coSFXTime.SelectedIndex = Properties.Settings.Default.SFXTime;

            //coLength.Items.Clear();
            //for (int i = 1; i <= 5; i++)
            //{
            //    string str = "     " + i.ToString() + " sec";
            //    coLength.Items.Add(str);
            //}
            //coLength.SelectedIndex = 1;
            ////coLength.SelectedIndex = Properties.Settings.Default.Length;

            //coLoop.Items.Clear();
            //for (int i = 1; i <= 5; i++)
            //{
            //    string str = "     " + i.ToString() + " round";
            //    coLoop.Items.Add(str);
            //}
            //coLoop.SelectedIndex = 2;
            ////coLoop.SelectedIndex = Properties.Settings.Default.Loop;

            //coRenderType.Items.Clear();
            //coRenderType.Items.Add("     MP4");
            //coRenderType.Items.Add("     GIF");
            //coRenderType.SelectedIndex = 0;
            ////coRenderType.SelectedIndex = Properties.Settings.Default.Type;

            //coRenderResolution.Items.Clear();
            //coRenderResolution.Items.Add("     3:2  [RAW]");
            //coRenderResolution.Items.Add("     16:9 [1080 H]");
            //coRenderResolution.Items.Add("     16:9 [720 H]");
            //coRenderResolution.Items.Add("     16:9 [1080 V]");
            //coRenderResolution.Items.Add("     16:9 [720 V]");
            //coRenderResolution.Items.Add("     1:1 [1080]");
            //coRenderResolution.Items.Add("     1:1 [720]");
            //coRenderResolution.SelectedIndex = 1;
            ////coRenderResolution.SelectedIndex = Properties.Settings.Default.Resolution;  
            ///
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            softversion.Text = "Version :" + version;

        }

        private void SaveConfig(object sender, EventArgs e)
        {
            //Properties.Settings.Default.SFXTime = coSFXTime.SelectedIndex + 1;
            //Properties.Settings.Default.Length = coLength.SelectedIndex + 1 ;
            //Properties.Settings.Default.Loop = coLoop.SelectedIndex + 1;
            //Properties.Settings.Default.Type = coRenderType.SelectedIndex;
            //Properties.Settings.Default.Resolution = coRenderResolution.SelectedIndex;
            //Properties.Settings.Default.Save();
        }

        #region Network Remote

        public Thread thdUDPServer;
        public UdpClient udpServer;
        byte data_recv = 0xFF;

        public void serverThread()
        {
            udpServer = new UdpClient(16000);
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpServer.Receive(ref RemoteIpEndPoint);
                data_recv = receiveBytes[0];
                //MessageBox.Show(data_recv.ToString());
                //string returnData = Encoding.ASCII.GetString(receiveBytes);
                // lbConnections.Items.Add(RemoteIpEndPoint.Address.ToString() + ":" + returnData.ToString());

                if (data_recv == 80)
                {
                    if (Properties.Settings.Default.Setting_mode == 1)
                    {
                        //MessageBox.Show("USB_MODE");
                        Fire();
                        if (serialPort1.IsOpen) serialPort1.Write("{Fire:1}");
                        //Connecting=====================
                        //for (int k = 0; k < MaxCamera; k++)
                        //{
                        //    if (CameraControl[k].isOpen() == true)
                        //    {
                        //        Invoke((Action)delegate
                        //        {
                        //            CameraControl[k].MainCamera.TakePhotoShutterAsync();
                        //        });
                        //        break;
                        //    }
                        //}
                        data_recv = 0xFF;
                    }
                    else
                    {
                        btnFire_Click(null, null);
                    }
                }
            }

        }

        public void Serial_mode_Take()
        {
            string com_port = Properties.Settings.Default.COM_Ports;
            SerialPort SerialSend = new SerialPort(com_port, 115200, Parity.None, 8, StopBits.One);
            SerialSend.Open();
            SerialSend.Write("T");
            SerialSend.Close();
            for (int k = 0; k < MaxCamera; k++)
            {
                try
                {
                    CameraStatus[CameraSlot[k]] = 1;
                }
                catch
                {
                    CameraStatus[CameraSlot[k]] = 0;
                }
            }
        }

        string camera_list_number = "";
        string time_sleep_customs = "";
        int shutter_mode;
        int time_sleep;
        string time_delay_custorm;
        public void BT_mode()
        {

            //coSFXtype.Items.Add("Freeze");
            //coSFXtype.Items.Add("Forward");
            //coSFXtype.Items.Add("Reward");
            //coSFXtype.Items.Add("Random");
            //coSFXtype.Items.Add("Custom");
            
            switch (Properties.Settings.Default.SFXType)
            {
                case 0:
                    Console.WriteLine("Freeze");
                    camera_list_number = "";
                    shutter_mode = 0;
                    time_sleep = Properties.Settings.Default.SFXTime;
                    break;
                case 1:
                    Console.WriteLine("Forward");
                    camera_list_number = "1,2,3,4,5";
                    shutter_mode = 1;
                    time_sleep = Properties.Settings.Default.SFXTime;
                    break;
                case 2:
                    Console.WriteLine("Reward");
                    camera_list_number = "5,4,3,2,1";
                    shutter_mode = 1;
                    time_sleep = Properties.Settings.Default.SFXTime;
                    break;
                case 3:
                    List<string> deck = new List<string>()
                    {
                        "1","2","3","4","5"
                    };
                    Random rand = new Random();
                    deck = deck.Select(x => new { card = x, rand = rand.Next() }).OrderBy(x => x.rand).Select(x => x.card).ToList();
                    camera_list_number = string.Join(",", deck);
                    shutter_mode = 1;
                    time_sleep = Properties.Settings.Default.SFXTime;
                    break;
                case 4:
                    time_delay_custorm += Properties.Settings.Default.Camera1_interval.ToString() + ",";
                    time_delay_custorm += Properties.Settings.Default.Camera2_interval.ToString() + ",";
                    time_delay_custorm += Properties.Settings.Default.Camera3_interval.ToString() + ",";
                    time_delay_custorm += Properties.Settings.Default.Camera4_interval.ToString() + ",";
                    time_delay_custorm += Properties.Settings.Default.Camera5_interval.ToString();
                    shutter_mode = 2;
                    Console.WriteLine("Customs");
                    camera_list_number = "1,2,3,4,5";
                    time_sleep_customs = time_delay_custorm;
                    break;
            }

            var data_raw = new
            {
                cameralists = camera_list_number,
                mode = shutter_mode,
                delay_time = time_sleep,
                delay_time_customs = time_sleep_customs
            };

            string com_port = Properties.Settings.Default.COM_Ports;
            var json = JsonConvert.SerializeObject(data_raw);
            SerialPort BTSend = new SerialPort(com_port, 115200, Parity.None, 8, StopBits.One);
            BTSend.Open();
            BTSend.Write(json);
            BTSend.Close();
            time_delay_custorm = "";
            for (int k = 0; k < MaxCamera; k++)
            {
                try
                {
                    CameraStatus[CameraSlot[k]] = 1;
                }
                catch
                {
                    CameraStatus[CameraSlot[k]] = 0;
                }
            }
        }

        public void NetworkRemote_Init()
        {
            thdUDPServer = new Thread(new ThreadStart(serverThread));
            thdUDPServer.Start();
        }

        public void NetworkRemote_Dispose()
        {
            thdUDPServer.Abort();
            udpServer.Close();
        }

        #endregion

        private void FromMatrix3DPhoto_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            //RunThread = false;
            //this.TopMost = true;
            try { CloseAllLiveView(); } catch { }
            try { FinalFrame.Stop(); } catch { }

            tCameraConnect.Enabled = false;
            tUpdateStatus.Enabled = false;
            tSync.Enabled = false;

            try { serialPort1.Dispose(); } catch { }
            try { NetworkRemote_Dispose(); } catch { }
            try { APIHandler.Dispose(); } catch { }

            for (int i = 0; i < MaxCamera; i++)
            {
                try
                {
                    if (CameraControl[i] != null)
                    {
                        CameraControl[i].Show();
                        CameraControl[i].Height = 0;
                        CameraControl[i].Width = 0;
                        CameraControl[i].Close();
                        CameraControl[i].Dispose();
                    }
                }
                catch { }
            }

            if (!formClosed)
            {
                this.formClosed = true;
                this.Close();
            }

            Application.Exit();
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Setting_mode == 1)
            {
                CameraConnectionChange = false;
                RefreshCamera();
                if (FireEnable)
                {
                    CloseAllLiveView();
                    FireEnable = false;
                    btnFire.Image = Properties.Resources.RECORD_BUTTOM_PUSHED;

                    if (Properties.Settings.Default.CountDownEnable == false && Properties.Settings.Default.PreviewBeforeFire == false)
                    {
                        Fire();
                    }
                    else ShowCountDown();
                }
                else
                {
                    if (count_down >= 0 && count_down <= 5)
                    {
                        count_down = 0;
                        Fire();
                    }
                }
            }
            else if(Properties.Settings.Default.Setting_mode == 2)
            {
                Serial_mode_Take();
            }
            else if (Properties.Settings.Default.Setting_mode == 3)
            {
                BT_mode();
            }
        }

        private void tCameraConnect_Tick(object sender, EventArgs e)
        {
            //if (CameraConnectionChange)
            //{
            //    //AutoConnect();

            //    CameraConnectionChange = false;
            //    RefreshCamera();

            //    for(int i=0;i<CameraListBox.Items.Count;i++)
            //    {
            //        bool NewCamera = true;
            //        for(int x=0;x<MaxCamera;x++)
            //        {
            //            if(CameraControl[x].MainCamera.SessionOpen==true)
            //            {
            //                if (CameraControl[x].MainCamera.ID == CamList[i].ID) NewCamera = false;
            //            }
            //        }
            //        if(NewCamera)
            //        {
            //            for (int x = 0; x < MaxCamera; x++)
            //            {
            //                if (CameraControl[x].MainCamera.SessionOpen == false)
            //                {
            //                    CameraControl[x].OpenSession(CamList[i]);
            //                }
            //            }
            //        }
            //    }
            //}

            //for (int i = 0; i < MaxCamera; i++)
            //{
            //    string path = GetFileName(CameraControl[i].SavePath);
            //    if (path != "" && !IsFileLocked(path))
            //    {
            //        if (!Directory.Exists(CameraSavePath)) Directory.CreateDirectory(CameraSavePath);
            //        string DestPath = CameraSavePath  + CameraControl[i].lbNo.Text + ".jpg";
            //        if (!File.Exists(DestPath))
            //        {
            //            File.Copy(path, DestPath, true);
            //            File.Delete(path);
            //        }
            //    }
            //}
        }
        //public void AutoConnect2()
        //{
        //    CamList = APIHandler.GetCameraList();
        //    for (int i = 0; i < CamList.Count; i++)
        //    {
        //        bool FindNewCamera = true;
        //        //Check New Camera
        //        for (int j = 0; j < MaxCamera; j++)
        //        {
        //            if (CameraControl[j].isOpen() == true)
        //            {
        //                if (CamList[i].ID == CameraControl[j].MainCamera.ID)
        //                {
        //                    FindNewCamera = false;
        //                    break;
        //                }
        //            }
        //            if (FindNewCamera)
        //            {
        //                //Connecting=====================
        //                for (int k = 0; k < MaxCamera; k++)
        //                {
        //                    if (CameraControl[k].isOpen() == false)
        //                    {
        //                        Invoke((Action)delegate
        //                        {
        //                            CameraControl[k].RefreshCamera();
        //                            CameraControl[k].CameraListBox.SelectedIndex = i;
        //                            CameraControl[k].OpenSession();
        //                            FindNewCamera = false;
        //                        });
        //                        //return;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        bool CameraAllReady = false;
        bool LastCameraAllReady = false;
        private void tUpdateStatus_Tick(object sender, EventArgs e)
        {

            //bool HaveCamera = false;
            //Camera Status ================================
            for (int i = 0; i < MaxCamera; i++)
            {
                CameraStatus[i] = 0xFF;
                CameraSlot[i] = 0xFF;
            }

            for (int i = 0; i < MaxCamera; i++)
            {
                if (CameraControl[i] != null)
                {
                    if (CameraControl[i].isOpen())
                    {
                        int id = 0;
                        try
                        {
                            id = Convert.ToInt16(CameraControl[i].CameraID) - 1;
                        }
                        catch
                        {
                            id = 0;
                        }

                        if (id < MaxCamera)
                        {
                            if (CameraStatus[id] != 100)
                            {
                                CameraStatus[id] = 1;
                                if (CameraControl[i].MainCamera.IsLiveViewOn) CameraStatus[id] = 2;
                            }
                            CameraSlot[id] = i;
                        }
                    }
                }
            }
            int CameraReady = 0;
            for (int i = 0; i < MaxCamera; i++)
            {
                Color cc = Color.FromArgb(64, 64, 64);
                if (CameraStatus[i] == 1) cc = Color.GreenYellow;
                else if (CameraStatus[i] == 2) cc = Color.AliceBlue;
                else if (CameraStatus[i] == 100) cc = Color.Red;

                if (CameraStatus[i] >= 1 && CameraStatus[i] < 255) CameraReady++;

                switch (i)
                {
                    case 0: btnC1.BackColor = cc; break;
                    case 1: btnC2.BackColor = cc; break;
                    case 2: btnC3.BackColor = cc; break;
                    case 3: btnC4.BackColor = cc; break;
                    case 4: btnC5.BackColor = cc; break;
                    case 5: btnC6.BackColor = cc; break;
                    case 6: btnC7.BackColor = cc; break;
                    case 7: btnC8.BackColor = cc; break;
                    case 8: btnC9.BackColor = cc; break;
                    case 9: btnC10.BackColor = cc; break;
                }

            }
            if (CameraReady == MaxCamera) CameraAllReady = true; else CameraAllReady = false;

            if (CameraAllReady != LastCameraAllReady)
            {
                LastCameraAllReady = CameraAllReady;

                if (CameraAllReady)
                {
                    FireEnable = true;
                    if (isFirstTime)
                    {
                        isFirstTime = false;
                        for (int i = 0; i < MaxCamera; i++)
                        {
                            if (CameraControl[i] != null)
                            {
                                if (Convert.ToInt16(CameraControl[i].CameraID) == Convert.ToInt16(Properties.Settings.Default.MasterConfig))
                                {
                                    CameraControl[i].QaCoBox.SelectedIndex = 4;
                                    PropertiesChange = true;
                                }
                            }
                        }

                        if (File.Exists(Properties.Settings.Default.PromoVdoPath))
                        {
                            pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
                            pv.ax.Ctlcontrols.play();
                        }
                    }
                }
                else FireEnable = false;
            }

            if (FireEnable)
            {
                btnFire.Image = Properties.Resources.RECORD_BUTTOM;
            }
            else
            {
                if (Properties.Settings.Default.PreviewBeforeFire == true && Properties.Settings.Default.CountDownEnable == false)
                {
                    if (count_down == 4) btnFire.Image = Properties.Resources.RECORD_BUTTOM;
                    else btnFire.Image = Properties.Resources.RECORD_BUTTOM_PUSHED;
                }
            }



            //UDP Shutter ==========================
            if (data_recv != 0xFF)
            {
                tUpdateStatus.Enabled = false;
                //MessageBox.Show(data_recv.ToString());
                if (data_recv == 80)
                {
                    //btnFire_Click(null, null);
                    if (serialPort1.IsOpen) serialPort1.Write("{Fire:1}");
                    //Connecting=====================
                    //for (int k = 0; k < MaxCamera; k++)
                    //{
                    //    if (CameraControl[k] != null)
                    //    {
                    //        if (CameraControl[k].isOpen() == true)
                    //        {
                    //            Invoke((Action)delegate
                    //            {
                    //                CameraControl[k].MainCamera.TakePhotoShutterAsync();
                    //            });
                    //            break;
                    //        }
                    //    }
                    //}
                    //MessageBox.Show(data_recv.ToString());
                    //if (Connected)
                    //{
                    //    LLC_Shutter();
                    //}
                    //    }
                }
                data_recv = 0xFF;
                tUpdateStatus.Enabled = true;
            }
        }

        int MasterSettingSlot = -1;
        bool UpdateValue = false;
        bool isFirstTime = true;
        bool LastSyncReady = false;
        bool PropertiesChange = false;
        bool SyncDelay = false;
        private void tSync_Tick(object sender, EventArgs e)
        {
            bool SyncReady = false;
            int x = 0;

            for (int i = 0; i < MaxCamera; i++)
            {
                if (CameraControl[i] != null)
                {
                    if (CameraControl[i].isOpen()) x++;
                }
            }
            if (x == MaxCamera)
            {
                SyncReady = true;
            }
            else
            {
                SyncReady = false;
                LastSyncReady = false;
            }

            if (SyncReady != LastSyncReady && SyncReady == true)
            {
                LastSyncReady = SyncReady;
                for (int i = 0; i < MaxCamera; i++)
                {
                    if (CameraControl[i] != null)
                    {
                        if (Convert.ToInt16(CameraControl[i].CameraID) == Convert.ToInt16(Properties.Settings.Default.MasterConfig))
                        {
                            CameraControl[i].MainCamera.PropertyChanged += MainCamera_PropertyChanged;
                        }
                    }
                }
            }

            if (SyncDelay == false && PropertiesChange == true)
            {
                SyncDelay = true;
                tSyncDelay.Enabled = true;
                PropertiesChange = false;

                // Camera Status ================================
                for (int i = 0; i < MaxCamera; i++)
                {
                    CameraStatus[i] = 0xFF;
                    CameraSlot[i] = 0xFF;
                }

                for (int i = 0; i < MaxCamera; i++)
                {
                    if (CameraControl[i] != null)
                    {
                        if (CameraControl[i].isOpen() == true)
                        {
                            /*if (Convert.ToInt16(CameraControl[i].CameraID) == Convert.ToInt16(Properties.Settings.Default.MasterConfig))
                            {

                                CameraControl[i].LoadValue();
                                for (int k = 0; k < MaxCamera; k++)
                                {
                                    if (k != i)
                                    {
                                        if (CameraControl[k] != null)
                                        {
                                            if (CameraControl[k].isOpen() == true)
                                            {
                                                CameraControl[k].ISOCoBox.SelectedIndex = CameraControl[i].ISOCoBox.SelectedIndex;
                                                CameraControl[k].AeCoBox.SelectedIndex = CameraControl[i].AeCoBox.SelectedIndex;
                                                CameraControl[k].AvCoBox.SelectedIndex = CameraControl[i].AvCoBox.SelectedIndex;
                                                CameraControl[k].TvCoBox.SelectedIndex = CameraControl[i].TvCoBox.SelectedIndex;
                                                CameraControl[k].WBCoBox.SelectedIndex = CameraControl[i].WBCoBox.SelectedIndex;
                                                CameraControl[k].MeCoBox.SelectedIndex = CameraControl[i].MeCoBox.SelectedIndex;
                                                CameraControl[k].PsCoBox.SelectedIndex = CameraControl[i].PsCoBox.SelectedIndex;
                                                CameraControl[k].QaCoBox.SelectedIndex = CameraControl[i].QaCoBox.SelectedIndex;
                                                if (CameraControl[i].WBCoBox.SelectedIndex == 9)
                                                {
                                                    CameraControl[k].txtColorTemp.Text = CameraControl[i].txtColorTemp.Text;
                                                    CameraControl[k].SetColorTemp(CameraControl[k].txtColorTemp.Text);
                                                }
                                            }
                                        }
                                    }
                                }
                            }*/

                            if (Convert.ToInt16(CameraControl[i].CameraID) == Convert.ToInt16(Properties.Settings.Default.MasterConfig))
                            {
                                CameraControl[i].LoadValue();
                                //LoadMainSettingList from MasterConfig ====================
                                AvCoBox.Items.Clear();//Av
                                for (int j = 0; j < CameraControl[i].AvCoBox.Items.Count; j++)
                                {
                                    AvCoBox.Items.Add(CameraControl[i].AvCoBox.Items[j]);
                                }
                                AeCoBox.Items.Clear();//Ae
                                for (int j = 0; j < CameraControl[i].AeCoBox.Items.Count; j++)
                                {
                                    AeCoBox.Items.Add(CameraControl[i].AeCoBox.Items[j]);
                                }
                                ISOCoBox.Items.Clear();//iso
                                for (int j = 0; j < CameraControl[i].ISOCoBox.Items.Count; j++)
                                {
                                    ISOCoBox.Items.Add(CameraControl[i].ISOCoBox.Items[j]);
                                }
                                TvCoBox.Items.Clear();//tv
                                for (int j = 0; j < CameraControl[i].TvCoBox.Items.Count; j++)
                                {
                                    TvCoBox.Items.Add(CameraControl[i].TvCoBox.Items[j]);
                                }
                                QaCoBox.Items.Clear();//qa
                                for (int j = 0; j < CameraControl[i].QaCoBox.Items.Count; j++)
                                {
                                    QaCoBox.Items.Add(CameraControl[i].QaCoBox.Items[j]);
                                }
                                WBCoBox.Items.Clear();//wb
                                for (int j = 0; j < CameraControl[i].WBCoBox.Items.Count; j++)
                                {
                                    WBCoBox.Items.Add(CameraControl[i].WBCoBox.Items[j]);
                                }
                                MeCoBox.Items.Clear();//me
                                for (int j = 0; j < CameraControl[i].MeCoBox.Items.Count; j++)
                                {
                                    MeCoBox.Items.Add(CameraControl[i].MeCoBox.Items[j]);
                                }
                                AvCoBox.Enabled = true;
                                AeCoBox.Enabled = true;
                                ISOCoBox.Enabled = true;
                                TvCoBox.Enabled = true;
                                QaCoBox.Enabled = true;
                                WBCoBox.Enabled = true;
                                MeCoBox.Enabled = true;

                                MasterSettingSlot = i;
                                UpdateValue = true;
                                AvCoBox.SelectedIndex = CameraControl[i].AvCoBox.SelectedIndex;
                                AeCoBox.SelectedIndex = CameraControl[i].AeCoBox.SelectedIndex;
                                ISOCoBox.SelectedIndex = CameraControl[i].ISOCoBox.SelectedIndex;
                                TvCoBox.SelectedIndex = CameraControl[i].TvCoBox.SelectedIndex;
                                QaCoBox.SelectedIndex = CameraControl[i].QaCoBox.SelectedIndex;
                                WBCoBox.SelectedIndex = CameraControl[i].WBCoBox.SelectedIndex;
                                MeCoBox.SelectedIndex = CameraControl[i].MeCoBox.SelectedIndex;
                                UpdateValue = false;
                                //==========================================================
                                for (int k = 0; k < MaxCamera; k++)
                                {
                                    if (k != i)
                                    {
                                        if (CameraControl[k] != null)
                                        {
                                            if (CameraControl[k].isOpen() == true)
                                            {
                                                CameraControl[k].ISOCoBox.SelectedIndex = CameraControl[i].ISOCoBox.SelectedIndex;
                                                CameraControl[k].AeCoBox.SelectedIndex = CameraControl[i].AeCoBox.SelectedIndex;
                                                CameraControl[k].AvCoBox.SelectedIndex = CameraControl[i].AvCoBox.SelectedIndex;
                                                CameraControl[k].TvCoBox.SelectedIndex = CameraControl[i].TvCoBox.SelectedIndex;
                                                CameraControl[k].WBCoBox.SelectedIndex = CameraControl[i].WBCoBox.SelectedIndex;
                                                CameraControl[k].MeCoBox.SelectedIndex = CameraControl[i].MeCoBox.SelectedIndex;
                                                CameraControl[k].PsCoBox.SelectedIndex = CameraControl[i].PsCoBox.SelectedIndex;
                                                CameraControl[k].QaCoBox.SelectedIndex = CameraControl[i].QaCoBox.SelectedIndex;
                                                if (CameraControl[i].WBCoBox.SelectedIndex == 9)
                                                {
                                                    CameraControl[k].txtColorTemp.Text = CameraControl[i].txtColorTemp.Text;
                                                    CameraControl[k].SetColorTemp(CameraControl[k].txtColorTemp.Text);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MainCamera_PropertyChanged(Camera sender, PropertyEventID eventID, PropertyID propID, int parameter)
        {
            PropertiesChange = true;
        }

        private void tSyncDelay_Tick(object sender, EventArgs e)
        {
            tSyncDelay.Enabled = false;
            SyncDelay = false;
        }

        string DT;
        private void tCheckFile_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < MaxCamera; i++)
            {
                if (CameraControl[i] != null)
                {
                    string path = GetFileName(CameraControl[i].SavePath);
                    if (path != "" && !IsFileLocked(path))
                    {
                        if (!Directory.Exists(CameraSavePath)) Directory.CreateDirectory(CameraSavePath);
                        string DestPath = CameraSavePath + CameraControl[i].lbNo.Text + ".jpg";
                        if (!File.Exists(DestPath))
                        {
                            File.Copy(path, DestPath, true);
                            File.Delete(path);
                        }
                    }
                }
            }

            //if (Render_Progress) pg_update();
            if (!Directory.Exists(FilePath)) Directory.CreateDirectory(FilePath);

            string[] FileList = Directory.GetFiles(FilePath, "*.jpg");


            for (int i = 1; i <= MaxCamera; i++)
            {
                string imgpath = FilePath + "/" + i.ToString("00") + ".jpg";

                if (!IsFileLocked(imgpath))
                {
                    switch (i)
                    {
                        case 1:
                            if (imgBox1.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox1.Image = img; }
                            break;
                        case 2:
                            if (imgBox2.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox2.Image = img; }
                            break;
                        case 3:
                            if (imgBox3.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox3.Image = img; }
                            break;
                        case 4:
                            if (imgBox4.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox4.Image = img; }
                            break;
                        case 5:
                            if (imgBox5.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox5.Image = img; }
                            break;
                        case 6:
                            if (imgBox6.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox6.Image = img; }
                            break;
                        case 7:
                            if (imgBox7.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox7.Image = img; }
                            break;
                        case 8:
                            if (imgBox8.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox8.Image = img; }
                            break;
                        case 9:
                            if (imgBox9.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox9.Image = img; }
                            break;
                        case 10:
                            if (imgBox10.Image == null && File.Exists(imgpath)) { Image img; using (var bmpTemp = new Bitmap(imgpath)) { img = new Bitmap(bmpTemp, 900, 600); } imgBox10.Image = img; }
                            break;
                    }
                }
            }

            if (FileList.Length >= 1 && FileList.Length < TotalFile)
            {
                //lbTimeout.Text = CheckFile_Timeout.ToString();
                pg_update();
                if (CheckFile_Timeout++ >= 2500)
                {
                    FireEnable = true;
                    pg_done();
                    CheckFile_Timeout = 0;

                    for (int i = 0; i < MaxCamera; i++)
                    {
                        CameraStatus[i] = 100;
                    }
                    for (int i = 0; i < TotalFile; i++)
                    {
                        string path = FilePath + (i + 1).ToString("00") + ".jpg";
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            CameraStatus[i] = 1;
                        }
                    }
                    //for (int i = 0; i < MaxCamera; i++)
                    //{
                    //    if (CameraStatus[i] == 100) MessageBox.Show("Camera Error " + i.ToString());
                    //}

                    imgBox1.Image = null;
                    imgBox2.Image = null;
                    imgBox3.Image = null;
                    imgBox4.Image = null;
                    imgBox5.Image = null;
                    imgBox6.Image = null;
                    imgBox7.Image = null;
                    imgBox8.Image = null;
                    imgBox9.Image = null;
                    imgBox10.Image = null;
                    NewFire = false;
                }
            }
            if (FileList.Length >= 1 && NewFire == true)
            {
                imgBox1.Image = null;
                imgBox2.Image = null;
                imgBox3.Image = null;
                imgBox4.Image = null;
                imgBox5.Image = null;
                imgBox6.Image = null;
                imgBox7.Image = null;
                imgBox8.Image = null;
                imgBox9.Image = null;
                imgBox10.Image = null;
                NewFire = false;
            }
            if (FileList.Length >= TotalFile)
            {
                //Show Center Cam =======================
                int master_cam = Properties.Settings.Default.MasterConfig;
                if (master_cam > 0 && master_cam <= MaxCamera)
                {
                    switch (master_cam)
                    {
                        case 1:
                            if (imgBox1.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox1.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 2:
                            if (imgBox2.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox2.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 3:
                            if (imgBox3.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox3.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 4:
                            if (imgBox4.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox4.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 5:
                            if (imgBox1.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox5.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 6:
                            if (imgBox6.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox6.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 7:
                            if (imgBox7.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox7.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 8:
                            if (imgBox8.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox8.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 9:
                            if (imgBox9.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox9.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                        case 10:
                            if (imgBox10.Image != null)
                            {
                                Image temp; using (var bmpTemp = new Bitmap(imgBox10.Image))
                                {
                                    temp = new Bitmap(bmpTemp);
                                }
                                imgBox.Image = temp;
                            }
                            break;
                    }
                }
                pg_update();
                //Create File Name
                DT = DateTime.Now.ToString("MMddHHmmss");
                int x = 0;
                for (int i = 0; i < TotalFile; i++)
                {
                    string imgpath = FilePath + (i + 1).ToString("00") + ".jpg";
                    if (!IsFileLocked(imgpath))
                    {
                        x++;
                        pg_update();
                    }
                }
                if (x >= TotalFile)
                {
                    string FolderName = DT + @"/";

                    try
                    {
                        if (Directory.Exists(RenderPath))
                        {
                            Directory.Delete(RenderPath, true);
                            File.Delete(CameraSavePath + "/temp_vdo_forward");
                            pg_update();
                        }
                    }
                    catch
                    { }

                    string InputPath = Properties.Settings.Default.InputPath + "/";
                    if (Directory.Exists(InputPath)) Directory.Delete(InputPath, true);
                    if (!Directory.Exists(InputPath)) Directory.CreateDirectory(InputPath);

                    if (!Directory.Exists(RenderPath)) Directory.CreateDirectory(RenderPath);
                    if (!Directory.Exists(RenderPath + "/MASK/")) Directory.CreateDirectory(RenderPath + "/MASK/");
                    if (!Directory.Exists(BackupVdoPath)) Directory.CreateDirectory(BackupVdoPath);
                    if (!Directory.Exists(BackupPath + FolderName)) Directory.CreateDirectory(BackupPath + FolderName);

                    for (int i = 0; i < TotalFile; i++)
                    {
                        File.Copy(FilePath + (i + 1).ToString("00") + ".jpg", InputPath + (i + 1).ToString("00") + ".jpg", true);
                        File.Copy(FilePath + (i + 1).ToString("00") + ".jpg", RenderPath + (i + 1).ToString("00") + ".jpg", true);
                        File.Copy(FilePath + (i + 1).ToString("00") + ".jpg", RenderPath + (((TotalFile * 2) - 1) - i).ToString("00") + ".jpg", true);
                        File.Copy(FilePath + (i + 1).ToString("00") + ".jpg", BackupPath + FolderName + (i + 1).ToString("00") + ".jpg", true);
                        File.Delete(FilePath + (i + 1).ToString("00") + ".jpg");
                        pg_update();
                    }
                    pg_done();
                    NewFire = true;

                    //Render ==========================================
                    //MP4
                    if (Properties.Settings.Default.Type >= 0)
                    {
                        if (Properties.Settings.Default.Length < 0) return;
                        if (Properties.Settings.Default.Loop < 0) return;
                        if (Properties.Settings.Default.Type == 1) render_gif = true; else render_gif = false;

                        pg_update();
                        //Render_Progress = true;

                        int Length = Properties.Settings.Default.Length;
                        int Loop = Properties.Settings.Default.Loop;

                        /*
                            coRenderResolution.Items.Clear();
                            coRenderResolution.Items.Add("     3:2  [RAW]");
                            coRenderResolution.Items.Add("     16:9 [1080 H]");
                            coRenderResolution.Items.Add("     16:9 [720 H]");
                            coRenderResolution.Items.Add("     16:9 [1080 V]");
                            coRenderResolution.Items.Add("     16:9 [720 V]");
                            coRenderResolution.Items.Add("     1:1 [1080]");
                            coRenderResolution.Items.Add("     1:1 [720]");
                            */

                        switch (Properties.Settings.Default.Resolution)
                        {
                            case 0: render_resolution = "1920:1200"; sqr_crop = false; render_ver = false; break;
                            case 1: render_resolution = "1920:1080"; sqr_crop = false; render_ver = false; break;
                            case 2: render_resolution = "1280:720"; sqr_crop = false; render_ver = false; break;
                            case 3: render_resolution = "1920:1080"; sqr_crop = false; render_ver = true; break;
                            case 4: render_resolution = "1280:720"; sqr_crop = false; render_ver = true; break;
                            case 5: render_resolution = "1080:1080"; sqr_crop = true; render_ver = false; break;
                            case 6: render_resolution = "720:720"; sqr_crop = true; render_ver = false; break;
                        }
                        string watermark_path = Properties.Settings.Default.WaterMarkPath;
                        if (File.Exists(watermark_path))
                        {
                            pg_update();
                            ImageProcessing(DllPath, RenderPath, TotalFile, BackupVdoPath + "/" + DT + ".mp4", Properties.Settings.Default.TitleVdoPath, Properties.Settings.Default.EndVdoPath, watermark_path, Length.ToString(), Loop, "1920:1200", Properties.Settings.Default.FilterSoundPath);
                            //ImageProcessing(DllPath, RenderPath, TotalFile, BackupVdoPath + "/" + DT + "/" + DT + ".mp4", Properties.Settings.Default.TitleVdoPath, Properties.Settings.Default.EndVdoPath, watermark_path, Length.ToString(), Loop, "1920:1200", Properties.Settings.Default.FilterSoundPath);

                        }
                        else
                        {
                            pg_update();
                            ImageProcessing(DllPath, RenderPath, TotalFile, BackupVdoPath + "/" + DT + ".mp4", Properties.Settings.Default.TitleVdoPath, Properties.Settings.Default.EndVdoPath, "", Length.ToString(), Loop, "1920:1200", Properties.Settings.Default.FilterSoundPath);
                            //ImageProcessing(DllPath, RenderPath, TotalFile, BackupVdoPath + "/" + DT + "/" + DT + ".mp4", Properties.Settings.Default.TitleVdoPath, Properties.Settings.Default.EndVdoPath, "", Length.ToString(), Loop, "1920:1200", Properties.Settings.Default.FilterSoundPath);

                        }
                    }
                }
            }
        }

        public int CheckFileCount(string path, string ext)
        {
            if (CheckFolderExite(path))
            {
                return Directory.GetFiles(path, ext, SearchOption.TopDirectoryOnly).Length;
            }
            return 0;
        }
        string render_dllpath;
        string render_sourcepath;
        int render_totalframe;
        string render_filtersoundpath;
        string render_outputpath;
        string render_vdoinfilepath;
        string render_vdooutfilepath;
        string render_maskfilepath;
        string render_duration;
        int render_repeat;
        string render_resolution;
        bool sqr_crop = false;
        bool render_ver = false;
        bool render_gif = false;
        int pngseq_render_count = 0;
        public string ImageProcessing(string dllpath, string sourcepath, int totalframe, string outputpath, string vdoinfilepath, string vdooutfilepath, string maskfilepath, string duration, int repeat, string resolution, string filtersoundpath)
        {

            dllpath.Replace(@"\", @"/");
            sourcepath.Replace(@"\", @"/");
            outputpath.Replace(@"\", @"/");
            vdoinfilepath.Replace(@"\", @"/");
            vdooutfilepath.Replace(@"\", @"/");
            maskfilepath.Replace(@"\", @"/");

            render_dllpath = dllpath;
            render_sourcepath = sourcepath;
            render_totalframe = totalframe;
            render_outputpath = outputpath;
            render_vdoinfilepath = vdoinfilepath;
            render_vdooutfilepath = vdooutfilepath;
            render_maskfilepath = maskfilepath;
            render_duration = duration;
            render_repeat = repeat;
            render_filtersoundpath = filtersoundpath;

            //render_resolution = resolution;
            //  cmd = "/c ";
            //PNG SEQ
            if (CheckFolderExite(Properties.Settings.Default.PNGSeqPath))
            {
                //MessageBox.Show("PNG SEQ OK");
                pngseq_render_count = 0;

                int pngseq_totalframe = (totalframe * 2) - 1;
                if (CheckFileCount(Properties.Settings.Default.PNGSeqPath, "*.png") >= pngseq_totalframe)
                {
                    // MessageBox.Show("PNG SEQ OK 2");
                    for (int i = 1; i <= pngseq_totalframe; i++)
                    {
                        pg_update();
                        string pngseq_mask_path = Properties.Settings.Default.PNGSeqPath + "/" + i.ToString("00") + ".png";
                        string pngsqr_camera_path = render_sourcepath + i.ToString("00") + ".jpg";
                        string pngseq_output_path = sourcepath + "/MASK/" + i.ToString("00") + ".jpg";
                        if (CheckFileExite(pngseq_mask_path) && CheckFileExite(pngsqr_camera_path) && !IsFileLocked(pngsqr_camera_path))
                        {
                            pg_update();
                            string c = @"/c " + DllPath + @" -y -i " + pngsqr_camera_path + " -i " + pngseq_mask_path + @" -filter_complex ""overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2"" " + pngseq_output_path + " & @";
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            startInfo.FileName = "cmd.exe";
                            startInfo.Arguments = c;
                            process.StartInfo = startInfo;
                            process.EnableRaisingEvents = true;
                            process.Exited += Process_Exited;
                            process.Start();
                            Render_Progress = true;
                        }
                    }
                    render_sourcepath = render_sourcepath + "MASK/";
                }
                else
                {
                    Process_Render(null, null);
                }
            }
            else
            {
                Process_Render(null, null);
            }

            //tRender.Enabled = true;

            // while (CheckFileCount(sourcepath + @"/M/", "*.jpg") < totalframe) { }

            return "OK";
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            pg_update();
            if (++pngseq_render_count >= (MaxCamera * 2) - 1)
            {
                Process_Render(sender, e);
                Render_Progress = true;
                //MessageBox.Show("PNG OK");
            }
        }
        private void Process_Render(object sender, EventArgs e)
        {
            string s = "";
            int in_w = 0;
            int in_h = 0;

            if (CheckFileCount(render_sourcepath, "*.jpg") >= render_totalframe - 1)
            {
                s += @"/c " + render_dllpath + @" -y -framerate " + render_totalframe.ToString() + "/" + render_duration + " -i " + render_sourcepath + "%02d.jpg  -c:v libx264 -r 29.97 -vf scale=1920:1200 -y -bsf:v h264_mp4toannexb -f mpegts " + render_sourcepath + "temp_vdo_forward.mp4 & @";
                if (CheckFileExite(render_maskfilepath))
                {
                    s += @" " + render_dllpath + @" -y -i " + render_sourcepath + "temp_vdo_forward.mp4 -i " + render_maskfilepath + @" -filter_complex ""overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2"" -vcodec libx264 -y -bsf:v h264_mp4toannexb -f mpegts " + render_sourcepath + "temp_vdo_forward_watermark.mp4 & @";
                }
                //VDO FADEIN
                if (CheckFileExite(render_vdoinfilepath)) s += @" " + render_dllpath + @" -y -i " + render_vdoinfilepath + " -c copy -bsf:v h264_mp4toannexb -f mpegts " + render_sourcepath + "temp_vdoin.mp4  & @";
                //VDO FADEOUT
                if (CheckFileExite(render_vdooutfilepath)) s += @" " + render_dllpath + @" -y -i " + render_vdooutfilepath + " -c copy -bsf:v h264_mp4toannexb -f mpegts " + render_sourcepath + "temp_vdoout.mp4  & @";

                pg_update();

                s += @" " + render_dllpath + @" -y -i ""concat:";

                //VDO FADEIN
                if (CheckFileExite(render_vdoinfilepath)) s += render_sourcepath + "temp_vdoin.mp4|";
                //LOOP
                for (int i = 0; i < render_repeat; i++)
                {
                    if (i > 0) s += "|";
                    if (CheckFileExite(render_maskfilepath)) s += render_sourcepath + "temp_vdo_forward_watermark.mp4";
                    else s += render_sourcepath + "temp_vdo_forward.mp4";
                    pg_update();
                }
                //VDO FADEOUT
                if (CheckFileExite(render_vdooutfilepath)) s += @"|" + render_sourcepath + "temp_vdoout.mp4";
                //OUTPUT
                s += @""" -c copy -bsf:a aac_adtstoasc " + render_sourcepath + "temp_vdo_final.mp4 & @";

                //=======================================================
                string filtersound_temppath = render_outputpath;
                if (CheckFileExite(render_filtersoundpath))
                {
                    render_outputpath = render_sourcepath + @"/prepare_filter.mp4";
                }
                //=======================================================
                if (render_ver)
                {
                    //Crop==============================
                    string[] res = render_resolution.Split(':');
                    in_w = Convert.ToUInt16(res[0]);
                    in_h = Convert.ToUInt16(res[1]);
                    if (in_w == 720) { in_w = 1080; in_h = 1080; }
                    if (in_w != 1080)
                    {
                        in_w = 1920;
                        in_h = 1080;
                    }
                    string crop_cmd = in_w.ToString() + ":" + (in_h + 4).ToString() + ":" + ((1920 - in_w) / 2).ToString() + ":" + ((1200 - in_h) / 2).ToString();
                    s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final.mp4 -filter:v ""crop=" + crop_cmd + @""" -c:a copy " + render_sourcepath + @"/temp_vdo_final_ver.mp4 & @";

                    //s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final.mp4 -filter:v ""crop=" + crop_cmd + @""" -vf scale=" + render_resolution + @" -c:a copy " + render_sourcepath + @"/temp_vdo_final_ver.mp4 & @";
                    s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final_ver.mp4 -vf ""transpose = 2"" -c:a copy " + render_outputpath + @" & @";

                }
                else
                {
                    //Crop==============================
                    string[] res = render_resolution.Split(':');
                    in_w = Convert.ToUInt16(res[0]);
                    in_h = Convert.ToUInt16(res[1]);
                    if (in_w == 720) { in_w = 1080; in_h = 1080; }
                    if (in_w != 1080)
                    {
                        in_w = 1920;
                        in_h = 1080;
                    }

                    string crop_cmd = in_w.ToString() + ":" + (in_h + 4).ToString() + ":" + ((1920 - in_w) / 2).ToString() + ":" + ((1200 - in_h) / 2).ToString();
                    s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final.mp4 -filter:v ""crop=" + crop_cmd + @""" -c:a copy " + render_outputpath + @" & @";


                    //s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final.mp4 -filter:v ""crop=" + crop_cmd  + @""" -vf scale=" + render_resolution + " -c:a copy " + render_outputpath + @" & @";
                }

                if (CheckFileExite(render_filtersoundpath))
                {
                    render_outputpath = filtersound_temppath;
                    // s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp_vdo_final_ver.mp4 -vf ""transpose = 2"" -c:a copy " + render_outputpath + @" & @";
                    s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/prepare_filter.mp4" + @" -i " + render_filtersoundpath + @" -filter_complex ""nullsrc = size = " + in_w.ToString() + "x" + in_h.ToString() + @"[base];[0:v] setpts=PTS-STARTPTS[top];[1:v] setpts=PTS-STARTPTS[bottom];[base] [top] overlay=shortest=1 [temp];[temp] [bottom] overlay=shortest=1"" -acodec aac -vcodec libx264 " + render_outputpath + @" & @";
                    //-i "in2.mp4" -i "in1.mov" -filter_complex "nullsrc=size=1080x1080 [base];[0:v] setpts=PTS-STARTPTS [top];[1:v] setpts=PTS-STARTPTS [bottom];[base][top] overlay=shortest=1 [temp];[temp][bottom] overlay=shortest=1" -acodec aac -vcodec libx264 out.mp4
                }

                //Output file mp4 ======================
                if (render_gif)
                {
                    render_gif_cmd = (@"/c " + render_dllpath + @" -i " + render_outputpath + " -f gif " + render_outputpath.Replace(".mp4", ".gif") + @" & @");
                }

                textBox1.Text = s;
                ////==================================
                System.Diagnostics.Process processA = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfoA = new System.Diagnostics.ProcessStartInfo();
                //startInfoA.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfoA.FileName = "cmd.exe";
                startInfoA.Arguments = s;
                processA.StartInfo = startInfoA;
                processA.EnableRaisingEvents = true;
                processA.Exited += ProcessA_Exited;
                processA.Start();
            }
        }

        bool Render_Progress = false;

        private void tRender_Tick(object sender, EventArgs e)
        {
            if (Render_Progress) pg_update();
            //tRender.Enabled = false;
            //string s = "";
            //if (CheckFileCount(render_sourcepath, "*.jpg") >= render_totalframe - 1)
            //{
            //    s += @"/c " + render_dllpath + @" -y -framerate " + render_totalframe.ToString() + "/" + render_duration + " -i " + render_sourcepath + "%02d.jpg  -c:v libx264 -r 30 -pix_fmt yuv420p -vf scale=1920:1200 -f mpegts temp_vdo_forward & @";
            //    //PNG MASK
            //    if (render_maskfilepath != "") s += @" " + render_dllpath + @" -y -i temp_vdo_forward -i " + render_maskfilepath + @" -filter_complex ""overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2"" -vcodec libx264 -f mpegts temp_forward & @";
            //    //VDO FADEIN
            //    if (render_vdoinfilepath != "") s += @" " + render_dllpath + @" -y -i " + render_vdoinfilepath + " -c copy -bsf:v h264_mp4toannexb -f mpegts temp_vdoin  & @";
            //    //VDO FADEOUT
            //    if (render_vdooutfilepath != "") s += @" " + render_dllpath + @" -y -i " + render_vdooutfilepath + " -c copy -bsf:v h264_mp4toannexb -f mpegts temp_vdoout  & @";

            //    s += @" " + render_dllpath + @" -y -i ""concat:";

            //    pg_update();

            //    //VDO FADEIN
            //    if (render_vdoinfilepath != "") s += "temp_vdoin|";
            //    //LOOP
            //    for (int i = 0; i < render_repeat; i++)
            //    {
            //        if (i > 0) s += "|";
            //        if (render_maskfilepath != "") s += "temp_forward";
            //        else s += "temp_vdo_forward";
            //        pg_update();
            //    }



            //    pg_update();
            //    //VDO FADEOUT
            //    if (render_vdooutfilepath != "")
            //    {
            //        s += @"|temp_vdoout"" -c copy " + render_sourcepath + "/temp.mp4 & @";
            //    }
            //    else
            //    {
            //        s += @""" -c copy " + render_sourcepath + "/temp.mp4 & @";
            //    }

            //    if (render_ver)
            //    {
            //        s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp.mp4 -vf ""transpose = 2"" -c:a copy " + render_sourcepath + "/temp2.mp4" + @" & @";
            //        //Crop==============================
            //        //s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp2.mp4 -filter:v ""crop = " + render_resolution + @":420:0"" -c:a copy " + render_outputpath + @" & @";
            //        string[] res = render_resolution.Split(':');
            //        int in_w = Convert.ToUInt16(res[0]);
            //        int in_h = Convert.ToUInt16(res[1]);
            //        string crop_cmd = render_resolution + ":" + ((1920 - in_w) / 2).ToString() + ":" + ((1200 - in_h) / 2).ToString();
            //        s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp2.mp4 -filter:v ""crop=" + crop_cmd + @""" -c:a copy " + render_outputpath + @" & @";

            //    }
            //    else
            //    {
            //        //Crop==============================
            //        string[] res = render_resolution.Split(':');
            //        int in_w = Convert.ToUInt16(res[0]);
            //        int in_h = Convert.ToUInt16(res[1]);
            //        string crop_cmd = render_resolution + ":" + ((1920-in_w)/2).ToString() + ":" + ((1200 - in_h) / 2).ToString();
            //        s += @" " + render_dllpath + @" -i " + render_sourcepath + @"/temp.mp4 -filter:v ""crop=" + crop_cmd + @""" -c:a copy " + render_outputpath + @" & @";
            //    }
            //    //Output file mp4 ======================
            //    if (render_gif)
            //    {
            //        render_gif_cmd = (@"/c " + render_dllpath + @" -i " + render_outputpath + " -f gif " + render_outputpath.Replace(".mp4", ".gif") + @" & @");
            //    }

            //    ////==================================
            //    System.Diagnostics.Process processA = new System.Diagnostics.Process();
            //    System.Diagnostics.ProcessStartInfo startInfoA = new System.Diagnostics.ProcessStartInfo();
            //    startInfoA.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    startInfoA.FileName = "cmd.exe";
            //    startInfoA.Arguments = s;
            //    processA.StartInfo = startInfoA;
            //    processA.EnableRaisingEvents = true;
            //    processA.Exited += ProcessA_Exited;
            //    processA.Start();

            //}
        }

        string render_gif_cmd = "";
        private void ProcessA_Exited(object sender, EventArgs e)
        {
            if (render_gif)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (File.Exists(render_outputpath))
                    {
                        pg_done();
                        ////==================================
                        System.Diagnostics.Process processB = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfoB = new System.Diagnostics.ProcessStartInfo();
                        startInfoB.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        startInfoB.FileName = "cmd.exe";
                        startInfoB.Arguments = render_gif_cmd;
                        processB.StartInfo = startInfoB;
                        processB.EnableRaisingEvents = true;
                        processB.Exited += ProcessB_Exited;
                        processB.Start();
                        return;
                    }
                    Thread.Sleep(100);
                }
            }
            else
            {
                try
                {
                    if (File.Exists(render_outputpath))
                    {
                        File.Copy(render_outputpath, OutputPath + DT + ".mp4", true);
                    }
                }
                catch { }

                Render_Progress = false;
                pg_done();
                FireEnable = true;
                ShowPreview();


            }
        }
        private void ProcessB_Exited(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(render_outputpath))
                {
                    File.Copy(render_outputpath, OutputPath + DT + ".mp4", true);
                }
            }
            catch { }

            Render_Progress = false;
            pg_done();
            FireEnable = true;
            ShowPreview();
        }

        FormPreview pv;
        int preview_loop_count = 0;
        private void InitPreview()
        {
            pv = new FormPreview();
            pv.ax.PlayStateChange += Ax_PlayStateChange;
            pv.ax.ClickEvent += Ax_ClickEvent;

            try
            {
                var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

                if (secondaryScreen != null)
                {
                    pv.StartPosition = FormStartPosition.Manual;
                    var workingArea = secondaryScreen.WorkingArea;
                    pv.Left = workingArea.Left;
                    pv.Top = workingArea.Top;
                    pv.Width = workingArea.Width;
                    pv.Height = workingArea.Height;
                    //pv.TopMost = true;
                    pv.ax.uiMode = "none";
                    //pv.ax.uiMode = "full";

                    pv.Show();
                    pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
                    pv.ax.Ctlcontrols.play();
                }
            }
            catch
            {

                pv.StartPosition = FormStartPosition.Manual;

                pv.Left = this.Left;
                pv.Top = this.Top;
                pv.Width = this.Width;
                pv.Height = this.Height;
                //pv.TopMost = true;
                pv.ax.uiMode = "none";
                //pv.ax.uiMode = "full";

                pv.Show();
                pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
                pv.ax.Ctlcontrols.play();

            }
            //pv.ax.uiMode = "none";
            //pv.Show();
            //pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
            //pv.ax.Ctlcontrols.play();
        }
        private void ShowPreview()
        {
            try
            {
                if (Properties.Settings.Default.PreviewEnable == true)
                {
                    pv.ax.Ctlcontrols.stop();
                    pv.ax.URL = null;
                    // pv.ax.close();
                    if (File.Exists(render_outputpath))
                    {
                        if (File.Exists(render_sourcepath + "/preview.mp4")) File.Delete(render_sourcepath + "/preview.mp4");
                        File.Copy(render_outputpath, render_sourcepath + "/preview.mp4", true);
                        if (File.Exists(render_sourcepath + "/preview.mp4"))
                        {
                            preview_loop_count = 3;
                            pv.ax.URL = render_sourcepath + "/preview.mp4";
                            pv.ax.Ctlcontrols.currentPosition = 0;
                            pv.ax.Ctlcontrols.play();

                        }
                    }
                }
            }
            catch { }
        }

        private void Ax_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            //pv.Close();
        }

        private void Ax_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (pv.ax.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                if (preview_loop_count > 0)
                {
                    preview_loop_count--;
                    pv.ax.Ctlcontrols.play(); pv.ax.Ctlcontrols.play();
                }
                if (preview_loop_count == 0)
                {
                    if (File.Exists(Properties.Settings.Default.PromoVdoPath))
                    {
                        if (pv.ax.URL != Properties.Settings.Default.PromoVdoPath) pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
                    }
                    pv.ax.Ctlcontrols.play();
                }
            }
        }

        private FilterInfoCollection CaptureDevice; // list of webcam
        private VideoCaptureDevice FinalFrame;
        private void ShowCountDown()
        {
            if (Properties.Settings.Default.PreviewBeforeFire == true)
            {
                FinalFrame.Start();
                count_down = 6;
            }
            else
            {
                count_down = 4;
            }

            tCountDown.Enabled = true;
        }

        void Fire()
        {
            switch (Properties.Settings.Default.SFXType)
            {
                case 0:
                    for (int k = 0; k < MaxCamera; k++)
                    {
                        try
                        {
                            EOSDigital.SDK.CanonSDK.EdsSendCommand(CameraControl[CameraSlot[k]].MainCamera.CamRef, CameraCommand.TakePicture, 0);
                        }
                        catch { }
                    }
                    break;
                case 1:
                    for (int k = 0; k < MaxCamera; k++)
                    {
                        try
                        {
                            EOSDigital.SDK.CanonSDK.EdsSendCommand(CameraControl[CameraSlot[k]].MainCamera.CamRef, CameraCommand.TakePicture, 0);

                            if (Properties.Settings.Default.SFXTime > 0) Thread.Sleep(Properties.Settings.Default.SFXTime);
                        }
                        catch { }
                    }
                    break;
                case 2:
                    for (int k = 0; k < MaxCamera; k++)
                    {
                        try
                        {
                            EOSDigital.SDK.CanonSDK.EdsSendCommand(CameraControl[CameraSlot[MaxCamera - k - 1]].MainCamera.CamRef, CameraCommand.TakePicture, 0);

                            if (Properties.Settings.Default.SFXTime > 0) Thread.Sleep(Properties.Settings.Default.SFXTime);
                        }
                        catch { }
                    }
                    break;
            }

            for (int k = 0; k < MaxCamera; k++)
            {
                try
                {
                    CameraStatus[CameraSlot[k]] = 1;
                }
                catch
                {
                    CameraStatus[CameraSlot[k]] = 0;
                }
            }
        }
        private void InitCountDown()
        {
            try
            {
                if (Properties.Settings.Default.WebcamDevice >= 0)
                {
                    CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);//constructor
                                                                                              //foreach (FilterInfo Device in CaptureDevice)
                                                                                              //{
                                                                                              //    comboBox1.Items.Add(Device.Name);
                                                                                              //}


                    FinalFrame = new VideoCaptureDevice();
                    FinalFrame = new VideoCaptureDevice(CaptureDevice[Properties.Settings.Default.WebcamDevice].MonikerString);// specified web cam and its filter moniker string
                                                                                                                               //FinalFrame.VideoCapabilities.
                    FinalFrame.NewFrame += FinalFrame_NewFrame;

                    for (int i = 0; i < FinalFrame.VideoCapabilities.Length; i++)
                    {
                        //string resolution = "Resolution Number " + i.ToString();
                        //string resolution_size = FinalFrame.VideoCapabilities[i].FrameSize.ToString();
                        if (FinalFrame.VideoCapabilities[i].FrameSize.Width == 640 && FinalFrame.VideoCapabilities[i].FrameSize.Height == 480)
                        {
                            FinalFrame.VideoResolution = FinalFrame.VideoCapabilities[i];
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        int count_down = 6;
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            try
            {
                int w = imgBox.Width;
                int h = (w / 16) * 9;

                int left = 0;
                int top = (imgBox.Height - h) / 2;

                Bitmap Frame_Bmp = (Bitmap)eventArgs.Frame.Clone();
                Bitmap result = new Bitmap(((imgBox.Height) / 9) * 16, imgBox.Height);
                Graphics g = Graphics.FromImage(result);
                g.Flush();

                g.DrawImage(Frame_Bmp, 0, 0, imgBox.Width, imgBox.Height);
                g.Flush();
                switch (count_down)
                {
                    case 3: g.DrawImage(CountDown_3_Bmp, left, top, w, h); break;
                    case 2: g.DrawImage(CountDown_2_Bmp, left, top, w, h); break;
                    case 1: g.DrawImage(CountDown_1_Bmp, left, top, w, h); break;
                    case 0: g.DrawImage(CountDown_Action_Bmp, left, top, w, h); break;
                }
                g.Flush();
                imgBox.Image = result;
                imgBox.Invalidate();
            }
            catch { }
        }

        private void tCountDown_Tick(object sender, EventArgs e)
        {

            try
            {
                if (Properties.Settings.Default.CountDownEnable == true)
                {
                    if (count_down > 0)
                    {
                        count_down--;
                    }
                }
                else
                {
                    if (count_down > 4)
                    {
                        count_down--;
                    }
                }

                if (Properties.Settings.Default.PreviewBeforeFire == false)
                {
                    try
                    {
                        int w = imgBox.Width;
                        int h = (w / 16) * 9;

                        int left = 0;
                        int top = (imgBox.Height - h) / 2;

                        Bitmap result = new Bitmap(((imgBox.Height) / 9) * 16, imgBox.Height);
                        Graphics g = Graphics.FromImage(result);

                        switch (count_down)
                        {
                            case 3: g.DrawImage(CountDown_3_Bmp, left, top, w, h); break;
                            case 2: g.DrawImage(CountDown_2_Bmp, left, top, w, h); break;
                            case 1: g.DrawImage(CountDown_1_Bmp, left, top, w, h); break;
                            case 0: g.DrawImage(CountDown_Action_Bmp, left, top, w, h); break;
                        }
                        g.Flush();
                        imgBox.Image = result;
                        imgBox.Invalidate();
                    }
                    catch { }
                }


                if (count_down == 0)
                {
                    Fire();
                }
                if (count_down == 0)
                {
                    tCountDown.Enabled = false;
                    FinalFrame.Stop();
                    imgBox.Image = null;
                    count_down = 0;
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.PromoVdoPath))
            {
                pv.ax.URL = Properties.Settings.Default.PromoVdoPath;
                pv.ax.Ctlcontrols.play();
                //MessageBox.Show(pv.ax.URL);
            }
        }

        private void FromMatrix3DPhoto_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Properties.Settings.Default.RigthClickFire == true) btnFire_Click(sender, e);

                cMenu.Show(this, e.X, e.Y);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            pv.ax.Ctlcontrols.stop();
            pv.ax.URL = null;
            // pv.ax.close();
            //if (File.Exists(render_outputpath))
            //{
            //if (File.Exists(render_sourcepath + "/preview.mp4")) File.Delete(render_sourcepath + "/preview.mp4");
            //File.Copy(render_outputpath, render_sourcepath + "/preview.mp4", true);
            if (File.Exists("preview.mp4"))
            {
                preview_loop_count = 3;
                pv.ax.URL = "preview.mp4";
                pv.ax.Ctlcontrols.currentPosition = 0;
                pv.ax.Ctlcontrols.play();

            }
            //}
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void cMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void cmSetting_Click(object sender, EventArgs e)
        {


            FormSetting fs = new FormSetting();
            fs.Top = this.Top;
            fs.Left = this.Left;
            fs.Width = this.Width;
            fs.Height = this.Height;
            fs.LoadConfig();
            fs.ShowDialog();

            //coLength.SelectedIndex = fs.coLength.SelectedIndex;
            //coSFXTime.SelectedIndex = fs.coSFXtime.SelectedIndex;
            //coLoop.SelectedIndex = fs.coLoop.SelectedIndex;
            //coRenderType.SelectedIndex = fs.coRenderType.SelectedIndex;
            //coRenderResolution.SelectedIndex = fs.coRenderResolution.SelectedIndex;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.Reload();
            MaxCamera = Convert.ToInt16(Properties.Settings.Default.MaxCamera);
            TotalFile = Convert.ToInt16(Properties.Settings.Default.MaxCamera);
            InitCountDown();
            RefreshCamera();
        }

        private void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].AvCoBox.SelectedIndex = AvCoBox.SelectedIndex;
                }
        }

        private void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].TvCoBox.SelectedIndex = TvCoBox.SelectedIndex;
                }
        }

        private void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].ISOCoBox.SelectedIndex = ISOCoBox.SelectedIndex;
                }
        }

        private void QaCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].QaCoBox.SelectedIndex = QaCoBox.SelectedIndex;
                }
        }

        private void AeCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].AeCoBox.SelectedIndex = AeCoBox.SelectedIndex;
                }
        }

        private void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].WBCoBox.SelectedIndex = WBCoBox.SelectedIndex;
                }
        }

        private void MeCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MasterSettingSlot >= 0)
                if (CameraControl[MasterSettingSlot].isOpen() == true && UpdateValue == false)
                {
                    CameraControl[MasterSettingSlot].MeCoBox.SelectedIndex = MeCoBox.SelectedIndex;
                }
        }

        private void version_software_Click(object sender, EventArgs e)
        {

        }
        private void FromMatrix3DPhoto_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                    live_view_key(0);
                    e.Handled = true;
                    break;
                case Keys.NumPad2:
                    live_view_key(1);
                    e.Handled = true;
                    break;
                case Keys.NumPad3:
                    live_view_key(2);
                    e.Handled = true;
                    break;
                case Keys.NumPad4:
                    live_view_key(3);
                    e.Handled = true;
                    break;
                case Keys.NumPad5:
                    live_view_key(4);
                    e.Handled = true;
                    break;
                case Keys.NumPad6:
                    live_view_key(5);
                    e.Handled = true;
                    break;
                case Keys.NumPad7:
                    live_view_key(6);
                    e.Handled = true;
                    break;
                case Keys.NumPad8:
                    live_view_key(7);
                    e.Handled = true;
                    break;
                case Keys.NumPad9:
                    live_view_key(8);
                    e.Handled = true;
                    break;
                case Keys.NumPad0:
                    live_view_key(9);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        public void live_view_key(int n)
        {
            if (CameraSlot[n] < MaxCamera)
            {
                if (CameraControl[CameraSlot[n]] != null)
                {
                    if (CameraControl[CameraSlot[n]].isOpen() == true) LiveViewProc(CameraSlot[n]);
                }
            }
        }
    }
}
