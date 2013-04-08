using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Text;
using HAC.Models.POCO;


//comments: some functions have been modificated based on this code:
//http://mikejorgensen.com/weblog/attachments/exifdata.cs.txt
public class ExifReader
{
    //private System.ComponentModel.Container components = null;
    DateTimeFormatInfo m_dateTimeFormat = new DateTimeFormatInfo();
    private ASCIIEncoding ascii;
    private System.Drawing.Image image;
    int[] MyPropertyIdList;

   
    public void FillPGImage(ref PGImage image) {

        EXIF exif = new EXIF();
        exif.Make= GetMake()!=null ? GetMake() : "";
        exif.WhiteBalance = GetWhiteBalance() != null ? GetWhiteBalance() : "";
        exif.Aperture = GetFNumber() != null ? GetFNumber() : "";
        exif.ISO = GetISO() != null ? GetISO() : "";
        exif.Exposure = GetExposureTime() != null ? GetExposureTime() : "";
        exif.ExposureBiasValue = GetExposureBiasValue() != null ? GetExposureBiasValue() : "";
        exif.SubjectDistance = GetSubjectDistance() != null ? GetSubjectDistance() : "";
        exif.FocalLength = GetFocalLength() != null ? GetFocalLength() : "";
        exif.Flash = GetFlash() != null ? GetFlash() : "";

        image.EXIF = exif;
    }


    public ExifReader(String file)
    {
        try
        {
            image = System.Drawing.Image.FromFile(file);
            MyPropertyIdList = image.PropertyIdList;  //Array
            Console.WriteLine("\nMyPropertyIdList.Length= " + MyPropertyIdList.Length);
            ascii = new ASCIIEncoding(); //System.Text
        }
        catch (Exception e)
        {
            Console.WriteLine("Reading Image Error: " + e);
        }
    }


