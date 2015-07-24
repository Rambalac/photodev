using System;
using System.Threading.Tasks;
using com.azi.Image;

namespace com.azi.Filters.RawToVectorMap.Demosaic
{
    public class AverageBGGRDebayer : IBGGRDebayer<ushort>
    {
        // B G B G
        // G R G R
        // B G B G
        // G R G R
        public VectorMap Process(RawBGGRMap<ushort> file)
        {
            if (file.Width % 2 != 0 || file.Height % 2 != 0) throw new ArgumentException("Width and Height should be even");
            var res = new VectorMap(file.Width, file.Height);

            ProcessTopLine(file, res);

            ProcessMiddleRows(file, res);

            ProcessBottomLine(file, res);

            return res;
        }

        private static void ProcessMiddleRows(RawBGGRMap<ushort> file, VectorMap res)
        {
            // Middle Rows
            Parallel.For(0, (res.Height - 2) / 2, yy =>
              {
                  var y = yy * 2 + 1;
                  ProcessMiddleOddRows(file.GetRow(y), res.Width, res.GetRow(y));

                  y++;

                  ProcessMiddleEvenRows(file.GetRow(y), res.Width, res.GetRow(y));
              });
        }

        private static void ProcessMiddleEvenRows(RawPixel<ushort> raw, int Width, VectorPixel pix)
        {
            float maxValue = raw.MaxValue;
            // Second left pixel
            pix.SetAndMoveNext(
                ((raw.GetRel(1, -1) + raw.GetRel(1, +1)) / maxValue / 2.0f),
                ((raw.GetRel(0, -1) + raw.GetRel(0, +1) + (raw.GetRel(1, 0) << 1)) / maxValue / 4.0f),
               raw.Value / maxValue);
            raw.MoveNext();

            var lastX = Width - 1;
            for (var x = 1; x < lastX; x += 2)
            {
                var xy = raw.Value;
                var x1y = raw.GetRel(+1, 0);
                var xy12 = raw.GetRel(0, -1) + raw.GetRel(0, +1);

                pix.SetAndMoveNext(
                    (xy12) / maxValue / 2.0f,
                    (xy) / maxValue,
                    ((raw.GetRel(-1, 0) + x1y)) / maxValue / 2.0f);

                pix.SetAndMoveNext(
                    ((xy12 + raw.GetRel(+2, -1) + raw.GetRel(+2, +1)) / maxValue / 4),
                    ((xy + raw.GetRel(+2, 0) + raw.GetRel(+1, -1) + raw.GetRel(+1, +1)) / maxValue / 4),
                    (x1y) / maxValue);
                raw.MoveNext();
                raw.MoveNext();
            }

            // Second right pixel
            pix.SetAndMoveNext(
                ((raw.GetRel(0, -1) + raw.GetRel(0, +1)) / maxValue / 2),
                (raw.Value) / maxValue,
                (raw.GetRel(-1, 0) / maxValue));
            raw.MoveNext();
        }

        private static void ProcessMiddleOddRows(RawPixel<ushort> raw, int Width, VectorPixel pix)
        {
            float maxValue = raw.MaxValue;
            // First left pixel
            pix.SetAndMoveNext(
                (raw.GetRel(1, 0) / maxValue),
                (raw.Value / maxValue),
                ((raw.GetRel(0, -1) + raw.GetRel(0, +1)) / maxValue / 2));
            raw.MoveNext();

            var lastX = Width - 1;
            for (var x = 1; x < lastX; x += 2)
            {
                var xy = raw.Value;
                var x1y = raw.GetRel(+1, 0);
                var x11y12 = raw.GetRel(+1, -1) + raw.GetRel(+1, +1);

                pix.SetAndMoveNext(
                    (xy / maxValue),
                    ((raw.GetRel(-1, 0) + x1y + raw.GetRel(0, -1) + raw.GetRel(0, +1)) / maxValue / 4),
                    ((raw.GetRel(-1, -1) + x11y12 + raw.GetRel(-1, +1)) / maxValue / 4));

                pix.SetAndMoveNext(
                    ((xy + raw.GetRel(+2, 0)) / maxValue / 2),
                    (x1y / maxValue),
                    (x11y12) / maxValue / 2);
                raw.MoveNext();
                raw.MoveNext();
            }

            // First right pixel
            pix.SetAndMoveNext(
                (raw.Value / maxValue),
                ((raw.GetRel(0, -1) + raw.GetRel(0, +1) + (raw.GetRel(-1, 0) << 1)) / maxValue/4),
                ((raw.GetRel(-1, -1) + raw.GetRel(-1, +1)) / maxValue/2));
            raw.MoveNext();
        }

        private static void ProcessTopLine(RawBGGRMap<ushort> map, VectorMap res)
        {
            float maxValue = map.MaxValue;
            var pix = res.GetPixel();
            var raw = map.GetRow(0);
            // Top Left pixel

            pix.SetAndMoveNext(
                (raw.GetRel(1, 1) / maxValue),
                (raw.GetRel(1, 0) + raw.GetRel(0, 1)),
                raw.Value << 1);
            raw.MoveNext();

            // Top row
            for (var x = 1; x < res.Width - 1; x += 2)
            {
                pix.SetAndMoveNext(
                    raw.GetRel(0, 1) << 1,
                    raw.Value << 1,
                    (raw.GetRel(-1, 0) + raw.GetRel(+1, 0)));

                pix.SetAndMoveNext(
                    (raw.GetRel(0, 1) + raw.GetRel(+2, 1)),
                    (raw.Value + raw.GetRel(+2, 0) + (raw.GetRel(+1, 1) << 1)) >> 1,
                    raw.GetRel(+1, 0) << 1);
                raw.MoveNext();
                raw.MoveNext();
            }

            // Top right pixel
            pix.SetAndMoveNext(
                (ushort)(raw.GetRel(res.Width - 1, 1) << 1),
                (ushort)(raw.GetRel(res.Width - 1, 0) << 1),
                (ushort)(raw.GetRel(res.Width - 2, 0) << 1));
            raw.MoveNext();
        }

        // B G B G
        // G R G R
        // B G B G
        // G R G R
        private static void ProcessBottomLine(RawBGGRMap<ushort> map, VectorMap res)
        {
            var pix = res.GetPixel();
            var raw = map.GetRow(res.Height - 1);

            // Bottom Left pixel
            var lastY = res.Height - 1;

            pix.SetAndMoveNext(
                raw.GetRel(1, 0) << 1,
                raw.Value << 1,
                raw.GetRel(0, -1) << 1);
            raw.MoveNext();

            // Bottom row
            for (var x = 1; x < res.Width - 1; x += 2)
            {
                pix.SetAndMoveNext(
                    raw.Value << 1,
                    (raw.GetRel(-1, 0) + raw.GetRel(+1, 0) + raw.GetRel(0, -1) << 1) >> 1,
                    (raw.GetRel(-1, 0) + raw.GetRel(+1, 0)));

                pix.SetAndMoveNext(
                    (raw.Value + raw.GetRel(+2, 0)),
                    raw.GetRel(+1, 0) << 1,
                    raw.GetRel(+1, 0 - 1) << 1);

                raw.MoveNext();
                raw.MoveNext();
            }

            // Bottom right pixel
            pix.SetAndMoveNext(
                raw.GetRel(-1, 0) << 1,
                (raw.GetRel(-1, -1) + raw.GetRel(-2, 0)),
                raw.GetRel(-2, -1) << 1);
            raw.MoveNext();
        }
    }
}