using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zlib;

namespace unzip_pc3
{
    class Program
    {
        // args[0] - ключ для паковки / распаковки
        //              e - распаковать / extract
        //              c - запаковать / compress
        // args[1] - путь до файла, который будем паковать или распаковывать / path to file
        
        static void Main(string[] args)
        {
            //' на основе вот этого топака : http://www.theswamp.org/index.php?topic=41529.0
            if (args.Length == 2)
            {
                if (File.Exists(args[1]))
                {
                    // тут распаковываем архивчик, Extract
                    if ((args[0] == "e") || (args[0] == "-e"))
                    {
                        string fileName = args[1];
                        string ext = Path.GetExtension(fileName).ToUpper();
                        if ((0 == String.Compare(ext, ".PC3"))||(0 == String.Compare(ext, ".PMP"))||(0 == String.Compare(ext, ".CTB")))
                        {
                            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
                            {
                                fs.Seek(60L, SeekOrigin.Begin);
                                using (ZlibStream zs = new ZlibStream(fs, CompressionMode.Decompress))
                                {
                                   // Console.WriteLine("\nStart:");
                                    int chr;
                                    chr = 0;
                                    string str = "";

                                    while (chr > -1)
                                    {
                                        chr = zs.ReadByte();
                                        if (chr > -1)
                                        {
                                            str = str + Convert.ToChar(chr);
                                        }
                                    }
                                    //Console.Write(str);
                                    //Console.WriteLine("\nend:");
                                    /* using (StreamReader sr = new StreamReader(zs))
                                    {
                                         //тут текстово содержание файла
                                         string s = sr.ReadToEnd();
                                         //Console.WriteLine(s);
                                    */
                                         using (FileStream fs_out = File.Open(fileName + ".txt", FileMode.Create, FileAccess.ReadWrite))
                                         {
                                            fs_out.Write(Encoding.Default.GetBytes(str), 0, Encoding.Default.GetBytes(str).Length);
                                        }
                                    //}
                                    Console.WriteLine("\n File saved to {0}.txt ", fileName);
                                }
                            }
                        }
                        else
                        {
                            Console.Write("File {0} is not a .pc3 file", args[1]);
                        }
                    }
                    else if ((args[0] == "c") || (args[0] == "-c"))// Тут запаковываем все обратно, COMPRESS
                    {
                        string fileName = args[1];
                        string ext = Path.GetExtension(fileName);
                        if (0 == String.Compare(ext.ToUpper(), ".TXT"))
                        {
                            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
                            {
                                using (StreamReader sr = new StreamReader(fs, Encoding.Default)) /*Mod by Arie add encoding*/
                                {
                                    // заголовок файла, без него автокад не работает
                                    String pref_s = "PIAFILEVERSION_2.0,PC3VER1,compress\r\npmzlibcodec";
                                    /*
                                    is a good starting point, but contains several errors/lack of information
                                    The header size is 60 No 59
                                    bytes 49->52 = ZlibCodec.Adler32
                                    bytes 53->56 = decompresse stream size
                                    bytes 57->60 = compressed stream size
                                    Эти данные(байты) должны вычеслятся
                                    */
                                    String s = sr.ReadToEnd();
                                    //Console.Write(s);
                                    //Byte[] ZlibCodec_Adler32 = { 157, 94, 173, 006 };
                                    long decompresse_stream_size = fs.Length;
                                    //тут считаем размер запакованных данных
                                    byte[] compressed_stream_size;
                                    using (var ms = new MemoryStream())
                                    {
                                        using (ZlibStream deflateStream = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.BestCompression, false))
                                        {
                                            deflateStream.Write(Encoding.Default.GetBytes(s), 0, Encoding.Default.GetBytes(s).Length);
                                        }
                                        compressed_stream_size = ms.ToArray();
                                    }

                                    // Тут все пишем в файл
                                    using (FileStream fs_out = File.Open(fileName + ".pc3", FileMode.Create, FileAccess.ReadWrite))
                                    {
                                        using (ZlibStream zs = new ZlibStream(fs_out, CompressionMode.Compress,
                                                                                 CompressionLevel.BestCompression, false))
                                        {
                                            fs_out.Write(Encoding.Default.GetBytes(pref_s), 0, Encoding.Default.GetBytes(pref_s).Length);
                                            //fs_out.Write(ZlibCodec_Adler32, 0, 4);
                                            fs_out.Write(BitConverter.GetBytes(new ZlibCodec(CompressionMode.Compress).Adler32), 0, 4);
                                            fs_out.Write(BitConverter.GetBytes(decompresse_stream_size), 0, 4);
                                            fs_out.Write(BitConverter.GetBytes(compressed_stream_size.Length), 0, 4);
                                            zs.Write(Encoding.Default.GetBytes(s), 0, Encoding.Default.GetBytes(s).Length);
                                        }
                                    }
                                    Console.WriteLine("\n File saved to {0}.pc3 ", fileName);
                                }
                            }
                        }
                        else
                        {
                            Console.Write("File {0} is not a .txt file", args[1]);
                        }
                    }
                    else
                    {
                        Console.Write("incorrect input\nFormat unzip_pc3 (AutoCAD zip/unzip .pc3 files) \n to extract: unzip_pc3 e xxx.pc3\n to Compress: unzip_pc3 c xxx.txt");
                    }
                }
                else
                {
                    Console.Write("File {0} does not exist", args[1]);
                }
            }
            else
            {
                Console.Write("incorrect input\nFormat unzip_pc3 (AutoCAD zip/unzip .pc3 files) \n to extract: unzip_pc3 e xxx.pc3\n to Compress: unzip_pc3 c xxx.txt");
            }
        }
    }
}
