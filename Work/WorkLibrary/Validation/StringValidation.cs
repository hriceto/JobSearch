using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace HristoEvtimov.Websites.Work.WorkLibrary.Validation
{
    public class StringValidation
    {
        public enum SanitizeEntityNames 
        {   
            Name, Phone, Location, PositionTitle, Address, City,
            CreditCardNumber, CreditCardCVV, Keywords, Url, Domain, CompanyName, Email, 
            SeUrl,
        };
        private Dictionary<SanitizeEntityNames, String> SanitizeEntityValues = new Dictionary<SanitizeEntityNames, String>()
        {
            { SanitizeEntityNames.Name, "[^a-zA-Z '\\.-]" },
            { SanitizeEntityNames.Phone, "[^0-9 \\[\\]\\(\\)\\.\\+-]" },
            { SanitizeEntityNames.Location, "[^\\w\\s&#,'\"/\\(\\)\\.-]" },
            { SanitizeEntityNames.PositionTitle, "[^\\w\\s&#,'\"/\\(\\)\\.-]" },
            { SanitizeEntityNames.Address, "[^\\w\\s&#:,'\"/\\.-]" },
            { SanitizeEntityNames.City, "[^a-zA-Z '\\.-]" },
            { SanitizeEntityNames.CreditCardNumber, "[^0-9]" },
            { SanitizeEntityNames.CreditCardCVV, "[^0-9]" },
            { SanitizeEntityNames.Keywords, "[^\\w\\s&#,\\(\\)\\.\\+-]" },
            { SanitizeEntityNames.Url, "[^\\w~:/#@!&;=%',\\]\\[\\(\\)\\+\\$\\*\\?\\.-]" },
            { SanitizeEntityNames.Domain, "[^\\w\\.-]" },
            { SanitizeEntityNames.CompanyName, "[^\\w\\s!@#$%&\\*\\?\\(\\)\\.\\+-]" },
            { SanitizeEntityNames.Email, "[^\\w\\.-@]" },
            { SanitizeEntityNames.SeUrl, "[^\\w/-]" },
        };

        /// <summary>
        /// Remove characters from string. this is used to clean input values such as names, phone numbers,
        /// addresses. The user is allowed to enter anything but some of the characters might get filtered out.
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="allowedStringSets"></param>
        /// <returns></returns>
        public string SanitizeUserInputString(string userInput, SanitizeEntityNames entityName)
        {
            Regex regex = new Regex(SanitizeEntityValues[entityName], RegexOptions.Singleline);
            string result = regex.Replace(userInput, "");
            return result;
        }

        public string GetAllowedCharacters(SanitizeEntityNames entityName)
        {
            string validationRule = SanitizeEntityValues[entityName];
            validationRule = validationRule.Replace("[^", "[").Replace("]", "]+");
            return validationRule;
        }
    }
}
