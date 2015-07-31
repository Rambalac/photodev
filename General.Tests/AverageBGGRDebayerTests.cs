using System.Diagnostics;
using System.IO;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters.RawToColorMap16.Demosaic;
using com.azi.Image;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace General.Tests
{
    [TestClass]
    public class AverageBGGRDebayerTests
    {
        [TestMethod]
        public void NotFail()
        {
            var decoder = new PanasonicRW2Decoder();

            var file = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open, FileAccess.Read);
            var raw = decoder.Decode(file);

            var debayer = new AverageBGGRDebayer();

            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 3;
            for (var iter = 0; iter < maxIter; iter++)
                debayer.Process((RawBGGRMap<ushort>) raw.Raw);
            stopwatch.Stop();

            //Console.WriteLine("AverageBGGRDebayer: " + stopwatch.ElapsedMilliseconds/3 + "ms");
        }
    }
}