#nullable disable

using System.Collections.Generic;
using System.Linq;
using IES_EduTrack.Models;

namespace IES_EduTrack.Services
{
    /// <summary>
    /// This class manages the in-memory student list and persists it via FileService.
    /// Owns the list so that ViewModels never hold or modify student data directly.
    /// </summary>
    public class StudentService
    {
        private const string StudentsFileName = "students.json";

        private readonly FileService _fileService;
        private List<Student> _students;

        // Constructor — loads existing data from disk on startup
        public StudentService(FileService fileService)
        {
            _fileService = fileService;
            _students = new List<Student>();
            LoadFromFile();
        }

        // Returns all students as a read-only list  so callers cannot modify the list
        public IReadOnlyList<Student> GetAllStudents()
        {
            return _students.AsReadOnly();
        }

        // Finds a student by PersonId and returns null if not found
        public Student GetStudentById(string personId)
        {
            return (from student in _students
                    where student.PersonId == personId
                    select student).FirstOrDefault();
        }

        // Returns all students in a given class group fo4r example "9A" osv
        public List<Student> GetStudentsByClassGroup(string classGroup)
        {
            return (from student in _students
                    where student.ClassGroup == classGroup
                    select student).ToList();
        }

        // Returns only students whose status is Active
        public List<Student> GetActiveStudents()
        {
            return (from student in _students
                    where student.IsActive()
                    select student).ToList();
        }

        // Searches by display name and is case-insensitive. It returns all if term is empty.
        public List<Student> SearchStudentsByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _students.ToList();
            }

            //wants to make all teext lower
            string term = searchTerm.ToLower();

            return (from student in _students
                    where student.GetDisplayName().ToLower().Contains(term)
                    select student).ToList();
        }

        // Method which Adds a student and  rejects null or duplicate PersonId, returns false if rejected
        public bool AddStudent(Student student)
        {
            if (student == null)
            {
                return false;
            }

            bool alreadyExists = (from s in _students
                                  where s.PersonId == student.PersonId
                                  select s).Any();

            if (alreadyExists)
            {
                return false;
            }

            _students.Add(student);
            SaveToFile();
            return true;
        }

        // Updates a student matched by PersonId  and  returns false if not found
        public bool UpdateStudent(Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return false;
            }

            int index = _students.FindIndex(s => s.PersonId == updatedStudent.PersonId);

            if (index < 0)
            {
                return false;
            }

            _students[index] = updatedStudent;
            SaveToFile();
            return true;
        }

        // Removes a student by PersonId and returns false if not found
        public bool RemoveStudent(string personId)
        {
            Student target = GetStudentById(personId);

            if (target == null)
            {
                return false;
            }

            _students.Remove(target);
            SaveToFile();
            return true;
        }

        // Returns total student count
        public int GetStudentCount()
        {
            return _students.Count;
        }

        // Writes the current list to disk
        private void SaveToFile()
        {
            _fileService.Save(StudentsFileName, _students);
        }

        // Loads from disk — keeps empty list if file does not exist yet
        private void LoadFromFile()
        {
            List<Student> loaded = _fileService.Load<List<Student>>(StudentsFileName);

            if (loaded != null)
            {
                _students = loaded;
            }
        }
    }
}