using MessagePack;

namespace BenScr.Serialization.Binary
{
    public static class Binary
    {
        public static readonly MessagePackSerializerOptions DefaultMsgPack =
            MessagePackSerializerOptions.Standard
                .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance)
                .WithCompression(MessagePackCompression.Lz4BlockArray);

        public static void Save<T>(string path, T obj, MessagePackSerializerOptions? options = null)
        {
            string? dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using FileStream fs = new FileStream(
                path, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 1 << 20, options: FileOptions.SequentialScan);

            options ??= DefaultMsgPack;
            MessagePackSerializer.Serialize(fs, obj, options);
        }
        public static T Load<T>(string path, T defaultValue = default!, MessagePackSerializerOptions? options = null)
        {
            if (!File.Exists(path)) return defaultValue;

            using FileStream fs = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 1 << 20, options: FileOptions.SequentialScan);

            options ??= DefaultMsgPack;
            return MessagePackSerializer.Deserialize<T>(fs, options) ?? defaultValue;
        }
    }
}
