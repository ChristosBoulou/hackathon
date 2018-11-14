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
            DrivingLicenseDto license = new DrivingLicenseDto()
            {
                Raw = lines
            };


            for (int i = 0; i < lines.Count; i++)
            {
                var lastName = SearchForItem("1.", i, lines, 1, "2");
                if(lastName != null && lastName.Any())
                {
                    license.LastName = lastName.First();
                }
                var firstNames = SearchForItem("2.", i, lines, 2, "3");
                if (firstNames != null && firstNames.Any())
                {
                    license.FirstName = firstNames[0];
                    license.MiddleNames = firstNames.Count > 1 ? firstNames[1] : String.Empty;
                }
                var licenseNumber = SearchForItem("5.", i, lines, 1, "6");
                if (licenseNumber != null && licenseNumber.Any())
                {
                    license.LicenseNumber = licenseNumber.First();
                }
                var addressInfo = SearchForItem("8.", i, lines, 3, "9");
                if (addressInfo != null && addressInfo.Any())
                {
                    license.AddressLine1 = addressInfo.First();
                    license.PostCode = addressInfo.Last();
                    license.AddressLine2 = addressInfo.Count > 2 ? addressInfo[1] : String.Empty;
                    if(addressInfo.Count > 3)
                    {
                        license.AddressLine3 = string.Join(", ", addressInfo.Skip(2).Take(addressInfo.Count - 1));
                    }
                }
            }

            return license;
        }

        private static List<string> SearchForItem(string searchItem, int currentIndex, List<string> lines, int numberOfLines, string breakText)
        {
            if(lines[currentIndex].Trim().StartsWith(searchItem))
            {
                var startIndex = currentIndex;
                var resultList = new List<string>();
                var result =  lines[currentIndex].Replace(searchItem, string.Empty).Trim();
                if(!string.IsNullOrWhiteSpace(result))
                {

                    var commaSplit = result.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (commaSplit.Count() > 1)
                    {
                        resultList.AddRange(commaSplit);
                        currentIndex=  startIndex = startIndex + commaSplit.Count() - 2;
                    }
                    else
                    {
                        resultList.Add(result);
                    }
                }
                else if(lines.Count > startIndex + 1)
                {
                    startIndex++;
                    currentIndex++;
                    resultList.Add(lines[currentIndex].Trim());
                }
                currentIndex++;
                while(currentIndex < startIndex + numberOfLines && lines.Count > currentIndex)
                {
                    var newLine = lines[currentIndex].Trim();
                    if(newLine.StartsWith(breakText))
                    {
                        break;
                    }
                    if(!string.IsNullOrWhiteSpace(newLine))
                    {
                        resultList.Add(newLine);
                    }
                    currentIndex++;
                }
                return resultList;

            }
            return null;
        }
    }
}
