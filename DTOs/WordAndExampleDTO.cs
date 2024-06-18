namespace TechDictionaryApi.DTOs
{
    public class CreateWordAndExamplesDTO
    {
        public string? Word { get; set; }
        public string? Class { get; set; }
        public string? Defination { get; set; }
        public string? Pronounciation { get; set; } //is not a required feild from db
        public string? History { get; set; } //is not a required feild from db
        public int StatusId { get; set; }

        //public string? CreatedBy { get; set; }
        public List<CreateExampleServiceDTO> Examples { get; set; }
    }
    public class CreateExampleServiceDTO
    {
        public string? WordExample { get; set; }
    }


    public class UpdateWordAndExamplesDTO
    {
        public long WordId { get; set; }
        public string? Word { get; set; }
        public string? Class { get; set; }
        public string? Defination { get; set; }
        public string? Pronounciation { get; set; }
        public string? History { get; set; }
        public int StatusId { get; set; }
        public DateTime UpdatedDate { get; set; }

        //public string? UpdatedBy { get; set; }
        public List<UpdateExampleServiceDTO> Examples { get; set; }
    }
    public class UpdateExampleServiceDTO
    {
        public long ExampleId { get; set; }
        public string? WordExample { get; set; }
    }



    public class DeleteWordAndExamplesDTO
    {
        public long WordId { get; set; }

        //public string? DeletedBy { get; set; }
    }


    public class WordAndExamplesDTO
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
        public List<ExampleServiceDTO> Examples { get; set; }
    }

    public class ExampleServiceDTO
    {
        public long ExampleId { get; set; }
        public string? WordExample { get; set; }
        public long WordId { get; set; }
    }
}
