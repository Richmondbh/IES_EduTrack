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
    /// This manages the attendance view, record attendance events and generate
    /// attendance reports per subject and period.
    /// </summary>
    public class AttendanceViewModel : BaseViewModel
    {
        private readonly AttendanceService _attendanceService;
        private readonly StudentService _studentService;

        private ObservableCollection<Student> _students;
        private ObservableCollection<AttendanceRecord> _records;

        private Student _selectedStudent;
        private AttendanceRecord _selectedRecord;
        private string _subjectId;
        private bool _isPresent;
        private DateTime _selectedDate;

        // Report fields
        private string _reportSubjectId;
        private string _reportPeriod;
        private string _reportOutput;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }

        public ObservableCollection<AttendanceRecord> Records
        {
            get { return _records; }
            set { _records = value; OnPropertyChanged(nameof(Records)); }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        public AttendanceRecord SelectedRecord
        {
            get { return _selectedRecord; }
            set { _selectedRecord = value; OnPropertyChanged(nameof(SelectedRecord)); }
        }

        public string SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; OnPropertyChanged(nameof(SubjectId)); }
        }

        public bool IsPresent
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        

            get { return _isPresent; }
            set { _isPresent = value; OnPropertyChanged(nameof(IsPresent)); }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
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

        // Bound to a readonly TextBox which displays generated report text
        public string ReportOutput
        {
            get { return _reportOutput; }
            set { _reportOutput = value; OnPropertyChanged(nameof(ReportOutput)); }
        }

        public ICommand AddRecordCommand { get; }
        public ICommand LoadRecordsCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ClearFormCommand { get; }

        // sets today as default date, loads students and all records
        public AttendanceViewModel(AttendanceService attendanceService, StudentService studentService)
        {
            _attendanceService = attendanceService;
            _studentService = studentService;
            _students = new ObservableCollection<Student>();
            _records = new ObservableCollection<AttendanceRecord>();
            _selectedDate = DateTime.Today;
            _isPresent = true;

            AddRecordCommand = new RelayCommand(_ => ExecuteAddRecord(), _ => CanExecuteAddRecord());
            LoadRecordsCommand = new RelayCommand(_ => ExecuteLoadRecords());
            GenerateReportCommand = new RelayCommand(_ => ExecuteGenerateReport(), _ => CanExecuteReport());
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            LoadStudents();
            LoadAllRecords();
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

        // Loads all attendance records into the list view
        private void LoadAllRecords()
        {
            Records.Clear();

            foreach (AttendanceRecord record in _attendanceService.GetAllRecords())
            {
                Records.Add(record);
            }
        }

        // Filters the list to show only records for the selected student
        private void ExecuteLoadRecords()
        {
            if (_selectedStudent == null)
            {
                LoadAllRecords();
                return;
            }

            Records.Clear();

            foreach (AttendanceRecord record in _attendanceService.GetRecordsByStudent(_selectedStudent.PersonId))
            {
                Records.Add(record);
            }
        }

        // Creates and saves a new attendance record from form input
        private void ExecuteAddRecord()
        {
            AttendanceRecord newRecord = new AttendanceRecord
            {
                RecordId = Guid.NewGuid().ToString(),
                StudentId = _selectedStudent.PersonId,
                SubjectId = _subjectId.Trim(),
                Date = _selectedDate,
                IsPresent = _isPresent
            };

            _attendanceService.AddRecord(newRecord);
            LoadAllRecords();
            ClearForm();
        }

        // Generates a report for a subject+period and writes it to ReportOutput
        private void ExecuteGenerateReport()
        {
            AttendanceReport report = _attendanceService.GenerateReportForSubject(
                _reportSubjectId.Trim(),
                _reportPeriod.Trim());

            ReportOutput = report.GenerateReport();
        }

        // Add requires a selected student and a subject id
        private bool CanExecuteAddRecord()
        {
            return _selectedStudent != null
           && !string.IsNullOrWhiteSpace(_subjectId);
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
            IsPresent = true;
            SelectedDate = DateTime.Today;
            ReportOutput = string.Empty;
        }
    }
}