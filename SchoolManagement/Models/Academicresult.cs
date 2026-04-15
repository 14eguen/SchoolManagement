using System;
using System.Collections.Generic;

namespace SchoolManagement.Models;

public partial class Academicresult
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int Semester { get; set; }

    public string AcademicYear { get; set; } = null!;

    public float? AverageScore { get; set; }

    public string? Classification { get; set; }

    public string? Conduct { get; set; }

    public virtual Student Student { get; set; } = null!;
}
