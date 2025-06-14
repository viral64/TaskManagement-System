namespace Multi_Tenant_Task_Management_System.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int AssignedToUserId { get; set; }
        public User AssignedTo { get; set; }

        public ICollection<TaskComment> Comments { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
