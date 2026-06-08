# IES EduTrack

A WPF desktop application for managing student records, class attendance, and subject grades at Engelska Skolan Östersund. Built as Assignment 6 for Programming in C# II at Malmö University.

---

## Screenshots

| Students | Attendance | Grades |
|----------|------------|--------|
| ![Students](screenshots/student_view.png) | ![Attendance](screenshots/attendance_view.png) | ![Grades](screenshots/grade_view.png) |

---

## About

IES EduTrack solves a usability problem for staff who find the main school platform slow for daily tasks. It provides a fast, focused local tool for three core workflows: student records, attendance tracking, and grade management  without replacing any existing system.

---

## Features

### Students
- Add, update, and remove student records
- Search students by name in real time
- Filter by class group and active status
- Student status tracked as Active, Inactive, or Withdrawn

### Attendance
- Record attendance events per student and subject
- Filter records by student
- Calculate attendance percentage per student
- Generate attendance reports by subject and period

### Grades
- Add and remove grade entries (Swedish A–F scale)
- View grade history per student
- Filter entries by student
- Generate grade reports with full A–F distribution by subject and period

---

## Tech Stack

| Area | Technology |
|------|------------|
| UI Framework | WPF (Windows Presentation Foundation) |
| Language | C# (.NET 9) |
| Architecture | MVVM (Model-View-ViewModel) |
| Persistence | JSON via Newtonsoft.Json |
| IDE | Visual Studio 2022 |

---

## Architecture

The project follows MVVM throughout with strict separation of concerns.

```
IES_EduTrack/
├── Helpers/
│   └── RelayCommand.cs
├── Interfaces/
│   └── IReportable.cs
├── Models/
│   ├── Person.cs
│   ├── Student.cs
│   ├── Staff.cs
│   ├── Subject.cs
│   ├── AttendanceRecord.cs
│   ├── AttendanceReport.cs
│   ├── GradeEntry.cs
│   └── GradeReport.cs
├── Services/
│   ├── FileService.cs
│   ├── StudentService.cs
│   ├── AttendanceService.cs
│   └── GradeService.cs
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── MainViewModel.cs
│   ├── StudentViewModel.cs
│   ├── AttendanceViewModel.cs
│   └── GradeViewModel.cs
└── Views/
    ├── MainWindow.xaml
    ├── StudentView.xaml
    ├── AttendanceView.xaml
    └── GradeView.xaml
```

### Key design decisions

- `MainViewModel` owns all three service instances — child ViewModels receive services by constructor injection and never create them directly
- `FileService` is generic — a single `Save<T>` / `Load<T>` pair handles all JSON persistence with `TypeNameHandling.Auto` for polymorphic types
- `AttendanceRecord` and `GradeEntry` are C# `record` types — immutable by design
- `Person` is an abstract base class; `Student` and `Staff` both inherit from it
- `AttendanceReport` and `GradeReport` implement `IReportable`, satisfying the dynamic binding requirement
- `Subject.EnrolledStudents` is marked `[JsonIgnore]` to prevent circular reference during serialization — the list is rebuilt at runtime
- Data is persisted to a local `EduTrackData/` folder next to the executable

---

## Reused Infrastructure

The following components are carried from previous assignments and documented on GitHub. All domain models, business logic, and views are written from scratch for this project.

| Component | Origin |
|-----------|--------|
| `RelayCommand.cs` | Assignment 4, Assignment 5 |
| `BaseViewModel.cs` | Assignment 5 (CashFlowManager) |
| `FileService` pattern | Assignment 5 (CashFlowManager) |
| Service / interface separation |

---



## Related Repositories


- [CashFlowManager](https://github.com/Richmondbh/CashFlowManager) — WPF, MVVM, Newtonsoft.Json, FileService pattern

---

*Assignment 6 — Programming in C# II — Malmö University*

