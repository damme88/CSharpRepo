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
            base.OnStartup(e);

            ServiceController service = new ServiceController("MySQL80");
            bool bStop = service.Status.Equals(ServiceControllerStatus.Stopped);
            bool bPending = service.Status.Equals(ServiceControllerStatus.StopPending);
            if (bStop || bPending)
            {
                service.Start();
            }
        }
    }
}
