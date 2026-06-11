using System;

namespace IES_EduTrack.Models
{
    /// <summary>
    /// Immutable record of a single attendance event.
    /// Records cannot be changed after creation  use C# record type.
    /// </summary>
    public record AttendanceRecord
    {
        public string RecordId { get; init; }
        public string StudentId { get; init; }
        public string? SubjectId { get; init; }
        public DateTime Date { get; init; }
        public bool IsPresent { get; init; }
    }
}