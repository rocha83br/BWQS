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
        private static string internalSerialType;
        private static BWQS.BWQS internalEngine;
        private static string initilizeMsg = "Initialize Engine First.";

        public static void InitializeEngine(object dataSource, Type sourceType, string serialType)
        {
            var bwqsInstance = new BWQS.BWQS();
            var asmBuffer = Compressor.ZipText(Serializer.SerializeArray(Reflector.GetAssemblyBuffer(sourceType)));
            string serialDataSource = string.Empty;

            if (serialType.Equals("xml"))
                serialDataSource = Compressor.ZipText(Serializer.SerializeXML(dataSource));
            else
                serialDataSource = Compressor.ZipText(JsonConvert.SerializeObject(dataSource));

            bwqsInstance.Initialize(asmBuffer, sourceType.FullName, serialDataSource);

            internalSerialType = serialType;
            internalEngine = bwqsInstance;
        }

        public static void Close()
        {
            internalEngine.Dispose();
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

        public static IList GetGroupResult<T>(string queryResult)
        {
            var grpResult = JsonConvert.DeserializeObject<List<GroupResult>>(queryResult);

            if (grpResult[0].Values != null)
            {
                foreach (var rawItem in grpResult)
                {
                    var jsonItem = JsonConvert.SerializeObject(rawItem.Key);
                    rawItem.Key = JsonConvert.DeserializeObject<T>(jsonItem);

                    jsonItem = JsonConvert.SerializeObject(rawItem.Values);
                    var jsonVals = JsonConvert.DeserializeObject<List<T>>(jsonItem);

                    List<object> resValues = new List<object>();
                    foreach (var item in (IList)jsonVals)
                    {
                        var resVal = Activator.CreateInstance(typeof(T));
                        Reflector.CloneObjectData(item, resVal);
                        resValues.Add(resVal);
                    }

                    rawItem.Values = resValues;
                }

                return grpResult;
            }
            else
                return  JsonConvert.DeserializeObject<IList>(queryResult);
        }

        public static string Query(string extExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return Compressor.UnZipText(internalEngine.Query(extExpr));
        }

        public static string Where(string extExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return  Compressor.UnZipText(internalEngine.Where(extExpr, internalSerialType));
        }

        public static string OrderBy(string extExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return  Compressor.UnZipText(internalEngine.OrderBy(extExpr, internalSerialType));
        }

        public static string OrderByDescending(string extExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return  Compressor.UnZipText(internalEngine.OrderByDescending(extExpr, internalSerialType));
        }

        public static string GroupBy(string grpExpr, string _byExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return  Compressor.UnZipText(internalEngine.GroupBy(grpExpr, _byExpr));
        }
    }
}
