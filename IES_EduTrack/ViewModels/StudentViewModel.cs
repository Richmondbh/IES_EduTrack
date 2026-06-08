#nullable disable
using IES_EduTrack.Helpers;
using IES_EduTrack.Models;
using IES_EduTrack.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IES_EduTrack.ViewModels
{
    /// <summary>
    /// Manages the student list view — add, update, remove, and search students.
    /// All data operations go through StudentService; this ViewModel never touches the list directly.
    /// </summary>
    public class StudentViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;

        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private string _searchTerm;

        // Input fields for add/edit form
        private string _inputName;
        private string _inputEmail;
        private string _inputClassGroup;
        private StudentStatus _inputStatus;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                PopulateInputFields();
            }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                ExecuteSearch();
            }
        }

        public string InputName
        {
            get { return _inputName; }
            set { _inputName = value; OnPropertyChanged(nameof(InputName)); }
        }

        public string InputEmail
        {
            get { return _inputEmail; }
            set { _inputEmail = value; OnPropertyChanged(nameof(InputEmail)); }
        }

        public string InputClassGroup
        {
            get { return _inputClassGroup; }
            set { _inputClassGroup = value; OnPropertyChanged(nameof(InputClassGroup)); }
        }

        public StudentStatus InputStatus
        {
            get { return _inputStatus; }
            set { _inputStatus = value; OnPropertyChanged(nameof(InputStatus)); }
        }

        // Status values for ComboBox — sourced from enum
        public StudentStatus[] StatusOptions { get; } =
        {
            StudentStatus.Active,
            StudentStatus.Inactive,
            StudentStatus.Withdrawn
        };

        public ICommand AddStudentCommand { get; }
        public ICommand UpdateStudentCommand { get; }
        public ICommand RemoveStudentCommand { get; }
        public ICommand ClearFormCommand { get; }

        // Constructor — loads the initial student list from service
        public StudentViewModel(StudentService studentService)
        {
            _studentService = studentService;
            _students = new ObservableCollection<Student>();

            AddStudentCommand = new RelayCommand(_ => ExecuteAddStudent(), _ => CanExecuteAdd());
            UpdateStudentCommand = new RelayCommand(_ => ExecuteUpdateStudent(), _ => CanExecuteUpdate());
            RemoveStudentCommand = new RelayCommand(_ => ExecuteRemoveStudent(), _ => CanExecuteUpdate());
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            LoadStudents();
        }

        // Refreshes the ObservableCollection from the service
        private void LoadStudents()
        {
            Students.Clear();

            foreach (Student student in _studentService.GetAllStudents())
            {
                Students.Add(student);
            }
        }

        // Runs a name search and refreshes the list with results
        private void ExecuteSearch()
        {
            Students.Clear();

            foreach (Student student in _studentService.SearchStudentsByName(_searchTerm))
            {
                Students.Add(student);
            }
        }

        // Populates form fields when a student is selected in the list
        private void PopulateInputFields()
        {
            if (_selectedStudent == null)
            {
                return;
            }

            InputName = _selectedStudent.Name;
            InputEmail = _selectedStudent.Email;
            InputClassGroup = _selectedStudent.ClassGroup;
            InputStatus = _selectedStudent.Status;
        }

        // Creates a new Student from form input and passes it to the service
        private void ExecuteAddStudent()
        {
            Student newStudent = new Student
            {
                PersonId = System.Guid.NewGuid().ToString(),
                Name = _inputName.Trim(),
                Email = _inputEmail?.Trim(),
                ClassGroup = _inputClassGroup?.Trim(),
                Status = _inputStatus,
                EnrollmentDate = System.DateTime.Today
            };

            _studentService.AddStudent(newStudent);
            LoadStudents();
            ClearForm();
        }

        // Applies form input changes to the selected student
        private void ExecuteUpdateStudent()
        {
            // Copy PersonId from selected — it must not change
            Student updated = new Student
            {
                PersonId = _selectedStudent.PersonId,
                Name = _inputName.Trim(),
                Email = _inputEmail?.Trim(),
                ClassGroup = _inputClassGroup?.Trim(),
                Status = _inputStatus,
                EnrollmentDate = _selectedStudent.EnrollmentDate
            };

            _studentService.UpdateStudent(updated);
            LoadStudents();
            ClearForm();
        }

        // Removes the selected student from the service and refreshes the list
        private void ExecuteRemoveStudent()
        {
            _studentService.RemoveStudent(_selectedStudent.PersonId);
            LoadStudents();
            ClearForm();
        }

        // Add is valid when name and class group are filled
        private bool CanExecuteAdd()
        {
            return !string.IsNullOrWhiteSpace(_inputName)
                && !string.IsNullOrWhiteSpace(_inputClassGroup);
        }

        // Update and remove require a selection
        private bool CanExecuteUpdate()
        {
            return _selectedStudent != null;
        }

        // Resets all form input fields and clears selection
        private void ClearForm()
        {
            SelectedStudent = null;
            InputName = string.Empty;
            InputEmail = string.Empty;
            InputClassGroup = string.Empty;
            InputStatus = StudentStatus.Active;
        }
    }
}