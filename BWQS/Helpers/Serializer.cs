using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;

namespace System.Linq.Dynamic.BitWise.Helpers
{
    public class Serializer
    {
        #region Public Methods

        public static string SerializeArray(byte[] sourceArray)
        {
            return Convert.ToBase64String(sourceArray);
        }

        public static byte[] DeserializeArray(string serialArray)
        {
            return Convert.FromBase64String(serialArray);
        }

		public static string SerializeText(object sourceObject)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(sourceObject.GetType());

            xmlSerializer.Serialize(memStream, sourceObject);

            memStream.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return Convert.ToBase64String(memStream.ToArray());
        }

        public static object DeserializeText(string serialObject, Type objectType)
        {
            object result = null;

            if (!string.IsNullOrEmpty(serialObject) && (objectType != null))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objectType);
                MemoryStream memStream = new MemoryStream(Convert.FromBase64String(serialObject));

                result = xmlSerializer.Deserialize(memStream);
            }

            return result;
        }
		
        public static string SerializeText(string source)
        {
            byte[] textBuffer = source.ToCharArray()
                                      .Select(chr => Convert.ToByte(chr)).ToArray();

            return Convert.ToBase64String(textBuffer);
        }

        public static string DeserializeText(string source)
        {
            byte[] sourceObj = Convert.FromBase64String(source);

            char[] textBuffer = sourceObj.Select(bt => Convert.ToChar(bt)).ToArray();

            return new string(textBuffer);
        }

        public static string SerializeXML(object sourceObject)
        {
            StringBuilder resultText = new StringBuilder();
            XmlWriter xmlGen = XmlWriter.Create(resultText);

            new XmlSerializer(sourceObject.GetType()).Serialize(xmlGen, sourceObject);

            return resultText.ToString();
        }

        public static object DeserializeXML(string serialObject, Type objType)
        {
            return new XmlSerializer(objType).Deserialize(new StringReader(serialObject));
        }

        public static string SerializeCSV(IList sourceObject)
        {
            decimal fakeNum = 0;
            StringBuilder result = new StringBuilder();

            if ((sourceObject != null) && (sourceObject.Count > 0))
            {
                var objProps = Reflector.GetObjectProps(sourceObject[0]);
                foreach (var prp in objProps)
                    result.Append(string.Concat(prp.Name, ";"));

                result.Remove(result.Length - 1, 1);
                result.AppendLine();

                foreach (var src in sourceObject)
                {
                    var ObjPropValues = Reflector.GetObjectPropValues(src, objProps);
                    foreach (var prpVal in ObjPropValues)
                    {
						if (prpVal != null)
                        {
                            var objPrpValue = prpVal.ToString();
                            if (decimal.TryParse(prpVal.ToString(), out fakeNum))
                                objPrpValue = objPrpValue.Replace(".", string.Empty).Replace(",", ".");
                            result.Append(string.Concat(objPrpValue.ToString(), ";"));
                        }
                        else
                            result.Append("null;");
                    }

                    result.Remove(result.Length - 1, 1);
                    result.AppendLine();
                }
            }

            return result.ToString();
        }

        public static object[] DeserializeCSV(string serialObject, Type objectType)
        {
            List<object> result = new List<object>();
            StringReader csvReader = new StringReader(serialObject);

            string[] headerColumns = csvReader.ReadLine().Split(';');

            string valueRow = string.Empty;
            string[] valueCols = new string[0];
            while (valueRow != null)
            {
                valueRow = csvReader.ReadLine();

                if (valueRow != null)
                {
                    valueCols = valueRow.Split(';');

                    object resultItem = Activator.CreateInstance(objectType);

                    int counter = 0;
                    foreach (var hedCol in headerColumns)
                    {
                        var hedProp = objectType.GetProperty(hedCol);

                        if (!hedProp.PropertyType.Namespace.Equals(objectType.Namespace))
                        {
                            var colValue = valueCols[counter];
                            var typedValue = Reflector.GetTypedValue(hedProp.PropertyType, colValue);
                            hedProp.SetValue(resultItem, typedValue, null);
                        }
                        counter++;
                    }

                    result.Add(resultItem);
                }
            }

            return result.ToArray();
        }

		public static byte[] SerializeBinary(object sourceObject)
        {
            BinaryFormatter binSerializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();

            binSerializer.Serialize(memStream, sourceObject);

            memStream.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream.ToArray();
        }

        public static object DeserializeBinary(byte[] serialObject)
        {
            object result = null;

            if (serialObject != null)
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream(serialObject);

                result = binSerializer.Deserialize(memStream);
            }

            return result;
        }
		
        #endregion
	}
}
