using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rct_lmis
{
    public static class NumberToWordsConverter
    {
        private static readonly string[] unitsMap =
        {
        "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };

        private static readonly string[] tensMap =
        {
        "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };

        public static string ConvertToWords(decimal number)
        {
            if (number == 0)
                return "ZERO PESOS";

            int intPart = (int)number;
            int decimalPart = (int)((number - intPart) * 100); // Handle cents

            string words = NumberToWords(intPart) + " PESOS";

            if (decimalPart > 0)
            {
                words += $" AND {decimalPart:00}/100";
            }

            return words.ToUpper();
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "ZERO";

            if (number < 20)
                return unitsMap[number];

            if (number < 100)
                return tensMap[number / 10] + (number % 10 > 0 ? " " + unitsMap[number % 10] : "");

            if (number < 1000)
                return unitsMap[number / 100] + " HUNDRED" + (number % 100 > 0 ? " " + NumberToWords(number % 100) : "");

            if (number < 1000000)
                return NumberToWords(number / 1000) + " THOUSAND" + (number % 1000 > 0 ? " " + NumberToWords(number % 1000) : "");

            if (number < 1000000000)
                return NumberToWords(number / 1000000) + " MILLION" + (number % 1000000 > 0 ? " " + NumberToWords(number % 1000000) : "");

            return NumberToWords(number / 1000000000) + " BILLION" + (number % 1000000000 > 0 ? " " + NumberToWords(number % 1000000000) : "");
        }
    }
}
