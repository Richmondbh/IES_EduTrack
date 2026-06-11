
using System.Collections.Generic;
using System.Linq;
using IES_EduTrack.Interfaces;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Aggregates grade entries for a subject over a period.
    /// Implements IReportable alongside AttendanceReport for dynamic binding.
    /// </summary>
    public class GradeReport : IReportable
    {
        private string _subjectId;
        private string? _period;
        private List<GradeEntry>? _entries;

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

        public List<GradeEntry> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        // Average is not meaningful for A–F grades here so this returns entry count instead
        // If numeric grades are added later, it replaces it with a real average calculation
        public int GetEntryCount()
        {
            if (_entries == null)
                return 0;

            return _entries.Count();
        }
        // Grade distribution summary for example A=3 B=5 C=2 etc 
        // just extra implementation is added for the grades
        public string GetGradeDistribution()
        {
            if (_entries == null || !_entries.Any())
                return "No entries";

            return string.Join(" ",
            _entries
                    .GroupBy(e => e.Grade)
                    .OrderBy(g => g.Key)
                    .Select(g => $"{g.Key}:{g.Count()}"));

        }
        public string GenerateReport()
        {
            return $"Grade Report | Subject: {_subjectId} | Period: {_period} | Entries: {GetEntryCount()}";
        }

        public string GetSummary()
        {
            return $"{_subjectId} — {_period} — {GetEntryCount()} entries";
        }
    }
}