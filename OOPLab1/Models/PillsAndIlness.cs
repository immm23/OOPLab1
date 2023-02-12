using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class PillsAndIlness
{
    public int IllnesId { get; set; }

    public int PillId { get; set; }

    public int Id { get; set; }

    public virtual Ilness Illnes { get; set; } = null!;

    public virtual Pill Pill { get; set; } = null!;
}
