using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;
namespace Messaging.EMS.Producers
{
   public  class Messageproducer
    {

       public Queue  GetReplyQueue (Message msg)

       { return (Queue)msg.ReplyTo;  }

       public static void  SendMessage ( TIBCO.EMS.Message  msg , TIBCO.EMS.Queue  queue) 

       {


           try
           {
               Session session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.CLIENT_ACKNOWLEDGE);

               TIBCO.EMS.MessageProducer producer = session.CreateProducer(queue);

               producer.Send(msg);
           }
           catch (EMSException e)


           { throw e; }
           
       }
       public static void SendReplyMessage(TIBCO.EMS.Message msg, TIBCO.EMS.Queue ReplyQueue, ref Session session)
       {
          

           try
           {
               TIBCO.EMS.MessageProducer producer = session.CreateProducer(ReplyQueue);

               producer.Send(msg);
           }
           catch (EMSException e) { throw e; }
       }

       public static void SendReplyMessage(TIBCO.EMS.Message msg, string ReplyQueueName)
       {
           try
           {
               Session session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);
               Queue ReplyQueue = session.CreateQueue(ReplyQueueName);
               Messaging.EMS.Producers.Messageproducer.SendReplyMessage(msg, ReplyQueue, ref session);
           }
           catch (EMSException e) { throw e; }

       }

       public static void SendReplyMessage(TIBCO.EMS.Message ReplyMessage, TIBCO.EMS.Queue ReplyQueue )
       {
           try {
           Session session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);
         Messaging.EMS.Producers.Messageproducer.SendReplyMessage(ReplyMessage, ReplyQueue, ref session);
                    
   }  
       catch (EMSException e) { throw e; }
       }
       public static void SendReplyMessage(TIBCO.EMS.Message ReplyMessage)
       {
           try
           {
               Session session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);
                
               Messageproducer.SendReplyMessage(ReplyMessage, (Queue)ReplyMessage.ReplyTo, ref session);

           }
           catch (EMSException e) { throw e; }
       }
       public static Message SendRequestAndReceiveReply(TIBCO.EMS.Message msg, TIBCO.EMS.Queue queue)
       {

           try
           {
               
               QueueSession session = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateQueueSession(false, Session.AUTO_ACKNOWLEDGE);
               TIBCO.EMS.QueueRequestor qr = new TIBCO.EMS.QueueRequestor(session, queue);
               Message ReplyMessage = qr.Request(msg);
               qr.Close();
               return ReplyMessage ;

           }
           catch (EMSException e) {   throw e; }

         
       }

          }
}
