using Serializer.Src;

namespace BenScr.Serialization
{
    internal class SerializerUtility
    {
        public static string CombinePathWithExtension(Extension extension, params string[] paths) => Path.ChangeExtension(Path.Combine(paths), extension.ToString());

        public static string[] GetDirectoryFiles(string dirPath, bool recursive = false) => recursive ? GetDirectoryFilesRecursive(dirPath) : Directory.GetFiles(dirPath);

        private static string[] GetDirectoryFilesRecursive(string dirPath)
        {
            List<string> files = new List<string>();

            foreach (var dir in Directory.GetDirectories(dirPath))
            {
                files.AddRange(GetDirectoryFiles(dir));
            }

            return files.ToArray();
        }

        public static void DeleteAllDirectories(string path, bool onlyFiles)
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);

                foreach (string directory in directories)
                {
                    DeleteAllFromDirectory(directory, false);

                    if (!onlyFiles)
                        Directory.Delete(directory);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static void DeleteAllFromDirectory(string directoryPath, bool onlyFiles)
        {
            if (Directory.GetDirectories(directoryPath).Length > 0)
                DeleteAllDirectories(directoryPath, onlyFiles);

            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
