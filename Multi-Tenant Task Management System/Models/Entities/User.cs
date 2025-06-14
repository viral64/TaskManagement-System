namespace Multi_Tenant_Task_Management_System.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<TaskEntity> AssignedTasks { get; set; }
        public ICollection<TaskComment> Comments { get; set; }

        public string Role { get; set; } // e.g., Admin, Manager, User
    }
}
