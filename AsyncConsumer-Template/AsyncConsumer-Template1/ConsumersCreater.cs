using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using TIBCO.EMS;
using Messaging.EMS.Connection;

namespace ConsumerTemplate
{ 
    class ConsumersCreater
    {
        
        static void Main (string[] args)
        {
          
 
            Process CurrentProcess = Process.GetCurrentProcess();
          /*  ProcessThreadCollection ptc = CurrentProcess.Threads;
            foreach (ProcessThread pt in ptc)
            {

                Console.WriteLine("\nTHread=" + pt.ToString() + "  ThreadiD=" +pt.Id);
            }
            Console.WriteLine("\nDomain=" + Thread.GetDomain().FriendlyName + "\nexeAssembly =" + Assembly.GetEntryAssembly().FullName);*/

            String MainThreadName = Environment.MachineName + "_" + CurrentProcess.ProcessName + "_" + CurrentProcess.Id;

            Thread.CurrentThread.Name = MainThreadName;

            string QueueName = System.Configuration.ConfigurationManager.AppSettings["QueueName"];

            int NoOfSessions = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NoOfSessions"]);

            switch (args[0] )
            {
                case "Async-Delegate" :
                    new ConsumerForDULDXXXX_UsingDelegate(QueueName, NoOfSessions);
                    break;
                case "Async-Listener" :
                    new ConsumerForDULDXXXX(QueueName, NoOfSessions);
                    break;
                case "Sync-Receive":
                    new SyncConsumerForDULDXXXX(QueueName,NoOfSessions, false);
                    break;
                case "Async-Both":
                    if (NoOfSessions < 2)
                    { 
                    Console.WriteLine("For both Async types the NoOfSessions should be >= 2.Check the config.");
                    Environment.Exit(-1);
                    }

                    new ConsumerForDULDXXXX(QueueName,  (int)Math.Ceiling((double)NoOfSessions / 2));

                    new ConsumerForDULDXXXX_UsingDelegate(QueueName, (int)Math.Floor((double)NoOfSessions / 2));

                    break;
                default :
                    Console.WriteLine("The Consumption Mechanism has to be Async-Delegate,Async-Listener,Async-Both or Sync-Receive ");
                    Environment.Exit(-1);
                    break;

            }

            Messaging.EMS.CommonUtils.printConsumers(QueueName);

            

  
        }


    }
}
