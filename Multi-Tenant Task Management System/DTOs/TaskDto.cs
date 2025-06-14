namespace Multi_Tenant_Task_Management_System.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public string AssignedTo { get; set; }   // 👈 Must match this!
        public string ProjectName { get; set; }  // 👈 And this!

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
