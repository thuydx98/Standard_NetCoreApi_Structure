using StandardApi.Common.Entities;
using System;
using System.Collections.Generic;

namespace StandardApi.Data.Entities.User
{
    public partial class PermissionEntity : IBaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public PermissionEntity()
        {
            UserOtherPermisisons = new HashSet<UserPermissionEntity>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Permission { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual RoleEntity Role { get; set; }
        public virtual ICollection<UserPermissionEntity> UserOtherPermisisons { get; set; }
    }
}
