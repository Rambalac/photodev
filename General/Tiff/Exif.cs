using System;
using System.Collections.Generic;
using System.IO;

namespace com.azi.tiff
{
    public class Exif
    {
        private readonly List<IdfBlock> _idfBlocks = new List<IdfBlock>();

        public Fraction Aperture;
        public string DateTimeDigitized;
        public string DateTimeOriginal;
        public string ExifVersion;
        public Fraction ExposureBiasValue;
        public int ExposureProgram;
        public int FileSource;
        public int Flash;
        public Fraction FocalLength;
        public int ImageHeight;
        public int ImageWidth;
        public string Maker;
        public Fraction MaxApertureValue;
        public int MeteringMode;
        public string Model;
        public int Orientation;
        public Fraction Shutter;
        public int StripByteCounts;
        public int StripOffset;
        public string SubsecTimeDigitized;
        public string SubsecTimeOriginal;
        public float[] WhiteMultiplier;

        protected void InternalParse(Stream stream)
        {
            var reader = new BinaryReader(stream);
            var order = reader.ReadUInt16();
            if (order != 0x4949 && order != 0x4d4d) throw new ArgumentException("Wrong file");
            reader.ReadUInt16();

            while (reader.PeekChar() != -1)
            {
                var offset = reader.ReadUInt32();
                if (offset == 0) return;
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                ParseIfd(reader);
            }
        }

        public static Exif Parse(Stream stream)
        {
            var result = new Exif();
            result.InternalParse(stream);
            return result;
        }

        protected virtual void ParseIdfBlock(IdfBlock block, BinaryReader reader)
        {
            switch (block.tag)
            {
                case IdfTag.ImageWidth:
                    ImageWidth = block.GetUInt16();
                    break;
                case IdfTag.ImageLength:
                    ImageHeight = block.GetUInt16();
                    break;
                case IdfTag.Make:
                    Maker = block.GetString();
                    break;
                case IdfTag.Model:
                    Model = block.GetString();
                    break;
                case IdfTag.StripOffsets:
                    StripOffset = (int)block.GetUInt32();
                    break;
                case IdfTag.Orientation:
                    Orientation = (int)block.GetUInt32();
                    break;
                case IdfTag.RowsPerStrip:
                    StripByteCounts = (int)block.GetUInt32();
                    break;
                case IdfTag.ExifIFD:
                    reader.BaseStream.Seek(block.GetUInt32(), SeekOrigin.Begin);
                    ParseIfd(reader);
                    break;
                case IdfTag.ExposureTime:
                    Shutter = block.GetFraction();
                    break;
                case IdfTag.FNumber:
                    Aperture = block.GetFraction();
                    break;
                case IdfTag.ExposureProgram:
                    ExposureProgram = (int)block.GetUInt32();
                    break;
                case IdfTag.ExifVersion:
                    ExifVersion = block.GetString();
                    break;
                case IdfTag.DateTimeOriginal:
                    DateTimeOriginal = block.GetString();
                    break;
                case IdfTag.DateTimeDigitized:
                    DateTimeDigitized = block.GetString();
                    break;
                case IdfTag.ExposureBiasValue:
                    ExposureBiasValue = block.GetFraction();
                    break;
                case IdfTag.MaxApertureValue:
                    MaxApertureValue = block.GetFraction();
                    break;
                case IdfTag.MeteringMode:
                    MeteringMode = (int)block.GetUInt32();
                    break;
                case IdfTag.Flash:
                    Flash = (int)block.GetUInt32();
                    break;
                case IdfTag.FocalLength:
                    FocalLength = block.GetFraction();
                    break;
                case IdfTag.SubsecTimeOriginal:
                    SubsecTimeOriginal = block.GetString();
                    break;
                case IdfTag.SubsecTimeDigitized:
                    SubsecTimeDigitized = block.GetString();
                    break;
                case IdfTag.FileSource:
                    FileSource = (int)block.GetUInt32();
                    break;
                default: //skip 
                    break;
            }
        }

        private void ParseIfd(BinaryReader reader)
        {
            var blocksnumber = reader.ReadUInt16();
            if (blocksnumber > 512) throw new ArgumentException("Too many items in ifd");
            while (blocksnumber-- > 0)
            {
                var block = IdfBlock.parse(reader);
                _idfBlocks.Add(block);
                ParseIdfBlock(block, reader);
                block.moveNext(reader);
            }
        }
    }
}