using System.Diagnostics;

namespace monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string processName = args[0];
            int maxTTL = Convert.ToInt32(args[1]);
            int monitoringFreq = Convert.ToInt32(args[2]);

            var timer = new System.Threading.Timer((e) => { StartMon(processName, maxTTL); }, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(monitoringFreq));

            while (true)
            {
                Console.WriteLine("Exit: q : ");
                var key = Console.ReadKey().KeyChar;
                if (key == 'q')
                {
                    timer.Dispose();
                    break;
                }
            }
        }

        private static void StartMon(string processName, int maxLifeTime)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process i in processes)
            {
                TryToKillProcess(i, maxLifeTime);
            }
        }

        private static bool TryToKillProcess(Process process, int maxLifeTime)
        {
            TimeSpan executionTime = DateTime.Now.Subtract(process.StartTime);
            if (executionTime.TotalMinutes >= maxLifeTime)
            {
                Log("Kill: " + process + " " + executionTime.TotalMinutes);
                process.Kill();
                return true;
            }
            else
            {
                Log("Not Kill: " + process + " " + executionTime.TotalMinutes);
                return false;
            }
        }

        private static void Log(string msg)
        {
            File.AppendAllText("log.txt", DateTime.Now + " " + msg + Environment.NewLine);
        }
    }
}