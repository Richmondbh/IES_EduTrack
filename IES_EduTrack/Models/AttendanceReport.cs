using IES_EduTrack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IES_EduTrack.Models
{
    public class AttendanceReport : IReportable
    {
        public IReportable IReportable
        {
            get => default;
            set
            {
            }
        }

        public AttendanceRecord AttendanceRecord
        {
            get => default;
            set
            {
            }
        }
    }
}