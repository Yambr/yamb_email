using System;

namespace Yambr.Email.Common.Models.Records.Nested
{
    /// <summary>
    ///  Владелец пиьсма
    /// </summary>
    public class ContractorSummary : ISummary, IEquatable<ContractorSummary>
    {
        public ContractorSummary()
        { }

        public ContractorSummary(Contractor contractor)
        {
            if (contractor == null) throw new ArgumentNullException(nameof(contractor));
            Name = contractor.Name;
            Ref = contractor.CreateDBRef();
        }

        public string Name { get; set; }

        public MongoDBRef Ref { get; set; }

        public bool Equals(ContractorSummary other)
        {
            return other != null && Ref.Equals(other.Ref);
        }
    }
}
