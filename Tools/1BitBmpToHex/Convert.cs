﻿using System;
using System.Text;
using System.Drawing;
using System.IO;

namespace BmpToHex
{   
    /// <summary>
    /// Converts a 1 bit depth bitmap to a series of hex codes for inclusion in a program.
    /// Also outputs a binary file, named after the original file and a ".bin" extension, containing the converted bytes.
    /// This conversion program fits in context with the netduino tutorial written by Fabien Royer & Bertrand Le Roy.
    /// See http://fabienroyer.wordpress.com/ and http://weblogs.asp.net/bleroy/archive/tags/Netduino/default.aspx for more details
    /// You can try it on the bitmaps in the "Sample bitmaps" folder included with this project.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Write("Please provide a filename for the bitmap to convert.");
                return;
            }

            using(var bmp = new Bitmap(args[0]))
            {
                if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
                {
                    Console.Write("Please provide a monochrome bitmap to convert (1 bit depth).");
                    return;
                }

                byte Buffer;
	            int Count = 0;

                FileStream bin = new FileStream( (string) (args[0] + ".bin"), FileMode.Create, FileAccess.Write);

	            for (int row = 0; row < bmp.Height; row++)
	            {
		            Buffer = 0;
		            Count = 0;

		            for (int column = 0; column < bmp.Width; column++)
		            {
			            Color pix = bmp.GetPixel(column, row);

                        if (pix.R != 0 || pix.G != 0 || pix.B != 0)
			            {
				            Buffer |= 0;
			            }
			            else
			            {
				            Buffer |= 1;
			            }

			            if (Count == 7)
			            {
				            Console.Write("0x{0},",Buffer.ToString("x"));
                            bin.WriteByte(Buffer);
				            Buffer = 0;
				            Count = 0;
			            }
			            else
			            {
				            Count++;
				            Buffer <<= 1;
			            }
		            }
		            Console.Write("\r\n");
	            }

                bin.Close();
            }
        }
    }
}
