using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace System.Linq.Dynamic.BitWise.Service
{
    public static class QueryRegister
    {
        public static bool RegisterQueryLog(string className, string bwqExpr, string action, string resultCount)
        {
            var result = false;
            var clientIP = getClientIP();
            var logFilePath = Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();
            var logFileName = string.Concat(DateTime.Now.ToString("yyyyMMdd_HHmm"), "_", clientIP.Replace(".", string.Empty).Replace("::", string.Empty), ".txt");
            var logContent = string.Format("On {0} the Client connected on address {1}, \n gave the Expression [{2}] to {3} a collection \n of {4} that generated {5} records result.", 
                                             DateTime.Now.ToString("dd/MM/yyyy HH:mm"), clientIP, bwqExpr, action, className, resultCount);

            File.WriteAllText(string.Concat(logFilePath, logFileName), logContent);
            result = true;

            return result;
        }

        private static string getClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            
            return endpoint.Address;
        }
    }
}
