using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EOSDigital.API;
using EOSDigital.SDK;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace CanonSDK
{
    public partial class MainForm : Form
    {
        /*-----------------------------------------------------------------------------
White Balance
-----------------------------------------------------------------------------*/
        public const int WhiteBalance_Click = -1;
        public const int WhiteBalance_Auto = 0;
        public const int WhiteBalance_Daylight = 1;
        public const int WhiteBalance_Cloudy = 2;
        public const int WhiteBalance_Tangsten = 3;
        public const int WhiteBalance_Fluorescent = 4;
        public const int WhiteBalance_Strobe = 5;
        public const int WhiteBalance_WhitePaper = 6;
        public const int WhiteBalance_Shade = 8;
        public const int WhiteBalance_ColorTemp = 9;
        public const int WhiteBalance_PCSet1 = 10;
        public const int WhiteBalance_PCSet2 = 11;
        public const int WhiteBalance_PCSet3 = 12;
        /*-----------------------------------------------------------------------------
        AE Mode
       -----------------------------------------------------------------------------*/
        public const uint AEMode_Program = 0;
        public const uint AEMode_Tv = 1;
        public const uint AEMode_Av = 2;
        public const uint AEMode_Manual = 3;
        public const uint AEMode_Bulb = 4;
        public const uint AEMode_A_DEP = 5;
        public const uint AEMode_DEP = 6;
        public const uint AEMode_Custom = 7;
        public const uint AEMode_Lock = 8;
        public const uint AEMode_Green = 9;
        public const uint AEMode_NigntPortrait = 10;
        public const uint AEMode_Sports = 11;
        public const uint AEMode_Portrait = 12;
        public const uint AEMode_Landscape = 13;
        public const uint AEMode_Closeup = 14;
        public const uint AEMode_FlashOff = 15;
        public const uint AEMode_CreativeAuto = 19;
        public const uint AEMode_Movie = 20;
        public const uint AEMode_PhotoInMovie = 21;
        public const uint AEMode_SceneIntelligentAuto = 22;
        public const uint AEMode_SCN = 25;
        public const uint AEMode_Unknown = 0xffffffff;

        public int tcpPort = 10000;
        public int CID = 0;
        public string CameraSN = "";
        public string CameraID = "";
        bool formClosed = false;
        #region Variables

        public string SavePath = Application.StartupPath + @"\temp\0\";

        public CanonAPI APIHandler;
        public Camera MainCamera;
        
        public List<Camera> CamList;
        bool IsInit = false;
        bool Evf_Bmp_Change;
        Bitmap Evf_Bmp;

        int LVBw, LVBh, w, h;
        float LVBratio, LVration;

        int ErrCount;
        object ErrLock = new object();
        object LvLock = new object();

        public bool SDKEnable = false;
        #endregion

        public MainForm()
        {
            try
            {
                InitializeComponent();
                //APIHandler = new CanonAPI();
               // APIHandler.CameraAdded += APIHandler_CameraAdded;
                //ErrorHandler.SevereErrorHappened += ErrorHandler_SevereErrorHappened;
                //ErrorHandler.NonSevereErrorHappened += ErrorHandler_NonSevereErrorHappened;
                //  SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
                SavePathTextBox.Text = SavePath;//Application.StartupPath + "/";

                SaveFolderBrowser.Description = "Save Images To...";
                LiveViewPicBox.Paint += LiveViewPicBox_Paint;
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
                RefreshCamera();
                IsInit = true;
            }
            catch (DllNotFoundException) { ReportError("Canon DLLs not found!", true); }
            catch (Exception ex) { ReportError(ex.Message, true); }
            
        }

        public bool isOpen()
        {
            if (MainCamera?.SessionOpen == true) return true;
            return false;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                IsInit = false;
                MainCamera?.Dispose();
                APIHandler?.Dispose();
            }
            catch { }
            //catch (Exception ex) { ReportError(ex.Message, false); }

            if (!formClosed)
            {
                this.formClosed = true;
                this.Close();
            }
        }

        #region API Events

        private void APIHandler_CameraAdded(CanonAPI sender)
        {
            try { Invoke((Action)delegate { RefreshCamera(); }); }
            catch (Exception ex) { ReportError(ex.Message, false); }
            //MessageBox.Show("x");
        }

        private void MainCamera_StateChanged(Camera sender, StateEventID eventID, int parameter)
        {
            try { if (eventID == StateEventID.Shutdown && IsInit) { Invoke((Action)delegate { CloseSession(); }); } }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }
        
        private void MainCamera_ProgressChanged(object sender, int progress)
        {
            try { Invoke((Action)delegate { MainProgressBar.Value = progress; }); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void MainCamera_LiveViewUpdated(Camera sender, Stream img)
        {
            try
            {
                lock (LvLock)
                {
                    //Evf_Bmp?.Dispose();
                    Evf_Bmp = new Bitmap(img);
                    //Evf_Bmp2 = new Bitmap(img);
                    Evf_Bmp_Change = true;
                }
                //LiveViewPicBox.Invalidate();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void MainCamera_DownloadReady(Camera sender, DownloadInfo Info)
        {
            try
            {
                string dir = null;
                //Invoke((Action)delegate { dir = SavePathTextBox.Text; }); SavePath
                Invoke((Action)delegate { dir = SavePath; }); 
                sender.DownloadFile(Info, dir);
                Invoke((Action)delegate { MainProgressBar.Value = 0; });
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void ErrorHandler_NonSevereErrorHappened(object sender, ErrorCode ex)
        {
            //MessageBox.Show("1x");
            ReportError($"SDK Error code: {ex} ({((int)ex).ToString("X")})", false);
        }

        private void ErrorHandler_SevereErrorHappened(object sender, Exception ex)
        {
            //MessageBox.Show("2x");
            ReportError(ex.Message, true);
            
        }

        #endregion

        #region Session

        private void SessionButton_Click(object sender, EventArgs e)
        {
           if (MainCamera?.SessionOpen == true) CloseSession();
            else OpenSession();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try { RefreshCamera(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Settings

        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((string)TvCoBox.SelectedItem == "Bulb") MainCamera.TakePhotoBulbAsync((int)BulbUpDo.Value);
                else MainCamera.TakePhotoShutterAsync();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void RecordVideoButton_Click(object sender, EventArgs e)
        {
            try
            {
                Recording state = (Recording)MainCamera.GetInt32Setting(PropertyID.Record);
                if (state != Recording.On)
                {
                    MainCamera.StartFilming(true);
                    RecordVideoButton.Text = "Stop Video";
                }
                else
                {
                    bool save = STComputerRdButton.Checked || STBothRdButton.Checked;
                    MainCamera.StopFilming(save);
                    RecordVideoButton.Text = "Record Video";
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(SavePathTextBox.Text)) SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;
                if (SaveFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    if (AvCoBox.SelectedIndex < 0) return;
                    MainCamera.SetSetting(PropertyID.Av, AvValues.GetValue((string)AvCoBox.SelectedItem).IntValue);
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    if (TvCoBox.SelectedIndex < 0) return;

                    MainCamera.SetSetting(PropertyID.Tv, TvValues.GetValue((string)TvCoBox.SelectedItem).IntValue);
                    BulbUpDo.Enabled = (string)TvCoBox.SelectedItem == "Bulb";
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    if (ISOCoBox.SelectedIndex < 0) return;
                    MainCamera.SetSetting(PropertyID.ISO, ISOValues.GetValue((string)ISOCoBox.SelectedItem).IntValue);
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SaveToRdButton_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (IsInit)
            //    {
            //        if (STCameraRdButton.Checked)
            //        {
            //            MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Camera);
            //            BrowseButton.Enabled = false;
            //            SavePathTextBox.Enabled = false;
            //        }
            //        else
            //        {
            //            if (STComputerRdButton.Checked) MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
            //            else if (STBothRdButton.Checked) MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Both);

            //            MainCamera.SetCapacity(4096, int.MaxValue);
            //            BrowseButton.Enabled = true;
            //            SavePathTextBox.Enabled = true;
            //        }
            //    }
            //}
            //catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Live view

        private void LiveViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MainCamera.IsLiveViewOn) { MainCamera.StartLiveView(); LiveViewButton.Text = "Stop LV"; }
                else { MainCamera.StopLiveView(); LiveViewButton.Text = "Start LV"; }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
                LiveViewPicBox.Invalidate();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_Paint(object sender, PaintEventArgs e)
        {
            if (MainCamera == null || !MainCamera.SessionOpen) return;

            if (!MainCamera.IsLiveViewOn) e.Graphics.Clear(BackColor);
            else
            {
                lock (LvLock)
                {
                    if (Evf_Bmp != null)
                    {
                        LVBratio = LVBw / (float)LVBh;
                        LVration = Evf_Bmp.Width / (float)Evf_Bmp.Height;
                        if (LVBratio < LVration)
                        {
                            w = LVBw;
                            h = (int)(LVBw / LVration);
                        }
                        else
                        {
                            w = (int)(LVBh * LVration);
                            h = LVBh;
                        }
                        e.Graphics.DrawImage(Evf_Bmp, 0, 0, w, h);
                    }
                }
            }
        }

        private void FocusNear3Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Near3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear2Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Near2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear1Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Near1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar1Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Far1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar2Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Far2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar3Button_Click(object sender, EventArgs e)
        {
            try { MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)DriveLens.Far3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Subroutines
        public bool EditValue = false;
        public void CloseSession()
        {
            try
            {
                MainCamera.CloseSession();
            }
            catch { }
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            SettingsGroupBox.Enabled = false;
            LiveViewGroupBox.Enabled = false;
            SessionButton.Text = "Open Session";
            SessionLabel.Text = "No open session";
            LiveViewButton.Text = "Start LV";
            this.Text = "[" + CID.ToString() + "] CANON INTERFACE";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //if (!SDKEnable) Application.Exit();
            tcpDataStart();
            tcpLiveStart();
            tcpJpgStart();
        }

    private void SettingsGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void SaveToGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void BulbUpDo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void MainProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void InitGroupBox_Enter(object sender, EventArgs e)
        {

        }
        
        public void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    switch (WBCoBox.SelectedIndex)
                    {
                        case 0: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Auto); break;
                        case 1: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Daylight); break;
                        case 2: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Cloudy); break;
                        case 3: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Tangsten); break;
                        case 4: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Fluorescent); break;
                        case 5: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Strobe); break;
                        case 6: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_WhitePaper); break;
                        case 7: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_Shade); break;
                        case 8: MainCamera.SetSetting(PropertyID.WhiteBalance, WhiteBalance_ColorTemp); break;
                    }
                    if (WBCoBox.SelectedIndex == 9)
                    {
                        txtColorTemp.Text = MainCamera.GetUInt32Setting(PropertyID.ColorTemperature).ToString();
                        txtColorTemp.Enabled = true;
                        btnSetColorTemp.Enabled = true;
                    }
                    else
                    {
                        txtColorTemp.Enabled = false;
                        btnSetColorTemp.Enabled = false;
                    }
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }

           
        }

        public UInt32 GetImageQuality()
        {
            return MainCamera.GetUInt32Setting(PropertyID.ImageQuality);
        }

        public void SetImageQuality(UInt32 Qa)
        {
            MainCamera.SetSetting(PropertyID.ImageQuality, Qa);
        }
        private void QaCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    switch (QaCoBox.SelectedIndex)
                    {
                        case 0: MainCamera.SetSetting(PropertyID.ImageQuality, 0x013FF0F); break;//LF
                        case 1: MainCamera.SetSetting(PropertyID.ImageQuality, 0x012FF0F); break;//L
                        case 2: MainCamera.SetSetting(PropertyID.ImageQuality, 0x113FF0F); break;//MF
                        case 3: MainCamera.SetSetting(PropertyID.ImageQuality, 0x112FF0F); break;//M
                        case 4: MainCamera.SetSetting(PropertyID.ImageQuality, 0xE13FF0F); break;//SF
                        case 5: MainCamera.SetSetting(PropertyID.ImageQuality, 0xE12FF0F); break;//S1
                        case 6: MainCamera.SetSetting(PropertyID.ImageQuality, 0xF13FF0F); break;//S2
                        case 7: MainCamera.SetSetting(PropertyID.ImageQuality, 0x1013FF0F); break;//S3
                        case 8: MainCamera.SetSetting(PropertyID.ImageQuality, 0x640013); break;//Raw + Large
                        //case 9: MainCamera.SetSetting(PropertyID.ImageQuality, 0x64FF0F); break;//Raw
                    }
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void AeCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    switch (AeCoBox.SelectedIndex)
                    {
                        case 0: MainCamera.SetSetting(PropertyID.MeteringMode, 0x03); break;
                        case 1: MainCamera.SetSetting(PropertyID.MeteringMode, 0x04); break;
                        case 2: MainCamera.SetSetting(PropertyID.MeteringMode, 0x05); break;
                    }
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        public void RefreshCamera()
        {
            CameraListBox.Items.Clear();
            CamList = APIHandler.GetCameraList();
            foreach (Camera cam in CamList) CameraListBox.Items.Add(cam.DeviceName);
            if (MainCamera?.SessionOpen == true) CameraListBox.SelectedIndex = CamList.FindIndex(t => t.ID == MainCamera.ID);
            else if (CamList.Count > 0)
            {
                CameraListBox.SelectedIndex = 0;
            }
        }

       
        public string JsonSN() { return "SN:" + CameraSN; }
        public string JsonID() { return "ID:" + CameraID; }
        public string JsonClock() { return "ID:" + CameraID; }

        public string JsonCam() { return ListboxEncodeToJson(CameraListBox); }
        public string JsonCam(string val) { return ListboxEncodeToJson(CameraListBox); }

        public string JsonAv() { return CoboxEncodeToJson(AvCoBox);}
        public string JsonAv(string val) {return JsonDecodeToCobox(AvCoBox, val); }

        public string JsonTv() { return CoboxEncodeToJson(TvCoBox); }
        public string JsonTv(string val) { return JsonDecodeToCobox(TvCoBox, val); }

        public string JsonISO() { return CoboxEncodeToJson(ISOCoBox); }
        public string JsonISO(string val) { return JsonDecodeToCobox(ISOCoBox, val); }

        public string JsonWb() { return CoboxEncodeToJson(WBCoBox); }
        public string JsonWb(string val) { return JsonDecodeToCobox(WBCoBox, val); }

        public string JsonQa() { return CoboxEncodeToJson(QaCoBox); }
        public string JsonQa(string val) { return JsonDecodeToCobox(QaCoBox, val); }

        public string JsonAe() { return CoboxEncodeToJson(AeCoBox); }
        public string JsonAe(string val) { return JsonDecodeToCobox(AeCoBox, val); }

        public string JsonMe() { return CoboxEncodeToJson(MeCoBox); }
        public string JsonMe(string val) { return JsonDecodeToCobox(MeCoBox, val); }

        public string JsonCameraList()
        {
            string json = "CameraList:{";
            for(int i=0;i<CameraListBox.Items.Count;i++)
            {
                json += CameraListBox.Items[i];
                if (i < (CameraListBox.Items.Count - 1)) json += ",";
            }
            json += "}";
            return json;
        }
        public string JsonEncoder()
        {
            string json = "";
            json += "{";
            json += JsonAv() +",";
            json += JsonTv() + ",";
            json += JsonISO() + ",";
            json += JsonWb() + ",";
            json += JsonQa() + ",";
            json += JsonAe() + ",";
            json += JsonMe();
            json += "}";
            return json;
        }

        public string ListboxEncodeToJson(ListBox cb)
        {
            string json = "";
            json += cb.Name.Substring(0, 3) + "List:{";
            for (int i = 0; i < cb.Items.Count; i++)
            {
                json += cb.Items[i];
                if (i < (cb.Items.Count - 1)) json += ",";
            }
            json += "}," + cb.Name.Substring(0, 2) + ":" + cb.Text;
            return json;
        }
        public string CoboxEncodeToJson(ComboBox cb)
        {
            string json = "";
            json += cb.Name.Substring(0, 2) + "List:{";
            for (int i = 0; i < cb.Items.Count; i++)
            {
                json += cb.Items[i];
                if (i < (cb.Items.Count-1)) json += ",";
            }
            json += "}," + cb.Name.Substring(0, 2) + ":" + cb.Text;
            return json;
        }
        public string JsonDecodeToCobox(ComboBox cb , string val)
        {
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (cb.Items[i].ToString() == val)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
            return CoboxEncodeToJson(cb);
        }

        public string JsonDecoder(string json)
        {
            if (json.Length > 5)
            {
                if (json[0] == '{' && json[json.Length - 1] == '}')
                {
                    string[] JsonField = json.Substring(1, json.Length - 2).Split(',');
                    for (int i = 0; i < JsonField.Length; i++)
                    {
                        string[] JsonData = JsonField[i].Split(':');
                        if (JsonData.Length >= 2)
                        {
                            string val = JsonData[1].ToString();
                            switch (JsonData[0])
                            {
                                case "Av": return JsonAv(val); break;
                                case "Tv": return JsonTv(val); break;
                                case "ISO": return JsonISO(val); break;
                                case "Wb": return JsonWb(val); break;
                                case "Qa": return JsonQa(val); break;
                                case "Ae": return JsonAe(val); break;
                                case "Me": return JsonMe(val); break;
                                case "Cam": return JsonCam(val); break;
                                case "LiveView": return JsonLiveView(val); break;
                            }
                        }
                    }
                }
            }
            return "{Error:0}";
        }

        public string JsonLiveView(string val)
        {
            string json = "LiveView:";
            switch(val)
            {
                case "on":
                    if (!MainCamera.IsLiveViewOn){MainCamera.StartLiveView(); LiveViewButton.Text = "Stop LV"; }
                    break;
                case "off":
                    if (MainCamera.IsLiveViewOn){ MainCamera.StopLiveView(); LiveViewButton.Text = "Start LV"; }
                    break;
            }
            if (MainCamera.IsLiveViewOn) json += "on"; else json += "off";
            return json;
        }
        private void button1_Click(object sender, EventArgs e)
        {

 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JsonDecoder(textBox1.Text);
        }
        //private void OpenSession2()
        //{
        //    if (CameraListBox.SelectedIndex >= 0)
        //    {
        //        MainCamera = CamList[CameraListBox.SelectedIndex];
        //        MainCamera.OpenSession();
        //        MainCamera.LiveViewUpdated += MainCamera_LiveViewUpdated;
        //        MainCamera.ProgressChanged += MainCamera_ProgressChanged;
        //        MainCamera.StateChanged += MainCamera_StateChanged;
        //        MainCamera.DownloadReady += MainCamera_DownloadReady;

        //        SessionButton.Text = "Close Session";
        //        SessionLabel.Text = MainCamera.DeviceName;
        //        AvList = MainCamera.GetSettingsList(PropertyID.Av);
        //        TvList = MainCamera.GetSettingsList(PropertyID.Tv);
        //        ISOList = MainCamera.GetSettingsList(PropertyID.ISO);
        //        foreach (var Av in AvList) AvCoBox.Items.Add(Av.StringValue);
        //        foreach (var Tv in TvList) TvCoBox.Items.Add(Tv.StringValue);
        //        foreach (var ISO in ISOList) ISOCoBox.Items.Add(ISO.StringValue);
        //        AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(AvValues.GetValue(MainCamera.GetInt32Setting(PropertyID.Av)).StringValue);
        //        TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(TvValues.GetValue(MainCamera.GetInt32Setting(PropertyID.Tv)).StringValue);
        //        ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(ISOValues.GetValue(MainCamera.GetInt32Setting(PropertyID.ISO)).StringValue);
        //        SettingsGroupBox.Enabled = true;
        //        LiveViewGroupBox.Enabled = true;
        //    }
        //}

        public void OpenSession()
        {
            if (CameraListBox.SelectedIndex >= 0)
            {

                MainCamera = CamList[CameraListBox.SelectedIndex];
                MainCamera.OpenSession();
                MainCamera.LiveViewUpdated += MainCamera_LiveViewUpdated;
                MainCamera.ProgressChanged += MainCamera_ProgressChanged;
                //MainCamera.StateChanged += MainCamera_StateChanged;
                MainCamera.DownloadReady += MainCamera_DownloadReady;

                SessionButton.Text = "Close Session";
                SessionLabel.Text = MainCamera.DeviceName;

                LoadValue();

                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
                CameraSN = GetCameraSN();
                CameraID = GetCameraID();
    
                lbSN.Text = CameraSN;
                lbNo.Text = CameraID;
                this.Text = "[" + lbNo.Text + "]" + "[" + CameraSN + "]";

                //if (STCameraRdButton.Checked)
                //{
                //    MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Camera);
                //    BrowseButton.Enabled = false;
                //    SavePathTextBox.Enabled = false;
                //}
                //else
                //{
                MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
                MainCamera.SetCapacity(4096, int.MaxValue);
                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
                SavePathTextBox.Text = SavePath;

                // }

            }
        }
        public void OpenSession(Camera cm)
        {
            //if (CameraListBox.SelectedIndex >= 0)
            //{

                MainCamera = cm;//CamList[CameraListBox.SelectedIndex];
                MainCamera.OpenSession();
                MainCamera.LiveViewUpdated += MainCamera_LiveViewUpdated;
                MainCamera.ProgressChanged += MainCamera_ProgressChanged;
                MainCamera.StateChanged += MainCamera_StateChanged;
                MainCamera.DownloadReady += MainCamera_DownloadReady;
               
                SessionButton.Text = "Close Session";
                SessionLabel.Text = MainCamera.DeviceName;

                LoadValue();

                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
                CameraSN = GetCameraSN();
                CameraID = GetCameraID();
                lbSN.Text = CameraSN;
                lbNo.Text = CameraID;
                this.Text = "[" + lbNo.Text + "]" + "[" + CameraSN + "]";

                //if (STCameraRdButton.Checked)
                //{
                //    MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Camera);
                //    BrowseButton.Enabled = false;
                //    SavePathTextBox.Enabled = false;
                //}
                //else
                //{
                MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
                MainCamera.SetCapacity(4096, int.MaxValue);
                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
                SavePathTextBox.Text = SavePath;

               // }

            //}
        }

        private void ReportError(string message, bool lockdown)
        {
            //int errc;
            //lock (ErrLock) { errc = ++ErrCount; }

            //if (lockdown) EnableUI(false);

            //if (errc < 4) MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else if (errc == 4) MessageBox.Show("Many errors happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //lock (ErrLock) { ErrCount--; }
        }

        private void EnableUI(bool enable)
        {
            if (InvokeRequired) Invoke((Action)delegate { EnableUI(enable); });
            else
            {
                SettingsGroupBox.Enabled = enable;
                InitGroupBox.Enabled = enable;
                LiveViewGroupBox.Enabled = enable;
            }
        }

        #endregion

        private void MeCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    if (MeCoBox.SelectedIndex < 0) return;
                    MainCamera.SetSetting(PropertyID.MeteringMode, MeteringModeValues.GetValue((string)MeCoBox.SelectedItem).IntValue);
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        public CameraValue[] AvList;
        public CameraValue[] TvList;
        public CameraValue[] ISOList;
        public CameraValue[] MeList;
        public CameraValue[] AeList;
        public CameraValue[] WbList;
        public void LoadValue()
        {
            EditValue = false;
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            MeCoBox.Items.Clear();
            AvList = null;
            TvList = null;
            ISOList = null;
            //AeCoBox.Items.Clear();
            //QaCoBox.Items.Clear();
            //WBCoBox.Items.Clear();

            try
            {
                AvList = MainCamera.GetSettingsList(PropertyID.Av);
                TvList = MainCamera.GetSettingsList(PropertyID.Tv);
                ISOList = MainCamera.GetSettingsList(PropertyID.ISO);
                MeList = MainCamera.GetSettingsList(PropertyID.MeteringMode);
                //AeList = MainCamera.GetSettingsList(PropertyID.AEMode);
                //WbList = MainCamera.ge(PropertyID.WhiteBalance);

                foreach (var Av in AvList) AvCoBox.Items.Add(Av.StringValue);
                foreach (var Tv in TvList) TvCoBox.Items.Add(Tv.StringValue);
                foreach (var ISO in ISOList) ISOCoBox.Items.Add(ISO.StringValue);
                foreach (var Me in MeList) MeCoBox.Items.Add(Me.StringValue);
                //foreach (var Ae in AeList) AeCoBox.Items.Add(Ae.StringValue);
                //foreach (var Wb in WbList) WBCoBox.Items.Add(Wb.StringValue);

                AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(AvValues.GetValue(MainCamera.GetInt32Setting(PropertyID.Av)).StringValue);
                TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(TvValues.GetValue(MainCamera.GetInt32Setting(PropertyID.Tv)).StringValue);
                ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(ISOValues.GetValue(MainCamera.GetInt32Setting(PropertyID.ISO)).StringValue);
                MeCoBox.SelectedIndex = MeCoBox.Items.IndexOf(MeteringModeValues.GetValue(MainCamera.GetInt32Setting(PropertyID.MeteringMode)).StringValue);
                // AeCoBox.SelectedIndex = AeCoBox.Items.IndexOf(AEModeValues.GetValue(MainCamera.GetInt32Setting(PropertyID.AEMode)).StringValue);

            }
            catch { }

            Int32 wbidx = MainCamera.GetInt32Setting(PropertyID.WhiteBalance);
            switch (wbidx)
            {
                case WhiteBalance_Auto: WBCoBox.SelectedIndex = 0; break;
                case WhiteBalance_Daylight: WBCoBox.SelectedIndex = 1; break;
                case WhiteBalance_Cloudy: WBCoBox.SelectedIndex = 2; break;
                case WhiteBalance_Tangsten: WBCoBox.SelectedIndex = 3; break;
                case WhiteBalance_Fluorescent: WBCoBox.SelectedIndex = 4; break;
                case WhiteBalance_Strobe: WBCoBox.SelectedIndex = 5; break;
                case WhiteBalance_WhitePaper: WBCoBox.SelectedIndex = 6; break;
                case WhiteBalance_Shade: WBCoBox.SelectedIndex = 7; break;
                case WhiteBalance_ColorTemp: WBCoBox.SelectedIndex = 7; break;
                default: WBCoBox.SelectedIndex = -1;
                    break;
            }

            Int32 Quality = MainCamera.GetInt32Setting(PropertyID.ImageQuality);
            switch (Quality)
            {
                case 0x0013FF0F: QaCoBox.SelectedIndex = 0; break;//Large Fine
                case 0x0012FF0F: QaCoBox.SelectedIndex = 1; break;//Large 
                case 0x0113FF0F: QaCoBox.SelectedIndex = 2; break;//Medium Fine
                case 0x0112FF0F: QaCoBox.SelectedIndex = 3; break;//Medium
                case 0x0E13FF0F: QaCoBox.SelectedIndex = 4; break;//Small Fine
                case 0x0E12FF0F: QaCoBox.SelectedIndex = 5; break;//Small
                case 0x0F13FF0F: QaCoBox.SelectedIndex = 6; break;//Small 2
                case 0x1013FF0F: QaCoBox.SelectedIndex = 7; break;//Small 3
                case 0x00640013: QaCoBox.SelectedIndex = 8; break;//Raw + Large Fine
                case 0x0064FF0F: QaCoBox.SelectedIndex = 9; break;//Raw
                default: QaCoBox.SelectedIndex = -1; break;
                    //Raw + L  0x640013
                    //Raw 0x64FF0F
            }

            uint Aeid = MainCamera.GetUInt16Setting(PropertyID.AEMode);
            switch (Aeid)
            {
                case AEMode_Manual: AeCoBox.SelectedIndex = 0; break;
                case AEMode_Av: AeCoBox.SelectedIndex = 1; break;
                case AEMode_Tv: AeCoBox.SelectedIndex = 2; break;
                case AEMode_Program: AeCoBox.SelectedIndex = 3; break;
                case AEMode_SceneIntelligentAuto: AeCoBox.SelectedIndex = 4; break;
                case AEMode_FlashOff: AeCoBox.SelectedIndex = 5; break;
                case AEMode_CreativeAuto: AeCoBox.SelectedIndex = 6; break;
                case AEMode_Portrait: AeCoBox.SelectedIndex = 7; break;
                case AEMode_Landscape: AeCoBox.SelectedIndex = 8; break;
                case AEMode_Closeup: AeCoBox.SelectedIndex = 9; break;
                case AEMode_Sports: AeCoBox.SelectedIndex = 10; break;
                case AEMode_NigntPortrait: AeCoBox.SelectedIndex = 11; break;
                case AEMode_Movie: AeCoBox.SelectedIndex = 12; break;
                default: AeCoBox.SelectedIndex = 13; break;
            }

            PsCoBox.Items.Clear();
            PsCoBox.Items.Add("Standard");
            PsCoBox.Items.Add("Portrait");
            PsCoBox.Items.Add("Landscape");
            PsCoBox.Items.Add("Neutral");
            PsCoBox.Items.Add("Faithful");   
            PsCoBox.Items.Add("Monochrome");
            PsCoBox.Items.Add("Auto");
            PsCoBox.Items.Add("FineDetail");
            PsCoBox.Items.Add("User Define 1");
            PsCoBox.Items.Add("User Define 2");
            PsCoBox.Items.Add("User Define 3");
            UInt32 PictureStyle = MainCamera.GetUInt32Setting(PropertyID.PictureStyle);
            //MessageBox.Show(PictureStyle.ToString("X4"));
            switch (PictureStyle)
            {
                case 0x0081: PsCoBox.SelectedIndex = 0; break;//Standard
                case 0x0082: PsCoBox.SelectedIndex = 1; break;//PT
                case 0x0083: PsCoBox.SelectedIndex = 2; break;//Land
                case 0x0084: PsCoBox.SelectedIndex = 3; break;//Neutral
                case 0x0085: PsCoBox.SelectedIndex = 4; break;//Faithful
                case 0x0086: PsCoBox.SelectedIndex = 5; break;//Monochrome
                case 0x0087: PsCoBox.SelectedIndex = 6; break;//Auto
                case 0x0088: PsCoBox.SelectedIndex = 7; break;//FineDetail
                case 0x0021: PsCoBox.SelectedIndex = 8; break;//User Define 1
                case 0x0022: PsCoBox.SelectedIndex = 9; break;//User Define 2
                case 0x0023: PsCoBox.SelectedIndex = 10; break;//User Define 3
            }

            EditValue = true;
        }
        public string GetCameraSN()
        {
            try
            {
                return MainCamera.GetStringSetting(PropertyID.BodyIDEx).ToString();
            }
            catch
            {
                return "0";
            }

        }
        public string SetCameraID(int n)
        {
            try
            {
                MainCamera.SetSetting(PropertyID.OwnerName, n.ToString("00"));
                return GetCameraID();
            }
            catch
            {
                return "0";
            }
        }

        //TCP ===================================================
        Socket serverData = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket serverImg = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        #region tcpData
        bool tcpDataonListen;
        bool tcpDataonConnect;
        TcpListener tcpDataListener;
        TcpClient serverDataClient;
        NetworkStream steamDataClient;


        public void tcpDataStart()
        {
            try
            {
                tcpDataonListen = true;
                tcpDataListener = new TcpListener(IPAddress.Any, tcpPort);
                tcpDataListener.Start();

                Thread sThread = new Thread(new ThreadStart(serverDataLoop));
                sThread.IsBackground = true;
                sThread.Start();
            }
            catch
            {
                tcpDataonListen = false;
            }
        }
        string responseData;
        public void serverDataLoop()
        {
            while (tcpDataonListen)
            {
                try
                {
                    if (tcpDataListener.Pending())
                    {
                        serverDataClient = tcpDataListener.AcceptTcpClient();
                        steamDataClient = serverDataClient.GetStream();
                        tcpDataonConnect = true;
                    }
                    if (tcpDataonConnect)
                    {
                        if(steamDataClient.DataAvailable)
                        {
                            byte[] data = new byte[100000];
                            Int32 bytes = steamDataClient.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            string res = "{";
                            Invoke((Action)delegate { res += JsonDecoder(responseData); });
                            res += "}" + '\r' + '\n';
                            byte[] resdata = System.Text.Encoding.UTF8.GetBytes(res);
;                           steamDataClient.Write(resdata, 0, resdata.Length);
                        }
                    }
                }
                catch
                {
                    tcpDataonConnect = false;
                }
                Thread.Sleep(100);
            }

        }
        #endregion

        #region tcpLive
        bool tcpLiveonListen;
        bool tcpLiveonConnect;
        TcpListener tcpLiveListener;
        Socket serverLiveListener;
        public void tcpLiveStart()
        {
            try
            {
                tcpLiveonListen = true;
                tcpLiveListener = new TcpListener(IPAddress.Any, tcpPort + 20);
                tcpLiveListener.Start();

                Thread sThread = new Thread(new ThreadStart(serverLiveLoop));
                sThread.IsBackground = true;
                sThread.Start();
            }
            catch
            {
                tcpLiveonListen = false;
            }
        }
        public void serverLiveLoop()
        {
            while (tcpLiveonListen)
            {
                try
                {
                    if (tcpLiveListener.Pending())
                    {
                        serverLiveListener = tcpLiveListener.AcceptSocket();
                        tcpLiveonConnect = true;
                    }
                    if (tcpLiveonConnect)
                    {
                        if (Evf_Bmp_Change)
                        {
                            Evf_Bmp_Change = false;
                            byte[] buffer = ObjectToByteArray(Evf_Bmp);
                            serverLiveListener.Send(buffer, buffer.Length, SocketFlags.None);
                        }
                    }
                }
                catch
                {
                    tcpLiveonConnect = false;

                }
                Thread.Sleep(100);
            }

        }
        #endregion

        #region tcpJpg
        bool tcpJpgonListen;
        bool tcpJpgonConnect;
        TcpListener tcpJpgListener;
        Socket serverJpgListener;
        public void tcpJpgStart()
        {
            try
            {
                tcpJpgonListen = true;
                tcpJpgListener = new TcpListener(IPAddress.Any, tcpPort + 10);
                tcpJpgListener.Start();

                Thread sThread = new Thread(new ThreadStart(serverJpgLoop));
                sThread.IsBackground = true;
                sThread.Start();
            }
            catch
            {
                tcpJpgonListen = false;
            }
        }
        public void serverJpgLoop()
        {
            while (tcpJpgonListen)
            {
                try
                {
                    if (tcpJpgListener.Pending())
                    {
                        serverJpgListener = tcpJpgListener.AcceptSocket();
                        tcpJpgonConnect = true;
                    }
                    if (tcpJpgonConnect)
                    {
                        string[] FileList = Directory.GetFiles(SavePath, "*.jpg");
                        Bitmap img;
                        if (FileList.Length > 0)
                        {
                            string FilePath = FileList[0];
                            if (!IsFileLocked(FilePath))
                            {
                                img = new Bitmap(FilePath);
                                byte[] buffer = ObjectToByteArray(img);
                                serverJpgListener.Send(buffer, buffer.Length, SocketFlags.None);
                                img?.Dispose();
                                File.Delete(FilePath);
                            }
                        }
                    }
                }
                catch
                {
                    tcpJpgonConnect = false;
                }
                Thread.Sleep(100);
            }
        }

        public Bitmap LoadFirstJpgFile()
        {
            string[] FileList = Directory.GetFiles(SavePath, "*.jpg");
            Bitmap img;
            if(FileList.Length>0)
            {
                string FilePath = SavePath + FileList[0];
                if (!IsFileLocked(FilePath))
                {
                    img = new Bitmap(FilePath);
                    return img;
                }
            }
            return null;
        }
        public bool IsFileLocked(string filePath)
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

            return false;
        }
        #endregion



        private byte[] ObjectToByteArray(Object obj)
        {
            try
            {
                if (obj == null)
                    return null;
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        // Convert a byte array to an Object
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
        private void LiveViewPicBox_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }


        private void PsCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EditValue)
                {
                    if (PsCoBox.SelectedIndex < 0) return;
                    switch (PsCoBox.SelectedIndex)
                    {
                        case 0: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0081); break;
                        case 1: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0082); break;
                        case 2: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0083); break;
                        case 3: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0084); break;
                        case 4: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0085); break;
                        case 5: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0086); break;
                        case 6: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0087); break;
                        case 7: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0088); break;
                        case 8: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0021); break;
                        case 9: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0022); break;
                        case 10: MainCamera.SetSetting(PropertyID.PictureStyle, 0x0023); break;
                    }
                   
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            LoadValue();


        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void btnKevin_Click(object sender, EventArgs e)
        {
            SetColorTemp(txtColorTemp.Text);
        }
        public void SetColorTemp(string x)
        {
            try
            {
                int k = Convert.ToInt32(x);
                if(k >= 2800 && k <= 10000)
                {
                    MainCamera.SetSetting(PropertyID.ColorTemperature, k);
                }
            }
            catch { }
        }

        private void btnSetClock_Click(object sender, EventArgs e)
        {

        }

        private void btnAutoOff_Click(object sender, EventArgs e)
        {
            //MainCamera.se
        }

        public string GetCameraID()
        {
            try
            {
                return MainCamera.GetStringSetting(PropertyID.Copyright).ToString();
            }
            catch
            {
                return "0";
            }
        }
        public struct EdsTime
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
            public int Milliseconds;
        }
        public string GetClock()
        {
            try
            {
                EdsTime time = MainCamera.GetStructSetting<EdsTime>(PropertyID.DateTime);
                string stime = "";
                stime += time.Year.ToString("0000");
                stime += time.Month.ToString("00");
                stime += time.Day.ToString("00");
                stime += time.Hour.ToString("00");
                stime += time.Minute.ToString("00");
                stime += time.Second.ToString("00");
                return stime;
            }
            catch
            {
                return "";
            }
        }
        public string SetClock(int yyyy,int mm,int dd,int h,int m,int s)
        {
            try
            {
                EdsTime time = MainCamera.GetStructSetting<EdsTime>(PropertyID.DateTime);
                time.Year = yyyy;
                time.Month = mm;
                time.Day = dd;
                time.Hour = h;
                time.Minute = m;
                time.Second = s;
                //time.Year = DateTime.Now.Year;
                //time.Month = DateTime.Now.Month;
                //time.Day = DateTime.Now.Day;
                //time.Hour = DateTime.Now.Hour;
                //time.Minute = DateTime.Now.Minute;
                //time.Second = DateTime.Now.Second;
                MainCamera.SetStructSetting<EdsTime>(PropertyID.DateTime, time);
                return GetClock();
            }
            catch
            {
                return "";
            }
        }
    }


}
