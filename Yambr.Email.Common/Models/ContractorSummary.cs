using System;

namespace Yambr.Email.Common.Models
{
    /// <summary>
    ///  Владелец пиьсма
    /// </summary>
    public class ContractorSummary :  IEquatable<ContractorSummary>
    {
        public ContractorSummary()
        { }

        public string Name { get; set; }

        public bool Equals(ContractorSummary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ContractorSummary) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
