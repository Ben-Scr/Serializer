using BenScr.Serialization;
using BenScr.Serialization.Binary;
using BenScr.Serialization.Json;
using BenScr.Serialization.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer.Src
{
    public enum Extension { json = 0, xml = 1, bin = 2 }

    public static class EasySerialize
    {
        public static string MainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BenScr", "EasySerialize");
        public static Format Format = Format.Json;

        public static string GetPathOfKey<T>(string key)
    => CreatePathFromType<T>(key);
        private static string CreatePathFromType<T>(string key, Extension extension = Extension.json)
    => SerializerUtility.CombinePathWithExtension(extension, MainPath, typeof(T).Name, key);

        public static int GetSavedFilesCount() => SerializerUtility.GetDirectoryFiles(MainPath, true).Length;
        public static string[] GetSavedFilesPath() => SerializerUtility.GetDirectoryFiles(MainPath, true);

        public static bool Serialize<T>(string key, T obj)
        {
            if (key == null) return false;

            try
            {
                string path = CreatePathFromType<T>(key, (Extension)Format);

                switch (Format)
                {
                    case Format.Json:
                        Json.Save(path, obj); break;
                    case Format.Xml:
                        Xml.Save(path, obj); break;
                    case Format.Binary:
                        Binary.Save(path, obj); break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static T Derialize<T>(string key, T defaultValue = default!)
        {
            if (key == null) return defaultValue;


            T obj = defaultValue;

            try
            {
                string path = CreatePathFromType<T>(key, (Extension)Format);

                switch (Format)
                {
                    case Format.Json:
                        obj = Json.Load<T>(path); break;
                    case Format.Xml:
                        obj = Xml.Load<T>(path); break;
                    case Format.Binary:
                        obj = Binary.Load<T>(path); break;
                }


            }
            catch { }

            return obj;
        }

        public static void Delete<T>(string key)
        {
            string path = CreatePathFromType<string>(key, (Extension)Format);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File with key \"{key}\" at path \"{path}\" doesn't exist!");

            File.Delete(path);
        }

        public static void ClearAll(bool filesOnly = false) => SerializerUtility.DeleteAllDirectories(MainPath, filesOnly);
    }
}
