using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace ChannelMixMatcher
{
    static class Helpers
    {

        static public double[] findCoefficients(double[] Rout, double[] Rin, double[] Gin, double[] Bin)
        {
          
            // Works (but division by zero error)! double a = (Rout[2] / Rin[2] / (1 - Rin[1] / Gin[1] * Gin[2] / Rin[2]) - Rout[1] / Gin[1] * Gin[2] / Rin[2] / (1 - Rin[1] / Gin[1] * Gin[2] / Rin[2]) + Rout[0] / Bin[0] / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]) * (Bin[1] / Gin[1] * Gin[2] / Rin[2] - Bin[2] / Rin[2]) / (1 - Rin[1] / Gin[1] * Gin[2] / Rin[2]) - Rout[1] / Gin[1] * Gin[0] / Bin[0] / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]) * (Bin[1] / Gin[1] * Gin[2] / Rin[2] - Bin[2] / Rin[2]) / (1 - Rin[1] / Gin[1] * Gin[2] / Rin[2])) / (1 - (Rin[1] / Gin[1] * Gin[0] / Bin[0] - Rin[0] / Bin[0]) / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]) * (Bin[1] / Gin[1] * Gin[2] / Rin[2] - Bin[2] / Rin[2]) / (1 - Rin[1] / Gin[1] * Gin[2] / Rin[2]));
            //double c = Rout[0] / Bin[0] / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]) - Rout[1] / Gin[1] * Gin[0] / Bin[0] / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]) + a * (Rin[1] / Gin[1] * Gin[0] / Bin[0] - Rin[0] / Bin[0]) / (1 - Bin[1] / Gin[1] * Gin[0] / Bin[0]);
            //double b = Rout[1] / Gin[1] - a * Rin[1] / Gin[1] - c * Bin[1] / Gin[1];

            // Improved versions: simplified
            double d, e, f, g, h, i, j, k, l, m, n, o;
            (d, e, f) = (Rout[0], Rout[1], Rout[2]);
            (g, h, i) = (Rin[0], Rin[1], Rin[2]);
            (j, k, l) = (Gin[0], Gin[1], Gin[2]);
            (m, n, o) = (Bin[0], Bin[1], Bin[2]);

            double a = (-d * k * o + d * l * n + e * j * o - e * l * m - f * j * n + f * k * m) / (-g * k * o + g * l * n + h * j * o - h * l * m - i * j * n + i * k * m);
            double c = (-d * h * l + d * i * k + e * g * l - e * i * j - f * g * k + f * h * j) / (-g * k * o + g * l * n + h * j * o - h * l * m - i * j * n + i * k * m);
            double b = (-d * h * o + d * i * n + e * g * o - e * i * m - f * g * n + f * h * m) / (g * k * o - g * l * n - h * j * o + h * l * m + i * j * n - i * k * m);


            return new double[3] {a,b,c };
        }

        static public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        static public string matrixToString<T>(T[,] matrix)
        {
            return "{{" + matrix[0, 0].ToString() + "," + matrix[0, 1].ToString() + "," + matrix[0, 2].ToString() + "},{" + matrix[1, 0].ToString() + "," + matrix[1, 1].ToString() + "," + matrix[1, 2].ToString() + "},{" + matrix[2, 0].ToString() + "," + matrix[2, 1].ToString() + "," + matrix[2, 2].ToString() + "}}";
        }

        static public Bitmap ResizeBitmapNN(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(sourceBMP, 0, 0, width, height);
            }
            return result;
        }
    }
}
