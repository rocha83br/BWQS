using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
            XmlReader xmlReader = XmlReader.Create(serialObject);

            new XmlSerializer(objType).Deserialize(xmlReader);

            return xmlReader.ReadContentAsObject();
        }

        #endregion
	}
}
