using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace System.Linq.Dynamic.BitWise.Service.Api
{
    public class QController : ApiController
    {
        public string Get(string query, string type)
        {
            return new QueryController().Get(query, type);
        }

        public void Post([FromBody]string value)
        {

        }
    }
}
