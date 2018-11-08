using System;
using System.Collections.Generic;

namespace OcrFunctions.Dtos
{
    public class DrivingLicenseDto
    {
        public DrivingLicenseDto()
        {
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
        public string LicenseNumber { get; set; }

        public List<string> Raw { get; set; }
    }
}
