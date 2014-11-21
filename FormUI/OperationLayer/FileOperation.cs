using System;
using System.IO;

namespace FormUI.OperationLayer
{
    public class FileOperation
    {
        private static readonly string _picLute = string.Format("{0}\\Picture\\", Environment.CurrentDirectory);

        public static void SaveFile(string srcFile)
        {
            if (File.Exists(_picLute))
            {
                File.Delete(_picLute);
            }
            if (!File.Exists(_picLute))
            {
                Directory.CreateDirectory(_picLute);
            }


            string desPic = _picLute + "BackGroundPic.jpg";
            File.Copy(srcFile, desPic, true);
        }

        public static string GetFile()
        {
            string[] files = Directory.GetFileSystemEntries(_picLute);
            string desFile = null;
            foreach (string file in files)
            {
                desFile = file;
            }
            return desFile;
        }
    }
}