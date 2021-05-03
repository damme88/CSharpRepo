using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;

namespace BTea
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Tlog.GetInstance().Init();
            Tlog.GetInstance().WriteLog("Bat dau chay chuong trinh");

            base.OnStartup(e);

            string MSQLName =  ConfigurationManager.AppSettings["mysqlname"].ToString();
            ServiceController service = new ServiceController(MSQLName);

            try
            {
                bool bStop = service.Status.Equals(ServiceControllerStatus.Stopped);
                bool bPending = service.Status.Equals(ServiceControllerStatus.StopPending);
                if (bStop || bPending)
                {
                    service.Start();
                }
            }
            catch(Exception ex)
            {
                Tlog.GetInstance().WriteLog("Cong ket noi co van de: " + ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
