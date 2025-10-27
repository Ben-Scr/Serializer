using System.Xml.Serialization;

namespace BenScr.Serialization.Xml
{
    public static class Xml
    {
        public static void Save<T>(string path, T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(path, FileMode.Create);
            xmlSerializer.Serialize(fs, obj);
            fs.Close();
        }

        public static T Load<T>(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(path, FileMode.Open);
            T obj = (T)xmlSerializer.Deserialize(fs);
            fs.Close();
            return obj;
        }
    }
}
