# Multi-Tenant Task Management System

## ⚙️ Configuration

1. **Update Connection String**

Open `appsettings.json` and replace:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=TaskDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

Run these commands in the terminal:
# Add Migration (if not already created)
dotnet ef migrations add InitialCreate -s Multi_Tenant_Task_Management_System.API

# Apply Migration
dotnet ef database update -s Multi_Tenant_Task_Management_System.API


Full SQL Script
-- Insert Companies
INSERT INTO Companies (Name) VALUES
('OpenAI'),
('TechNova');

-- Insert Users
INSERT INTO Users (FullName, Email, PasswordHash, CompanyId, Role) VALUES
('User1', 'alice@openai.com', 'User1@123', 1, 'Admin'),
('User2', 'bob@openai.com', 'User2@123', 1, 'User'),
('User3', 'charlie@technova.com', 'User3@123', 2, 'Admin'),
('User4', 'diana@technova.com', 'User4@123', 2, 'User');

-- Insert Projects
INSERT INTO Projects (Name, CompanyId, IsDeleted, CreatedAt, CreatedBy, UpdatedAt) VALUES
('OpenAI Portal', 1, 0, GETDATE(), 'User1', NULL),
('OpenAI Mobile', 1, 0, GETDATE(), 'User1', NULL),
('TechNova CRM', 2, 0, GETDATE(), 'User3', NULL),
('TechNova App', 2, 0, GETDATE(), 'User3', NULL);

-- Insert Tasks
INSERT INTO Tasks (Title, Description, ProjectId, AssignedToUserId, IsDeleted, CreatedAt, CreatedBy, UpdatedAt) VALUES
('Design UI', 'Create initial UI layout', 1, 2, 0, GETDATE(), 'User1', NULL),
('Setup DB', 'Design database schema', 1, 1, 0, GETDATE(), 'User2', NULL),
('API Integration', 'Connect backend services', 3, 4, 0, GETDATE(), 'User3', NULL),
('Authentication', 'Add login and registration', 3, 3, 0, GETDATE(), 'User4', NULL);

-- Insert Task Comments
INSERT INTO TaskComments (Comment, TaskEntityId, UserId, CreatedAt) VALUES
('Started working on UI', 1, 2, GETDATE()),
('DB schema completed', 2, 1, GETDATE()),
('API connected successfully', 3, 4, GETDATE()),
('Login feature tested', 4, 3, GETDATE());

