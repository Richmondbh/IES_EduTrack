#nullable disable
namespace IES_EduTrack.Models
{
    /// <summary>
    /// Represents a staff member such as a teacher or school coordinator.
    /// It also inherits shared identity fields from Person.
    /// </summary>
    public class Staff : Person
    {
        private StaffRole _role;
        private string _department;

        public StaffRole Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        // Returns the role as a readable string for display
        public string GetRole()
        {
            return _role.ToString();
        }

        // Teachers and coordinators can manage grades but admins cannot
        public bool CanManageGrades()
        {
            return _role == StaffRole.Teacher || _role == StaffRole.Coordinator;
        }
    }

    
}