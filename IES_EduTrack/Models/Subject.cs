#nullable disable

using Newtonsoft.Json;
using System.Collections.Generic;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Represents a school subject with an assigned teacher.
    /// EnrolledStudents is excluded from JSON to avoid circular reference with Student.
    /// </summary>
    public class Subject
    {
        private string _subjectId;
        private string _name;
        private string _teacherId;
        private int _credits;
        private List<Student> _enrolledStudents;

        public string SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        // Stores the id only — full Staff object resolved at runtime
        public string TeacherId
        {
            get { return _teacherId; }
            set { _teacherId = value; }
        }

        public int Credits
        {
            get { return _credits; }  
            set { _credits = value; }
        }

        [JsonIgnore]
        public List<Student> EnrolledStudents
        {
            get { return _enrolledStudents; }
            set { _enrolledStudents = value; }
        }

        // Display label for ComboBox and ListBox bindings
        public override string ToString()
        {
            return $"{_name} ({_subjectId})";
        }
    }
}