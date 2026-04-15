using System;
using System.Collections.Generic;

namespace SchoolManagement.Models;

public partial class Timetable
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int SubjectId { get; set; }

    public int TeacherId { get; set; }

    public int Semester { get; set; }

    public int DayOfWeek { get; set; }

    public int Period { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
