using StandardApi.Common.Entities;
using System;

namespace StandardApi.Data.Entities.User
{
    public partial class UserAvatarEntity : IBaseEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileType { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
