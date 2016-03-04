using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackWebAPIAuth.Utils
{
    public static class DefaultValues
    {
        public const string DefaultStringPlaceholder = "\0";
        public const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string LocalAuthority = "LOCAL AUTHORITY";
        public const string StringValueType = "http://www.w3.org/2001/XMLSchema#string";
    }
}
