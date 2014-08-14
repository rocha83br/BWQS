using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq.Dynamic.BitWise.Helpers;
using System.Collections;

namespace System.Linq.Dynamic.BitWise.Service
{
    [Serializable]
    public class BitWiseServiceBridge
    {
        #region Declarations

        static string queryClass = string.Empty;
        static Type classType = null;
        static Type itemType = null;
        static byte[] asmBuffer = null;
        static object dataSouce = null;
        
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
                    classType = getTypeCollection();

                    if (unpackedSource.Contains("xml"))
                        dataSouce = Serializer.DeserializeXML(unpackedSource, classType);
                    else if ((unpackedSource.StartsWith("[")) || (unpackedSource.StartsWith("{")))
                        dataSouce = JsonConvert.DeserializeObject(unpackedSource, classType);
                    else
                        dataSouce = Serializer.DeserializeCSV(unpackedSource, itemType);
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

                        try
                        {
                            QueryRegister.RegisterQueryLog(itemType.Name, bwqExpr, "query", qryResult.Count.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        result = getSerialObject(qryResult, serialType);

                        result = Compressor.ZipText(result);
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

                        try
                        {
                            QueryRegister.RegisterQueryLog(itemType.Name, bwqExpr, "filter", qryResult.Count.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        result = getSerialObject(qryResult, serialType);
                    }

                    result = Compressor.ZipText(result);
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

                        try
                        {
                            QueryRegister.RegisterQueryLog(itemType.Name, bwqExpr, "order", qryResult.Count.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        result = getSerialObject(qryResult, serialType);

                        result = Compressor.ZipText(result);
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

                        try
                        {
                            QueryRegister.RegisterQueryLog(itemType.Name, bwqExpr, "order", qryResult.Count.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        result = getSerialObject(qryResult, serialType);

                        result = Compressor.ZipText(result);
                    }
                }

                return result;
            }

            public string GroupBy(string grpExpr, string _byExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(grpExpr) && string.IsNullOrEmpty(_byExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = GroupBy(grpExpr, _byExpr);

                        try
                        {
                            string grpConcatExpr = string.Concat(_byExpr, "][", grpExpr);
                            QueryRegister.RegisterQueryLog(itemType.Name, grpConcatExpr, "group", qryResult.Count.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        result = getSerialObject(qryResult, serialType);

                        result = Compressor.ZipText(result);
                    }
                }

                return result;
            }
        
        #endregion

        #region Helper Methods

            private IBitWiseQuery getEngineGenType()
            {
                var asmInstance = Assembly.Load(asmBuffer);
                var engineType = typeof(BitWiseQuery<>);
                var clsType = asmInstance.GetType(queryClass);
                List<Type> typeTmpList = new List<Type>();
                typeTmpList.Add(clsType);
                var srcGenType = engineType.MakeGenericType(typeTmpList.ToArray());
                var objResult = Activator.CreateInstance(srcGenType, (IList)dataSouce, clsType);
                var result = ((IBitWiseQuery)objResult);

                return result;
            }

            private Type getTypeCollection()
            {
                var asmInstance = Assembly.Load(asmBuffer);
                var engineType = typeof(List<>);
                itemType = asmInstance.GetType(queryClass);
                List<Type> typeTmpList = new List<Type>();
                typeTmpList.Add(itemType);
                
                return engineType.MakeGenericType(typeTmpList.ToArray());
            }

            private string getSerialObject(IList qryResult, string serialType)
            {
                if (serialType.ToLower().Equals("xml"))
                    return Serializer.SerializeXML(qryResult);
                else if (serialType.ToLower().Equals("csv"))
                    return Serializer.SerializeCSV(qryResult);
                else
                    return JsonConvert.SerializeObject(qryResult);
            }

            private IList Query(string bwqExpr, string standAlone)
            {
                IList result = new List<object>();

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(standAlone)))
                {
                    var queryEngine = getEngineGenType();

                    var queryResult = queryEngine.Query(bwqExpr, true);

                    foreach (var res in queryResult)
                        result.Add(res);
                }

                return result;
            }

            private IList Where(string bwqExpr)
            {
                IList result = (IList)Activator.CreateInstance(classType);

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType();

                    var queryResult = queryEngine.Where(bwqExpr);

                    foreach (var res in queryResult)
                    {
                        var newItem = Activator.CreateInstance(itemType);
                        Reflector.CloneObjectData(res, newItem);
                        
                        result.Add(newItem);
                    }
                }

                return result;
            }

            private IList OrderBy(string bwqExpr)
            {
                IList result = (IList)Activator.CreateInstance(classType);

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType();

                    var queryResult = queryEngine.OrderBy(bwqExpr);

                    foreach (var res in queryResult)
                    {
                        var newItem = Activator.CreateInstance(itemType);
                        Reflector.CloneObjectData(res, newItem);

                        result.Add(newItem);
                    }
                }

                return result;
            }

            private IList OrderByDescending(string bwqExpr)
            {
                IList result = (IList)Activator.CreateInstance(classType);

                if (!string.IsNullOrEmpty(bwqExpr))
                {
                    var queryEngine = getEngineGenType();

                    var queryResult = queryEngine.OrderByDescending(bwqExpr);

                    foreach (var res in queryResult)
                    {
                        var newItem = Activator.CreateInstance(itemType);
                        Reflector.CloneObjectData(res, newItem);

                        result.Add(newItem);
                    }
                }

                return result;
            }

            private IList GroupBy(string grpExpr, string _byExpr)
            {
                IList result = null;

                if (!string.IsNullOrEmpty(grpExpr) && !string.IsNullOrEmpty(_byExpr))
                {
                    var queryEngine = getEngineGenType();

                    var rawResult = queryEngine.GroupBy(_byExpr, grpExpr);

                    var qryResult = rawResult as IEnumerable<IGrouping<object, object>>;

                    if (qryResult != null)
                    {
                        result = new List<GroupResult>();
                        foreach (var res in qryResult)
                        {
                            var itemResult = new GroupResult() { Key = res.Key };
                            var valueList = (IList)Activator.CreateInstance(classType);

                            foreach (var resVal in res)
                            {
                                var valItem = Activator.CreateInstance(itemType);
                                Reflector.CloneObjectData(resVal, valItem);
                                valueList.Add(valItem);
                            }

                            result.Add(new GroupResult() { Key = res.Key, Values = valueList });
                        }
                    }
                    else
                    {
                        var rawList = rawResult as IEnumerable;
                        var rawResultList = new List<object>();
                        foreach (var rwi in rawList)
                            rawResultList.Add(rwi);

                        return rawResultList;
                    }
                }

                return result;
            }

        #endregion
    }
}
