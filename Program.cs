using System;
using System.IO;
using System.Text;
using System.Drawing;

namespace IJPCG
{
    class Program
    {
        static private int lno = 100;                   // 行番号
        static private int adr = 0x700;                 // POKEするアドレス

        /// <summary>
        /// メイン
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: IJPCG [Filename].png");
            }
            else
            {
                string filename = args[0];
                if (File.Exists(filename) == false)
                {
                    Console.WriteLine("'" + filename + "' File not found.");
                }
                else
                {
                    ConvertBitmap(filename);
                }


            }
        }

        #region 汎用メソッド

        /// <summary>
        /// ビットマップ変換
        /// </summary>
        /// <param name="filename"></param>
        static private void ConvertBitmap(string filename)
        {
            //            print("ConvertBitmap(" + filename + ")");

            var bitmap = new Bitmap(System.Drawing.Image.FromFile(filename));
//            var bitmap = new Bitmap(filename);

            //           print("Pixel format: " + bitmap.PixelFormat.ToString());
            //           print("Width: " + bitmap.Width + " Height: " + bitmap.Height);

            for (int j = 0; j < bitmap.Height; j += 8)
            {
                for (int i = 0; i < bitmap.Width; i += 8)
                {
                    var sb = new StringBuilder();
                    sb.Append(lno.ToString());
                    sb.AppendFormat(" POKE #{0:X3},", adr);
                    for (int y = 0; y < 8; y++)
                    {
                        int bt = 0;
                        int py = j + y;
                        for (int x = 0; x < 8; x++)
                        {
                            int px = i + x;
                            Color col = bitmap.GetPixel(px, py);
                            bt <<= 1;
                            if (col.A > 128)
                            {
                                bt |= 1;
                            }
                        }
                        sb.AppendFormat(bt.ToString());
                        if (y < 7)
                        {
                            sb.Append(',');
                        }
                        else
                        {
                            Console.WriteLine(sb.ToString());
                            lno += 10;
                        }
                        adr++;
                    }

                }

            }
        }

        #endregion

    }
}
