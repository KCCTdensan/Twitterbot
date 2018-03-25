using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace mandaychecker
{
    public partial class mandaychecker : ServiceBase
    {
        public mandaychecker()
        {
            InitializeComponent();
        }

        EventLog eventLog = new EventLog();

        protected override void OnStart(string[] args)
        {
            if (!EventLog.SourceExists("MandayChecker"))
            {
                EventLog.CreateEventSource("MandayChecker", "MandayChecker");
            }
            eventLog.Source = "MandayChecker";
            eventLog.Log = "MandayChecker";

            eventLog.WriteEntry("作業を開始しました");
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("サービスを停止しました");
        }

        protected override void OnShutdown()
        {
            eventLog.WriteEntry("シャッドダウンしました");

        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            eventLog.WriteEntry(powerStatus.ToString()  + "しました");
            return true;
        }
    }
}
