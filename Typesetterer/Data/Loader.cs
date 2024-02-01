using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Typesetterer.Data
{
    internal static class Loader
    {
        private readonly static ConcurrentDictionary<Type, XmlSerializer> serializers;

        static Loader()
        {
            Loader.serializers = new ConcurrentDictionary<Type, XmlSerializer>();
        }

        public static void ExportAsXml(this object item, string filepath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Unicode))
                {
                    Loader.GetSerializer(item.GetType()).Serialize(streamWriter, item);
                }
            }
        }

        private static XmlSerializer GetSerializer(Type type)
        {
            return Loader.serializers.GetOrAdd(type, (Type t) => new XmlSerializer(t));
        }

        public static T ImportAsXml<T>(string filepath)
        {
            T t;
            try
            {
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
                {
                    t = Loader.ImportAsXml<T>(fileStream);
                }
            }
            catch
            {
                t = default(T);
            }
            return t;
        }

        public static T ImportAsXml<T>(Stream fs)
        {
            T t;
            try
            {
                using (StreamReader streamReader = new StreamReader(fs, Encoding.Unicode))
                {
                    t = (T)Loader.GetSerializer(typeof(T)).Deserialize(streamReader);
                }
            }
            catch
            {
                t = default(T);
            }
            return t;
        }
    }
}