namespace Multi_Tenant_Task_Management_System.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }

        public int TaskEntityId { get; set; }
        public TaskEntity TaskEntity { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
