using System;

namespace BookShop.Domain.Entities.Abstract
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        
        string CreatedBy { get; set; }

        DateTime DateCreated { get; set; }

        string LastModifiedBy { get; set; }

        DateTime? DateLastModified { get; set; }
    }
}