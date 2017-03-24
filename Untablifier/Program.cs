using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untablifier
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 2)
            {
                if (Directory.Exists(args[0]) == false)
                {
                    Console.WriteLine("Path {0} doesn't exist.", args[0]);
                    return;
                }

                untablifyAll(args[0], args[1]);
                Console.WriteLine("Total file(s): {0}    File(s) changed: {1}", fileProcessed, fileChanged);
            }

            else
            {
                Console.WriteLine(@"usage: Untablifier <targetDirectory> <fileEndWith>");
                Console.WriteLine(@"Example: Untablifier c:\target .cs");
            }
        }

        static UInt32 fileProcessed = 0;
        static UInt32 fileChanged = 0;

        static void untablifyAll(string path, string fileEndWith)
        {
            string[] files = Directory.GetFiles(path);

            foreach(string file in files)
            {
                fileProcessed++;
                if (file.EndsWith(fileEndWith))
                {
                    if (untablifyFile(file))
                        fileChanged++;
                }
            }

            string[] folders = Directory.GetDirectories(path);

            foreach(string folder in folders)
            {
                Console.WriteLine("Processing folder {0} ...", folder);
                untablifyAll(folder, fileEndWith);
            }
        }

        static Boolean untablifyFile(string filename)
        {
            String allText = File.ReadAllText(filename);
            StringBuilder sb = new StringBuilder();

            bool replaceWithSpace = true;
            UInt32 replacementCount = 0;

            foreach (char c in allText)
            {
                if (replaceWithSpace)
                {
                    if (c == '\t')
                    {
                        sb.Append("    ");
                        replacementCount++;
                    }
                    else
                    {
                        sb.Append(c);

                        if (c != '\r' && c != '\n' && c != ' ')
                            replaceWithSpace = false;
                    }

                }
                else
                {
                    if (c == '\r' || c == '\n')
                        replaceWithSpace = true;

                    sb.Append(c);
                }
            }

            if (replacementCount > 0)
            {
                Console.WriteLine("  File {0} ...", filename);
                Console.WriteLine("    Total replacement: {0}", replacementCount);

                File.WriteAllText(filename, sb.ToString());

                return true;
            }

            return false;
        }
    }
}
