#define SSL
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;
using System.Threading;
using System.Configuration;
namespace Messaging
{

    namespace EMS
    {
        namespace Connection
        {
            public class EMSQueueConnection
            {

                /*  This would part of config. and not memebers of this class
                 * no extensibillty intended :)
                String EMSServerURL = null;
                String Username = null;
                String password = null; */


                public static QueueConnection connection;  /* The COnnection memeber is static because it has to be 
                                                            called by Static Construtor*/
                //public QueueConnectionFactory qcf;

                static void Initialize()
                {
#if SSL_Undefined
        try {
            tibemsUtilities.initSSLParams(serverUrl,args);
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Exception: "+e.Message);
            System.Console.WriteLine(e.StackTrace);
            System.Environment.Exit(-1);
        }
#endif

                }

                static EMSQueueConnection()  /* Static Constructor for creating Connection at Start-up */
                {
                  



                    try
                    {


                        string password = System.Configuration.ConfigurationManager.ConnectionStrings["password"].ConnectionString;
                        string Username = System.Configuration.ConfigurationManager.ConnectionStrings["username"].ConnectionString;
                        string ClientID = System.Configuration.ConfigurationManager.ConnectionStrings["Client-ID"].ConnectionString;
                        string EMSServerURL = System.Configuration.ConfigurationManager.ConnectionStrings["EMSServerURL"].ConnectionString;

                        QueueConnectionFactory qcf = new QueueConnectionFactory(EMSServerURL, ClientID);
                       
                        if (qcf == null) Console.WriteLine("\n\nQueueConnectionFactory creation failed :" );
                         

                        connection = qcf.CreateQueueConnection(Username, password);
                       connection.ExceptionHandler += new EMSExceptionHandler(_HandleException);
                        
                        connection.Start();
                    }
                    catch (Exception E)
                    {
                        Console.WriteLine("ERROR thrown in EMSQueueConnection Constructor  :" + E.ToString());
                        
                    }
                }
                public static void _HandleException(object sender, EMSExceptionEventArgs arg)
                {
                    EMSException e = arg.Exception;
                
                    // print the connection exception status

                    Console.WriteLine("\n\n********** On EMSExceptionEvent **********\n\n********** ThreadName:" + Thread.CurrentThread.Name +
                        "\n\n********** Exception Message: " + e.Message + "\n\n********** Sender Object:" + sender.ToString()
                        + "\n\n********** Stacktrace:" + e.StackTrace + "\n\n********** Source:"+e.Source
                         + "\n\n********** TargetSite:"+ e.TargetSite
                        );

                     
                }
       
            }
        }



    }

}
