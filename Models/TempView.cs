using System;
using System.Collections.Generic;

namespace AttendanceApp.Models
{
    public partial class TempView
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
