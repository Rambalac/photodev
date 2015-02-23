﻿using System.Collections.Generic;
using System.IO;
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

        public ushort[] Black = new ushort[4];
        public float[] CamMul = new float[3];
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