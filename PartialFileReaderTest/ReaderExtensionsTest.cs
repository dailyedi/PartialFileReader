using EmbeddedResourcesHandler;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace PartialFileReader.Test
{
    public class ReaderExtensionsTest
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
        private string expectedChars(string path, int count, int startIndex = 0)
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
        public async Task ReadCharsAsync_Working_count(int count)
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            string actual = await stream.ReadCharsAsync(count);

            string expected = expectedChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_Working_NewstartIndex(int count)
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            string actual = await stream.ReadCharsAsync(count, startIndex, startIndexPosition);

            string expected = expectedChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public async Task ReadCharsAsync_NotWorking_exceed_count(int count)
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<ArgumentException>(async () => await stream.ReadCharsAsync(count));

        }

        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadCharsAsync_NotWorking_negative_count(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(() => stream.ReadCharsAsync(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(() => stream.ReadCharsAsync(count, 1000));

        }
        #endregion

        #region sync
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);



            string expected = expectedChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);

            var actual = stream.ReadChars(count);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_Working_NewstartIndex(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);

            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;


            string expected = expectedChars("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);

            var actual = stream.ReadChars(count, startIndex, startIndexPosition);

            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        public void ReadChars_NotWorking(int count)//Don't Excute the error
        {

            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);

            Assert.Throws<ArgumentException>(() => stream.ReadChars(count));

        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadChars_NotWorking_negative_count(int count)// OverflowException not handled 
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadChars(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadChars(count, 1000));

        }
        #endregion
        #endregion
        #region Bytes

        #region async 
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytesAsync_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            var actual = await actualStream.ReadBytesAsync(count);

            var expected = await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);


                Assert.Equal(expected, actual);
            
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytesAsync_Working_NewstartIndex(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            int startIndex = 500;
            Stream actualStream = new MemoryStream(bytes);
            var actual = await actualStream.ReadBytesAsync(count, startIndex);

            var expected = await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);

            Assert.Equal(expected, actual);

        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public async Task ReadBytesAsync_NotWorking_count_is_greater_than_the_streamLenght(int count) // nigative (another error for giving nigative idex for ) && strat index (not error return 0 index array of bytes)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<ArgumentException>(() => stream.ReadBytesAsync(count, 1000));

        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_NotWorking_negative_count(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(() => stream.ReadBytesAsync(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(() => stream.ReadCharsAsync(count, 1000));

        }
        #endregion

        #region sync 
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytes_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            var actual = actualStream.ReadBytes(count);

            var expected = await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count);


            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytes_Working_NewstartIndex(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            int startIndex = 500;
            var actual = actualStream.ReadBytes(count, startIndex);

            var expected = await expectedBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt", count, startIndex);


            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public void ReadBytes_NotWorking(int count) // nigative (another error for giving nigative idex for ) && strat index (not error return 0 index array of bytes)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<ArgumentException>(() => stream.ReadBytes(count, 1000));

        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadBytes_NotWorking_negative_count(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadBytes(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadBytes(count, 1000));

        }
        #endregion


        #endregion

        #region embedded resource with default predicate
        //[Theory]
        //[InlineData(925)]
        //[InlineData(950)]
        //[InlineData(1000)]
        //public async Task ReadCharsFromEmbeddedResourceAsync_NotWorking(int count)
        //{
        //    //byte[] bytes = File.ReadAllBytes("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Text Document.txt");
        //    //Stream stream = new MemoryStream(bytes);
        //    Assembly assem = Assembly.GetCallingAssembly();
        //    await Assert.ThrowsAsync<FileNotFoundException>(() => assem.ReadCharsFromEmbeddedResourceAsync("D:\\Temp\\PartialFileReader\\bin\\Debug\\netstandard2.1\\New Document.txt", count));
        //}

        //[Theory]
        //[InlineData(0)]
        //[InlineData(36)]
        //[InlineData(68)]
        //[InlineData(71)]
        //[InlineData(395)]
        //public async void ReadCharsFromEmbeddedResourceAsync_Working(int count)
        //{
        //    Assembly assem = Assembly.GetExecutingAssembly();
        //    var resourceName = "PartialFileReaderTest.DLL.NewTextDocument.txt";
        //    var expected = expectedCharsFromAssembly(resourceName, count);
        //    var x = assem.GetFileStream(resourceName);
        //    var actual = await assem.ReadCharsFromEmbeddedResourceAsync("D:\\VS projects\\PartialFileReaderTest\\DLL\\NewTextDocument.txt", count);


        //    Assert.Equal(expected, actual);
        //}
        #endregion
    }
}

