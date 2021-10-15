using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Compress
{
    internal static class Program
    {
        private static SHA256 Sha256 = SHA256.Create();

        private static bool CompareArray<T>(ref T[] array1, ref T[] array2)
        {
            bool isSame = true;
            if(array1.Length == array2.Length)
            {
                for(int i = 0; i < array1.Length; i++)
                {
                    isSame &= array1[i].Equals(array2[i]);
                }
            }
            return isSame;
        }

        private static void WriteDeflate(string pathIN, string pathOUT)
        {
            using FileStream streamIN = new(pathIN, FileMode.Open);
            using FileStream streamOUT = new(pathOUT, FileMode.Create);
            using DeflateStream deflate = new(streamOUT, CompressionLevel.Optimal);
            streamIN.CopyTo(deflate);
        }

        private static async Task<bool> CompareHash(string pathFile, string pathArchive)
        {
            
            using FileStream stream1 = new(pathFile, FileMode.Open);
            using FileStream stream2 = new(pathArchive, FileMode.Open);
            using DeflateStream deflate = new(stream2, CompressionMode.Decompress);
            var hashTask1 = Sha256.ComputeHashAsync(stream1);
            var hashTask2 = Sha256.ComputeHashAsync(deflate);
            var hash1 = await hashTask1;
            var hash2 = await hashTask2;
            return CompareArray<byte>(ref hash1, ref hash2);
        }

        private static void Main(string[] args)
        {
            if (args.Length <= 0) return;
            string fileIN = args[0];
            string fileOUT = args[0] + ".deflate";
            if (args.Length >= 2)
            {
                fileOUT = args[1];
            }
            if (File.Exists(args[0]))
            {
                bool isSame = false;
                if (File.Exists(fileOUT))
                {
                    var task = CompareHash(fileIN, fileOUT);
                    task.Wait();
                    isSame = task.Result;
                    Console.WriteLine($"File {fileIN} and {fileOUT} skipped, SHA256 is equal!");
                }
                if (!isSame) WriteDeflate(fileIN, fileOUT);
            }
        }
    }
}