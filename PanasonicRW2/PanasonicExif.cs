using com.azi.tiff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace con.azi.decoder.panasonic.rw2
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
        public int CropLeft;
        public int CropRight;
        public int CropTop;
        public int CropBottom;
        public int Filters;
        public int Iso;
        public ushort[] Black = new ushort[4];
        public float[] CamMul = new float[3];
        public byte[] Thumb;
        public int RawOffset;

        public PanasonicExif(Stream stream)
            : base(stream)
        {
        }

        new public static PanasonicExif parse(Stream stream)
        {
            return new PanasonicExif(stream);
        }

        override protected void parseIdfBlock(IdfBlock block, BinaryReader reader)
        {
            PanasoncIdfTag tag;
            if (!TagMap.TryGetValue(block.rawtag, out tag))
            {
                base.parseIdfBlock(block, reader);
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

        static Dictionary<int, PanasoncIdfTag> TagMap = new Dictionary<int, PanasoncIdfTag>()
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
    }
}
