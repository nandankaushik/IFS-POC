
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;
using System.Threading;
using System.Configuration;
using System.Security;
namespace Messaging
{

    namespace EMS
    {
        namespace Connection
        {
            public class EMSQueueConnection
            {

                /*  This would part of only config. and not memebers of this class
                 * no extensibillty/reusbilty  intended  yet
                String EMSServerURL = null;
                String Username = null;
                String password = null; */


                public static QueueConnection connection;  /* The Connection memeber is static because it has to be 
                                                            called by Static Constructor*/
                //public QueueConnectionFactory qcf;


          

                static EMSQueueConnection()  /* Static Constructor for creating Connection at Start-up */
                {

                    EMSSSL EMSSSL;
                    LookupContext LookupContext;
                    //    Configuration SSL = (Configuration)System.Configuration.ConfigurationManager.GetSection("SSL");
                        string password = System.Configuration.ConfigurationManager.ConnectionStrings["password"].ConnectionString;
                        string Username = System.Configuration.ConfigurationManager.ConnectionStrings["username"].ConnectionString;
                        string ClientID = System.Configuration.ConfigurationManager.ConnectionStrings["Client-ID"].ConnectionString;
                        string JNDIContextURL = System.Configuration.ConfigurationManager.ConnectionStrings["JNDIContextURL"].ConnectionString;
                        string QueueConnectionFactory = System.Configuration.ConfigurationManager.ConnectionStrings["QueueConnectionFactory"].ConnectionString;
                        
                        Hashtable Environment = new Hashtable();    
                        Environment.Add(LookupContext.SECURITY_PRINCIPAL, Username);
                        Environment.Add(LookupContext.SECURITY_CREDENTIALS, password);
                    //    Environment.Add(LookupContext.PROVIDER_URL, JNDIContextURL);
                    
                        /*SSL Connection creation  start  */
                        try
                        {
                            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
                            string TargetHostName = System.Configuration.ConfigurationManager.ConnectionStrings["TargetHostName"].ConnectionString;
                           
                          //  EMSSSL.SetTargetHostName(TargetHostName);
                            string JmsSslProviderUrl = System.Configuration.ConfigurationManager.ConnectionStrings["JmsSslProviderUrl"].ConnectionString;
                            string JNDIContextSSLURL = System.Configuration.ConfigurationManager.ConnectionStrings["JNDIContextSSLURL"].ConnectionString;
                            string EMSServercertiFicate = System.Configuration.ConfigurationManager.ConnectionStrings["EMSServercertiFicate"].ConnectionString;
                            string CA_Certificate = System.Configuration.ConfigurationManager.ConnectionStrings["CA-CertiFicate"].ConnectionString;
                            string ssl_trace = System.Configuration.ConfigurationManager.ConnectionStrings["ssl_trace"].ConnectionString;
                            string TrustStore = System.Configuration.ConfigurationManager.ConnectionStrings["TrustStore"].ConnectionString;
                           

                          
                         /* Standard practice is to specifiy the folder of certs to be individually imported in the TrustStore. Seen it in BW to 
                          * work better. Also supports incremental testing  by placing one cert at a time.The root cert should be in Windows Central store. 
                          * Preferably  left to support people to import the same Or  use EMSSSL class...
                          */
                            foreach ( string filename in System.IO.Directory.GetFiles(TrustStore))
                            {

                                try
                                {
                                    storeInfo.SetSSLTrustedCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate(filename));
                                }

                                catch (Exception E)
                                {
                                    Console.WriteLine("\nNot imported into the TrustStore:" + filename ); 
                                    /*replce this writeln  with Debug/Error logging using E object*/
                                                                       ;

                                }
                                    
                            }



                            storeInfo.SetSSLTrustedCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate(EMSServercertiFicate));

                       
                            Console.WriteLine("\n\n TargetHostName=" + TargetHostName + "\n CA_Certificate=" + CA_Certificate+ "\n EMSServercertiFicate=" + EMSServercertiFicate+"\n\n"); 
                        

                          /*  Environment.Add(EMSSSL.STORE_INFO, storeInfo);
                             Environment.Add(EMSSSL.STORE_TYPE, EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE);*/


                           
                              //  EMSSSL.SetClientTracer(new System.IO.StreamWriter(System.Console.OpenStandardOutput()));
                          
                            


                            Environment.Add(LookupContext.PROVIDER_URL, JmsSslProviderUrl);
                            Environment.Add(LookupContext.SSL_TRACE, ssl_trace);
                            Environment.Add(LookupContext.SSL_STORE_TYPE, EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE);
                            Environment.Add(LookupContext.SECURITY_PROTOCOL, "ssl");
                            Environment.Add(LookupContext.SSL_HOST_NAME_VERIFIER, new EMSSSLHostNameVerifier(new Messaging.EMS.CommonUtils().verifyHost));
                            Environment.Add(LookupContext.SSL_TARGET_HOST_NAME, TargetHostName);
                            Environment.Add(LookupContext.SSL_STORE_INFO, storeInfo); 
                            /*
                             * 
                             * 
                             */
                            //  EMSSSL.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_SYSTEM, storeInfo);
                            LookupContext = new LookupContext(Environment);

                   
                       
                            QueueConnectionFactory SSl_QCF = (QueueConnectionFactory)LookupContext.Lookup(QueueConnectionFactory);
                            //QueueConnectionFactory SSl_QCF = new QueueConnectionFactory(JmsSslProviderUrl, ClientID, Environment);
                           

                           /* 
                            Console.Write("\nSSl_QCF.GetCertificateStore().ToString()=" + SSl_QCF.GetCertificateStore().ToString()+"\n");
                          */
                            /*SSL Connection creation  end   */


                            /* string JNDIContextURL = System.Configuration.ConfigurationManager.ConnectionStrings["JNDIContextURL"].ConnectionString;

                             QueueConnectionFactory qcf = new QueueConnectionFactory(JNDIContextURL, ClientID);
                       
                             if (qcf == null) Console.WriteLine("\n\nQueueConnectionFactory creation failed :" );
                              */
                            SSl_QCF.SetTargetHostName(TargetHostName);
                            SSl_QCF.SetClientID(ClientID);
                            connection = SSl_QCF.CreateQueueConnection(Username, password);
                            connection.ExceptionHandler += new EMSExceptionHandler(_HandleException);

                            connection.Start();
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine("\n###############\nERROR thrown in EMSQueueConnection Constructor  :" + E.ToString());
                            if (E is EMSException)
                            {
                                EMSException je = (EMSException)E;

                                if (je.LinkedException != null)
                                {
                                    System.Console.WriteLine("##### Linked Exception:");
                                    System.Console.WriteLine(je.LinkedException.StackTrace);
                                }
                            }

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