    public string GetAll()
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            int i = 0;
            sb.Append("Count" + "\t" + "Decimal Tag" + "\t" + "Hex Tag");
            foreach (int MyPropertyId in MyPropertyIdList)
            {
                PropertyItem propItem = image.GetPropertyItem(MyPropertyId);
                int propDec = MyPropertyIdList[i]; //Decimal Tag ID
                String propHex = propItem.Id.ToString("x"); // Hexadecimal Tag ID (Use Decimal instead)
                sb.Append(i + "\t" + propDec + "\t\t" + propHex);
                i++;
            }
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return sb.ToString();
    }

    public void PrintAll()
    {
        try
        {
            int i = 0;
            Console.WriteLine("Count" + "\t" + "Decimal Tag" + "\t" + "Hex Tag");
            foreach (int MyPropertyId in MyPropertyIdList)
            {
                PropertyItem propItem = image.GetPropertyItem(MyPropertyId);
                int propDec = MyPropertyIdList[i]; //Decimal Tag ID
                String propHex = propItem.Id.ToString("x"); // Hexadecimal Tag ID (Use Decimal instead)
                Console.WriteLine(i + "\t" + propDec + "\t\t" + propHex);
                i++;
            }
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
    }

    //256 ImageWidth
    public String GetImageWidth()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(256);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //257 ImageLength
    public String GetImageLength()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(257);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //258 BitsPerSample
    public String GetBitsPerSample()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(258);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //259 Compression
    public String GetCompression()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(259);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //262 PhotometricInterpretation
    //270 ImageDescription

    //271 Make				ASCII
    public String GetMake()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(271);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //272 Model
    public String GetModel()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(272);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //273 StripOffsets

    //274 Orientation		Short
    public String GetOrientation()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(274);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "1") return "Correct";
            if (pv0 == "6") return "Requires Rotation to the Right";
            if (pv0 == "8") return "Requires Rotation to the Left";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //277 SamplesPerPixel 	Short
    //278 RowsPerStrip
    //279 StripByteCounts
    //282 XResolution		Rational
    //283 YResolution
    //284 PlanarConfiguration
    //296 ResolutionUnit
    //301 TransferRate

    //305 Software
    public String GetSoftware()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(305);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //306 DateTime
    public String GetDateTime()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(306);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //315 Artist				ASCII
    public String GetArtist()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(315);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //218 WhitePoint
    //319 PrimaryChromaticities
    //513 JPEGInterchangeFormat
    //514 JPEGInterchangeFormatLength
    //529 YCbCrCoefficients
    //530 YCbCrCoefficients
    //530 YCbCrSubSampling
    //531 YCbCrPositioning
    //532 ReferenceBlackWhite

    //33432 Copyright			ASCII
    public String GetCopyright()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(33432);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //34665 Exif IFD Pointer
    //34853 GPSInfo IFD Pointer

    // Same as Shutter Speed
    public String GetExposureTime()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(33434);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            String pv1 = BitConverter.ToInt16(propItem.Value, 4).ToString();
            // Turn 10/2500 into 1/250
            if (pv0.EndsWith("0") && pv1.EndsWith("0"))
            {
                pv0 = pv0.Substring(0, pv0.Length - 1);
                pv1 = pv1.Substring(0, pv1.Length - 1);
            }
            //Turn 8/1 into 8
            if (pv1 == "1") return pv0;
            return pv0 + "/" + pv1;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";


    }

    //Same as FStop
    public String GetFNumber()
    {
        try
        {

            return (Convert.ToDecimal(System.BitConverter.ToInt32(image.GetPropertyItem(33437).Value, 0)) / Convert.ToDecimal(System.BitConverter.ToInt32(image.GetPropertyItem(33437).Value, 4))).ToString();

            /*
PropertyItem propItem = image.GetPropertyItem(33437);
String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
if (pv0.Length == 3) pv0 = pv0.Substring(0, 2);
if (pv0 != "22" && pv0 != "32" && !pv0.StartsWith("1"))
    pv0 = pv0.Substring(0, pv0.Length - 1) + "." + pv0.Substring(pv0.Length - 1, 1);
pv0 = pv0.Replace(".0", "");
return pv0;
*/
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //********No tag in my jpegs
    public String GetExposureProgram()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(34850);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Not Defined";
            if (pv0 == "1") return "Manual";
            if (pv0 == "2") return "Program";
            if (pv0 == "3") return "Aperature Priority";
            if (pv0 == "4") return "Shutter Priority";
            if (pv0 == "5") return "Creative Program";
            if (pv0 == "6") return "Action Program";
            if (pv0 == "7") return "Portait Mode";
            if (pv0 == "8") return "Landscape Mode";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //34852 SpectralSensitivity	ASCII
    public String GetSpectralSensitivity()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(34852);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //34855 ISO
    public String GetISO()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(34855);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //34856 OECF	Undefined (Opto-Electric Conversion Function)

    public String GetExifVersion()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(36864);
            //ASCII
            String pv = ascii.GetString(propItem.Value);
            // Turn 0220 into 02.20
            pv = pv.Substring(0, 2) + "." + pv.Substring(2, pv.Length - 2);
            // Turn 02.20 into 2.20
            if (pv.StartsWith("0")) pv = pv.Substring(1, pv.Length - 1);
            //Console.WriteLine("Model = " + decodedString);
            return pv;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";

    }

    //36867 DateTimeOriginal
    public String GetDateTimeOriginal()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(36867);
            String decodedString = ascii.GetString(propItem.Value);
            //Console.WriteLine("Model = " + decodedString);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //36868 DateTimeDigitized			ASCII
    public String GetDateTimeDigitized()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(36868);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37121 ComponentsConfiguration		Undefined
    //37122 CompressedBitsPerPixel		Rational
    //37377 ShutterSpeedValue 			SRational (Not the same as exposure for some reason.)
    //37378 ApertureValue 				Rational (Not the same as FNumber for some reason.)
    //37379 BrightnessValue				SRational

    //37380 ExposureBiaValue
    public String GetExposureBiasValue()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(37380);
            float pvf = (float)(BitConverter.ToInt16(propItem.Value, 0)) / 2;
            String pv0 = pvf.ToString();
            if (!pv0.StartsWith("-")) pv0 = "+" + pv0;
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37381 MaxApertureValue	Rational

    //37382 SubjectDistance
    public String GetSubjectDistance()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(37382);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }



    //37383 MeteringMode
    public String GetMeteringMode()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(37383);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "1") return "Average Metering";
            if (pv0 == "2") return "Center Weighted Average Metering";
            if (pv0 == "3") return "Spot Metering";
            if (pv0 == "4") return "MultiSpot Metering";
            if (pv0 == "5") return "Matrix Metering";
            if (pv0 == "6") return "Partial Metering";

            return "Unknown";
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37384 LightSource

    //37385 Flash
    public String GetFlash()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(37385);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "No Flash";
            if (pv0 == "1") return "Flash";
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37386 FocalLength
    public String GetFocalLength()
    {
        try
        {
            return (System.BitConverter.ToInt32(image.GetPropertyItem(37386).Value, 0) / System.BitConverter.ToInt32(image.GetPropertyItem(37386).Value, 4)).ToString() + "mm";

            //PropertyItem propItem = image.GetPropertyItem(37386);
            //String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            //return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37396 SubjectArea				Short
    public String GetSubjectArea()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(37396);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //37500 MakerNote				Undefined
    //37510 UserCommment			Undefined
    //37520 SubSecTime				ASCII
    //37521 SubSecTimeOriginal		ASCII
    //37522 SubSecTimeDigitized		ASCII
    //40960 FlashpixVersion			Undefined
    //40961 ColorSpace				Short
    //40962 PixelXDimension			Short
    //40963 PixelYDimension			Short
    //40964 RelatedSoundFile		ASCII
    //41483 FlashEnergy				Rational
    //41484 SpatialFrequencyResponse
    //41486 FocalPlaneXResolution
    //41487 FocalPlaneYResolution
    //41488 FocalPlaneResolutionUnit

    //41492 SubjectLocation			Short
    public String GetSubjectLocation()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41492);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41493 ExposureIndex			Rational
    //41495 SensingMethod			Short
    //41728 FileSource
    //41729 ScenType
    //41730 CFAPattern


    //41986 ExposureMode
    public String GetExposureMode()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41986);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Auto Exposure";
            if (pv0 == "1") return "Manual Exposure";
            if (pv0 == "2") return "Auto Bracket";
            if (pv0 == "3") return "Reserved";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41987 WhiteBalance
    public String GetWhiteBalance()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41987);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Auto";
            if (pv0 == "1") return "Manual";
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41988 DigitalZoomRatio
    public String GetDigitalZoomRatio()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41989);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "No Digital Zoom Used";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }



    //41989 FocalLengthIn35mmFilm		Short
    public String GetFocalLengthIn35mmFilm()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41989);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41990 SceneCaptureType
    //41991 GainControl

    //41992 Contrast					Short
    public String GetContrast()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41992);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Normal";
            if (pv0 == "1") return "Soft";
            if (pv0 == "2") return "Hard";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41993 Saturation					Short
    public String GetSaturation()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41993);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Normal";
            if (pv0 == "1") return "Low";
            if (pv0 == "2") return "High";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41994 Sharpness					Short
    public String GetSharpness()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41994);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            if (pv0 == "0") return "Normal";
            if (pv0 == "1") return "Soft";
            if (pv0 == "2") return "Hard";
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //41995 DeviceSettingDescription

    //41996 SubjectDistanceRange		Short
    public String GetSubjectDistanceRange()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(41996);
            String pv0 = BitConverter.ToInt16(propItem.Value, 0).ToString();
            return pv0;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    //42016 ImageUniqueID
    public String GetImageUniqueID()
    {
        try
        {
            PropertyItem propItem = image.GetPropertyItem(42016);
            String decodedString = ascii.GetString(propItem.Value);
            return decodedString;
        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.ArgumentNullException")
            {
            }
        }
        return "";
    }

    // Release Image object so we can delete file if we wish
    public void MyDispose()
    {
        image.Dispose();
    }



    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main(String[] Args)
    {
        if (Args.Length != 1)
        {
            Console.WriteLine("Usage: ExifReader <path to image>");
            Environment.Exit(-1);
        }

        String file = Args[0];

        ExifReader r = new ExifReader(file);  //file is String

        Console.WriteLine("");
        r.PrintAll();
        Console.WriteLine("");
        Console.WriteLine("Make: " + r.GetMake());
        Console.WriteLine("Model: " + r.GetModel());

        Console.WriteLine("White Balance: " + r.GetWhiteBalance());
        // User this one for FStop
        Console.WriteLine("F" + r.GetFNumber());
        // Use this one for Shutter Speed
        Console.WriteLine("Exposure Time: " + r.GetExposureTime() + " Sec");
        Console.WriteLine("Exposure Program: " + r.GetExposureProgram());
        Console.WriteLine("ISO: " + r.GetISO());
        Console.WriteLine("Exposure Bias Value: " + r.GetExposureBiasValue());
        Console.WriteLine("Subject Distance: " + r.GetSubjectDistance());
        Console.WriteLine("FocalLength: " + r.GetFocalLength());

        Console.WriteLine("Flash: " + r.GetFlash());
        Console.WriteLine("Exposure Mode: " + r.GetExposureMode());
        Console.WriteLine("EXIF Version: " + r.GetExifVersion());
        Console.WriteLine("DateTime: " + r.GetDateTime());
        Console.WriteLine("DateTimeOriginal: " + r.GetDateTimeOriginal());
        Console.WriteLine("Artist: " + r.GetArtist());
        Console.WriteLine("Copyright: " + r.GetCopyright());
        Console.WriteLine("Software: " + r.GetSoftware());
        Console.WriteLine("SpectralSensitivity: " + r.GetSpectralSensitivity());
        Console.WriteLine("DateTimeDigitized: " + r.GetDateTimeDigitized());
        Console.WriteLine("ImageWidth: " + r.GetImageWidth());
        Console.WriteLine("ImageLength: " + r.GetImageLength());
        Console.WriteLine("BitsPerSample: " + r.GetBitsPerSample());
        Console.WriteLine("Compression: " + r.GetCompression());
        Console.WriteLine("Orientation: " + r.GetOrientation());
        Console.WriteLine("SubjectArea: " + r.GetSubjectArea());
        Console.WriteLine("SubjectLocation: " + r.GetSubjectLocation());
        Console.WriteLine("FocalLengthIn35mmFilm: " + r.GetFocalLengthIn35mmFilm());
        Console.WriteLine("Contrast: " + r.GetContrast());
        Console.WriteLine("Saturation: " + r.GetSaturation());
        Console.WriteLine("Sharpness: " + r.GetSharpness());
        Console.WriteLine("SubjectDistanceRange: " + r.GetSubjectDistanceRange());
        Console.WriteLine("MeteringMode: " + r.GetMeteringMode());
        Console.WriteLine("DigitalZoomRatio: " + r.GetDigitalZoomRatio());
        Console.WriteLine("ImageUniqueID: " + r.GetImageUniqueID());
        //Console.WriteLine(": " + r.Get());


    }


}


