using StandardApi.Common.Entities;
using System;

namespace StandardApi.Data.Entities.User
{
    public partial class UserAddressEntity : IBaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Receiver { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Detail { get; set; }
        public bool? IsDefault { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
