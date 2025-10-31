using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace BenScr.Serialization.Json
{
    public static class Json
    {
        public static readonly JsonSerializerOptions DefaultJson = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
        public static readonly JsonSerializerOptions FormatedJson = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static void Save<T>(string path, T obj, JsonSerializerOptions? options = null)
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using FileStream fs = new FileStream(
                path, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 1 << 20, FileOptions.SequentialScan);

            options ??= DefaultJson;

            using (fs)
            {
                JsonSerializer.Serialize(fs, obj, options);
            }
        }
        public static void SaveCompressed<T>(string path, T item, CompressionLevel compressionLevel = CompressionLevel.Fastest, JsonSerializerOptions? options = null)
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 1 << 20,
                options: FileOptions.SequentialScan);

            options ??= DefaultJson;

            using var gzip = new GZipStream(fs, compressionLevel, leaveOpen: false);
            JsonSerializer.Serialize(gzip, item, DefaultJson);
        }

        public static T Load<T>(string path, T defaultValue = default!, JsonSerializerOptions? options = null)
        {
            if (!File.Exists(path)) return defaultValue;

            using FileStream fs = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 1 << 20, FileOptions.SequentialScan);

            options ??= DefaultJson;

            using (fs)
            {
                return JsonSerializer.Deserialize<T>(fs, options) ?? defaultValue;
            }
        }

        public static T LoadCompressed<T>(string path, T defaultValue = default!, JsonSerializerOptions? options = null)
        {
            if (!File.Exists(path)) return defaultValue;

            using var fs = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1 << 20,
                options: FileOptions.SequentialScan);

            options ??= DefaultJson;

            using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);
            return JsonSerializer.Deserialize<T>(gzip, DefaultJson) ?? defaultValue;
        }
    }
    public static class JsonSecure
    {
        public static readonly JsonSerializerOptions DefaultJson = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        private const int saltSize = 16;
        private const int ivSize = 16;
        private const int keySize = 32;
        private const int iterations = 100_000;

        public static void Save<T>(string path, string password, T obj, JsonSerializerOptions? options = null)
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using FileStream fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 1 << 20,
                options: FileOptions.SequentialScan);

            options ??= DefaultJson;

            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] iv = RandomNumberGenerator.GetBytes(ivSize);

            using var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] key = kdf.GetBytes(keySize);

            fs.Write(salt);
            fs.Write(iv);

            using Aes aes = Aes.Create()!;
            aes.KeySize = keySize * 8;
            aes.BlockSize = ivSize * 8;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;


            using var crypto = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
            JsonSerializer.Serialize(crypto, obj, options);
        }
        public static T Load<T>(string path, string password, T defaultValue = default!, JsonSerializerOptions? options = null)
        {
            if (!File.Exists(path))
                return defaultValue;

            try
            {
                using FileStream fs = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 1 << 20,
                    options: FileOptions.SequentialScan);

                byte[] salt = new byte[saltSize];
                byte[] iv = new byte[ivSize];
                if (fs.Read(salt) != saltSize || fs.Read(iv) != ivSize)
                    throw new InvalidDataException("Invalid fileformat");

                options ??= DefaultJson;

                using var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                byte[] key = kdf.GetBytes(keySize);

                using Aes aes = Aes.Create()!;
                aes.KeySize = keySize * 8;
                aes.BlockSize = ivSize * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;

                using var crypto = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read);

                try
                {
                    return JsonSerializer.Deserialize<T>(crypto, options)!;
                }
                catch
                {
                    return defaultValue;
                }
            }
            catch
            {
                throw new InvalidDataException("Wrong password or damaged file");
            }
        }
    }
}
