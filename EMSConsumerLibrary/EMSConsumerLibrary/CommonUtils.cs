using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using TIBCO.EMS.ADMIN;
using TIBCO.EMS;


namespace Messaging.EMS.Test
{
    public class CommonUtils
    {

        static public void ProcessEMSMessage(Message msg) {
          

            string separator1 = "\n*********************************************************************************************************************************";
            Console.WriteLine(separator1);
            string separator2 = "\n===================";
            try
            {


                Console.WriteLine(separator2 + " Message Received by the Thread \"" + Thread.CurrentThread.Name + "\":\n   " + msg.ToString());
                StackTrace stackTrace = new StackTrace();
                String methodcallingsequence = ""; int methodnumber = 1;
                Session ProducerSession = Messaging.EMS.Connection.EMSQueueConnection.connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);
                foreach (StackFrame sf in stackTrace.GetFrames())
                {

                    methodcallingsequence += "\n\t\t ("+methodnumber++.ToString() +") " + sf.GetMethod().ToString();
                }
                if (msg.ReplyTo != null)
                {
                    
                    

                        
                      
                    string LogAndSend = "\n\nReplying with .NET runtime specific info :\n"  +
                        "\n\n MachineName: " + Environment.MachineName +
                        "\n ReplyingProcessName: " + Process.GetCurrentProcess().ProcessName +
                        "\n ReplyingProcessId: " + Process.GetCurrentProcess().Id +
                        "\n ThreadName:\"" + Thread.CurrentThread.Name + "\"" +
                        "\n ClientId: " + Messaging.EMS.Connection.EMSQueueConnection.connection.ClientID +
                        "\n New Producer SessionId used for reply: " + ProducerSession.SessID +
                        "\n MethodCallingSequence /*Most Recent at the top*/:\n" + methodcallingsequence 
                        ;

                    TIBCO.EMS.TextMessage reply = ProducerSession.CreateTextMessage();
                    reply.Text = LogAndSend; 
                    reply.ReplyTo = msg.ReplyTo;
                    
                    Messaging.EMS.Producers.Messageproducer.SendReplyMessage(reply, (TIBCO.EMS.Queue)msg.ReplyTo, ref ProducerSession);
                    Console.Write(separator2 + " This Reply has been sent:   " + LogAndSend + " \n\n ");

                }



                else
                {

                    Console.WriteLine(separator2 + " No reply sought for above  message as ReplyQueue is NUll . Threadname is \"" +

                      Thread.CurrentThread.Name + "\"" +
                        "\n\n MethodCallingSequence /*Most Recent at the top*/ :\n" + methodcallingsequence);
                }
                Console.WriteLine(separator1);
                msg.Acknowledge();
            }

            catch (EMSException e)
            {
                Console.Write("\n\n(******** Exception :\n" + e.StackTrace);
                msg.Acknowledge();
            }
        }
        public static void printConsumers(String QueueName)
        {
            /*
             *This will create a seperate Admin Connection to EMS .A normal EMS client may not be given
             *EMS admin credentials , hence this function is not for everyone except Admin
             * 
             */
            try
            {

                QueueInfo queue = new QueueInfo(QueueName);
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["EMSServerURL"].ConnectionString;
                string user = System.Configuration.ConfigurationManager.ConnectionStrings["username"].ConnectionString; ;
                string password = System.Configuration.ConfigurationManager.ConnectionStrings["password"].ConnectionString;
                Admin admin = new Admin(url, user, password);
                ConsumerInfo[] cia = admin.GetConsumers(null, user, queue, false, 0);
                System.Console.WriteLine("\n");
                string ConsumerInfoString = "";

                foreach (ConsumerInfo ci in cia)
                {
                    ConsumerInfoString += "\n----------------------------------------------------------\n  ConnectionId : " + ci.ConnectionID +
                        "\n\n  ConsumerID : " + ci.ID +
                        "\n\n  SessionId : " + ci.SessionID +
                        "\n\n  DestinationName : " + ci.DestinationName +
                        "\n\n  ThreadName : " + Thread.CurrentThread.Name;
                }

                System.Console.WriteLine(ConsumerInfoString);

                admin.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Occured:" + e.StackTrace.ToString());

            }
        }
    }

        
        
        }



