using System;
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
            decoder.decode(file);

        }
    }
}
