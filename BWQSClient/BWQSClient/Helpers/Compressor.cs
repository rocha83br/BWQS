using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BWQS_Client.Helpers
{
    public static class Compressor
    {
        static GZipStream gzipStream = null;
        static MemoryStream memSource = null;
        static MemoryStream memDestination = null;

        public static byte[] ZipBinary(byte[] rawSource)
        {
            memDestination = new MemoryStream();
            memSource = new MemoryStream(rawSource);
            gzipStream = new GZipStream(memDestination, CompressionMode.Compress);

            Streamer.CopyStream(memSource, gzipStream);

            gzipStream.Close();

            return memDestination.ToArray();
        }

        public static byte[] UnZipBinary(byte[] compressedSource)
        {
            byte[] unpackedContent = new byte[compressedSource.Length * 10];
            memSource = new MemoryStream(compressedSource);

            gzipStream = new GZipStream(memSource, CompressionMode.Decompress);

            var readedBytes = gzipStream.Read(unpackedContent, 0, unpackedContent.Length);

            memDestination = new MemoryStream(unpackedContent, 0, readedBytes);

            return memDestination.ToArray();
        }

        public static string ZipText(string rawText)
        {
            var cont = 0;
            byte[] rawBinary = new byte[rawText.Length];
            byte[] compressedBinary = null;

            foreach (var chr in rawText.ToCharArray())
                rawBinary[cont++] = Convert.ToByte(chr);

            compressedBinary = ZipBinary(rawBinary);

            return Convert.ToBase64String(compressedBinary);
        }

        public static string UnZipText(string compressedText)
        {
            StringBuilder result = new StringBuilder();
            byte[] compressedBinary = Convert.FromBase64String(compressedText);
            byte[] destinBinary = UnZipBinary(compressedBinary);

            foreach (var byt in destinBinary)
                result.Append(Convert.ToChar(byt));

            return result.ToString();
        }
    }
}
