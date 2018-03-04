using System;
using System.Diagnostics;
using System.Threading;

namespace Network_Speed_Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            int sleepTime;
            if (args.Length >= 1 && int.TryParse(args[0], out sleepTime))
            {
                // nothing to do here (don't know a better alternative)
            }
            else
            {
                sleepTime = 1; // default sleep time is 1 second
            }
            sleepTime = sleepTime * 1000; // convert seconds to milliseconds

            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[1]; // in my system the lan is the second one [1], usually it's the first one [0]

            // this lists all available network interfaces. To select the first net interface, you do: GetInstanceNames()[0], for 2nd [1] and so on...
            //string[] instances = performanceCounterCategory.GetInstanceNames();
            //foreach (string name in instances)
            //{
            //    Console.WriteLine(name);
            //}

            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            float currentSent, currentReceived;

            // initialize counters so that the first value won't be 0
            performanceCounterSent.NextValue();
            performanceCounterReceived.NextValue();
            Thread.Sleep(1000); // we need 1000ms if we want the next value to be accurate

            while (true)
            {
                currentSent = performanceCounterSent.NextValue();
                currentReceived = performanceCounterReceived.NextValue();
                Console.WriteLine(DateTime.Now.ToShortTimeString() + " - " + (currentReceived / 1024).ToString("#.##") + " KB/s");
                Thread.Sleep(sleepTime);
            }
        }
    }
}