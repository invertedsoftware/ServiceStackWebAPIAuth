# ServiceStackWebAPIAuth
Authenticate a ServiceStack service call with a Web API Bearer token

Use a Web API Bearer token to authenticate a ServiceStack service.

Getting a token:

http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api

Securing your ServiceStack service:

[AuthenticateWithOAuthBearer(ApplyTo.All)]
public class Hello : IReturn<HelloResponse>
	{
		public string Name { get; set; }  
    	}
	public class HelloResponse
	{
		public string Result { get; set; }
	}


*Before adding this to your service, add custom validation logic to: OAuthTokenManager.IsAuthenticationTicketValid

Need commecial support?

contact@invertedsoftware.com
