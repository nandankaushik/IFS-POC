#define SSL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;

using Messaging.EMS.Connection;



namespace Messaging.EMS.AsyncMessageConsumers
{
    

    public class AsyncMessageConsumerUsingMessageListener : TIBCO.EMS.IExceptionListener, TIBCO.EMS.IMessageListener
    {

       protected string QueueName;
       protected int NoOfSessions;
     



        public AsyncMessageConsumerUsingMessageListener(String QueueName, int NoOfSessions)
        {

            EMSQueueConnection.connection.Stop();
            this.QueueName = QueueName;
            this.NoOfSessions = NoOfSessions;

            TIBCO.EMS.Session session = null;
            TIBCO.EMS.MessageConsumer msgConsumer = null;
            TIBCO.EMS.Destination destination = null;
    
            try
            {

             
                for (int i = 0; i < NoOfSessions; i++)
                {
                    /*
                      Create a new session which in turn creates a thread interanlly .
                     * Ack mode is hard coded for now , see no possibulty for  it being other than CLient_ack*/
                    session = EMSQueueConnection.connection.CreateSession(false, Session.CLIENT_ACKNOWLEDGE);
                                    
                   // create the consumer
                    if (destination == null) destination = session.CreateQueue(QueueName);
                      msgConsumer = session.CreateConsumer(destination);
                   
                    // set the message listener
                    msgConsumer.MessageListener = this;
                  
                    /*
                      Console.WriteLine("\n Subscribing to destination: " + QueueName);Console.WriteLine("************************************************\n ThreadName:" 
                         + System.Threading.Thread.CurrentThread.Name + "\n  Session:" + session.ToString()

             + "\n  SessionID:" + session.SessID + "\n  Connection:" + session.Connection.ToString() +
             "\n  MessageConsumer:" + msgConsumer.ToString()  );*/


                }

                EMSQueueConnection.connection.Start();
               // Start pub-sub messages
            
       


            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception in AsyncMsgConsumer: " +e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }




        public virtual void OnException(EMSException e)
        {}

        public virtual void OnMessage(Message msg)
        {}
        //public void printConsumers()
        //{
        //    /*
        //     *This will create a seperate Admin Connection to EMS .
        //     * 
        //     */
        //    try
        //    {

        //        QueueInfo queue = new QueueInfo(QueueName);
        //        string url = System.Configuration.ConfigurationManager.ConnectionStrings["EMSServerURL"].ConnectionString;
        //        string user = System.Configuration.ConfigurationManager.ConnectionStrings["username"].ConnectionString; ;
        //        string password = System.Configuration.ConfigurationManager.ConnectionStrings["password"].ConnectionString;
        //        Admin admin = new Admin(url, user, password);
        //        ConsumerInfo[] cia = admin.GetConsumers(null, user, queue, false, 0);
        //        System.Console.WriteLine("\n");
        //        string ConsumerinfoString = "";

        //        foreach (ConsumerInfo ci in cia)
        //        {
        //            ConsumerinfoString += "\n*********************************\n  ConnectionId:" + ci.ConnectionID +
        //                "\n  ConsumerID:" + ci.ID +
        //                "\n  SessionId:" + ci.SessionID +
        //                "\n  DestinationName:" + ci.DestinationName +
        //                "\n  ThreadName:" + System.Threading.Thread.CurrentThread.Name;
        //        }

        //        System.Console.WriteLine(ConsumerinfoString);

        //        admin.Close();

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception Occured:"+ e.StackTrace.ToString());
                
        //    }
        //}

    }
    public class AsyncMessageConsumerUsingDelegate
    {
        protected string QueueName;
        protected int NoOfSessions;


       // object stateLock = new object();        bool stop = false;

        public AsyncMessageConsumerUsingDelegate(String QueueName , int NoOfSessions)
        {
            this.QueueName = QueueName;
            this.NoOfSessions = NoOfSessions;

            Session session = null;
            MessageConsumer msgConsumer = null;
            Destination destination = null;

            try
            {


                // Use  the static connection


                for (int i = 0; i < NoOfSessions; i++)
                {
                    // create the session
                    session = EMSQueueConnection.connection.CreateSession(false, Session.CLIENT_ACKNOWLEDGE);

                    

                    if (destination == null)
                    destination = session.CreateQueue(QueueName);

                    // create the consumer
                    msgConsumer = session.CreateConsumer(destination);
                  

                    // add the message listener
                    
                    msgConsumer.MessageHandler += new EMSMessageHandler(HandleMessage);
                    
                    /*   Console.WriteLine("Subscribing to destination: " + QueueName); 
                     * Console.WriteLine("\n ThreadName:" + System.Threading.Thread.CurrentThread.Name + " \nSessionName:" + session.ToString()

  + "\nSessionID:" + session.SessID + " \nConnection:" + session.Connection.ToString() +
  "\n MEssageConsumer:" + msgConsumer.ToString());
                      */

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in AsyncMessageConsumerUsingDelegate: " +
                                  e.Message);
             
            }
        }




        public virtual void HandleMessage(object sender, EMSMessageEventArgs arg)
        {
            try
            {
                Message msg = arg.Message;
               /* Console.WriteLine("\n\nOn _HandleMessage \n********** ThreadName:" + 
                    System.Threading.Thread.CurrentThread.Name + " \nSessionName:" + session.ToString()
                + " \nSessionID:" + session.SessID + " \nConnection:" + session.Connection.ToString() + "\nReceived message: " + msg);
                */
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception message callback!");
                Console.WriteLine(e.Message);
            }
        }


    }



}
