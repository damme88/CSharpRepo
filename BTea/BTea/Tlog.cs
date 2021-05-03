using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTea
{
    class Tlog
    {
        private static Tlog _instance;
        private string _logFilename;
        private string _logFilePath;
        public static Tlog GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Tlog();
            }
            return _instance;
        }

        public Tlog()
        {
        }
        public void Init()
        {
            _logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _logFilePath += "\\BTea";
            if (Directory.Exists(_logFilePath) == false)
            {
                Directory.CreateDirectory(_logFilePath);
            }

            _logFilename = "\\Tlog.log";
            string logPath = _logFilePath + _logFilename;
            if (File.Exists(logPath) == true)
            {
                File.Delete(logPath);
            }

            File.Create(logPath).Close();

            using (StreamWriter w = File.AppendText(logPath))
            {
                w.Write("================ BTea Log ===============");
            }
        }

        public void WriteLog(string msg)
        {
            if (_logFilePath != null && _logFilename != null)
            {
                string logPath = _logFilePath + _logFilename;
                using (StreamWriter w = File.AppendText(logPath))
                {
                    w.WriteLine("");
                    string strTime = DateTime.Now.ToLongTimeString();
                    string strDate = DateTime.Now.ToLongDateString();
                    string strLog = strTime + ":" + strDate + " : " + msg;
                    w.WriteLine(strLog);
                }
            }
            
        }
    }
}
