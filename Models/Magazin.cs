using System;
using System.Collections.Generic;

namespace Center.Models;

public partial class Magazin
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly CreatinDate { get; set; }

    public int Count { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int CreatorId { get; set; }

    public decimal Price { get; set; }

    public virtual Worker Creator { get; set; } = null!;

    public virtual ICollection<IssuingMagazine> IssuingMagazines { get; set; } = new List<IssuingMagazine>();

    public virtual ICollection<ReceivingMagazine> ReceivingMagazines { get; set; } = new List<ReceivingMagazine>();
}
