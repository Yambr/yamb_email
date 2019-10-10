using System;
using System.Collections.Generic;
using System.Linq;
using EP.Ner;
using EP.Ner.Address;
using EP.Ner.Org;
using EP.Ner.Person;
using EP.Ner.Uri;
using Newtonsoft.Json;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Pullenti.Extensions;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal class PersonReferrent : IPersonReferrent, IPersonStat
    {
        public PersonReferrent(PersonReferent personReferent)
        {
            Phones = new List<IPhoneReferent>();
            Emails = new List<string>();
            foreach (var slot in personReferent.Slots)
            {
                var stringValue = slot.Value.ToString();
                switch (slot.TypeName)
                {
                    case PersonReferent.ATTR_SEX:
                        Gender = stringValue;
                        break;
                    case PersonReferent.ATTR_FIRSTNAME:
                        if(stringValue.Length >1)
                            FirstName = stringValue;
                        break;
                    case PersonReferent.ATTR_MIDDLENAME:
                        if (stringValue.Length > 1)
                            MiddleName = stringValue;
                        break;
                    case PersonReferent.ATTR_LASTNAME:
                        if (stringValue.Length > 1)
                            LastName = stringValue;
                        break;
                    case PersonReferent.ATTR_ATTR:
                        UpdateAttributes(slot);
                        break;
                    case PersonReferent.ATTR_CONTACT:
                        UpdateContacts(slot, stringValue);
                        break;

                }
            }
        }

        private void UpdateContacts(Slot slot, string stringValue)
        {
            if (slot.Value is UriReferent uriReferent)
            {
                switch (uriReferent.Scheme)
                {
                    case "mailto":
                            Emails.Add(uriReferent.Value);
                        break;
                    case "http":
                    case "https":
                    {
                        if (Company == null)
                        {
                            Company = new CompanyReferent();
                        }

                        Company.Site = stringValue;
                        Site = stringValue;
                        break;
                    }
                }
            }

            if (slot.Value is EP.Ner.Phone.PhoneReferent phoneReferent)
            {
                Phones.Add(new PhoneReferent(phoneReferent));
            }

            if (slot.Value is AddressReferent addressReferent)
            {
                UpdateDescription($"Адрес: {addressReferent}");
            }
        }
        private static bool HasRussian(string name)
        {
            return name != null &&
                   name.Any(c => (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё');
        }
        private void UpdateAttributes(Slot slot)
        {
            if (!(slot.Value is PersonPropertyReferent personPropertyReferent)) return;
            UpdatePosition(personPropertyReferent);

            if (!(personPropertyReferent.Slots.FirstOrDefault(c => c.Value is OrganizationReferent)?.Value is
                OrganizationReferent organization)) return;
            if (Company == null)
            {
                Company = new CompanyReferent();
            }

            if (organization.Higher != null)
            {
                var higher = GetHigher(organization);
                ((CompanyReferent) Company).Fill(higher.Kind == OrganizationKind.Department
                    ? organization
                    : higher);
            }
            else
            {
                ((CompanyReferent) Company).Fill(organization);
            }

            UpdateDescription(organization);
        }

        private void UpdateDescription(OrganizationReferent organization)
        {
            var text = organization.Occurrence
                .OrderByDescending(c => c.EndChar - c.BeginChar)
                .FirstOrDefault()?.GetText()?.RemoveWhitespace();
            UpdateDescription($"Организация: {text}\n");
        }

     

        private void UpdateDescription(string formattableString)
        {
            Description += formattableString;
        }
        private void UpdatePosition(PersonPropertyReferent personPropertyReferent)
        {
            if ((Position?.Length ?? 0) < personPropertyReferent.Name.Length)
                Position = personPropertyReferent.Name;
        }

        private static OrganizationReferent GetHigher(OrganizationReferent organization)
        {
            while (true)
            {
                if (organization.Higher == null)
                    return organization;
                organization = organization.Higher;
            }
        }

        public string Gender { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        [JsonIgnore]
        public ICompanyReferent Company { get; set; }
        public ICollection<IPhoneReferent> Phones { get; set; }
        public ICollection<string> Emails { get; set; }


    }
}
