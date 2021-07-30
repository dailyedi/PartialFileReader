using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PartialFileReader
{
    public static class Reader
    {
        public static string ReadChars(string filename, int count) => ReadCharsAsync(filename, count).Result;

        public static string ReadChars(Stream stream, int count) => ReadCharsAsync(stream, count).Result;

        public static async Task<string> ReadCharsAsync(string filename, int count)
        {
            using (var stream = File.OpenRead(filename))
            {
                return await ReadCharsAsync(stream, count);
            }
        }

        public static async Task<string> ReadCharsAsync(Stream stream, int count)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var buffer = new char[count];
                var n = await reader.ReadBlockAsync(buffer, 0, count);

                var result = new char[n];

                Array.Copy(buffer, result, n);

                return new string(result);
            }
        }

        public static async Task<byte[]> ReadBytes(string filename, int count)
        {
            var buffer = new byte[count];
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                await fs.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
