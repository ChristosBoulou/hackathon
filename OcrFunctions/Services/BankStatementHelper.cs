using System;
using System.Collections.Generic;
using OcrFunctions.Dtos;

namespace OcrFunctions.Services
{
    public static class BankStatementHelper
    {

        public static BankStatementDto ExtractBankStatementInfo(this List<string> lines)
        {
            BankStatementDto bankStatement = new BankStatementDto
            {
                Raw = lines

            };
            return bankStatement;
        }

    }
}
