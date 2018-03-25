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

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnShutdown()
        {
            this.AutoLog = false;
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource( "MySource", "MyLog");
            }
            EventLog eventLog1 = new EventLog();
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyLog";
            eventLog1.WriteEntry("OnShutdown");

        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            this.AutoLog = false;
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource( "MySource", "MyLog");
            }
            EventLog eventLog1 = new EventLog();
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyLog";
            eventLog1.WriteEntry(powerStatus.ToString());
            return true;
        }
    }
}
