using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yambr.Analyzer.Models;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Extensions
{
    public static class ContactExtensions
    {

        public static Contact ToContact(this IPersonReferrent person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            var personLastName = FirstCharToUpper(person.LastName);
            var personFirstName = FirstCharToUpper(person.FirstName);
            var personMiddleName = FirstCharToUpper(person.MiddleName);
            var company = person.Company;
            var contractor = company !=null? new Contractor()
            {
                Name = company.Name,
                INN = company.INN,
                OGRN = company.OGRN,
                Description = company.Description,
                Site = company.Site
            } : null;

            return  new Contact()
            {
                Fio = $"{personLastName} {personFirstName} {personMiddleName}".Trim(),
                Gender = person.Gender,
                LastName = personLastName,
                FirstName = personFirstName,
                MiddleName = personMiddleName,
                Position = person.Position,
                Phones = person.Phones.Select(c => new Phone(c.PhoneString, c.FormattedPhoneString)).ToList(),
                Emails = person.Emails.Select(c => new Common.Models.Email(c)).ToList(),
                Contractor = contractor,
                Description = person.Description,
                Site = person.Site
            };
        }

        private static string FirstCharToUpper(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s= s.ToLower();
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
