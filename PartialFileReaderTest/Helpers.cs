using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PartialFileReaderTest
{
    class Helpers
    {
        public static string expectedCharsFromAssembly(string resourceName, int count, int startIndex = 0)
        {
            string result;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader expectedStream = new StreamReader(stream))
            {
                result = expectedStream.ReadToEnd();
            }

            return result.Substring(startIndex, count);
        }
        public static async Task<byte[]> expectedBytes(string path, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Stream expectedStream = new MemoryStream(bytes);

            expectedStream.Seek(startIndex, startIndexPosition);
            var expected = new byte[count];
            await expectedStream.ReadAsync(expected, 0, expected.Length);
            return expected;
        }
        public static string expectedChars(string path, int count, int startIndex = 0)
        {

            string expected = File.ReadAllText(path).Substring(startIndex, count);
            return expected;
        }


        public static Stream GetFileStream(string path, int count)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }

        public static string expectedChars_fromPath(string path, int count, int startIndex = 0)
        {

            string expected = File.ReadAllText(path).Substring(startIndex, count);
            return expected;
        }
    }
}
