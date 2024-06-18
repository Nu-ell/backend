namespace TechDictionaryApi.DTOs
{
    public class ExampleDTO
    {
        public long ExampleId { get; set; }
        public string? WordExample { get; set; }
        public long WordId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class CreateExampleDTO
    {
        public string? WordExample { get; set; }
        public long WordId { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class UpdateExampleDTO
    {
        public long ExampleId { get; set; }
        public string? WordExample { get; set; }
        public string? UpdatedBy { get; set; }
    }



    public class DeleteExampleDTO
    {
        public long ExampleId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
