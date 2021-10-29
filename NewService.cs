using System;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;

namespace PGLRefresher
{
    public partial class NewService : ServiceBase
    {
        #region Definitions
        private static string url = "http://url.here";
        private static string url2 = "http://url.here";
        private static readonly Timer timer = new Timer();

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
        #endregion

        public NewService()
        {
            InitializeComponent();
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);


        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Set the timer's interval to every 60 seconds
            timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
            timer.AutoReset = true;

            // Set to invoke the function when the timer's reached max count
            timer.Elapsed += Function;

            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
        }

        #region Main Function
        private static void Function(object sender, EventArgs e)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(url);
                    string s2 = client.DownloadString(url2);
                }

                Log.Write("PGL.txt and PGL_Users.txt refreshed.", Log.MessageType.INFO);
            }
            catch (Exception ex)
            {
                Log.Write(ex.ToString(), Log.MessageType.ERROR);
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion
    }
}
