using System;
using System.Diagnostics;
using System.IO;
using com.azi.Debayer;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace General.Tests
{
    [TestClass]
    public class PerfomanceTests
    {
        [TestMethod]
        public void FullProcessTest()
        {
            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 5;
            for (var iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open,
                    FileAccess.Read);
                var rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new DebayerFilter
                {
                    Debayer = new AverageBGGRDebayer()
                };
                var color16Image = debayer.Process(rawimage);

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                color16Image = white.Process(color16Image);

                var light = new LightFilter();
                light.AutoAdjust(color16Image);
                color16Image = light.Process(color16Image);

                var compressor = new RGBCompressorFilter
                {
                    Compressor = new SimpleCompressor()
                };
                compressor.Process(color16Image);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcess: " + stopwatch.ElapsedMilliseconds / maxIter + "ms");

        }
    }
}
