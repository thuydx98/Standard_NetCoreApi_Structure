using StandardApi.Common.Entities;
using System;
using System.Collections.Generic;

namespace StandardApi.Data.Entities.User
{
    public partial class UserEntity: IBaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public UserEntity()
        {
            Addresses = new HashSet<UserAddressEntity>();
            Avatars = new HashSet<UserAvatarEntity>();
            UserOtherPermisisons = new HashSet<UserPermissionEntity>();
            UserStatusLogs = new HashSet<LogUserStatusEntity>();
        }

        public int Id { get; set; }
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CitizenID { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public int? StatusId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual UserStatusEntity Status { get; set; }
        public virtual RoleEntity Role { get; set; }
        
        public virtual ICollection<UserAddressEntity> Addresses { get; set; }
        public virtual ICollection<UserAvatarEntity> Avatars { get; set; }
        public virtual ICollection<UserPermissionEntity> UserOtherPermisisons { get; set; }
        public virtual ICollection<LogUserStatusEntity> UserStatusLogs { get; set; }
    }
}
