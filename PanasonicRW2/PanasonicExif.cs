using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using com.azi.tiff;

namespace com.azi.Decoder.Panasonic
{
    public enum PanasoncIdfTag
    {
        CropLeft,
        CropWidth,
        CropTop,
        CropHeight,
        Filters,
        Iso,
        Black,
        CamMul,
        Thumb,
        RawOffset,
    }

    public class PanasonicExif : Exif
    {
        private static readonly Dictionary<int, PanasoncIdfTag> TagMap = new Dictionary<int, PanasoncIdfTag>
        {
            {5, PanasoncIdfTag.CropLeft},
            {6, PanasoncIdfTag.CropTop},
            {7, PanasoncIdfTag.CropWidth},
            {8, PanasoncIdfTag.CropHeight},
            {9, PanasoncIdfTag.Filters},
            {23, PanasoncIdfTag.Iso},
            {28, PanasoncIdfTag.Black},
            {29, PanasoncIdfTag.Black},
            {30, PanasoncIdfTag.Black},
            {36, PanasoncIdfTag.CamMul},
            {37, PanasoncIdfTag.CamMul},
            {38, PanasoncIdfTag.CamMul},
            {46, PanasoncIdfTag.Thumb},
            {280, PanasoncIdfTag.RawOffset},
        };

        public const int MaxBits = 12;

        public ushort[] Black = new ushort[4];
        public float[] CamMul;
        public int CropBottom;

        public int CropLeft;
        public int CropRight;
        public int CropTop;
        public int Filters;
        public int Iso;
        public int RawOffset;
        public byte[] Thumb;

        public new static PanasonicExif Parse(Stream stream)
        {
            var result = new PanasonicExif();
            result.InternalParse(stream);

            result.ColorMatrix = new[,] {
                {1.87f,-0.81f, -0.06f},
                {-0.16f,1.55f,-0.39f},
                {0.05f,-0.47f,1.42f}
            };

            if (result.CamMul == null) return result;

            var max = result.CamMul.Max();
            result.WhiteColor = result.CamMul.Select(v => (ushort)((1 << MaxBits) * max / v)).ToArray();
            result.Multiplier = max;

            return result;
        }

        protected override void ParseIdfBlock(IdfBlock block, BinaryReader reader)
        {
            PanasoncIdfTag tag;
            if (!TagMap.TryGetValue(block.rawtag, out tag))
            {
                base.ParseIdfBlock(block, reader);
                return;
            }

            switch (tag)
            {
                case PanasoncIdfTag.CropLeft:
                    CropLeft = (int)block.GetUInt32();
                    CropRight += CropLeft;
                    break;
                case PanasoncIdfTag.CropTop:
                    CropTop = (int)block.GetUInt32();
                    CropBottom += CropTop;
                    break;
                case PanasoncIdfTag.CropWidth:
                    CropRight += (int)block.GetUInt32();
                    break;
                case PanasoncIdfTag.CropHeight:
                    CropBottom += (int)block.GetUInt32();
                    break;
                case PanasoncIdfTag.Filters:
                    Filters = (int)block.GetUInt32();
                    break;
                case PanasoncIdfTag.Iso:
                    Iso = (int)block.GetUInt32();
                    break;
                case PanasoncIdfTag.Black:
                    Black[block.rawtag - 28] = block.GetUInt16();
                    Black[3] = Black[1];
                    break;
                case PanasoncIdfTag.CamMul:
                    if (CamMul == null) CamMul = new float[3];
                    CamMul[block.rawtag - 36] = block.GetUInt16();
                    break;
                case PanasoncIdfTag.Thumb:
                    Thumb = block.rawdata;
                    break;
                case PanasoncIdfTag.RawOffset:
                    RawOffset = (int)block.GetUInt32();
                    break;
            }
        }
    }
}