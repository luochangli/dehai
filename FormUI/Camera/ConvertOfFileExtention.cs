using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace FormUI.Camera
{
    public class ConvertOfFileExtention
    {
       
        private static long puff = 0;
        private static string _devicePath = string.Format(@"{0}\iVMS-4200Client\EncodeDevice",
                                                         Environment.CurrentDirectory);

        private delegate void LinstenEncodeDevice();

        private event LinstenEncodeDevice OnEncodeDevice;

        protected virtual void OnOnEncodeDevice()
        {
            LinstenEncodeDevice handler = OnEncodeDevice;
            if (handler != null) handler();
        }


        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetFileName(string path)
        {
            if (File.Exists(path))
                throw new Exception(path + "文件不存在!");
  
                if (path.Contains("\\"))
                {
                    string[] arr = path.Split('\\');
                    return arr[arr.Length - 1];
                }
                else
                {
                    string[] arr = path.Split('/');
                    return arr[arr.Length - 1];
                }
        }

       
        public ConvertOfFileExtention()
        {
            try
            {
                GetFex();
                OnEncodeDevice += GetFex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
          
        }

        /// <summary>
        /// 添加.db后缀名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void GetFex()
        {

            FileInfo fileInfo = new FileInfo(_devicePath);
            if (puff < fileInfo.Length)
            {
                 fileInfo.CopyTo(fileInfo.FullName + ".db",true);
                puff = fileInfo.Length;
            }
           
        }
    }
}