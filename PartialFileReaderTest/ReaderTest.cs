using PartialFileReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PartialFileReaderTest
{
    public class ReaderTest
    {
        #region Helpers
        private string expectedCharsFromAssembly(string resourceName, int count, int startIndex = 0)
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
        private async Task<byte[]> expectedBytes(string path, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Stream expectedStream = new MemoryStream(bytes);


            expectedStream.Seek(startIndex, startIndexPosition);
            var expected = new byte[count];
            await expectedStream.ReadAsync(expected, 0, expected.Length);
            return expected;
        }
        private Stream GetFileStream(string path, int count)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }

        private string expectedChars_fromPath(string path, int count, int startIndex = 0)
        {

            string expected = File.ReadAllText(path).Substring(startIndex, count);
            return expected;
        }
        #endregion



        #region Char

        #region async

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_FileName_Working_count(int count)
        {

            string actual = await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_FileName_Working_NewstartIndex(int count)
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            string actual = await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex, startIndexPosition);


            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);
            Assert.Equal(expected, actual);

        }


        [Fact]
        public async Task ReadCharsAsync_FileName_NotWorking_Not_Found_File()
        {

            await Assert.ThrowsAsync<FileNotFoundException>(async () => await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New  Document.txt", 0));
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadCharsAsync_FileName_NotWorking_exceed_count(int count)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadCharsAsync_FileName_NotWorking_negative_count(int count)
        {

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_FileName_NotWorking_exceed_start_index(int count)
        {
            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_stream_Working_count(int count)
        {

            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadCharsAsync_stream_NotWorking_exceed_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadCharsAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadCharsAsync_stream_NotWorking_negative_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_stream_NotWorking_exceed_start_index(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync(expectedStream, count, 1000));
        }


        #endregion

        #region sync
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_FileName_Working_count(int count)
        {
            string actual = Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);


            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadChars_FileName_NotWorking_Not_Found_File()
        {
            Assert.Throws<FileNotFoundException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New  Document.txt", 0));

        }

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadChars_FileName_NotWorking_exceed_count(int count)
        {
            Assert.Throws<ArgumentException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadChars_FileName_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_FileName_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadChars_stream_Working_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadChars_stream_NotWorking_exceed_count(int count) // should fire ArgumentException
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            Assert.Throws<ArgumentException>(() => Reader.ReadChars(expectedStream, count));
        }


        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadChars_stream_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_stream_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }
        #endregion
        #endregion


        #region Byte

        #region async

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_FileName_Working_count(int count)
        {

            var actual = await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            var expected = await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            Assert.Equal(expected, actual);

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_FileName_Working_NewstartIndex(int count)
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            var actual = await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex, startIndexPosition);


            var expected =await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);
            Assert.Equal(expected, actual);

        }


        [Fact]
        public async Task ReadBytesAsync_FileName_NotWorking_Not_Found_File()
        {

            await Assert.ThrowsAsync<FileNotFoundException>(async () => await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New  Document.txt", 0));
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadBytesAsync_FileName_NotWorking_exceed_count(int count)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_FileName_NotWorking_negative_count(int count)
        {

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_FileName_NotWorking_exceed_start_index(int count)
        {
            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_stream_Working_count(int count)
        {

            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            var actual = await Reader.ReadBytesAsync(expectedStream, count);
            var expected =await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadBytesAsync_stream_NotWorking_exceed_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadBytesAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_stream_NotWorking_negative_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_stream_NotWorking_exceed_start_index(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync(expectedStream, count, 1000));
        }


        #endregion

        #region sync
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_FileName_Working_count(int count)
        {
            string actual = Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);


            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadBytes_FileName_NotWorking_Not_Found_File()
        {
            Assert.Throws<FileNotFoundException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New  Document.txt", 0));

        }

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadBytes_FileName_NotWorking_exceed_count(int count)
        {
            Assert.Throws<ArgumentException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadBytes_FileName_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_FileName_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytes_stream_Working_count(int count)
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = expectedChars_fromPath("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadBytes_stream_NotWorking_exceed_count(int count) // should fire ArgumentException
        {
            var expectedStream = GetFileStream("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            Assert.Throws<ArgumentException>(() => Reader.ReadChars(expectedStream, count));
        }


        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadBytes_stream_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_stream_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, 1000));
        }
        #endregion
        #endregion

    }
}
