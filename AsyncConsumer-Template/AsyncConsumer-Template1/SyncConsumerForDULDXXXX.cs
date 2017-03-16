using System.Collections.Generic;
using System;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using Messaging.EMS.SyncMessageConsumers;
using Messaging.EMS.Producers;
using Messaging.EMS;
using TIBCO.EMS;

namespace ConsumerTemplate
{
    class SyncConsumerForDULDXXXX : SyncMessageConsumerUsingReceieve
    {

        public SyncConsumerForDULDXXXX(string EMSQueueName, int NoOfSessions , Boolean KeepAlive) : base(EMSQueueName, NoOfSessions , KeepAlive) { }
        public  override void ReceiveAndProcessMessage(Message msg)
        {


            /*  
             * Herein will lie the call to AMT report , to pass on  the msg consumed from EMS Queue .
             * Also, any response returned from AMT report call can be sent back to requestor 
             * as the reply-queue and correlation-id are avaible as part of JMS Header of msg . 
             * Not to be confused with void return .Also bear in mind , as many max. concurrent 
             * invocations of Onmessage will be allowed ,as specfied in NoOfSessions parameter passed to 
             * constructor as shown above.In other words ,If NoOfSessions=5 , 5  threads will be 
             * created to excute as many instances of ReceiveAndProcessMessage, at a time.
             * In order to mimic a simple Receive() function thats anyway called in the parent , please
             * set NoOfSessions=1 and KeepAlive=false
             * 
           */
            try
            {

                Messaging.EMS.CommonUtils.ProcessEMSMessage(msg);

            }

            catch (EMSException e)
            {
                Console.Write("\n\n(******** Exception :\n" + e.StackTrace);
                /*Put any more Exception Handling code here before you acknowledge the message  */
                msg.Acknowledge();
            }
        }
    }
}
