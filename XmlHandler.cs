using System;
using System.IO;
using System.Xml.Serialization;

namespace BT_Sport_Server
{
    public static class XmlHandler
    {
        public static object Deserialize(XmlSerializer serializer, string objectData)
        {
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static T DeserializeString<T>(string xml)
        {
            return (T) DeserializeString(xml, typeof(T));
        }

        public static T DeserializeString<T>(string xml, XmlRootAttribute rootAttribute)
        {
            return (T) DeserializeString(xml, typeof(T), rootAttribute);
        }

        public static object DeserializeString(string xml, Type type)
        {
            return Deserialize(new XmlSerializer(type), xml);
        }

        public static object DeserializeString(string xml, Type type, XmlRootAttribute rootAttribute)
        {
            return Deserialize(new XmlSerializer(type, rootAttribute), xml);
        }

        public static T DeserializeFile<T>(string filePath)
        {
            string text = String.Empty;

            using (StreamReader reader = new StreamReader(filePath))
            {
                text = reader.ReadToEnd();
            }

            return (T) DeserializeString(text, typeof(T));
        }

        public static T DeserializeFile<T>(string filePath, XmlRootAttribute rootAttribute)
        {
            string text = String.Empty;

            using (StreamReader reader = new StreamReader(filePath))
            {
                text = reader.ReadToEnd();
            }

            return (T) DeserializeString(text, typeof(T), rootAttribute);
        }
    }
}