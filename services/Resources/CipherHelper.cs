using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Resources
{
    public class CipherHelper
    {
        private List<originalText> pwPartsList;

        public class originalText
        {
            public string strText;
            public int intNumber;
            public int intSize; // Used for decrypting
        }

        public string DecriptPassword(string encryptedPassword)
        {
            string unencryptedPassword = "";

            string strCyphered = "";
            string strLastDigit = "";
            string strPartSize = "";
            int intPartSize = -1;
            string strThePart = "";
            string strDecript1 = "";
            string strNumber1 = "";
            string strNumber2 = "";

            // The password is coming in encrypted, so we must decript it.
            strCyphered = encryptedPassword;
            //logger.Debug("strCyphered (incoming) = " + strCyphered);

            strLastDigit = strCyphered.Substring(strCyphered.Length - 1, 1);
            //logger.Debug("strLastDigit = " + strLastDigit);

            strCyphered = strCyphered.Substring(0, strCyphered.Length - 1);
            //logger.Debug("strCyphered (after rem last digit) = " + strCyphered);

            strDecript1 = divideProcess(strCyphered, strLastDigit);
            //logger.Debug("strDecript1 (after unwrap first step)= " + strDecript1);

            // Separate out the pwHash, the client number, and the server number.
            strNumber1 = strDecript1.Substring((strDecript1.Length - 20), 10);
            strNumber2 = strDecript1.Substring((strDecript1.Length - 10), 10);
            strCyphered = strDecript1.Substring(0, (strDecript1.Length - 20));
            //logger.Debug("strCyphered = " + strCyphered);
            //logger.Debug("strNumber1 = " + strNumber1);
            //logger.Debug("strNumber2 = " + strNumber2);

            pwPartsList = new List<originalText>();

            int intCypheredLength = strCyphered.Length;
            while (intCypheredLength > 0)
            {
                originalText pwPart = new originalText();

                strPartSize = strCyphered.Substring(0, 1); // How many digits does the part have?
                intPartSize = Convert.ToInt32(strPartSize);
                pwPart.intSize = intPartSize;

                strCyphered = strCyphered.Substring(1); // Strip off the part size.

                strThePart = strCyphered.Substring(0, intPartSize);  // Extract the cyphered character
                pwPart.intNumber = Convert.ToInt32(strThePart);
                strCyphered = strCyphered.Substring(intPartSize); // Strip off the part.
                intCypheredLength = strCyphered.Length;

                pwPartsList.Add(pwPart);
            }

            int intFirstNumberLength = strNumber1.ToString().Length;
            for (int i = intFirstNumberLength - 1; i > -1; i--)
            {
                //logger.Debug("i = " + i);

                if ((i == 0) || (i == 3) || (i == 6) || (i == 9))
                {
                    foreach (var item in pwPartsList)
                    {
                        item.intNumber = item.intNumber - getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                    }
                }
                else if ((i == 1) || (i == 4) || (i == 7))
                {
                    foreach (var item in pwPartsList)
                    {
                        item.intNumber = item.intNumber / getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                    }
                }
                else if ((i == 2) || (i == 5) || (i == 8))
                {
                    foreach (var item in pwPartsList)
                    {
                        item.intNumber = item.intNumber + getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                    }
                }
            }
            unencryptedPassword = assemblePw();

            return unencryptedPassword;
        }

        private string divideProcess(string strCypher, string strNumber)
        {
            string strPwHashWithNumbers = "";
            string strA = "";
            string strB = "";

            strA = strCypher;
            strB = strNumber;

            strA.Trim();
            strB.Trim();

            int intALength = strA.Length;
            int intBLength = strB.Length;

            int intBDigit = -1;
            int intWholeNumber = -1;
            int intRemainder = -1;
            string strResult = "";

            string strRow1 = "";

            int intN = -1;

            // Example of what we are doing...
            //  246 / 2 = 123

            intBDigit = Convert.ToInt32(strB);

            for (int i = 0; i <= intALength - 1; i++)
            {
                strRow1 += strA.Substring(i, 1);
                intN = Convert.ToInt32(strRow1);

                intWholeNumber = Divide(intN, intBDigit);
                intRemainder = DivideRemainder(intN, intBDigit);
                strResult += intWholeNumber;

                strRow1 = intRemainder.ToString(); // Reset the row, after the math calculation.
            }

            strPwHashWithNumbers = strResult.TrimStart('0');

            return strPwHashWithNumbers;
        }

        private int Divide(int intDividend, int intDivisor)
        {
            int intWholeNumber = 0;

            intWholeNumber = intDividend / intDivisor;

            return intWholeNumber;
        }

        private int DivideRemainder(int intDividend, int intDivisor)
        {
            int intRemainder = 0;

            intRemainder = intDividend % intDivisor;

            return intRemainder;
        }

        private int getNumberFromPlace(int aNumber, string strSecondNumber)
        {
            //logger.Debug("aNumber = " + aNumber);
            int intDigitFromSecond = Convert.ToInt32(strSecondNumber.Substring(aNumber - 1, 1));

            return intDigitFromSecond;
        }

        private string assemblePw()
        {
            string result = "";

            foreach (var item in pwPartsList)
            {
                //Debug.WriteLine(item.strText + ", " + item.intNumber + ", " + item.intSize);
                //logger.Debug(item.strText + ", " + item.intNumber);
                //logger.Debug("***********");
                //logger.Debug("***********");

                result += (char)item.intNumber;
            }

            return result;
        }
    }
}