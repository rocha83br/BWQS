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
        public static bool RegisterQueryLog(string bwqExpr, string resultCount)
        {
            var result = false;
            var clientIP = getClientIP();
            var logFilePath = Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();
            var logFileName = string.Concat(DateTime.Now.ToString("yyyyMMdd_HHmm"), clientIP.Replace(".", string.Empty), ".txt");
            var logContent = string.Concat("On ", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), " client connected on IP : ", clientIP, 
                                           ",\n gave the Expression [", bwqExpr, "] that generated ", resultCount, " records result.");

            File.WriteAllText(string.Concat(logFilePath, logFileName), logContent);

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
