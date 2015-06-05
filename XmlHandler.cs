using System;
using System.IO;
using System.Xml.Serialization;

namespace Hancock.Helpers
{
    public static class XmlHandler
    {
        public static T Deserialize<T>(XmlSerializer serializer, string objectData) where T : class
        {         
            try
            {
                using (TextReader reader = new StringReader(objectData))
                {
                    return (T) serializer.Deserialize(reader);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            return default(T);
        }

        public static T DeserializeString<T>(string xml, XmlRootAttribute rootAttribute) where T : class
        {
            return Deserialize<T>(new XmlSerializer(typeof(T), rootAttribute), xml);
        }

        public static T DeserializeString<T>(string xml) where T : class
        {
            return Deserialize<T>(new XmlSerializer(typeof(T)), xml);
        }              

        public static T DeserializeFile<T>(string filePath) where T : class
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    DeserializeString<T>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deserializing file {0}, {1}", filePath, e.Message);
            }

            return default(T);
        }

        public static T DeserializeFile<T>(string filePath, XmlRootAttribute rootAttribute) where T : class
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    DeserializeString<T>(reader.ReadToEnd(), rootAttribute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deserializing file {0}, {1}", filePath, e.Message);
            }

            return default(T);
        }

        public static void WriteFile<T>(this T data, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating {0} xml file at {1}, {2}", data, filePath, e.Message);
            }
        }

        public static string SerializeString<T>(this T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, data);
                    return writer.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error serializing {0} to string, {1}", data, e.Message);
            }
            return String.Empty;
        }
    }
}
