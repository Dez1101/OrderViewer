namespace OrderViewer.API.DTOs
{
    public class FilterOrdersDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string[]? Statuses { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }

        // Sorting
        public string? SortBy { get; set; } // e.g. "CustomerName", "CreatedDate", "Total"
        public string? SortDirection { get; set; } = "asc"; // "asc" or "desc"
    }
}
