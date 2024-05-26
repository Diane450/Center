using System;
using System.Collections.Generic;

namespace Center.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int JobTitleId { get; set; }

    public int DepartmentId { get; set; }

    public int UserId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<IssuingMagazine> IssuingMagazines { get; set; } = new List<IssuingMagazine>();

    public virtual JobTitle JobTitle { get; set; } = null!;

    public virtual ICollection<Magazin> Magazins { get; set; } = new List<Magazin>();

    public virtual ICollection<ReceivingMagazine> ReceivingMagazines { get; set; } = new List<ReceivingMagazine>();

    public virtual User User { get; set; } = null!;
}
