using StandardApi.Common.Entities;
using System;

namespace StandardApi.Data.Entities.BackgroundWorker
{
    public partial class HangfireServiceEntity : IBaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public int Id { get; set; }
        public string Cron { get; set; }
        public string Period { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
