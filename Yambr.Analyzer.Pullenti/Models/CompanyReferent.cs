﻿using System.Linq;
using EP.Ner.Org;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Pullenti.Extensions;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal class CompanyReferent : ICompanyReferent
    {
        public CompanyReferent(OrganizationReferent orgainzation)
        {
            Fill(orgainzation);
        }
        public CompanyReferent()
        {
        }
        public void Fill(OrganizationReferent orgainzation)
        {
            Name = orgainzation.Occurrence
                .OrderByDescending(c=>c.EndChar - c.BeginChar)
                .FirstOrDefault()?.GetText()?.RemoveWhitespace();
            INN = orgainzation.INN;
            OGRN = orgainzation.OGRN;
           // Description = string.Join("\n", orgainzation.Slots.Select(c => c.Value.ToString()));
        }

        public string Name { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }

        public override string ToString()
        {
            return $"{Name} {INN} {OGRN} {Description} {Site}";
        }
    }
}