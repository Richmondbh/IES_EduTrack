using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IES_EduTrack.Models
{/// <summary>
 /// Represents a school subject taught by a staff member.
 /// </summary>
    public class Subject
    {
        public string SubjectId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Staff? Teacher { get; set; }
        public List<Student> EnrolledStudents { get; set; } = new List<Student>();
    }
}