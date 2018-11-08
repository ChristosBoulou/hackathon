using System;
using System.Collections.Generic;
using System.Linq;
using OcrFunctions.Dtos;

namespace OcrFunctions.Services
{
    public static class DrivingLicenseHelper
    {

        public static DrivingLicenseDto ExtractDrivingInfo(this List<string> lines)
        {
            DrivingLicenseDto license = new DrivingLicenseDto
            {
                Raw = lines,
                LastName = lines.FirstOrDefault(t => t.StartsWith("1"))?.Substring(2)?.Trim(),
                FirstName = lines.FirstOrDefault(t => t.StartsWith("2"))?.Substring(2)?.Trim(),
                LicenseNumber = lines.FirstOrDefault(t => t.StartsWith("5"))?.Substring(2)?.Trim(),
                AddressLine1 = lines.FirstOrDefault(t => t.StartsWith("8"))?.Substring(2)?.Trim()
            };

            return license;
        }
    }
}
