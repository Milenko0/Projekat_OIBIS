using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IDataManagment>, IDisposable
    {
        IDataManagment factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            try
            {
                factory = CreateChannel();
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Client";
                    log.WriteEntry($"Client-side channel opened successfully.", EventLogEntryType.SuccessAudit);
                }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Client";
                    log.WriteEntry($"Client-side channel failed to open. {e.Message} ", EventLogEntryType.FailureAudit);
                }
                */
            }
        }

        public Tuple<bool, string> Write(string key, string text)
        {
            try
            {
                string encryptedMessage = Encrypting.EncryptMessage(text, key);
                string answer = factory.Write(encryptedMessage);
                return Tuple.Create(true, answer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Tuple.Create(false, e.Message);
            }
        }
        public Tuple<bool, List<string>> Read(string secretKey)
        {
            try
            {
                
                List<string> answerCrypted = factory.Read();
                List<string> answer = new List<string>();
                foreach(string s in answerCrypted)
                {
                    answer.Add(Encrypting.DecryptMessage(s, secretKey));
                }
                return Tuple.Create(true, answer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Tuple.Create(false, new List<string>() { e.Message});
            }
        }
        
        public bool Connect(string secretKey)
        {
            string secretKeyHashed = Encrypting.Hash256(secretKey, "sfafsd");
            
            Tuple<string,string> res = factory.Connection(secretKeyHashed, "sfafsd");
            if (res == null)
            {
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Client";
                    log.WriteEntry($"Server did not recognize client.", EventLogEntryType.FailureAudit);
                }
                */
                return false;
            }
            if (Encrypting.Hash256(secretKey, res.Item2) == res.Item1)
            {
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Client";
                    log.WriteEntry($"Server and Client succesfully authenticated", EventLogEntryType.SuccessAudit);
                }
                */
                return true;

            }
            else
            {
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Client";
                    log.WriteEntry($"Server not recognized by the client", EventLogEntryType.FailureAudit);
                }*/
                return false;
                
            }
            
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

    }
}
