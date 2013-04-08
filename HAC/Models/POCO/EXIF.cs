using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAC.Models.POCO
{
    public class EXIF
    {
        public string Make { get; set; }
        public string WhiteBalance { get; set; }
        public string Aperture { get; set; }
        public string Exposure { get; set; }
        public string ISO { get; set; }
        public string ExposureBiasValue { get; set; }
        public string SubjectDistance { get; set; }
        public string FocalLength { get; set; }
        public string Flash { get; set; }

        public bool HasInfo {
            get {
                return (!string.IsNullOrWhiteSpace(Make)
                    || !string.IsNullOrWhiteSpace(WhiteBalance)
                    || !string.IsNullOrWhiteSpace(Aperture)
                    || !string.IsNullOrWhiteSpace(Exposure)
                    || !string.IsNullOrWhiteSpace(ISO)
                    || !string.IsNullOrWhiteSpace(ExposureBiasValue)
                    || !string.IsNullOrWhiteSpace(SubjectDistance)
                    || !string.IsNullOrWhiteSpace(FocalLength)
                    || !string.IsNullOrWhiteSpace(Flash)
                    );

            }
        }
    }
}