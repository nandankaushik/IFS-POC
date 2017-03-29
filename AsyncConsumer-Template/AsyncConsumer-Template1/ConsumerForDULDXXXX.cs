using System;
//using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using TIBCO.EMS;
using Messaging.EMS;
using Messaging.EMS.AsyncMessageConsumers;
using Messaging.EMS.Producers;
namespace ConsumerTemplate
{

    partial class ConsumerForDULDXXXX : AsyncMessageConsumerUsingMessageListener
    {


        public ConsumerForDULDXXXX(string QueueName, int NoOfSessions): base(QueueName, NoOfSessions){ }

     
      public override void OnMessage(TIBCO.EMS.Message msg)
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
  
           
         try {


             Messaging.EMS.CommonUtils.ProcessEMSMessage(msg);
     
 }

            catch (EMSException e)
            { 
            Console.Write("\n\n(******** Exception :\n"+ e.StackTrace);
            msg.Acknowledge(); 
            }
          
        }

        public override void OnException(TIBCO.EMS.EMSException e)
        {

            /* Handle any Exception related to EMS Connection */
            Console.Write("\n\n******** Exception :\n" + e.StackTrace);



        }




      
      
      
      
      

    }
}
