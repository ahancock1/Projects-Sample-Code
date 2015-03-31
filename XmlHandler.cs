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

        public static T DeserializeString<T>(string objectData)
        {
            return (T)DeserializeString(objectData, typeof(T));
        }

        public static T DeserializeString<T>(string objectData, XmlRootAttribute rootAttribute)
        {
            return (T)DeserializeString(objectData, typeof(T), rootAttribute);
        }

        public static object DeserializeString(string objectData, Type type)
        {
            return Deserialize(new XmlSerializer(type), objectData);
        }

        public static object DeserializeString(string objectData, Type type, XmlRootAttribute rootAttribute)
        {
            return Deserialize(new XmlSerializer(type, rootAttribute), objectData);
        }

        public static T DeserializeFile<T>(string filePath)
        {
            return (T)DeserializeString(new StreamReader(filePath).ReadToEnd(), typeof(T));
        }

        public static T DeserializeFile<T>(string filePath, XmlRootAttribute rootAttribute)
        {
            return (T)DeserializeString(new StreamReader(filePath).ReadToEnd(), typeof(T), rootAttribute);
        }
    }
}