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

        EventLog eventLog;

        DateTime startdate;

        void start_working()
        {
            startdate = DateTime.Now;
            eventLog.WriteEntry("作業を開始しました");
        }

        void stop_working()
        {
            eventLog.WriteEntry("作業を終了しました " + (DateTime.Now-startdate).TotalHours + "人時");
        }

        protected override void OnStart(string[] args)
        {
            if (!EventLog.SourceExists("MandayCheckerSource"))
            {
                EventLog.CreateEventSource("MandayCheckerSource", "MandayCheckerLog");
            }
            eventLog = new EventLog();
            eventLog.Source = "MandayCheckerSource";
            eventLog.Log = "MandayCheckerLog";

            start_working();
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("サービスを停止しました");
        }

        protected override void OnShutdown()
        {
            stop_working();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            eventLog.WriteEntry(powerStatus.ToString() + "しました");
            switch (powerStatus)
            {
                case PowerBroadcastStatus.ResumeSuspend:
                    start_working();
                    break;
                case PowerBroadcastStatus.Suspend:
                    stop_working();
                    break;
            }
            return true;//今起きているイベントを続行
        }
    }
}
