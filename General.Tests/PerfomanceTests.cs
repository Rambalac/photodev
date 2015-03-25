﻿using System;
using System.Diagnostics;
using System.IO;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters;
using com.azi.Filters.ColorMap16;
using com.azi.Filters.ColorMap16ToRgb8;
using com.azi.Filters.RawToColorMap16.Demosaic;
using com.azi.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace General.Tests
{
    [TestClass]
    public class PerfomanceTests
    {
        [TestMethod]
        public void FullProcessTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            const int maxIter = 5;
            for (int iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open,
                    FileAccess.Read);
                RawImageFile<ushort> rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new ColorCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.RawMapToRGB(rawimage.Raw);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcess: " + stopwatch.ElapsedMilliseconds/maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
        }

        [TestMethod]
        public void FullProcessP1460461Test()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            const int maxIter = 1;
            for (int iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1460461.RW2", FileMode.Open,
                    FileAccess.Read);
                RawImageFile<ushort> rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new ColorCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.RawMapToRGB(rawimage.Raw);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcess: " + stopwatch.ElapsedMilliseconds/maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
        }


        [TestMethod]
        public void FullProcessWithAutoAdjustTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            const int maxIter = 5;
            for (int iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open,
                    FileAccess.Read);
                RawImageFile<ushort> rawimage = new PanasonicRW2Decoder().Decode(stream);
                var debayer = new AverageBGGRDebayer();

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new ColorCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.AutoAdjust(rawimage.Raw, white);
                pipeline.AutoAdjust(rawimage.Raw, light);

                pipeline.RawMapToRGB(rawimage.Raw);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcessWithAutoAdjust: " + stopwatch.ElapsedMilliseconds/maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
        }
    }
}