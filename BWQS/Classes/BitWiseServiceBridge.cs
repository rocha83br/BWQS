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
                    else
                        dataSouce = JsonConvert.DeserializeObject(unpackedSource, classType);
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

            public string GroupBy(string grpBWQExpr, string bwqExpr, string serialResult, string serialType)
            {
                var result = string.Empty;

                if (!(string.IsNullOrEmpty(bwqExpr) && string.IsNullOrEmpty(serialResult) && string.IsNullOrEmpty(serialType)))
                {
                    if (serialResult.Equals(bool.TrueString) || serialResult.Equals("1"))
                    {
                        var qryResult = GroupBy(grpBWQExpr, bwqExpr);

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

            private IList GroupBy(string _byExpr, string grpExpr)
            {
                var result = new List<GroupResult>();

                if (!string.IsNullOrEmpty(grpExpr) && !string.IsNullOrEmpty(_byExpr))
                {
                    var queryEngine = getEngineGenType();

                    var qryResult = queryEngine.GroupBy(grpExpr, _byExpr) as IEnumerable<IGrouping<object, object>>;

                    foreach (var res in qryResult)
                    {
                        var itemResult = new GroupResult() { Key = res.Key };
                        var valueList = (IList)Activator.CreateInstance(classType);
                        
                        foreach(var resVal in res)
                        {
                            var valItem = Activator.CreateInstance(itemType);
                            Reflector.CloneObjectData(resVal, valItem);
                            valueList.Add(valItem);
                        }

                        result.Add(new GroupResult() { Key = res.Key, Values = valueList });
                    }
                }

                return result;
            }

        #endregion
    }
}
