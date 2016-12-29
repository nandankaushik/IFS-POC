using System.Collections.Generic;
using System;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using Messaging.EMS.SyncMessageConsumers;
using Messaging.EMS.Producers;
using Messaging.EMS.Test;
using TIBCO.EMS;

namespace ConsumerTemplate
{
    class SyncConsumerForDULDXXXX : SyncMessageConsumerUsingReceieve
    {

        public SyncConsumerForDULDXXXX(string EMSQueueName, int NoOfSessions) : base(EMSQueueName, NoOfSessions) { }
        public  override void ReceiveAndProcessMessage(Message msg)
        {


            /*  
             * Herein will lie the call to AMT report , to pass on  the msg consumed from EMS Queue .
             * Also, any response returned from AMT report call can be sent back to requestor 
             * as the reply-queue and correlation-id are avaible as part of JMS Header of msg . 
             * Not to be confused with void return .Also bear in mind , as many max. concurrent 
             * invocations of Onmessage will be allowed ,as specfied in NoOfSessions parameter passed to 
             * constructor as shown above.In other words ,If NoOfSessions=5 , 5  threads will be 
             * created to excute as many instances of OnMessage, at a time.
             
           */
            try
            {

                Messaging.EMS.Test.CommonUtils.ProcessEMSMessage(msg);

            }

            catch (EMSException e)
            {
                Console.Write("\n\n(******** Exception :\n" + e.StackTrace);
                msg.Acknowledge();
            }
        }
    }
}
