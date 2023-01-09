using System;
using System.Collections.Generic;

namespace Lesson3A.Models;

public partial class Subject
{
    public long Id { get; set; }

    public string? SubCode { get; set; }

    public string? SubName { get; set; }

    public string? Program { get; set; }

    public byte? Credits { get; set; }

    public string? Prerequisite { get; set; }

    public string? Parrellel { get; set; }

    public string? ClOs { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public string? References { get; set; }
}
