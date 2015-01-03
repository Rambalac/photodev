using com.azi.decoder.panasonic.rw2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace photodev
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static BitmapSource OpenFile(string p)
        {
            Stream stream = new FileStream(p, FileMode.Open, FileAccess.Read);
            var image = new PanasonicRW2Decoder().decode(stream);
            return BitmapSource.Create(image.Width, image.Height, 75, 75, PixelFormats.Rgb24, null, image.GetBytesRgb24(), image.GetStrideRgb24());
        }
    }
}
