using System;
using System.Collections.Generic;

namespace SchoolManagement.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GradeLevel { get; set; }

    public string AcademicYear { get; set; } = null!;

    public int? HomeroomTeacherId { get; set; }

    public virtual Teacher? HomeroomTeacher { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
