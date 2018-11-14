using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OcrFunctions.Dtos
{
    public class BankStatementDto
    {
        public BankStatementDto()
        {
        }
        public string CompanyName { get; set;}
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
        public string CompanyType { get; set; }
        public string AccountProvider { get; set; }
        public List<Line> Raw { get; set; }
    }
}
