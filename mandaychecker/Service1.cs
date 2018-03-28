using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

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

            string working_time_str=(DateTime.Now-startdate).TotalHours.ToString() ;
            eventLog.WriteEntry("作業を終了しました " +working_time_str + "人時");
            using (TcpClient report_client=new TcpClient("192.168.11.11",50000))
            {
                //時間をバイナリで送ってもいいけど可読性重視で文字として
                Byte[] working_time_bytes=System.Text.Encoding.ASCII.GetBytes(working_time_str);
                report_client.GetStream().Write(working_time_bytes,0,working_time_bytes.Length);
            }
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
