using System;
using System.Collections.Generic;

namespace Center.Models;

public partial class IssuingMagazine
{
    public int Id { get; set; }

    public int MagazinId { get; set; }

    public int WorkerId { get; set; }

    public DateOnly Date { get; set; }

    public int Count { get; set; }

    public virtual Magazin Magazin { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
