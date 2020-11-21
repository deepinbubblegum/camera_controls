using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using DeviceId;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MATRIX3DPHOTO
{
    static class Program
    {

        private static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "bubble123";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return " ";
            }

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            try
            {
                string deviceId_verify = System.IO.File.ReadAllText("license");
                deviceId_verify = Decrypt(deviceId_verify);
                //string[] lines = System.IO.File.ReadAllLines("license");

                //MessageBox.Show(text);

                //string deviceId_verify = "G755Q7OjKuLn7odxvJ_W0aaS1zdPG0P7Uilt5DLhYWs";
                string deviceId = new DeviceIdBuilder()
                    .AddMachineName()
                    .AddProcessorId()
                    .AddMotherboardSerialNumber()
                    .AddSystemDriveSerialNumber()
                    .ToString();


                //System.IO.File.WriteAllText("license", Encrypt(deviceId));
                if (deviceId != deviceId_verify)
                //if (false)
                {
                    MessageBox.Show("Server license is not verify");
                    System.Environment.Exit(1);
                }
            }
            catch
            {
                MessageBox.Show("Can't execut program");
                System.Environment.Exit(1);
            }



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FromMatrix3DPhoto());
        }


    }
}
