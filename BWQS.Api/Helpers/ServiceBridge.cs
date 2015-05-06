using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Linq.Dynamic.BitWise.Service.Api
{
    public static class ServiceBridge
    {
        private static BWQS svcInstance = new BWQS();

        public static void Initialize(byte[] asmBuffer, string className, string serialData)
        {
            svcInstance.Initialize(asmBuffer, className, serialData);
        }

        public static string Query(string bwqExpr, string returnType)
        {
            return svcInstance.Query(bwqExpr, returnType);
        }
    }
}