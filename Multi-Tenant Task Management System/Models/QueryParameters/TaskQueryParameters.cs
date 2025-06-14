namespace Multi_Tenant_Task_Management_System.Models.QueryParameters
{
    public class TaskQueryParameters
    {
        public string? Title { get; set; }
        public string? ProjectName { get; set; }

        public string? SortBy { get; set; } = "createdat";
        public bool IsDescending { get; set; } = true;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
