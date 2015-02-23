using System;
using com.azi.image;

namespace com.azi.Debayer
{
    public class AverageBGGRDebayer : IDebayer
    {
        // B G B G
        // G R G R
        // B G B G
        // G R G R
        public ColorMap<ushort> Debayer(RawImageFile file)
        {
            if (file.Width % 2 != 0 || file.Height % 2 != 0) throw new ArgumentException("Width and Height should be even");
            var res = new ColorMap<ushort>(file.Width, file.Height, file.MaxBits + 1);
            var pix = res.GetPixel();

            ProcessTopLine(file, pix, res);

            ProcessMiddleRows(file, res, pix);

            ProcessBottomLine(file, pix, res);

            return res;
        }

        private static void ProcessMiddleRows(RawImageFile file, ColorMap<ushort> res, Color<ushort> pix)
        {
            // Middle Rows
            for (var y = 1; y < res.Height - 1; y++)
            {
                ProcessMiddleOddRows(file, res, pix, y);

                y++;

                ProcessMiddleEvenRows(file, res, pix, y);
            }
        }

        private static void ProcessMiddleEvenRows(RawImageFile file, ColorMap<ushort> res, Color<ushort> pix, int y)
        {
            // Second left pixel
            pix.SetAndMoveNext(
                (ushort)((file.GetValue(1, y - 1) + file.GetValue(1, y + 1))),
                (ushort)(file.GetValue(0, y - 1) + file.GetValue(0, y + 1) + file.GetValue(1, y)),
                (ushort)(file.GetValue(0, y) << 1));

            var lastX = res.Width - 1;
            for (var x = 1; x < lastX; x += 2)
            {
                var xy = file.GetValue(x, y);
                var x1y = file.GetValue(x + 1, y);
                var xy12 = file.GetValue(x, y - 1) + file.GetValue(x, y + 1);

                pix.SetAndMoveNext(
                    (ushort)(xy12),
                    (ushort)(xy << 1),
                    (ushort)((file.GetValue(x - 1, y) + x1y)));

                pix.SetAndMoveNext(
                    (ushort)((xy12 + file.GetValue(x + 2, y - 1) + file.GetValue(x + 2, y + 1)) >> 1),
                    (ushort)((xy + file.GetValue(x + 2, y) + file.GetValue(x + 1, y - 1) + file.GetValue(x + 1, y + 1)) >> 1),
                    (ushort)(x1y << 1));
            }

            // Second right pixel
            pix.SetAndMoveNext(
                (ushort)((file.GetValue(lastX, y - 1) + file.GetValue(lastX, y + 1))),
                (ushort)(file.GetValue(lastX, y) << 1),
                (ushort)(file.GetValue(lastX - 1, y) << 1));
        }

        private static void ProcessMiddleOddRows(RawImageFile file, ColorMap<ushort> res, Color<ushort> pix, int y)
        {
            // First left pixel
            pix.SetAndMoveNext(
                (ushort)(file.GetValue(1, y) << 1),
                (ushort)(file.GetValue(0, y) << 1),
                (ushort)((file.GetValue(0, y - 1) + file.GetValue(0, y + 1))));

            var lastX = res.Width - 1;
            for (var x = 1; x < lastX; x += 2)
            {
                var xy = file.GetValue(x, y);
                var x1y = file.GetValue(x + 1, y);
                var x11y12 = file.GetValue(x + 1, y - 1) + file.GetValue(x + 1, y + 1);

                pix.SetAndMoveNext(
                    (ushort)(xy << 1),
                    (ushort)((file.GetValue(x - 1, y) + x1y + file.GetValue(x, y - 1) + file.GetValue(x, y + 1)) >> 1),
                    (ushort)((file.GetValue(x - 1, y - 1) + x11y12 + file.GetValue(x - 1, y + 1)) >> 1));

                pix.SetAndMoveNext(
                    (ushort)((xy + file.GetValue(x + 2, y))),
                    (ushort)(x1y << 1),
                    (ushort)(x11y12));
            }

            // First right pixel
            pix.SetAndMoveNext(
                (ushort)(file.GetValue(lastX, y) << 1),
                (ushort)((file.GetValue(lastX, y - 1) + file.GetValue(lastX, y + 1) + file.GetValue(lastX - 1, y) << 1) >> 1),
                (ushort)((file.GetValue(lastX - 1, y - 1) + file.GetValue(lastX - 1, y + 1))));
        }

        private static void ProcessTopLine(RawImageFile file, Color<ushort> pix, ColorMap<ushort> res)
        {
            // Top Left pixel

            pix.SetAndMoveNext(
                (ushort)(file.GetValue(1, 1) << 1),
                (ushort)((file.GetValue(1, 0) + file.GetValue(0, 1))),
                (ushort)(file.GetValue(0, 0) << 1));

            // Top row
            for (var x = 1; x < res.Width - 1; x += 2)
            {
                pix.SetAndMoveNext(
                    (ushort)(file.GetValue(x, 1) << 1),
                    (ushort)(file.GetValue(x, 0) << 1),
                    (ushort)((file.GetValue(x - 1, 0) + file.GetValue(x + 1, 0))));

                pix.SetAndMoveNext(
                    (ushort)((file.GetValue(x, 1) + file.GetValue(x + 2, 1))),
                    (ushort)((file.GetValue(x, 0) + file.GetValue(x + 2, 0) + file.GetValue(x + 1, 1))),
                    (ushort)(file.GetValue(x + 1, 0) << 1));
            }

            // Top right pixel
            pix.SetAndMoveNext(
                (ushort)(file.GetValue(res.Width - 1, 1) << 1),
                (ushort)(file.GetValue(res.Width - 1, 0) << 1),
                (ushort)(file.GetValue(res.Width - 2, 0) << 1));
        }

        // B G B G
        // G R G R
        // B G B G
        // G R G R
        private static void ProcessBottomLine(RawImageFile file, Color<ushort> pix, ColorMap<ushort> res)
        {
            // Bottom Left pixel
            var lastY = res.Height - 1;

            pix.SetAndMoveNext(
                (ushort)(file.GetValue(1, lastY) << 1),
                (ushort)(file.GetValue(0, lastY) << 1),
                (ushort)(file.GetValue(0, lastY - 1) << 1));

            // Bottom row
            for (var x = 1; x < res.Width - 1; x += 2)
            {
                pix.SetAndMoveNext(
                    (ushort)(file.GetValue(x, lastY) << 1),
                    (ushort)((file.GetValue(x - 1, lastY) + file.GetValue(x + 1, lastY) + file.GetValue(x, lastY - 1) << 1) >> 1),
                    (ushort)((file.GetValue(x - 1, lastY) + file.GetValue(x + 1, lastY))));

                pix.SetAndMoveNext(
                    (ushort)((file.GetValue(x, lastY) + file.GetValue(x + 2, lastY))),
                    (ushort)(file.GetValue(x + 1, lastY) << 1),
                    (ushort)(file.GetValue(x + 1, lastY - 1) << 1));
            }

            // Bottom right pixel
            pix.SetAndMoveNext(
                (ushort)(file.GetValue(res.Width - 1, lastY) << 1),
                (ushort)((file.GetValue(res.Width - 1, lastY - 1) + file.GetValue(res.Width - 2, 0))),
                (ushort)(file.GetValue(res.Width - 2, lastY - 1) << 1));
        }
    }
}