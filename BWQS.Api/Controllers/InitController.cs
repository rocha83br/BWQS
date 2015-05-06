using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace System.Linq.Dynamic.BitWise.Service.Api
{
    public class InitController : ApiController
    {
        [HttpPost]
        public void Post()
        {
            // Obtem o buffer da biblioteca de modelos do cliente para realizar as consultas
            // Obtem os valores do modelo a utilizar e a fonte de dados composta JSON/XML/CSV
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                MemoryStream asmBuffer = new MemoryStream();
                HttpContext.Current.Request.Files[0].InputStream.CopyTo(asmBuffer);

                string className = HttpContext.Current.Request.QueryString["className"];
                string dataSource = HttpContext.Current.Request.QueryString["dataSource"];

                if ((asmBuffer != null) && !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(dataSource))
                    ServiceBridge.Initialize(asmBuffer.ToArray(), className, dataSource);
                else
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
                throw new HttpResponseException(HttpStatusCode.NoContent);
        }
    }
}
