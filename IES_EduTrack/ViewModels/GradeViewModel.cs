#nullable disable


using IES_EduTrack.Helpers;
using IES_EduTrack.Models;
using IES_EduTrack.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IES_EduTrack.ViewModels
{
    /// <summary>
    /// Manages the grade view — add and remove grade entries,
    /// view a student's grade history, and generate grade reports.
    /// </summary>
    public class GradeViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private readonly StudentService _studentService;

        private ObservableCollection<Student> _students;
        private ObservableCollection<GradeEntry> _entries;

        private Student _selectedStudent;
        private GradeEntry _selectedEntry;
        private string _subjectId;
        private string _grade;
        private DateTime _gradeDate;

        // Report fields
        private string _reportSubjectId;
        private string _reportPeriod;
        private string _reportOutput;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }

        public ObservableCollection<GradeEntry> Entries
        {
            get { return _entries; }
            set { _entries = value; OnPropertyChanged(nameof(Entries)); }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        public GradeEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set { _selectedEntry = value; OnPropertyChanged(nameof(SelectedEntry)); }
        }

        public string SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; OnPropertyChanged(nameof(SubjectId)); }
        }

        // Grade value — Swedish A–F scale, validated before saving
        public string Grade
        {
            get { return _grade; }
            set { _grade = value; OnPropertyChanged(nameof(Grade)); }
        }

        public DateTime GradeDate
        {
            get { return _gradeDate; }
            set { _gradeDate = value; OnPropertyChanged(nameof(GradeDate)); }
        }

        public string ReportSubjectId
        {
            get { return _reportSubjectId; }
            set { _reportSubjectId = value; OnPropertyChanged(nameof(ReportSubjectId)); }
        }

        public string ReportPeriod
        {
            get { return _reportPeriod; }
            set { _reportPeriod = value; OnPropertyChanged(nameof(ReportPeriod)); }
        }

        // Bound to a read-only TextBox — displays generated report text
        public string ReportOutput
        {
            get { return _reportOutput; }
            set { _reportOutput = value; OnPropertyChanged(nameof(ReportOutput)); }
        }

        // Valid Swedish grade values — used for ComboBox binding
        public string[] GradeOptions { get; } = { "A", "B", "C", "D", "E", "F" };

        public ICommand AddEntryCommand { get; }
        public ICommand RemoveEntryCommand { get; }
        public ICommand LoadEntriesCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ClearFormCommand { get; }

        // Constructor — sets today as default date, loads students and all entries
        public GradeViewModel(GradeService gradeService, StudentService studentService)
        {
            _gradeService = gradeService;
            _studentService = studentService;
            _students = new ObservableCollection<Student>();
            _entries = new ObservableCollection<GradeEntry>();
            _gradeDate = DateTime.Today;

            AddEntryCommand = new RelayCommand(_ => ExecuteAddEntry(), _ => CanExecuteAdd());
            RemoveEntryCommand = new RelayCommand(_ => ExecuteRemoveEntry(), _ => CanExecuteRemove());
            LoadEntriesCommand = new RelayCommand(_ => ExecuteLoadEntries());
            GenerateReportCommand = new RelayCommand(_ => ExecuteGenerateReport(), _ => CanExecuteReport());
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            LoadStudents();
            LoadAllEntries();
        }

        // Loads the student list from StudentService into the ObservableCollection
        private void LoadStudents()
        {
            Students.Clear();

            foreach (Student student in _studentService.GetAllStudents())
            {
                Students.Add(student);
            }
        }

        // Loads all grade entries into the list view
        private void LoadAllEntries()
        {
            Entries.Clear();

            foreach (GradeEntry entry in _gradeService.GetAllEntries())
            {
                Entries.Add(entry);
            }
        }

        // Filters the entry list to show only the selected student's grades
        private void ExecuteLoadEntries()
        {
            if (_selectedStudent == null)
            {
                LoadAllEntries();
                return;
            }

            Entries.Clear();

            foreach (GradeEntry entry in _gradeService.GetEntriesByStudent(_selectedStudent.PersonId))
            {
                Entries.Add(entry);
            }
        }

        // Creates and saves a new grade entry from form input
        private void ExecuteAddEntry()
        {
            GradeEntry newEntry = new GradeEntry
            {
                EntryId = Guid.NewGuid().ToString(),
                StudentId = _selectedStudent.PersonId,
                SubjectId = _subjectId.Trim(),
                Grade = _grade,
                Date = _gradeDate
            };

            _gradeService.AddEntry(newEntry);
            LoadAllEntries();
            ClearForm();
        }

        // Removes the selected grade entry — correct path since GradeEntry is immutable
        private void ExecuteRemoveEntry()
        {
            _gradeService.RemoveEntry(_selectedEntry.EntryId);
            LoadAllEntries();
            SelectedEntry = null;
        }

        // Generates a grade report for a subject+period and writes it to ReportOutput
        private void ExecuteGenerateReport()
        {
            GradeReport report = _gradeService.GenerateReportForSubject(
                _reportSubjectId.Trim(),
                _reportPeriod.Trim());

            ReportOutput = report.GenerateReport()
                + System.Environment.NewLine
                + report.GetGradeDistribution();
        }

        // Add requires a selected student, a subject, and a grade value
        private bool CanExecuteAdd()
        {
            return _selectedStudent != null
                && !string.IsNullOrWhiteSpace(_subjectId)
                && !string.IsNullOrWhiteSpace(_grade);
        }

        // Remove requires a selected entry
        private bool CanExecuteRemove()
        {
            return _selectedEntry != null;
        }

        // Report requires both subject and period fields
        private bool CanExecuteReport()
        {
            return !string.IsNullOrWhiteSpace(_reportSubjectId)
                && !string.IsNullOrWhiteSpace(_reportPeriod);
        }

        // Resets form inputs — keeps student selection for quick multi-entry
        private void ClearForm()
        {
            SubjectId = string.Empty;
            Grade = string.Empty;
            GradeDate = DateTime.Today;
            ReportOutput = string.Empty;
        }
    }
}