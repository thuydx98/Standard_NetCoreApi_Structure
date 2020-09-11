using System;

namespace StandardApi.Common.Entities
{
    public interface ICreatedEntity
    {
        DateTime? CreatedAt { get; set; }
        string CreatedBy { get; set; }
    }
}
