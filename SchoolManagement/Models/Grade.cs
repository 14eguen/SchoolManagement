using System;
using System.Collections.Generic;

namespace SchoolManagement.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public int Semester { get; set; }

    public float? OralScore { get; set; }

    public float? FifteenMinScore { get; set; }

    public float? FortyFiveMinScore { get; set; }

    public float? FinalScore { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
