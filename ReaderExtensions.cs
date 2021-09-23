using System;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using EmbeddedResourcesHandler;

namespace PartialFileReader
{
    /// <summary>
    /// a partial file reader that allows you easily to read
    /// any amount of characters from a file instead of having to
    /// read the entire thing, which helps in cases when you need to
    /// read a file header or metadata without reading the entire file
    /// using the file name or a stream and the count you wish to read
    /// </summary>
    public static class ReaderExtensions
    {
        /// <summary>
        /// Read certain amount of characters from a stream
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsAsync(this Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (stream.Length < count)
                throw new ArgumentException($"The stream length is {stream.Length} while attempting to read {count} characters");

            if ((stream.Length < startIndex) || (count < 0))
                throw new OverflowException($"The start Index ({startIndex}) is Greater than the count ({count})");

            stream.Seek(startIndex, startIndexPosition);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var buffer = new char[count];
            var n = await reader.ReadBlockAsync(buffer, 0, count);

            var result = new char[n];

            Array.Copy(buffer, result, n);

            return new string(result);
        }

        /// <summary>
        /// Read certain amount of characters from a stream
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadChars(this Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            try
            {

                return ReadCharsAsync(stream, count, startIndex, startIndexPosition).Result;
            }
            catch (Exception ex)
            {

                ExceptionDispatchInfo EDI = ExceptionDispatchInfo.Capture(ex.InnerException);
                EDI.Throw();
                return "";
            }
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytes(this Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            try
            {

                return ReadBytesAsync(stream, count, startIndex, startIndexPosition).Result;
            }
            catch (Exception ex)
            {

                ExceptionDispatchInfo EDI = ExceptionDispatchInfo.Capture(ex.InnerException);
                EDI.Throw();
                return null;
            }
        }
        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesAsync(this Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (stream.Length < count)
                throw new ArgumentException($"The stream length is {stream.Length} while attempting to read {count} characters");


            if ((stream.Length < startIndex) || (count < 0))
                throw new OverflowException($"The start Index ({startIndex}) is Greater than the count ({count})");

            stream.Seek(startIndex, startIndexPosition);
            var buffer = new byte[count];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }


        #region embedded resource with default predicate

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadCharsFromEmbeddedResource(this Assembly assembly, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadCharsFromEmbeddedResourceAsync(assembly, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(this Assembly assembly, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(assembly.GetFileStream(filename), count);
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytesFromEmbeddedResource(this Assembly assembly, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadBytesFromEmbeddedResourceAsync(assembly, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(this Assembly assembly, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = assembly.GetFileStream(filename);
            fs.Seek(startIndex, startIndexPosition);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion

        #region embedded resource with matching predicate
        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadCharsFromEmbeddedResource(this Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadCharsFromEmbeddedResourceAsync(assembly, matchingPredicate, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(this Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(assembly.GetFileStream(filename, matchingPredicate), count, startIndex, startIndexPosition);
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytesFromEmbeddedResource(this Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadBytesFromEmbeddedResourceAsync(assembly, matchingPredicate, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(this Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = assembly.GetFileStream(filename, matchingPredicate);
            fs.Seek(startIndex, startIndexPosition);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion

        #region embedded resource with default predicate using EmbeddedResourcesServices instance
        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(this EmbeddedResourcesServices ers, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(ers.GetFileStream(filename), count, startIndex, startIndexPosition);
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytesFromEmbeddedResource(this EmbeddedResourcesServices ers, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadBytesFromEmbeddedResourceAsync(ers, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(this EmbeddedResourcesServices ers, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = ers.GetFileStream(filename);
            fs.Seek(startIndex, startIndexPosition);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion

        #region embedded resource with matching predicate using EmbeddedResourcesServices instance
        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadCharsFromEmbeddedResource(this EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadCharsFromEmbeddedResourceAsync(ers, matchingPredicate, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(this EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(ers.GetFileStream(filename, matchingPredicate), count, startIndex, startIndexPosition);
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytesFromEmbeddedResource(this EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
            => ReadBytesFromEmbeddedResourceAsync(ers, matchingPredicate, filename, count, startIndex, startIndexPosition).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(this EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = ers.GetFileStream(filename, matchingPredicate);
            fs.Seek(startIndex, startIndexPosition);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion
    }
}