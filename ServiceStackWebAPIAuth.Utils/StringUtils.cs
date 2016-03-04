using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackWebAPIAuth.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Encode a string to a Base64 URL with safe encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Decode a Base64 URL safe string to a byte[].
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] Decode(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return Convert.FromBase64String(Pad(text.Replace('-', '+').Replace('_', '/')));
        }

        /// <summary>
        /// Pad a string to create a valid Base64 string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Pad(string text)
        {
            var padding = 3 - ((text.Length + 3) % 4);
            if (padding == 0)
            {
                return text;
            }
            return text + new string('=', padding);
        }

        /// <summary>
        /// Zip Decompress an array of bytes.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
