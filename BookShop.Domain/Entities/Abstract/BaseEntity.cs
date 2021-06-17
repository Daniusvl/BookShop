using System;

namespace BookShop.Domain.Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        public virtual int Id { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        
        public virtual string LastModifiedBy { get; set; }

        public virtual DateTime? DateLastModified { get; set; }

        public override string ToString()
        {
            return nameof(BaseEntity) + $" Id: {Id}";
        }
    }
}
