
namespace BenScr.Serialization
{
    public enum Format { Json, Xml, Binary }

    public static class FileConverter
    {
        public static void ConvertTo(string path, Format original, Format to, string? newPath = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path must not be empty.", nameof(path));
            if (!File.Exists(path))
                throw new FileNotFoundException("Source file not found.", path);


            newPath ??= Path.ChangeExtension(path, GetExtension(to));

            if (original == to && string.Equals(path, newPath, StringComparison.OrdinalIgnoreCase))
                return;

            object? obj = original switch
            {
                Format.Json => Json.Json.Load<object>(path),
                Format.Xml => Xml.Xml.Load<object>(path),
                Format.Binary => Binary.Binary.Load<object>(path),
                _ => throw new NotSupportedException($"Unsupported format: {original}")
            };

            if (obj is null)
                throw new InvalidDataException("Could not deserialize the source file.");

            string? dir = Path.GetDirectoryName(newPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            switch (to)
            {
                case Format.Json: Json.Json.Save(newPath, obj); break;
                case Format.Xml: Xml.Xml.Save(newPath, obj); break;
                case Format.Binary: Binary.Binary.Save(newPath, obj); break;
                default: throw new NotSupportedException($"Unsupported format: {to}");
            }
        }

        private static string GetExtension(Format f) => f switch
        {
            Format.Json => ".json",
            Format.Xml => ".xml",
            Format.Binary => ".bin",
            _ => ".dat"
        };
    }
}
