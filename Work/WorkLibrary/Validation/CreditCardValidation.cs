using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.Validation
{
    public class CreditCardValidation
    {
        public enum CreditCardType { Other, Visa, MasterCard, Discover, Amex, Diners };

        public bool ValidateCreditCardNumber(string number)
        {
            char[] arrNumber = number.ToCharArray();
            Array.Reverse(arrNumber);
            number = new string(arrNumber);

            bool result = false;
            if (number.Length < 12 || number.Length > 19)
            {
                return false;
            }

            int sum = 0;

            for (int i = 0; i < number.Length; i++)
            {
                int digit = 0;
                if ((i % 2) != 0)
                {
                    int oddDigit = Int32.Parse(number[i].ToString());
                    int oddDigitDoubled = oddDigit * 2;
                    if (oddDigitDoubled >= 10)
                    {
                        digit = 1 + oddDigitDoubled % 10;
                    }
                    else
                    {
                        digit = oddDigitDoubled;
                    }
                }
                else
                {
                    digit = Int32.Parse(number[i].ToString());
                }

                sum += digit;
            }

            if (sum > 0 && (sum % 10 == 0))
            {
                result = true;
            }

            return result;
        }

        public bool ValidateCreditCardType(string firstFour, int length)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(firstFour))
            {
                CreditCardType cardType = GetCreditCardType(firstFour, length);
                if (cardType == CreditCardType.Visa || cardType == CreditCardType.MasterCard || cardType == CreditCardType.Discover)
                {
                    result = true;
                }
            }

            return result;
        }

        public CreditCardType GetCreditCardType(string firstFour, int length)
        {
            CreditCardType result = CreditCardType.Other;
            if ((firstFour.StartsWith("34") || firstFour.StartsWith("37")) &&
                length == 15)
            {
                result = CreditCardType.Amex;
            }
            else if (((firstFour.StartsWith("30") || firstFour.StartsWith("36") || firstFour.StartsWith("38") || firstFour.StartsWith("39")) &&
                length == 14) ||
                ((firstFour.StartsWith("54") || firstFour.StartsWith("55")) &&
                length == 16)
                )
            {
                result = CreditCardType.Diners;
            }
            else if ((firstFour.StartsWith("4")) &&
                (length == 13 || length == 16))
            {
                result = CreditCardType.Visa;
            }
            else if ((firstFour.StartsWith("51") || firstFour.StartsWith("52") || firstFour.StartsWith("53") || firstFour.StartsWith("54") || firstFour.StartsWith("55")) &&
                length == 16)
            {
                result = CreditCardType.MasterCard;
            }
            else if ((firstFour.StartsWith("6011") || firstFour.StartsWith("622") || firstFour.StartsWith("644") || firstFour.StartsWith("65")) &&
                length == 16)
            {
                result = CreditCardType.Discover;
            }
            return result;
        }
    }
}
