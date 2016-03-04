using ServiceStackWebAPIAuth.Authentication.Models;
using ServiceStackWebAPIAuth.Utils;
using ServiceStackWebAPIAuth.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackWebAPIAuth.Authentication
{
    public static class OAuthTokenManager
    {
        private static readonly string[] purpose =
        {
            "Microsoft.Owin.Security.OAuth",
            "Access_Token",
            "v1"
        };


        private const int TicketFormatVersion = 3;
        private const int PropertiesFormatVersion = 1;

        /// <summary>
        /// Gets an GetAuthenticationTicket from a Bearer token issued by Microsoft.Owin.Security.OAuth.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="validateTicket">Validate the user against the Identity database.</param>
        /// <returns>AuthenticationTicket or null if the ticket is invalid.</returns>
        public static AuthenticationTicket GetAuthenticationTicket(string token, bool validateTicket = true)
        {
            AuthenticationTicket ticket = null;

            try
            {
                var tokenBytes = StringUtils.Decode(token);
                var unprotectedData = StringUtils.Decompress(System.Web.Security.MachineKey.Unprotect(tokenBytes, purpose));
                var ms = new MemoryStream(unprotectedData);
                using (var reader = ms.CreateReader())
                {
                    ticket = ReadAuthenticationTicket(reader);
                }

                if (validateTicket && !IsAuthenticationTicketValid(ticket))
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ticket;
        }

        public static bool IsAuthenticationTicketValid(AuthenticationTicket ticket)
        {
            bool isValid = false;

            // Validate the ticket.

            if (!ticket.Identity.IsAuthenticated)
                return isValid;

            if (ticket.Identity.AuthenticationType != "Bearer")
                return isValid;

            if (ticket.Properties.ExpiresUtc < DateTimeOffset.UtcNow)
                return isValid;

            // Validate the user in the ticket.

            var userEmail = ticket.Identity.Name;
            var userID = ticket.Identity.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value;

            // This is an example. Use your own checks against the user and role stores
            
            //var manager = new Identity.UserStore<ApplicationUser>();
            //var user = manager.FindByIdAsync(int.Parse(userID)).Result;
            //if (user.EmailAddress != userEmail)
            //    return isValid;

            // Ticket is valid
            isValid = true;
            return isValid;
        }

        private static AuthenticationTicket ReadAuthenticationTicket(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.ReadInt32() != TicketFormatVersion)
            {
                return null;
            }

            string authenticationType = reader.ReadString();
            string nameClaimType = ReadWithDefault(reader, DefaultValues.NameClaimType);
            string roleClaimType = ReadWithDefault(reader, DefaultValues.RoleClaimType);
            int count = reader.ReadInt32();
            var claims = new Claim[count];
            for (int index = 0; index != count; ++index)
            {
                string type = ReadWithDefault(reader, nameClaimType);
                string value = reader.ReadString();
                string valueType = ReadWithDefault(reader, DefaultValues.StringValueType);
                string issuer = ReadWithDefault(reader, DefaultValues.LocalAuthority);
                string originalIssuer = ReadWithDefault(reader, issuer);
                claims[index] = new Claim(type, value, valueType, issuer, originalIssuer);
            }
            var identity = new ClaimsIdentity(claims, authenticationType, nameClaimType, roleClaimType);
            int bootstrapContextSize = reader.ReadInt32();
            if (bootstrapContextSize > 0)
            {
                identity.BootstrapContext = new BootstrapContext(reader.ReadString());
            }

            AuthenticationProperties properties = ReadProperties(reader);
            return new AuthenticationTicket(identity, properties);
        }

        public static AuthenticationProperties ReadProperties(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.ReadInt32() != PropertiesFormatVersion)
            {
                return null;
            }

            int count = reader.ReadInt32();
            var extra = new Dictionary<string, string>(count);
            for (int index = 0; index != count; ++index)
            {
                string key = reader.ReadString();
                string value = reader.ReadString();
                extra.Add(key, value);
            }
            return new AuthenticationProperties(extra);
        }

        private static string ReadWithDefault(BinaryReader reader, string defaultValue)
        {
            string value = reader.ReadString();
            if (string.Equals(value, DefaultValues.DefaultStringPlaceholder, StringComparison.Ordinal))
            {
                return defaultValue;
            }
            return value;
        }
    }
}
