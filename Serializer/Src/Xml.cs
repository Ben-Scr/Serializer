using System.IO.Compression;
using System.Text.Json;
using System.Xml.Serialization;

namespace BenScr.Serialization
{
    public static class Xml
    {
        public static void Save<T>(string path, T obj)
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(path, FileMode.Create);
            xmlSerializer.Serialize(fs, obj);
            fs.Close();
        }

        public static void SaveCompressed<T>(string path, T obj, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 1 << 20,
                options: FileOptions.SequentialScan);


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using var gzip = new GZipStream(fs, compressionLevel, leaveOpen: false);
            xmlSerializer.Serialize(gzip, obj);
        }

        public static T Load<T>(string path, T defaultValue = default)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(path, FileMode.Open);

            using (fs)
            {
                return (T)xmlSerializer.Deserialize(fs) ?? defaultValue;
            }

        }

        public static T LoadCompressed<T>(string path, T defaultValue = default!)
        {
            if (!File.Exists(path)) return defaultValue;

            using var fs = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1 << 20,
                options: FileOptions.SequentialScan);

            using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(gzip) ?? defaultValue;
        }

        public static byte[] Serialize<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using var memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
        public static byte[] SerializeCompressed<T>(T obj, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using var memoryStream = new MemoryStream();

            using (var gzip = new GZipStream(memoryStream, compressionLevel, leaveOpen: true))
            {
                xmlSerializer.Serialize(gzip, obj);
            }

            return memoryStream.ToArray();
        }
    }
}
