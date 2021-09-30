using EmbeddedResourcesHandler;
using PartialFileReaderTest;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace PartialFileReader.Test
{
    public class ReaderExtensionsTest
    {
        #region Testing_Read_Chars_Methods
        #region Testing_Async_Read_Chars_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_Working_count(int count)
        {

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            string actual = await stream.ReadCharsAsync(count);

            string expected = Helpers.expectedChars(@"..\..\..\Testing\New Text Document.txt", count);
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

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            string actual = await stream.ReadCharsAsync(count, startIndex, startIndexPosition);

            string expected = Helpers.expectedChars(@"..\..\..\Testing\New Text Document.txt", count, startIndex);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public async Task ReadCharsAsync_NotWorking_exceed_count(int count)
        {

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(async() =>await stream.ReadCharsAsync(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(async() =>await stream.ReadCharsAsync(count, 1000));

        }
        #endregion

        #region  Testing_Sync_Read_Chars_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);



            string expected = Helpers.expectedChars(@"..\..\..\Testing\New Text Document.txt", count);

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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);

            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;


            string expected = Helpers.expectedChars(@"..\..\..\Testing\New Text Document.txt", count, startIndex);

            var actual = stream.ReadChars(count, startIndex, startIndexPosition);

            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        public void ReadChars_NotWorking(int count)//Don't Excute the error
        {

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadChars(count, 1000));

        }
        #endregion
        #endregion

        #region  Testing_Read_Bytes_Methods
        #region Testing_Async_Read_Bytes_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytesAsync_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            var actual = await actualStream.ReadBytesAsync(count);

            var expected = await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count);


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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            int startIndex = 500;
            Stream actualStream = new MemoryStream(bytes);
            var actual = await actualStream.ReadBytesAsync(count, startIndex);

            var expected = await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count, startIndex);

            Assert.Equal(expected, actual);

        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public async Task ReadBytesAsync_NotWorking_count_is_greater_than_the_streamLenght(int count) // nigative (another error for giving nigative idex for ) && strat index (not error return 0 index array of bytes)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<ArgumentException>(async() =>await stream.ReadBytesAsync(count, 1000));

        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_NotWorking_negative_count(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(async() =>await stream.ReadBytesAsync(count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_NotWorking_exceed_start_index(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            await Assert.ThrowsAsync<OverflowException>(async () =>await stream.ReadCharsAsync(count, 1000));

        }
        #endregion

        #region Testing_Sync_Read_Bytes_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async void ReadBytes_Working(int count)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            var actual = actualStream.ReadBytes(count);

            var expected = await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count);


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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream actualStream = new MemoryStream(bytes);
            int startIndex = 500;
            var actual = actualStream.ReadBytes(count, startIndex);

            var expected = await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count, startIndex);


            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(925)]
        [InlineData(950)]
        [InlineData(1000)]
        public void ReadBytes_NotWorking(int count) // nigative (another error for giving nigative idex for ) && strat index (not error return 0 index array of bytes)
        {
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
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
            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
            Stream stream = new MemoryStream(bytes);

            Assert.Throws<OverflowException>(() => stream.ReadBytes(count, 1000));

        }
        #endregion
        #endregion

        #region embedded_resource_with_default_predicate
        //[Theory]
        //[InlineData(925)]
        //[InlineData(950)]
        //[InlineData(1000)]
        //public async Task ReadCharsFromEmbeddedResourceAsync_NotWorking(int count)
        //{
        //    //byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");
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

