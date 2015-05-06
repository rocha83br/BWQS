using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic.BitWise.Service;

namespace System.Linq.Dynamic.BitWise.Service.Api
{
    public class QueryController : ApiController
    {
        public string Get(string query, string type)
        {
            return new BWQS().Query(query, type);
        }
    }
}
