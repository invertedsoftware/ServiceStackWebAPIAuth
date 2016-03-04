using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStackWebAPIAuth.ServiceModel.Attributes;

namespace ServiceStackWebAPIAuth.ServiceModel
{
    [Route("/hello/{Name}")]
    [AuthenticateWithOAuthBearer(ApplyTo.All)]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
}
