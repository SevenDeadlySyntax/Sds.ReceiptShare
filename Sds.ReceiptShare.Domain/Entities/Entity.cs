using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Domain.Entities
{
    public abstract class Entity
    {
        // TODO: change to GUID or make the PK for group GUID and inherit from another type
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
