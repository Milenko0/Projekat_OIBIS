using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class DomainController : ISConnection, ICConnection
    {
		AuthentificationService AS = new AuthentificationService();
		TicketGrantingService TGS = new TicketGrantingService();

		public DomainController(AuthentificationService aS, TicketGrantingService tGS)
		{
			AS = aS;
			TGS = tGS;

			if (AS == null)
			{
				using (EventLog log = new EventLog("Application"))
				{
					log.Source = "Domain Controller";
					log.WriteEntry($"Authentification Service failed to open.", EventLogEntryType.FailureAudit);
				}
				throw new ArgumentNullException("AS was null");
            }
            else
            {
				using (EventLog log = new EventLog("Application"))
				{
					log.Source = "Domain Controller";
					log.WriteEntry($"Authentification Service opened successfully.", EventLogEntryType.SuccessAudit);
				}
			}
			
			if (TGS == null)
			{
				using (EventLog log = new EventLog("Application"))
				{
					log.Source = "Domain Controller";
					log.WriteEntry($"Ticket Granting Service failed to open.", EventLogEntryType.FailureAudit);
				}
				throw new ArgumentNullException("TGS was null");
            }
            else
            {
				using (EventLog log = new EventLog("Application"))
				{
					log.Source = "Domain Controller";
					log.WriteEntry($"Ticket Granting Service opened successfully.", EventLogEntryType.SuccessAudit);
				}
			}
		
		}

		public DomainController()
		{
		}

		public Tuple<string, string> ServiceRequest(string service, string username)
		{
			using (EventLog log = new EventLog("Application"))
			{
				log.Source = "Domain Controller";
				log.WriteEntry($"Recieved request for service information and secret key.", EventLogEntryType.Information);
			}
			return TGS.GetServiceEndpointAndSecretKey(service);
		}

		public bool ValidateUser(string username, string password)
		{
			return AS.AuthentificateClient(username, password);
		}

		public bool RegisterService(string IPAddr, string hostName, string port, string hashPassword, string username)
		{
			if (AS.AuthenticateServer(IPAddr, hostName, port, hashPassword, username))
			{
				TGS.RegisterService(IPAddr, hostName, port, hashPassword);
				Console.WriteLine("Uspesna autentifikacija Servera");
				return true;
			}
			else
			{
				return false;
			}
		}

		public void SignOutService(string hostName)
		{
			TGS.SignOutService(hostName);
		}
    }
}
