namespace documentManagerService.Model
{
    public class FileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int FileType { get; set; }

        public string FileBase64 { get; set; }
    }
}