using System;
//using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using TIBCO.EMS;
using Messaging.EMS.AsyncMessageConsumers;
using Messaging.EMS.Producers;
using Messaging.EMS.Test;

namespace ConsumerTemplate
{


    partial class ConsumerForDULDXXXX_UsingDelegate : AsyncMessageConsumerUsingDelegate
    {


        public ConsumerForDULDXXXX_UsingDelegate(string QueueName, int NoOfSessions) : base(QueueName, NoOfSessions) {  }


        public override void HandleMessage(object sender, EMSMessageEventArgs arg)
        {

            /*  
             * Herein will lie the call to AMT report , to pass on  the msg consumed from EMS Queue .
             * Also, any response returned from AMT report call can be sent back to requestor 
             * as the reply-queue and correlation-id are avaible as part of JMS Header of msg . 
             * Not to be confused with void return .Also bear in mind , as many max. concurrent 
             * invocations of Onmessage will be allowed ,as specfied in NoOfSessions parameter passed to 
             * constructor as shown above.In other words ,If NoOfSessions=5 , 5  threads will be 
             * created to excute as many instances of HandleMessage, at a time.
             */
            try
            {
                Message msg = arg.Message;
                Messaging.EMS.Test.CommonUtils.ProcessEMSMessage(msg);
                //ConsumersCreater.processMessage(msg);
                msg.Acknowledge();
            }


          /*   string separator1 = "\n*********************************************************************************************************************************";
             Console.WriteLine(separator1);
             string separator2 = "\n\n*********************************";
             try
             {
                 Console.WriteLine(separator2 + " Message Received by the Thread \"" + System.Threading.Thread.CurrentThread.Name + "\":\n   " + msg.ToString()+
                     "\n Sent by:"+sender.ToString());

                 if (msg.ReplyTo != null)
                 {

                     Session session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);

                     string LogAndSend = "\n\nEchoing back the Message Received:" + msg.ToString() +

                         "\n\n ReplyingProcessName: " + Process.GetCurrentProcess().ProcessName +
                         "\n ReplyingProcessId: " + Process.GetCurrentProcess().Id +
                         "\n ThreadName:\"" + System.Threading.Thread.CurrentThread.Name + "\"" +
                         "\n ClientId: " + Messaging.EMS.Connection.EMSQueueConnection.connection.ClientID +
                         "\n New Producer SessionId used for reply: " + session.SessID +
                         "\n\n";



                     TIBCO.EMS.TextMessage reply = session.CreateTextMessage();
                     reply.Text = LogAndSend;
                     Console.Write(separator2 + " This Reply has been sent:   " + LogAndSend + " \n\n ");
                     Messaging.EMS.Producers.Messageproducer.SendReplyMessage(reply, (TIBCO.EMS.Queue)msg.ReplyTo);
                 }





                 else
                 {

                     Console.WriteLine("\n********************************* No reply sought for above  message as ReplyQueue is NUll . Threadname is \"" +

                       System.Threading.Thread.CurrentThread.Name + "\"\n\n");
                 }
                 Console.WriteLine(separator1);
                 msg.Acknowledge();
             } */

            catch (EMSException e)
            {
                Console.Write("\n\n******** Exception :\n" + e.StackTrace);
                arg.Message.Acknowledge();
            }

        }











    }
}
