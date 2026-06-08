using System.Collections.Generic;
using System.Linq;
using IES_EduTrack.Interfaces;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Aggregates attendance records for a subject over a period.
    /// Implements IReportable to satisfy the dynamic binding requirement.
    /// </summary>
    public class AttendanceReport : IReportable
    {
        private string _subjectId;
        private string _period;
        private List<AttendanceRecord> _records;

        public string SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; }
        }

        public string Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public List<AttendanceRecord> Records
        {
            get { return _records; }
            set { _records = value; }
        }

        // Percentage of sessions where student was absent
        public double GetAbsenceRate()
        {
            if (_records == null || !_records.Any())
                return 0;

            int absentCount = _records
                .Where(r => !r.IsPresent)
                .Count();

            return (double)absentCount / _records.Count * 100;
        }

        // IReportable — full detail string for report view
        public string GenerateReport()
        {
            return $"Attendance Report | Subject: {_subjectId} | Period: {_period} | Absence Rate: {GetAbsenceRate():F1}%";
        }

        // IReportable — short summary for list display
        public string GetSummary()
        {
            return $"{_subjectId} — {_period} — {_records?.Count ?? 0} records";
        }
    }
}