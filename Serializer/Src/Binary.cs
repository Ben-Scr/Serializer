using System.Buffers.Binary;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BenScr.Serialization.Binary
{
    public static class Binary
    {
        public static void Save<T>(string path, ReadOnlySpan<T> span, CompressionLevel level = CompressionLevel.Fastest) where T : unmanaged
        {
            string dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1 << 20, FileOptions.SequentialScan);
            using var gzip = new GZipStream(fs, level, leaveOpen: false);


            Span<byte> len = stackalloc byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(len, span.Length);
            gzip.Write(len);


            ReadOnlySpan<byte> raw = MemoryMarshal.AsBytes(span);
            gzip.Write(raw);
        }
        public static ReadOnlySpan<T> Load<T>(string path) where T : unmanaged
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found at path ({path})");

            using var fs = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1 << 20,
                FileOptions.SequentialScan);

            using var gzip = new GZipStream(fs, CompressionMode.Decompress, leaveOpen: false);


            Span<byte> lenSpan = stackalloc byte[4];
            int read = gzip.Read(lenSpan);
            if (read != lenSpan.Length)
                throw new EndOfStreamException($"Expected {lenSpan.Length} bytes but only read {read}.");

            int count = BinaryPrimitives.ReadInt32LittleEndian(lenSpan);


            int byteCount = count * Unsafe.SizeOf<T>();
            byte[] buffer = new byte[byteCount];
            int offset = 0;
            while (offset < byteCount)
            {
                int n = gzip.Read(buffer, offset, byteCount - offset);
                if (n == 0)
                    throw new EndOfStreamException();
                offset += n;
            }

            return MemoryMarshal.Cast<byte, T>(buffer);
        }
    }
}
