using System;
using System.Collections.Generic;

namespace SchoolManagement.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Coefficient { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
