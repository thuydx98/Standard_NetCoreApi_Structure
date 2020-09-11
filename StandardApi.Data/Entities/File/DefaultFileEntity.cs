using StandardApi.Common.Entities;

namespace StandardApi.Data.Entities.File
{
    public partial class DefaultFileEntity : IBaseEntity
    {
        public int Id { get; set; }
        public string DefaultType { get; set; }
        public string Title { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileType { get; set; }
        public bool Status { get; set; }
    }
}
