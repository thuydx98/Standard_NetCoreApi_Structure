using StandardApi.Common.Entities;
using System;
using System.Collections.Generic;

namespace StandardApi.Data.Entities.User
{
    public partial class RoleEntity :IBaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public RoleEntity()
        {
            Users = new HashSet<UserEntity>();
            Permissions = new HashSet<PermissionEntity>();
        }

        public int Id { get; set; }
        public string Role { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
        public virtual ICollection<PermissionEntity> Permissions { get; set; }
    }
}
