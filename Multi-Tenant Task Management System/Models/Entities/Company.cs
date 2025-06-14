namespace Multi_Tenant_Task_Management_System.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
