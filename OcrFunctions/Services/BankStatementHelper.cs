using System;
using System.Collections.Generic;
using OcrFunctions.Dtos;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OcrFunctions.Services
{
    public static class BankStatementHelper
    {

        public static BankStatementDto ExtractBankStatementInfo(this List<Line> lines)
        {


            var maxcoordinates = lines.GetMaxXandY();
            List<Line> companyInfoLines = new List<Line>();
            foreach( var line in lines)
            {
                var isWithinPercentage = BoundingBoxHelper.IsWithinThisPercentage(line, maxcoordinates, 
                30, 30);

                if(isWithinPercentage.Item1 && isWithinPercentage.Item2)
                {
                    companyInfoLines.Add(line);
                }
                if(!isWithinPercentage.Item2)
                {
                    break;
                }

            }

            BankStatementDto bankStatement = new BankStatementDto
            {
                CompanyName = companyInfoLines[0].Text,
                AddressLine1 = companyInfoLines[1].Text,
                AddressLine2 = companyInfoLines[2].Text,
                AddressLine3 = companyInfoLines[3].Text,
                PostCode = companyInfoLines[4].Text,
                CompanyType = GetCompanyType(companyInfoLines[0].Text),
                AccountProvider = lines.First().Text,
                Raw = lines.Select( x=> x.Text).ToList()

            };
            return bankStatement;
        }

        public static string GetCompanyType( this string companyName)
        {
            var companyType = String.Empty;
            var formattedCompanyName = companyName.ToUpper();
            if(formattedCompanyName.Contains("LTD"))
            {
                companyType = "Private Limited Company";
            }
            else if(formattedCompanyName.Contains("PLC"))
            {
                companyType = "Public Limited Company";
            }
            else 
            {
                companyType = "Sole trader";
            }

            return companyType;
        }

    }
}
