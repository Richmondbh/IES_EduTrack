using System;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Immutable record of a grade awarded to a student for a subject.
    /// Grade values follow the Swedish A–F scale stored as a string.
    /// </summary>
    public record GradeEntry
    {
        public string EntryId { get; init; }
        public string StudentId { get; init; }
        public string SubjectId { get; init; }
        public string Grade { get; init; }
        public DateTime Date { get; init; }
    }
}