﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DS_Wortschatz.Models;

public partial class Worter
{
    public int Idw { get; set; }

    public int DartikelId { get; set; }

    public string Deutsch { get; set; }

    public string Serbisch { get; set; }

    public int SartikelId { get; set; }

    public virtual ArtikelD Dartikel { get; set; }

    public virtual Artikel Sartikel { get; set; }
}