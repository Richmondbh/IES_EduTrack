#nullable disable
using System;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Represents a student enrolled at Engelska Skolan Östersund.
    /// Inherits shared identity fields from Person.
    /// </summary>
    public class Student : Person
    {
        private string _classGroup;
        private DateTime _enrollmentDate;
        private StudentStatus _status;

        public string ClassGroup
        {
            get { return _classGroup; }
            set { _classGroup = value; }
        }

        public DateTime EnrollmentDate
        {
            get { return _enrollmentDate; }
            set { _enrollmentDate = value; }
        }

        public StudentStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        // Active means enrolled and not withdrawn
        public bool IsActive()
        {
            return _status == StudentStatus.Active;
        }

        // Summary string used in report views
        public string GetFullRecord()
        {
            return $"{GetDisplayName()} | Class: {_classGroup} | Enrolled: {_enrollmentDate:yyyy-MM-dd} | Status: {_status}";
        }
    }

    /// <summary>
    /// Enrollment status options for a student.
    /// </summary>
    public enum StudentStatus
    {
        Active,
        Inactive,
        Withdrawn
    }
}