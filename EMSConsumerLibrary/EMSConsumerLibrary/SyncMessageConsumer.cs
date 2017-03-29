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
       public SyncMessageConsumerUsingReceieve(string EMSQueueName, int NoOfSessions, Boolean KeepAlive)
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
        if (!KeepAlive) break; /* With NoOfSessions=1 and KeepAlive=false , achieves effect of a single Receive call that
                                * returns as and when message is received. NoOfSessions > 1 and KeepAlive=false  achieves concurrent
                                * consumption thats one-time i.e as soon as processing is completed ,so is the thread. 
                                * Sync. consumer will be required when a flow triggered by some non-ems mechanism/protocol needs to 
                                * fetch messages off EMS . Most likely, NoOfSessions=1 and KeepAlive=false  in this sceanrio.
                                * Main thread does not wait for the threads to finish , however multiple receivers/consumers so created 
                                * will be destroyed all at once and not one-by-one.
                                */
}

   
}

    );

            t1.Start();


    
                
            }


                      
        }
       public SyncMessageConsumerUsingReceieve(string EMSQueueName, int NoOfSessions, int Timeout)
       {
           for (int i = 0; i < NoOfSessions; i++)
           {
               QueueSession session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateQueueSession(false, QueueSession.CLIENT_ACKNOWLEDGE);
               Queue queue = session.CreateQueue(EMSQueueName);
               QueueReceiver receiver = session.CreateReceiver(queue);
               Message ReceivedMessage;


               Thread t1 = new Thread(() =>
               {
                   Thread.CurrentThread.Name = "Explicit Thread for Session-" + session.SessID;
                   while ((ReceivedMessage = receiver.Receive(Timeout)) != null)
                   {

                       this.ReceiveAndProcessMessage(ReceivedMessage);
                        /* With NoOfSessions=1 and KeepAlive=false , achieves effect of a single Receive call that
                                * returns as and when message is received. NoOfSessions > 1 and KeepAlive=false  achieves concurrent
                                * consumption thats one-time i.e as soon as processing is completed ,so is the thread. 
                                * Sync. consumer will be required when a flow triggered by some non-ems mechanism/protocol needs to 
                                * fetch messages off EMS . Most likely, NoOfSessions=1 and KeepAlive=false  in this sceanrio.
                                * Main thread does not wait for the threads to finish , however multiple receivers/consumers so created 
                                * will be destroyed all at once and not one-by-one.
                                */
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
