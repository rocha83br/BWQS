using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq.Dynamic.BitWise.Helpers;

namespace System.Linq.Dynamic.BitWise.Service
{
    [Serializable]
    public class BitWiseServiceBridge
    {
        #region Declarations

        string queryClass = string.Empty;
        Type classType = null;
        byte[] asmBuffer = null;
        object dataSouce = null;
        
        #endregion

        #region Contructors

            public BitWiseServiceBridge()
            {
                
            }

        #endregion

        #region Public Methods

            public void Initialize(string packedAsmBuffer, string className, string packedDataSource)
            {
                asmBuffer = Serializer.DeserializeArray(Compressor.UnZipText(packedAsmBuffer));
                var asmInstance = Assembly.Load(asmBuffer);
                queryClass = className;
                
                var unpackedSource = Compressor.UnZipText(packedDataSource);

                try
                {
                    classType = asmInstance.GetType(className);

                    if (unpackedSource.Contains("xml"))
                        dataSouce = Serializer.DeserializeXML(Compressor.UnZipText(unpackedSource), classType);
                    else
                        dataSouce = JsonConvert.DeserializeObject(Compressor.UnZipText(packedDataSource));
                }
                catch (Exception ex)
                {
                    // Debug only
                    throw ex;
                }
            }

            public string Query(string bwqExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = Query(bwqExpr, bool.TrueString);
                        dataSouce = qryResult;

                        if (serialType.ToLower().Equals("xml"))
                            result = Serializer.SerializeXML(qryResult);
                        else
                            result = JsonConvert.SerializeObject(qryResult);
                    }
                }

                return result;
            }

            public string Where(string bwqExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = Where(bwqExpr);
                        dataSouce = qryResult;

                        if (serialType.ToLower().Equals("xml"))
                            result = Serializer.SerializeXML(qryResult);
                        else
                            result = JsonConvert.SerializeObject(qryResult);
                    }
                }

                return result;
            }

            public string OrderBy(string bwqExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = OrderBy(bwqExpr);
                        dataSouce = qryResult;

                        if (serialType.ToLower().Equals("xml"))
                            result = Serializer.SerializeXML(qryResult);
                        else
                            result = JsonConvert.SerializeObject(qryResult);
                    }
                }

                return result;
            }

            public string OrderByDescending(string bwqExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = OrderByDescending(bwqExpr);
                        dataSouce = qryResult;

                        if (serialType.ToLower().Equals("xml"))
                            result = Serializer.SerializeXML(qryResult);
                        else
                            result = JsonConvert.SerializeObject(qryResult);
                    }
                }

                return result;
            }
        
        #endregion

        #region Helper Methods

            private object getEngineGenType()
            {
                var asmInstance = Assembly.Load(asmBuffer);
                var engineType = typeof(BitWiseQuery<>);
                var clsType = asmInstance.GetType(queryClass);
                List<Type> typeTmpList = new List<Type>();
                typeTmpList.Add(clsType);
                var srcGenType = engineType.MakeGenericType(typeTmpList.ToArray());
                return Activator.CreateInstance(srcGenType, dataSouce);
            }

            private object[] Query(string bwqExpr, string standAlone)
            {
                object[] result = null;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(standAlone)))
                {
                    var queryEngine = getEngineGenType() as BitWiseQuery<object>;

                    var queryResult = queryEngine.Query(bwqExpr, true);
                    result = new object[queryResult.Count()];

                    int count = 0;
                    foreach (var qrr in queryResult)
                        result[count++] = Convert.ChangeType(qrr, classType);
                }

                return result;
            }

            private object[] Where(string bwqExpr)
            {
                object[] result = null;

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType() as BitWiseQuery<object>;

                    var queryResult = queryEngine.Where(bwqExpr);
                    result = new object[queryResult.Count()];

                    int count = 0;
                    foreach (var qrr in queryResult)
                        result[count++] = Convert.ChangeType(qrr, classType);
                }

                return result;
            }

            private object[] OrderBy(string bwqExpr)
            {
                object[] result = null;

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType() as BitWiseQuery<object>;

                    var queryResult = queryEngine.OrderBy(bwqExpr);
                    result = new object[queryResult.Count()];

                    int count = 0;
                    foreach (var qrr in queryResult)
                        result[count++] = Convert.ChangeType(qrr, classType);
                }

                return result;
            }

            private object[] OrderByDescending(string bwqExpr)
            {
                object[] result = null;

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType() as BitWiseQuery<object>;

                    var queryResult = queryEngine.OrderByDescending(bwqExpr);
                    result = new object[queryResult.Count()];

                    int count = 0;
                    foreach (var qrr in queryResult)
                        result[count++] = Convert.ChangeType(qrr, classType);
                }

                return result;
            }

        #endregion
    }
}
