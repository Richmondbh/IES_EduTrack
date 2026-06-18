#nullable disable


using IES_EduTrack.Helpers;
using IES_EduTrack.Models;
using IES_EduTrack.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IES_EduTrack.ViewModels
{
    /// <summary>
    /// MainModel — owns all service instances and coordinates
    /// navigation between the three main views via CurrentViewModel.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly FileService _fileService;
        private readonly StudentService _studentService;
        private readonly AttendanceService _attendanceService;
        private readonly GradeService _gradeService;

        private BaseViewModel _currentViewModel;

        // The active child ViewModel — View binds ContentControl to this
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        // Navigation commands — bound to sidebar/menu buttons in MainWindow
        public ICommand ShowStudentsCommand { get; }
        public ICommand ShowAttendanceCommand { get; }
        public ICommand ShowGradesCommand { get; }

        // creates all services here; they are never created elsewhere
        public MainViewModel()
        {
            _fileService = new FileService();
            _studentService = new StudentService(_fileService);
            _attendanceService = new AttendanceService(_fileService);
            _gradeService = new GradeService(_fileService);

            ShowStudentsCommand = new RelayCommand(_ => NavigateToStudents());
            ShowAttendanceCommand = new RelayCommand(_ => NavigateToAttendance());
            ShowGradesCommand = new RelayCommand(_ => NavigateToGrades());

            // Start on the student view
            NavigateToStudents();
        }

        // Switches the active view to StudentViewModel
        private void NavigateToStudents()
        {
            CurrentViewModel = new StudentViewModel(_studentService);
        }

        // Switches the active view to AttendanceViewModel
        private void NavigateToAttendance()
        {
            CurrentViewModel = new AttendanceViewModel(_attendanceService, _studentService);
        }

        // Switches the active view to GradeViewModel
        private void NavigateToGrades()
        {
            CurrentViewModel = new GradeViewModel(_gradeService, _studentService);
        }
    }
}