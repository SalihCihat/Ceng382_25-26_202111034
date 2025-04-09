namespace lab6.Models
{
    public class ClassInformationTable
    {
        public int Id { get; set; }  // Arka planda kullanılacak
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
