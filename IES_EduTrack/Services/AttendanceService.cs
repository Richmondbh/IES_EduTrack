#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using IES_EduTrack.Models;

namespace IES_EduTrack.Services
{
    /// <summary>
    /// Manages attendance records and produces AttendanceReport objects.
    /// Reports are grouped by subject — matching AttendanceReport's SubjectId/Period shape.
    /// </summary>
    public class AttendanceService
    {
        private const string AttendanceFileName = "attendance.json";

        private readonly FileService _fileService;
        private List<AttendanceRecord> _records;

        // Constructor — loads existing records from disk on startup
        public AttendanceService(FileService fileService)
        {
            _fileService = fileService;
            _records = new List<AttendanceRecord>();
            LoadFromFile();
        }

        // Returns all records as a read-only list
        public IReadOnlyList<AttendanceRecord> GetAllRecords()
        {
            return _records.AsReadOnly();
        }

        // Returns all records for a given student
        public List<AttendanceRecord> GetRecordsByStudent(string studentId)
        {
            return (from record in _records
                    where record.StudentId == studentId
                    select record).ToList();
        }

        // Returns all records for a given subject
        public List<AttendanceRecord> GetRecordsBySubject(string subjectId)
        {
            return (from record in _records
                    where record.SubjectId == subjectId
                    select record).ToList();
        }

        // Returns all records on a given date — time component is ignored
        public List<AttendanceRecord> GetRecordsByDate(DateTime date)
        {
            return (from record in _records
                    where record.Date.Date == date.Date
                    select record).ToList();
        }

        // Adds a record — rejects null, returns false if rejected
        public bool AddRecord(AttendanceRecord record)
        {
            if (record == null)
            {
                return false;
            }

            _records.Add(record);
            SaveToFile();
            return true;
        }

        // Builds an AttendanceReport for a subject and period using object initialiser
        public AttendanceReport GenerateReportForSubject(string subjectId, string period)
        {
            List<AttendanceRecord> subjectRecords = GetRecordsBySubject(subjectId);

            AttendanceReport report = new AttendanceReport
            {
                SubjectId = subjectId,
                Period = period,
                Records = subjectRecords
            };

            return report;
        }

        // Returns attendance percentage for a student — 0 if no records exist
        public double GetAttendancePercentage(string studentId)
        {
            List<AttendanceRecord> studentRecords = GetRecordsByStudent(studentId);

            if (studentRecords.Count == 0)
            {
                return 0;
            }

            int presentCount = (from record in studentRecords
                                where record.IsPresent
                                select record).Count();

            return (double)presentCount / studentRecords.Count * 100;
        }

        // Writes the current record list to disk
        private void SaveToFile()
        {
            _fileService.Save(AttendanceFileName, _records);
        }

        // Loads from disk — keeps empty list if file does not exist yet
        private void LoadFromFile()
        {
            List<AttendanceRecord> loaded = _fileService.Load<List<AttendanceRecord>>(AttendanceFileName);

            if (loaded != null)
            {
                _records = loaded;
            }
        }
    }
}