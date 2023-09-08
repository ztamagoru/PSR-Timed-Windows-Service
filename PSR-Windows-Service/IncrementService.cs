using System;
using System.ComponentModel;
using System.Timers;
using System.IO;
using System.ServiceProcess;

namespace IncrementService
{
    public class IncrementService : ServiceBase
    {
        private Timer timer;
        private int counter;
        private string path = System.Reflection.Assembly.GetExecutingAssembly()
               .Location + @"\..\..\..\..\PSR-Windows-Service\Resources\counter.txt";
        private DateTime schedule;

        public IncrementService()
        {
            this.ServiceName = "IncrementService";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            counter = 0;

            schedule = new DateTime(2023, 9, 8, 12, 0, 0);
            //año - mes - día - hora - min - seg

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            TimeSpan inicialInterval = schedule - DateTime.Now;

            timer = new Timer();

            timer.Interval = inicialInterval.TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            schedule = schedule.AddHours(1);
            TimeSpan interval = schedule - DateTime.Now;
            timer.Interval = interval.TotalMilliseconds;

            counter++;

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(counter);
            }
        }
    }
}
