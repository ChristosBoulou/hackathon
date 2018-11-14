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
                Raw = companyInfoLines

            };
            return bankStatement;
        }

    }
}
