using Common;
using System;
using System.Collections.Generic;
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
				
				throw new ArgumentNullException("AS was null");
			}
			
			if (TGS == null)
			{
				
				throw new ArgumentNullException("TGS was null");
			}
		
		}

		public DomainController()
		{
		}

		public Tuple<string, string> ServiceRequest(string service, string username)
		{
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
