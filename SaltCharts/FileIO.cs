using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Yaml.Serialization;

namespace SaltCharts
{
    static class FileIO
    {
        public static Map Load(String file)
        {
            var serializer = new YamlSerializer();
            if (File.Exists(file))
                return (Map)(serializer.DeserializeFromFile(file)[0]);
            else
               return new Map();
        }

        public static void Save(String file, Map m)
        {
            var serializer = new YamlSerializer();
            serializer.SerializeToFile(file, m);
        }

        public static bool SaveImage(String file, Map m)
        {
            // Build the Image
            Bitmap bmp = new Bitmap(SaltCharts.Properties.Resources.Grid);
            Graphics g = Graphics.FromImage(bmp);
                //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                foreach (IMapItem i in m.GetItems())
                    g.DrawImageUnscaled(i.GetImage(), i.GetLocation().X, i.GetLocation().Y);

                // Find the format
                ImageFormat format;
                string[] split = file.Split('.');
                string extension;
                if (split.Length < 2)
                    extension = ".err";
                else
                    extension = split[split.Length - 1];

                switch (extension.ToLower())
                {
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                        format = ImageFormat.Jpeg;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    case "tiff":
                    case "tif":
                        format = ImageFormat.Tiff;
                        break;
                    default:
                        return false;
                }

                bmp.Save(file, format);
                return true;
            }
    }
}
