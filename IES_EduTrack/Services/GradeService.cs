#nullable disable

using System.Collections.Generic;
using System.Linq;
using IES_EduTrack.Models;

namespace IES_EduTrack.Services
{
    /// <summary>
    /// Manages grade entries and produces GradeReport objects.
    /// Grades follow the Swedish A–F scale stored as strings on GradeEntry.
    /// </summary>
    public class GradeService
    {
        private const string GradesFileName = "grades.json";

        private readonly FileService _fileService;
        private List<GradeEntry> _entries;

        // loads existing entries from disk on startup
        public GradeService(FileService fileService)
        {
            _fileService = fileService;
            _entries = new List<GradeEntry>();
            LoadFromFile();
        }

        // Returns all grade entries as a read-only list
        public IReadOnlyList<GradeEntry> GetAllEntries()
        {
            return _entries.AsReadOnly();
        }

        // Returns all entries for a given student
        public List<GradeEntry> GetEntriesByStudent(string studentId)
        {
            return (from entry in _entries
                    where entry.StudentId == studentId
                    select entry).ToList();
        }

        // Returns all entries for a given subject
        public List<GradeEntry> GetEntriesBySubject(string subjectId)
        {
            return (from entry in _entries
                    where entry.SubjectId == subjectId
                    select entry).ToList();
        }

        // Returns entries for a specific student in a specific subject
        public List<GradeEntry> GetEntriesByStudentAndSubject(string studentId, string subjectId)
        {
            return (from entry in _entries
                    where entry.StudentId == studentId
                    && entry.SubjectId == subjectId
                    select entry).ToList();
        }

        // Adds a grade entry rejects null and returns false if rejected
        // GradeEntry is an immutable record so no update path exists, I used RemoveEntry + AddEntry
        public bool AddEntry(GradeEntry entry)
        {
            if (entry == null)
            {
                return false;
            }

            _entries.Add(entry);
            SaveToFile();
            return true;
        }

        // Removes an entry by EntryId — correct mutation path for immutable records
        public bool RemoveEntry(string entryId)
        {
            GradeEntry target = (from entry in _entries
                                 where entry.EntryId == entryId
                                 select entry).FirstOrDefault();

            if (target == null)
            {
                return false;
            }

            _entries.Remove(target);
            SaveToFile();
            return true;
        }

        // Builds a GradeReport for a subject and period using object initialiser
        public GradeReport GenerateReportForSubject(string subjectId, string period)
        {
            List<GradeEntry> subjectEntries = GetEntriesBySubject(subjectId);

            GradeReport report = new GradeReport
            {
                SubjectId = subjectId,
                Period = period,
                Entries = subjectEntries
            };

            return report;
        }

        // Returns the most recent grade entry for a student in a subject . null if none is available
        public GradeEntry GetLatestGrade(string studentId, string subjectId)
        {
            return (from entry in _entries
                    where entry.StudentId == studentId
                    && entry.SubjectId == subjectId
                    orderby entry.Date descending
                    select entry).FirstOrDefault();
        }

        // Writes the current entry list to disk
        private void SaveToFile()
        {
            _fileService.Save(GradesFileName, _entries);
        }

        // Loads from disk  and keeps empty list if file does not exist yet
        private void LoadFromFile()
        {
            List<GradeEntry> loaded = _fileService.Load<List<GradeEntry>>(GradesFileName);

            if (loaded != null)
            {
                _entries = loaded;
            }
        }
    }
}