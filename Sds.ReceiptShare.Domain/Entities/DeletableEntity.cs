using System;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class DeletableEntity : Entity
    {
        public bool IsDeleted { get; set; }
        public DateTime Deleted { get; set; }
    }
}
