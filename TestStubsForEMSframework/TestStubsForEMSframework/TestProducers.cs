using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using TIBCO.EMS;
using Messaging.EMS.Producers;
using Messaging.EMS.Connection;
namespace TestStubsForEMSframework
{
    public class TestProducers
    {
       static TextMessage msg;

                static void Main(string[] args)
        {
             if(args.Length==0) 
                    {  
                 Console.WriteLine("\n  The Message Producing Pattern has to be either FF or RR\n"+ "  Usage:"+

                 Process.GetCurrentProcess().ProcessName + "  FF(or RR)  CorrelationID");

                 Environment.Exit(-1);
             }

            try
            {
                
                QueueSession session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateQueueSession(false, Session.AUTO_ACKNOWLEDGE);
                
                Queue queue = session.CreateQueue(System.Configuration.ConfigurationManager.AppSettings["QueueName"]);
               
                Process CurrentProcess = Process.GetCurrentProcess();
                
                String MainThreadName = Environment.MachineName + "_" + CurrentProcess.ProcessName + "_" + CurrentProcess.Id;
                Thread.CurrentThread.Name = MainThreadName;
                
                msg = (TextMessage)session.CreateTextMessage("hellothere-");
                
                string Text =  
        "\n\n MachineName : " + Environment.MachineName +
        "\n SendingProcessName : " + Process.GetCurrentProcess().ProcessName +
        "\n SendingProcessId : " + Process.GetCurrentProcess().Id +
        "\n Method : "+System.Reflection.MethodBase.GetCurrentMethod().Name +
        "\n ThreadName:\"" + Thread.CurrentThread.Name + "\"" +
        "\n ClientId: " + Messaging.EMS.Connection.EMSQueueConnection.connection.ClientID ;
                string seperator = "\n\n*****************************************************************************************\n";
             
               msg.Text += Text;
               try { msg.CorrelationID = (args[1]); } /*Absorb this AOOB exception :) */ catch (Exception e) { };


               switch (args[0])
               {
                   case "FF":
                       Console.WriteLine(seperator + "About to Fire (and Forget ) :\n\n" + msg.ToString());   
                       Messaging.EMS.Producers.Messageproducer.SendMessage(msg, queue);
                       break;

                   case "RR":
                       Console.WriteLine(seperator + "About to send request:\n\n" + msg.ToString());
                       Message reply = Messaging.EMS.Producers.Messageproducer.SendRequestAndReceiveReply(msg, queue);
                       if (reply is TextMessage) Console.WriteLine(seperator + "Received Response:\n\n" + reply.ToString());
                       break;

                   default:
                       Console.WriteLine("The Message Producing Pattern has to be either FF or RR  ");
                       Environment.Exit(-1);
                       break;

               }
 
             

              
 
               // Messaging.EMS.Producers.Messageproducer.SendMessage(msg, queue);


                
                
                 
                        
            }
            catch (Exception e) { Console.WriteLine("\n\n*********Exception thrown:\n" + "\n\n***********"+e.StackTrace+e.ToString()); }
   
        }
    }
}
