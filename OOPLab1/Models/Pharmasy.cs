using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class Pharmasy
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<PillsAndPharmasy> PillsAndPharmasies { get; } = new List<PillsAndPharmasy>();
}
