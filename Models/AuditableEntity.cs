using Microservices.Common.Enums;

namespace Microservices.Common.Models
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public RowStatus RowStatus { get; set; } = RowStatus.Active;
    }
}