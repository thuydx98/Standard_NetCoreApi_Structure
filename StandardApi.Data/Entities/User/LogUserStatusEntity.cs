using StandardApi.Common.Entities;
using System;

namespace StandardApi.Data.Entities.User
{
    public partial class LogUserStatusEntity : IBaseEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserStatusEntity Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
