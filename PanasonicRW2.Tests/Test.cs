using com.azi.Compressor;
using com.azi.Debayer;
using com.azi.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestDecodeDoesNotFail()
        {
            var decoder = new com.azi.decoder.panasonic.rw2.PanasonicRW2Decoder();

            var file = new FileStream(@"..\..\P1350577.RW2", FileMode.Open, FileAccess.Read);
            var rawimage = decoder.Decode(file);
            var debayer = new DebayerFilter
            {
                Debayer = new AverageDebayer()
            };
            var color16Image = debayer.Process(rawimage);
            var compressor = new ColorMap16ToRgb8CompressorFilter
            {
                Compressor = new SimpleCompressor()
            };
            var image = compressor.Process(color16Image);

        }
    }
}
