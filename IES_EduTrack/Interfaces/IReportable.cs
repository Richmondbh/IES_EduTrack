namespace IES_EduTrack.Interfaces
{
    /// <summary>
    /// Contract for any class that can produce a report.
    /// AttendanceReport and GradeReport both implement this,
    /// enabling dynamic binding in the report view.
    /// </summary>
    public interface IReportable
    {
        string GenerateReport();
        string GetSummary();
    }
}