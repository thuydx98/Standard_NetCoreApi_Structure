using StandardApi.Common.Entities;
using System;
using System.Collections.Generic;

namespace StandardApi.Data.Entities.User
{
    public partial class UserStatusEntity : IBaseEntity, ICreatedEntity
    {
        public UserStatusEntity()
        {
            Users = new HashSet<UserEntity>();
            UserStatusLogs = new HashSet<LogUserStatusEntity>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public bool IsBlockAccess { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
        public virtual ICollection<LogUserStatusEntity> UserStatusLogs { get; set; }
    }
}
