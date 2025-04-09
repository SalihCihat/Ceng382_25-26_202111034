namespace lab5.Models
{
    public class ClassInformationTable
    {
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public string Description { get; set; } = string.Empty;

        public int Id { get; set; } // Arkaplan işlemleri için
    }
}
