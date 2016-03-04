using ServiceStack;
using ServiceStack.Web;
using ServiceStackWebAPIAuth.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackWebAPIAuth.ServiceModel.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AuthenticateWithOAuthBearerAttribute : RequestFilterAttribute
    {
        public AuthenticateWithOAuthBearerAttribute(ApplyTo applyTo)
            : base(applyTo)
        { }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            if (string.IsNullOrWhiteSpace(req.GetHeader("Authorization")) || req.GetHeader("Authorization").ToLower() == "null")
                throw HttpError.Unauthorized("Authorization Header Missing");

            var bearer = req.GetHeader("Authorization").Split(null).Last();
            if (string.IsNullOrWhiteSpace(bearer) || bearer.ToLower() == "null" || OAuthTokenManager.GetAuthenticationTicket(bearer) == null)
            {
                throw HttpError.Unauthorized("Unauthorized");
            }
        }
    }
}
