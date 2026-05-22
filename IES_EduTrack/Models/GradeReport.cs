using IES_EduTrack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IES_EduTrack.Models
{
    public class GradeReport: IReportable
    {
        public IReportable IReportable
        {
            get => default;
            set
            {
            }
        }

        public GradeEntry GradeEntry
        {
            get => default;
            set
            {
            }
        }
    }
}