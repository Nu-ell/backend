namespace TechDictionaryApi.DTOs
{
    public class DashboardResponseDTO
    {
        public int DailySearches { get; set; }
        public int PublishedWords { get; set; }
        public int PendingWords { get; set; }
        public List<string>? MostlySearchedWords { get; set; }
        public int ActiveUserRequests { get; set; }
    }

    public class GeneralUserDashboardResponseDTO
    {
        public WordDTO? WordOfTheDay { get; set; }
        public List<WordDTO>? TopSearchedWords { get; set; }
        public List<WordDTO>? RecentlyAddedWords { get; set; }
        public List<WordDTO>? RecentlyUpdatedWords { get; set; }
    }
}
