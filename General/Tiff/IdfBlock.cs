using con.azi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace com.azi.tiff
{
    public class IdfType
    {
        static public IdfType UInt16 = new IdfType { BytesLength = 2 };
        static public IdfType UInt32 = new IdfType { BytesLength = 4 };
        static public IdfType UInt32Fraction = new IdfType { BytesLength = 8 };

        public int BytesLength { get; set; }
    }

    public class IdfBlock
    {
        public ushort rawtag;
        public ushort rawtype;
        public uint length;
        public uint nextOffset;
        public byte[] rawdata;

        public IdfTag? tag;
        public IdfType type;


        public static IdfBlock parse(BinaryReader reader)
        {
            IdfBlock result = new IdfBlock();
            result.rawtag = reader.ReadUInt16();
            IdfTag tag;
            result.tag = (TagMap.TryGetValue(result.rawtag, out tag)) ? (IdfTag?)tag : null;

            result.rawtype = reader.ReadUInt16();
            result.type = IdfTypes[result.rawtype];

            var length = reader.ReadUInt32();
            result.length = length;
            result.nextOffset = ((uint)reader.BaseStream.Position) + 4;
            var datalength = length * result.type.BytesLength;
            if (datalength > 4)
                reader.BaseStream.Seek(reader.ReadUInt32(), SeekOrigin.Begin);

            result.rawdata = reader.ReadBytes((int)datalength);
            if (datalength < 4) reader.ReadBytes(4 - (int)datalength);
            return result;
        }
        public uint moveNext(BinaryReader reader)
        {
            reader.BaseStream.Seek(nextOffset, SeekOrigin.Begin);
            return nextOffset;
        }

        public uint GetUInt32()
        {
            if (type == IdfType.UInt16)
                return BitConverter.ToUInt16(rawdata, 0);
            else if (type == IdfType.UInt32)
                return BitConverter.ToUInt32(rawdata, 0);
            else
                throw new ArgumentException("rawdata", "GetUInt32 failed on wrong type: " + rawtype);
        }

        public ushort GetUInt16()
        {
            uint res = GetUInt32();
            if (res > ushort.MaxValue) throw new ArgumentException("rawdata", "GetUInt16 failed on value bigger than UInt16: " + res);
            return (ushort)res;
        }

        private static Dictionary<int, IdfTag> TagMap = new Dictionary<int, IdfTag>()
        {
            { 254, IdfTag.NewSubfileType },
            { 255, IdfTag.SubfileType },
            { 2, IdfTag.ImageWidth },
            { 256, IdfTag.ImageWidth },
            { 257, IdfTag.ImageLength },
            { 3, IdfTag.ImageLength },
            { 258, IdfTag.BitsPerSample },
            { 259, IdfTag.Compression },
            { 262, IdfTag.PhotometricInterpretation },
            { 263, IdfTag.Threshholding },
            { 264, IdfTag.CellWidth },
            { 265, IdfTag.CellLength },
            { 266, IdfTag.FillOrder },
            { 269, IdfTag.DocumentName },
            { 270, IdfTag.ImageDescription },
            { 271, IdfTag.Make },
            { 272, IdfTag.Model },
            { 273, IdfTag.StripOffsets },
            { 274, IdfTag.Orientation },
            { 277, IdfTag.SamplesPerPixel },
            { 278, IdfTag.RowsPerStrip },
            { 279, IdfTag.StripByteCounts },
            { 280, IdfTag.MinSampleValue },
            { 281, IdfTag.MaxSampleValue },
            { 282, IdfTag.XResolution },
            { 283, IdfTag.YResolution },
            { 284, IdfTag.PlanarConfiguration },
            { 285, IdfTag.PageName },
            { 286, IdfTag.XPosition },
            { 287, IdfTag.YPosition },
            { 288, IdfTag.FreeOffsets },
            { 289, IdfTag.FreeByteCounts },
            { 290, IdfTag.GrayResponseUnit },
            { 291, IdfTag.GrayResponseCurve },
            { 292, IdfTag.T4Options },
            { 293, IdfTag.T6Options },
            { 296, IdfTag.ResolutionUnit },
            { 297, IdfTag.PageNumber },
            { 301, IdfTag.TransferFunction },
            { 305, IdfTag.Software },
            { 306, IdfTag.DateTime },
            { 315, IdfTag.Artist },
            { 316, IdfTag.HostComputer },
            { 317, IdfTag.Predictor },
            { 318, IdfTag.WhitePoint },
            { 319, IdfTag.PrimaryChromaticities },
            { 320, IdfTag.ColorMap },
            { 321, IdfTag.HalftoneHints },
            { 322, IdfTag.TileWidth },
            { 323, IdfTag.TileLength },
            { 324, IdfTag.TileOffsets },
            { 325, IdfTag.TileByteCounts },
            { 326, IdfTag.BadFaxLines },
            { 327, IdfTag.CleanFaxData },
            { 328, IdfTag.ConsecutiveBadFaxLines },
            { 330, IdfTag.SubIFDs },
            { 332, IdfTag.InkSet },
            { 333, IdfTag.InkNames },
            { 334, IdfTag.NumberOfInks },
            { 336, IdfTag.DotRange },
            { 337, IdfTag.TargetPrinter },
            { 338, IdfTag.ExtraSamples },
            { 339, IdfTag.SampleFormat },
            { 340, IdfTag.SMinSampleValue },
            { 341, IdfTag.SMaxSampleValue },
            { 342, IdfTag.TransferRange },
            { 343, IdfTag.ClipPath },
            { 344, IdfTag.XClipPathUnits },
            { 345, IdfTag.YClipPathUnits },
            { 346, IdfTag.Indexed },
            { 347, IdfTag.JPEGTables },
            { 351, IdfTag.OPIProxy },
            { 400, IdfTag.GlobalParametersIFD },
            { 401, IdfTag.ProfileType },
            { 402, IdfTag.FaxProfile },
            { 403, IdfTag.CodingMethods },
            { 404, IdfTag.VersionYear },
            { 405, IdfTag.ModeNumber },
            { 433, IdfTag.Decode },
            { 434, IdfTag.DefaultImageColor },
            { 512, IdfTag.JPEGProc },
            { 513, IdfTag.JPEGInterchangeFormat },
            { 514, IdfTag.JPEGInterchangeFormatLength },
            { 515, IdfTag.JPEGRestartInterval },
            { 517, IdfTag.JPEGLosslessPredictors },
            { 518, IdfTag.JPEGPointTransforms },
            { 519, IdfTag.JPEGQTables },
            { 520, IdfTag.JPEGDCTables },
            { 521, IdfTag.JPEGACTables },
            { 529, IdfTag.YCbCrCoefficients },
            { 530, IdfTag.YCbCrSubSampling },
            { 531, IdfTag.YCbCrPositioning },
            { 532, IdfTag.ReferenceBlackWhite },
            { 559, IdfTag.StripRowCounts },
            { 700, IdfTag.XMP },
            { 18246, IdfTag.ImageRating },
            { 18249, IdfTag.ImageRatingPercent },
            { 32781, IdfTag.ImageID },
            { 32932, IdfTag.WangAnnotation },
            { 33421, IdfTag.CFARepeatPatternDim },
            { 33422, IdfTag.CFAPattern },
            { 33423, IdfTag.BatteryLevel },
            { 33432, IdfTag.Copyright },
            { 33434, IdfTag.ExposureTime },
            { 33437, IdfTag.FNumber },
            { 33445, IdfTag.MDFileTag },
            { 33446, IdfTag.MDScalePixel },
            { 33447, IdfTag.MDColorTable },
            { 33448, IdfTag.MDLabName },
            { 33449, IdfTag.MDSampleInfo },
            { 33450, IdfTag.MDPrepDate },
            { 33451, IdfTag.MDPrepTime },
            { 33452, IdfTag.MDFileUnits },
            { 33550, IdfTag.ModelPixelScaleTag },
            { 33723, IdfTag.IPTCNAA },
            { 33918, IdfTag.INGRPacketDataTag },
            { 33919, IdfTag.INGRFlagRegisters },
            { 33920, IdfTag.IrasBTransformationMatrix },
            { 33922, IdfTag.ModelTiepointTag },
            { 34016, IdfTag.Site },
            { 34017, IdfTag.ColorSequence },
            { 34018, IdfTag.IT8Header },
            { 34019, IdfTag.RasterPadding },
            { 34020, IdfTag.BitsPerRunLength },
            { 34021, IdfTag.BitsPerExtendedRunLength },
            { 34022, IdfTag.ColorTable },
            { 34023, IdfTag.ImageColorIndicator },
            { 34024, IdfTag.BackgroundColorIndicator },
            { 34025, IdfTag.ImageColorValue },
            { 34026, IdfTag.BackgroundColorValue },
            { 34027, IdfTag.PixelIntensityRange },
            { 34028, IdfTag.TransparencyIndicator },
            { 34029, IdfTag.ColorCharacterization },
            { 34030, IdfTag.HCUsage },
            { 34031, IdfTag.TrapIndicator },
            { 34032, IdfTag.CMYKEquivalent },
            { 34033, IdfTag.Reserved },
            { 34034, IdfTag.Reserved },
            { 34035, IdfTag.Reserved },
            { 34264, IdfTag.ModelTransformationTag },
            { 34377, IdfTag.Photoshop },
            { 34665, IdfTag.ExifIFD },
            { 34675, IdfTag.InterColorProfile },
            { 34732, IdfTag.ImageLayer },
            { 34735, IdfTag.GeoKeyDirectoryTag },
            { 34736, IdfTag.GeoDoubleParamsTag },
            { 34737, IdfTag.GeoAsciiParamsTag },
            { 34850, IdfTag.ExposureProgram },
            { 34852, IdfTag.SpectralSensitivity },
            { 34853, IdfTag.GPSInfo },
            { 34855, IdfTag.ISOSpeedRatings },
            { 34856, IdfTag.OECF },
            { 34857, IdfTag.Interlace },
            { 34858, IdfTag.TimeZoneOffset },
            { 34859, IdfTag.SelfTimeMode },
            { 34864, IdfTag.SensitivityType },
            { 34865, IdfTag.StandardOutputSensitivity },
            { 34866, IdfTag.RecommendedExposureIndex },
            { 34867, IdfTag.ISOSpeed },
            { 34868, IdfTag.ISOSpeedLatitudeyyy },
            { 34869, IdfTag.ISOSpeedLatitudezzz },
            { 34908, IdfTag.HylaFAXFaxRecvParams },
            { 34909, IdfTag.HylaFAXFaxSubAddress },
            { 34910, IdfTag.HylaFAXFaxRecvTime },
            { 36864, IdfTag.ExifVersion },
            { 36867, IdfTag.DateTimeOriginal },
            { 36868, IdfTag.DateTimeDigitized },
            { 37121, IdfTag.ComponentsConfiguration },
            { 37122, IdfTag.CompressedBitsPerPixel },
            { 37377, IdfTag.ShutterSpeedValue },
            { 37378, IdfTag.ApertureValue },
            { 37379, IdfTag.BrightnessValue },
            { 37380, IdfTag.ExposureBiasValue },
            { 37381, IdfTag.MaxApertureValue },
            { 37382, IdfTag.SubjectDistance },
            { 37383, IdfTag.MeteringMode },
            { 37384, IdfTag.LightSource },
            { 37385, IdfTag.Flash },
            { 37386, IdfTag.FocalLength },
            { 37387, IdfTag.FlashEnergy },
            { 37388, IdfTag.SpatialFrequencyResponse },
            { 37389, IdfTag.Noise },
            { 37390, IdfTag.FocalPlaneXResolution },
            { 37391, IdfTag.FocalPlaneYResolution },
            { 37392, IdfTag.FocalPlaneResolutionUnit },
            { 37393, IdfTag.ImageNumber },
            { 37394, IdfTag.SecurityClassification },
            { 37395, IdfTag.ImageHistory },
            { 37396, IdfTag.SubjectLocation },
            { 37397, IdfTag.ExposureIndex },
            { 37398, IdfTag.TIFFEPStandardID },
            { 37399, IdfTag.SensingMethod },
            { 37500, IdfTag.MakerNote },
            { 37510, IdfTag.UserComment },
            { 37520, IdfTag.SubsecTime },
            { 37521, IdfTag.SubsecTimeOriginal },
            { 37522, IdfTag.SubsecTimeDigitized },
            { 37724, IdfTag.ImageSourceData },
            { 40091, IdfTag.XPTitle },
            { 40092, IdfTag.XPComment },
            { 40093, IdfTag.XPAuthor },
            { 40094, IdfTag.XPKeywords },
            { 40095, IdfTag.XPSubject },
            { 40960, IdfTag.FlashpixVersion },
            { 40961, IdfTag.ColorSpace },
            { 40962, IdfTag.PixelXDimension },
            { 40963, IdfTag.PixelYDimension },
            { 40964, IdfTag.RelatedSoundFile },
            { 40965, IdfTag.InteroperabilityIFD },
            { 41483, IdfTag.FlashEnergy },
            { 41484, IdfTag.SpatialFrequencyResponse },
            { 41486, IdfTag.FocalPlaneXResolution },
            { 41487, IdfTag.FocalPlaneYResolution },
            { 41488, IdfTag.FocalPlaneResolutionUnit },
            { 41492, IdfTag.SubjectLocation },
            { 41493, IdfTag.ExposureIndex },
            { 41495, IdfTag.SensingMethod },
            { 41728, IdfTag.FileSource },
            { 41729, IdfTag.SceneType },
            { 41730, IdfTag.CFAPattern },
            { 41985, IdfTag.CustomRendered },
            { 41986, IdfTag.ExposureMode },
            { 41987, IdfTag.WhiteBalance },
            { 41988, IdfTag.DigitalZoomRatio },
            { 41989, IdfTag.FocalLengthIn35mmFilm },
            { 41990, IdfTag.SceneCaptureType },
            { 41991, IdfTag.GainControl },
            { 41992, IdfTag.Contrast },
            { 41993, IdfTag.Saturation },
            { 41994, IdfTag.Sharpness },
            { 41995, IdfTag.DeviceSettingDescription },
            { 41996, IdfTag.SubjectDistanceRange },
            { 42016, IdfTag.ImageUniqueID },
            { 42032, IdfTag.CameraOwnerName },
            { 42033, IdfTag.BodySerialNumber },
            { 42034, IdfTag.LensSpecification },
            { 42035, IdfTag.LensMake },
            { 42036, IdfTag.LensModel },
            { 42037, IdfTag.LensSerialNumber },
            { 42112, IdfTag.GDAL_METADATA },
            { 42113, IdfTag.GDAL_NODATA },
            { 48129, IdfTag.PixelFormat },
            { 48130, IdfTag.Transformation },
            { 48131, IdfTag.Uncompressed },
            { 48256, IdfTag.ImageWidth },
            { 48257, IdfTag.ImageHeight },
            { 48258, IdfTag.WidthResolution },
            { 48259, IdfTag.HeightResolution },
            { 48320, IdfTag.ImageOffset },
            { 48321, IdfTag.ImageByteCount },
            { 48322, IdfTag.AlphaOffset },
            { 48323, IdfTag.AlphaByteCount },
            { 48324, IdfTag.ImageDataDiscard },
            { 48325, IdfTag.AlphaDataDiscard },
            { 48132, IdfTag.ImageType },
            { 50215, IdfTag.OceScanjobDescription },
            { 50216, IdfTag.OceApplicationSelector },
            { 50217, IdfTag.OceIdentificationNumber },
            { 50218, IdfTag.OceImageLogicCharacteristics },
            { 50341, IdfTag.PrintImageMatching },
            { 50706, IdfTag.DNGVersion },
            { 50707, IdfTag.DNGBackwardVersion },
            { 50708, IdfTag.UniqueCameraModel },
            { 50709, IdfTag.LocalizedCameraModel },
            { 50710, IdfTag.CFAPlaneColor },
            { 50711, IdfTag.CFALayout },
            { 50712, IdfTag.LinearizationTable },
            { 50713, IdfTag.BlackLevelRepeatDim },
            { 50714, IdfTag.BlackLevel },
            { 50715, IdfTag.BlackLevelDeltaH },
            { 50716, IdfTag.BlackLevelDeltaV },
            { 50717, IdfTag.WhiteLevel },
            { 50718, IdfTag.DefaultScale },
            { 50719, IdfTag.DefaultCropOrigin },
            { 50720, IdfTag.DefaultCropSize },
            { 50721, IdfTag.ColorMatrix1 },
            { 50722, IdfTag.ColorMatrix2 },
            { 50723, IdfTag.CameraCalibration1 },
            { 50724, IdfTag.CameraCalibration2 },
            { 50725, IdfTag.ReductionMatrix1 },
            { 50726, IdfTag.ReductionMatrix2 },
            { 50727, IdfTag.AnalogBalance },
            { 50728, IdfTag.AsShotNeutral },
            { 50729, IdfTag.AsShotWhiteXY },
            { 50730, IdfTag.BaselineExposure },
            { 50731, IdfTag.BaselineNoise },
            { 50732, IdfTag.BaselineSharpness },
            { 50733, IdfTag.BayerGreenSplit },
            { 50734, IdfTag.LinearResponseLimit },
            { 50735, IdfTag.CameraSerialNumber },
            { 50736, IdfTag.LensInfo },
            { 50737, IdfTag.ChromaBlurRadius },
            { 50738, IdfTag.AntiAliasStrength },
            { 50739, IdfTag.ShadowScale },
            { 50740, IdfTag.DNGPrivateData },
            { 50741, IdfTag.MakerNoteSafety },
            { 50778, IdfTag.CalibrationIlluminant1 },
            { 50779, IdfTag.CalibrationIlluminant2 },
            { 50780, IdfTag.BestQualityScale },
            { 50781, IdfTag.RawDataUniqueID },
            { 50784, IdfTag.AliasLayerMetadata },
            { 50827, IdfTag.OriginalRawFileName },
            { 50828, IdfTag.OriginalRawFileData },
            { 50829, IdfTag.ActiveArea },
            { 50830, IdfTag.MaskedAreas },
            { 50831, IdfTag.AsShotICCProfile },
            { 50832, IdfTag.AsShotPreProfileMatrix },
            { 50833, IdfTag.CurrentICCProfile },
            { 50834, IdfTag.CurrentPreProfileMatrix },
            { 50879, IdfTag.ColorimetricReference },
            { 50931, IdfTag.CameraCalibrationSignature },
            { 50932, IdfTag.ProfileCalibrationSignature },
            { 50933, IdfTag.ExtraCameraProfiles },
            { 50934, IdfTag.AsShotProfileName },
            { 50935, IdfTag.NoiseReductionApplied },
            { 50936, IdfTag.ProfileName },
            { 50937, IdfTag.ProfileHueSatMapDims },
            { 50938, IdfTag.ProfileHueSatMapData1 },
            { 50939, IdfTag.ProfileHueSatMapData2 },
            { 50940, IdfTag.ProfileToneCurve },
            { 50941, IdfTag.ProfileEmbedPolicy },
            { 50942, IdfTag.ProfileCopyright },
            { 50964, IdfTag.ForwardMatrix1 },
            { 50965, IdfTag.ForwardMatrix2 },
            { 50966, IdfTag.PreviewApplicationName },
            { 50967, IdfTag.PreviewApplicationVersion },
            { 50968, IdfTag.PreviewSettingsName },
            { 50969, IdfTag.PreviewSettingsDigest },
            { 50970, IdfTag.PreviewColorSpace },
            { 50971, IdfTag.PreviewDateTime },
            { 50972, IdfTag.RawImageDigest },
            { 50973, IdfTag.OriginalRawFileDigest },
            { 50974, IdfTag.SubTileBlockSize },
            { 50975, IdfTag.RowInterleaveFactor },
            { 50981, IdfTag.ProfileLookTableDims },
            { 50982, IdfTag.ProfileLookTableData },
            { 51008, IdfTag.OpcodeList1 },
            { 51009, IdfTag.OpcodeList2 },
            { 51022, IdfTag.OpcodeList3 },
            { 51041, IdfTag.NoiseProfile },
            { 51089, IdfTag.OriginalDefaultFinalSize },
            { 51090, IdfTag.OriginalBestQualityFinalSize },
            { 51091, IdfTag.OriginalDefaultCropSize },
            { 51107, IdfTag.ProfileHueSatMapEncoding },
            { 51108, IdfTag.ProfileLookTableEncoding },
            { 51109, IdfTag.BaselineExposureOffset },
            { 51110, IdfTag.DefaultBlackRender },
            { 51111, IdfTag.NewRawImageDigest },
            { 51112, IdfTag.RawToPreviewGain },
            { 51125, IdfTag.DefaultUserCrop }
        };

        static Dictionary<int, IdfType> IdfTypes = new Dictionary<int, IdfType>()
        {
            // 11124811248484
            {1, new IdfType{
                BytesLength=1   
            }},
            {2, new IdfType{
                BytesLength=1   
            }},
            {3, IdfType.UInt16 },
            {4, IdfType.UInt32 },
            {5, IdfType.UInt32Fraction },
            {6, new IdfType{
                BytesLength=1   
            }},
            {7, IdfType.UInt16},
            {10, IdfType.UInt32Fraction },
        };

        public string GetString()
        {
            return Encoding.UTF8.GetString(rawdata, 0, rawdata.Length).Trim(new[] { ' ', '\t', '\0' });
        }

        public Fraction GetFraction()
        {
            if (type == IdfType.UInt32Fraction)
                return new con.azi.Fraction((int)BitConverter.ToUInt32(rawdata, 0), (int)BitConverter.ToUInt32(rawdata, 4));
            else
                throw new ArgumentException("rawdata", "readFraction failed on wrong type: " + rawtype);
        }
    }
}
