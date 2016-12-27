#define SSL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TIBCO.EMS;

using Messaging.EMS.Connection;

namespace Messaging.EMS.SyncMessageConsumers
{
   public class SyncMessageConsumerUsingReceieve
    {

        //protected String EMSQueueName;
        //protected int  NoOfSessions;
        public SyncMessageConsumerUsingReceieve (string EMSQueueName,int  NoOfSessions )
        {
                      for (int i = 0; i < NoOfSessions ;i++)
            {
            QueueSession session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateQueueSession(false, QueueSession.CLIENT_ACKNOWLEDGE);
            Queue queue = session.CreateQueue(EMSQueueName);
            QueueReceiver receiver = session.CreateReceiver(queue);
            Message ReceivedMessage;
                          
                          
            Thread t1 = new Thread(() =>
{
    Thread.CurrentThread.Name = "Explicit Thread for Session-" + session.SessID;
    while ((ReceivedMessage = receiver.Receive()) != null   )
    {
   
        this.ReceiveAndProcessMessage(ReceivedMessage);
        
    }

   
}

    );

            t1.Start();


    
                
            }
        }

     public virtual   void ReceiveAndProcessMessage(Message msg)
        { }
    }
}
