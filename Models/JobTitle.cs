using System;
using System.Collections.Generic;

namespace Center.Models;

public partial class JobTitle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
