using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using BWQS_Client;
using BWQS_Client.Helpers;

namespace System.Linq.Dynamic.BitWise
{
    public static class BWQSClient
    {
        public static BWQS.BWQS GetEngineInstance(object dataSource, Type sourceType)
        {
            var bwqsInstance = new BWQS.BWQS();
            var asmBuffer = Compressor.ZipText(Serializer.SerializeArray(Reflector.GetAssemblyBuffer(sourceType)));
            var serialDataSource = Compressor.ZipText(JsonConvert.SerializeObject(dataSource));

            bwqsInstance.Initialize(asmBuffer, sourceType.FullName, serialDataSource);

            return bwqsInstance;
        }
    }
}
