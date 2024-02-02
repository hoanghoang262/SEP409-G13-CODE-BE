using System;
using System.Collections.Generic;

namespace User.gRPC.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePict { get; set; }

    public bool? Status { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
