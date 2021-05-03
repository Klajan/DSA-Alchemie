﻿using System.IO;
using System.IO.Compression;

namespace Compress
{
    static class Program
    {
        static void WriteDeflate(string pathIN, string pathOUT)
        {
            using FileStream streamIN = new (pathIN, FileMode.Open);
            using FileStream streamOUT = new (pathOUT, FileMode.Create);
            using DeflateStream deflate = new (streamOUT, CompressionLevel.Optimal);
            streamIN.CopyTo(deflate);
        }

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (File.Exists(args[0]))
                {
                    WriteDeflate(args[0], args[0] + ".deflate");
                }
            }
        }
    }
}
