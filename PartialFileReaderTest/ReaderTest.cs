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
        #region Testing_Read_Chars_Methods

        #region Testing_Async_Read_Chars_Methods

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_FileName_Working_count(int count)
        {

            string actual = await Reader.ReadCharsAsync(@"..\..\..\Testing\New Text Document.txt", count);

            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);
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

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            string actual = await Reader.ReadCharsAsync(@"..\..\..\Testing\New Text Document.txt", count, startIndex, startIndexPosition);


            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count, startIndex);
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
            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadCharsAsync(@"..\..\..\Testing\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadCharsAsync_FileName_NotWorking_negative_count(int count)
        {

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_FileName_NotWorking_exceed_start_index(int count)
        {
            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadCharsAsync_stream_Working_count(int count)
        {

            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadCharsAsync_stream_NotWorking_exceed_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadCharsAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadCharsAsync_stream_NotWorking_negative_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

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
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadCharsAsync(expectedStream, count, 1000));
        }


        #endregion

        #region Testing_Sync_Read_Chars_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_FileName_Working_count(int count)
        {
            string actual = Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count);

            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);


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
            Assert.Throws<ArgumentException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadChars_FileName_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_FileName_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadChars_stream_Working_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadChars_stream_NotWorking_exceed_count(int count) // should fire ArgumentException
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            Assert.Throws<ArgumentException>(() => Reader.ReadChars(expectedStream, count));
        }


        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadChars_stream_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadChars_stream_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }
        #endregion
        #endregion


        #region Testing_Read_Bytes_Methods

        #region Testing_Async_Read_Bytes_Methods

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_FileName_Working_count(int count)
        {

            var actual = await Reader.ReadBytesAsync(@"..\..\..\Testing\New Text Document.txt", count);

            var expected = await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count);

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

            byte[] bytes = File.ReadAllBytes(@"..\..\..\Testing\New Text Document.txt");

            Stream stream = new MemoryStream(bytes);
            int startIndex = 500;
            SeekOrigin startIndexPosition = SeekOrigin.Begin;
            var actual = await Reader.ReadBytesAsync(@"..\..\..\Testing\New Text Document.txt", count, startIndex, startIndexPosition);


            var expected =await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count, startIndex);
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
            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadBytesAsync(@"..\..\..\Testing\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_FileName_NotWorking_negative_count(int count)
        {

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_FileName_NotWorking_exceed_start_index(int count)
        {
            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytesAsync_stream_Working_count(int count)
        {

            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            var actual = await Reader.ReadBytesAsync(expectedStream, count);
            var expected =await Helpers.expectedBytes(@"..\..\..\Testing\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public async Task ReadBytesAsync_stream_NotWorking_exceed_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            await Assert.ThrowsAsync<ArgumentException>(async () => await Reader.ReadBytesAsync(expectedStream, count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public async Task ReadBytesAsync_stream_NotWorking_negative_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

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
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            await Assert.ThrowsAsync<OverflowException>(async () => await Reader.ReadBytesAsync(expectedStream, count, 1000));
        }


        #endregion

        #region Testing_Sync_Read_Bytes_Methods
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_FileName_Working_count(int count)
        {
            string actual = Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count);

            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);


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
            Assert.Throws<ArgumentException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));
        }
        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadBytes_FileName_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_FileName_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public async Task ReadBytes_stream_Working_count(int count)
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            string actual = await Reader.ReadCharsAsync(expectedStream, count);
            string expected = Helpers.expectedChars_fromPath(@"..\..\..\Testing\New Text Document.txt", count);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        public void ReadBytes_stream_NotWorking_exceed_count(int count) // should fire ArgumentException
        {
            var expectedStream = Helpers.GetFileStream(@"..\..\..\Testing\New Text Document.txt", count);

            Assert.Throws<ArgumentException>(() => Reader.ReadChars(expectedStream, count));
        }


        [Theory]
        [InlineData(-36)]
        [InlineData(-68)]
        [InlineData(-71)]
        [InlineData(-395)]
        public void ReadBytes_stream_NotWorking_negative_count(int count)
        {

            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count));

        }
        [Theory]
        [InlineData(0)]
        [InlineData(36)]
        [InlineData(68)]
        [InlineData(71)]
        [InlineData(395)]
        public void ReadBytes_stream_NotWorking_exceed_start_index(int count)
        {
            Assert.Throws<OverflowException>(() => Reader.ReadChars(@"..\..\..\Testing\New Text Document.txt", count, 1000));
        }
        #endregion
        #endregion

    }
}
