using con.azi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.azi.tiff
{
    public class Exif
    {
        List<IdfBlock> idfBlocks = new List<IdfBlock>();

        public int ImageWidth;
        public int ImageHeight;
        public string Maker;
        public string Model;
        public int StripOffset;
        public int Orientation;
        public int StripByteCounts;
        public Fraction Shutter;
        public Fraction Aperture;
        public int ExposureProgram;
        public string ExifVersion;
        public string DateTimeOriginal;
        public string DateTimeDigitized;
        public Fraction ExposureBiasValue;
        public Fraction MaxApertureValue;
        public int MeteringMode;
        public int Flash;
        public Fraction FocalLength;
        public string SubsecTimeOriginal;
        public string SubsecTimeDigitized;
        public int FileSource;

        public static Exif parse(Stream stream)
        {
            return new Exif(stream);
        }

        protected Exif(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            var order = reader.ReadUInt16();
            if (order != 0x4949 && order != 0x4d4d) throw new ArgumentException("Wrong file");
            reader.ReadUInt16();

            while (reader.PeekChar() != -1)
            {
                var offset = reader.ReadUInt32();
                if (offset == 0) return;
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                parseIfd(reader);
            }

        }

        protected virtual void parseIdfBlock(IdfBlock block, BinaryReader reader)
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
                    reader.BaseStream.Seek((long)block.GetUInt32(), SeekOrigin.Begin);
                    parseIfd(reader);
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
        private void parseIfd(BinaryReader reader)
        {
            ushort blocksnumber = reader.ReadUInt16();
            if (blocksnumber > 512) throw new ArgumentException("Too many items in ifd");
            while (blocksnumber-- > 0)
            {
                IdfBlock block = IdfBlock.parse(reader);
                idfBlocks.Add(block);
                parseIdfBlock(block, reader);
                block.moveNext(reader);
            }
        }

    }
}
