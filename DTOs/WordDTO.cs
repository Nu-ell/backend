namespace TechDictionaryApi.DTOs
{
    public class WordDTO
    {
        public long WordId { get; set; }
        public string? Word { get; set; }
        public string? Class { get; set; }
        public string? Defination { get; set; }
        public string? Pronounciation { get; set; }
        public string? History { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class CreateWordDTO
    {
        public string? Word { get; set; }
        public string? Class { get; set; }
        public string? Defination { get; set; }
        public string? Pronounciation { get; set; }
        public string? History { get; set; }
        public int StatusId { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class UpdateWordDTO
    {
        public long WordId { get; set; }
        public string? Word { get; set; }
        public string? Class { get; set; }
        public string? Defination { get; set; }
        public string? Pronounciation { get; set; }
        public string? History { get; set; }
        public int StatusId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class DeleteWordDTO
    {
        public long WordId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
