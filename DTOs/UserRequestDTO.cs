namespace TechDictionaryApi.DTOs
{
    public class UserRequestListDTO
    {
        public long UserRequestId { get; set; }
        public string? Word { get; set; }
        public string? RequestDefinitionOrDescription { get; set; }
        public int UserRequestStatusId { get; set; }
        public string? UserRequestStatusName { get; set; }
        public int UserRequestTypeId { get; set; }
        public string? UserRequestType { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class UserRequestDTO
    {
        public string? Word { get; set; }
        public string? RequestDefinitionOrDescription { get; set; }
        public int UserRequestTypeId { get; set; }
        public string? CreatedBy { get; set; }
    }

}
