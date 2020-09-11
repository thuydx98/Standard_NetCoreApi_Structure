using StandardApi.Common.Entities;
using System;

namespace StandardApi.Data.Entities.User
{
    public partial class UserPermissionEntity : ICreatedEntity, IUpdatedEntity
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public UserEntity User { get; set; }
        public PermissionEntity Permisison { get; set; }
    }
}
