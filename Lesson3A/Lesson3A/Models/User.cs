using System;
using System.Collections.Generic;

namespace Lesson3A.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime? Lastlogin { get; set; }

    public string? Fullname { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }
}
