using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using BWQS_Client;
using BWQS_Client.Helpers;
using System.Linq.Dynamic.BitWise.BWQS;
using System.Collections;

namespace System.Linq.Dynamic.BitWise
{
    public static class BWQSClient
    {
        public static BWQS.BWQS GetEngineInstance(object dataSource, Type sourceType, string serialType)
        {
            var bwqsInstance = new BWQS.BWQS();
            var asmBuffer = Compressor.ZipText(Serializer.SerializeArray(Reflector.GetAssemblyBuffer(sourceType)));
            string serialDataSource = string.Empty;

            if (serialType.Equals("xml"))
                serialDataSource = Compressor.ZipText(Serializer.SerializeXML(dataSource));
            else
                serialDataSource = Compressor.ZipText(JsonConvert.SerializeObject(dataSource));

            bwqsInstance.Initialize(asmBuffer, sourceType.FullName, serialDataSource);

            return bwqsInstance;
        }

        public static List<T> GetQueryResult<T>(string queryResult)
        {
            List<T> typedResult = null;

            if (queryResult.Contains("xml"))
                typedResult = Serializer.DeserializeXML(queryResult, typeof(List<T>)) as List<T>;
            else
                typedResult = JsonConvert.DeserializeObject<List<T>>(queryResult);

            return typedResult;
        }

        public static List<GroupResult> GetGroupResult<T>(string queryResult)
        {
            var rawResult = JsonConvert.DeserializeObject<List<GroupResult>>(queryResult);

            foreach (var rawItem in rawResult)
            {
                var jsonItem = JsonConvert.SerializeObject(rawItem.Key);
                rawItem.Key = JsonConvert.DeserializeObject<T>(jsonItem);

                jsonItem = JsonConvert.SerializeObject(rawItem.Values);
                var jsonVals = JsonConvert.DeserializeObject<List<T>>(jsonItem);

                List<object> resValues = new List<object>();
                foreach(var item in (IList)jsonVals)
                {
                    var resVal = Activator.CreateInstance(typeof(T));
                    Reflector.CloneObjectData(item, resVal);
                    resValues.Add(resVal);
                }
                    
                rawItem.Values = resValues;
            }

            return rawResult;
        }
    }
}
