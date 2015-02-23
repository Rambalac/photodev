using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.azi.Debayer;
using com.azi.Decoder.Panasonic.Rw2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace General.Tests
{
    [TestClass]
    public class AverageDebayerTests
    {
        [TestMethod]
        public void NotFail()
        {
            var decoder = new PanasonicRW2Decoder();

            var file = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open, FileAccess.Read);
            var raw = decoder.Decode(file);

            var debayer = new AverageDebayer();

            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 3;
            for (var iter = 0; iter < maxIter; iter++)
                debayer.Debayer(raw);
            stopwatch.Stop();

            Console.WriteLine("AverageDebayer: " + stopwatch.ElapsedMilliseconds / 3 + "ms");
        }
    }
}
