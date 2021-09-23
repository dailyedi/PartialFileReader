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
    public static class Reader
    {
        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadChars(string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            try
            {

                return ReadCharsAsync(filename, count, startIndex, startIndexPosition).Result;
            }
            catch (Exception ex)
            {

                ExceptionDispatchInfo EDI = ExceptionDispatchInfo.Capture(ex.InnerException);
                EDI.Throw();
                return "";
            }
        }
        /// <summary>
        /// Read certain amount of characters from a stream
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the string read from the file</returns>
        public static string ReadChars(Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
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
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsAsync(string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            await using var stream = File.OpenRead(filename);
            return await ReadCharsAsync(stream, count, startIndex, startIndexPosition);
        }

        /// <summary>
        /// Read certain amount of characters from a stream
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsAsync(Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
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
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytes(string filename, int count)
        {
            try
            {
                return ReadBytesAsync(filename, count).Result;
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
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesAsync(string filename, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            if (count < 0)
                throw new OverflowException($"The of ({count}) the array byte can't be null");

            var buffer = new byte[count];
            await using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            if (fs.Length < count)
                throw new ArgumentException($"The stream length is {fs.Length} while attempting to read {count} characters");

            if (fs.Length < startIndex) 
                throw new OverflowException($"The start Index ({startIndex}) is Greater than the count ({count})");

            fs.Seek(startIndex, startIndexPosition);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// Read certain amount of bytes from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the byte count required
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="count">the count of bytes to read</param>
        /// <returns>return the byte[] read from the file</returns>
        public static byte[] ReadBytes(Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
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
        public static async Task<byte[]> ReadBytesAsync(Stream stream, int count, int startIndex = 0, SeekOrigin startIndexPosition = SeekOrigin.Begin)
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
        public static string ReadCharsFromEmbeddedResource(Assembly assembly, string filename, int count)
            => ReadCharsFromEmbeddedResourceAsync(assembly, filename, count).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of string read from the file</returns>
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(Assembly assembly, string filename, int count)
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
        public static byte[] ReadBytesFromEmbeddedResource(Assembly assembly, string filename, int count)
            => ReadBytesFromEmbeddedResourceAsync(assembly, filename, count).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="assembly">the assembly with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(Assembly assembly, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = assembly.GetFileStream(filename);
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
        public static string ReadCharsFromEmbeddedResource(Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count)
            => ReadCharsFromEmbeddedResourceAsync(assembly, matchingPredicate, filename, count).Result;

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
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(assembly.GetFileStream(filename, matchingPredicate), count);
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
        public static byte[] ReadBytesFromEmbeddedResource(Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count)
            => ReadBytesFromEmbeddedResourceAsync(assembly, matchingPredicate, filename, count).Result;

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
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(Assembly assembly,
            Func<string, string, bool> matchingPredicate, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = assembly.GetFileStream(filename, matchingPredicate);
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
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(EmbeddedResourcesServices ers, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(ers.GetFileStream(filename), count);
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
        public static byte[] ReadBytesFromEmbeddedResource(EmbeddedResourcesServices ers, string filename, int count)
            => ReadBytesFromEmbeddedResourceAsync(ers, filename, count).Result;

        /// <summary>
        /// Read certain amount of characters from a file on desk
        /// opens the file in read mode and uses a stream reader
        /// to read the characters count required
        /// </summary>
        /// <param name="ers">the EmbeddedResourcesServices instance with the resource embedded in it</param>
        /// <param name="filename">the path for the file on desk to read</param>
        /// <param name="count">the count of characters to read</param>
        /// <returns>return the Task of byte[] read from the file</returns>
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(EmbeddedResourcesServices ers, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = ers.GetFileStream(filename);
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
        public static string ReadCharsFromEmbeddedResource(EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count)
            => ReadCharsFromEmbeddedResourceAsync(ers, matchingPredicate, filename, count).Result;

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
        public static async Task<string> ReadCharsFromEmbeddedResourceAsync(EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            return await ReadCharsAsync(ers.GetFileStream(filename, matchingPredicate), count);
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
        public static byte[] ReadBytesFromEmbeddedResource(EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count)
            => ReadBytesFromEmbeddedResourceAsync(ers, matchingPredicate, filename, count).Result;

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
        public static async Task<byte[]> ReadBytesFromEmbeddedResourceAsync(EmbeddedResourcesServices ers,
            Func<string, string, bool> matchingPredicate, string filename, int count)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file specified was not found on desk {filename}");

            var buffer = new byte[count];
            await using var fs = ers.GetFileStream(filename, matchingPredicate);
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion
    }
}
