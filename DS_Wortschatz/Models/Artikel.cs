﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DS_Wortschatz.Models;

public partial class Artikel
{
    public int IdS { get; set; }

    public string TajTaTo { get; set; }

    public virtual ICollection<Worter> Worters { get; set; } = new List<Worter>();
}