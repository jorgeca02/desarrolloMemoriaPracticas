﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Modelos;

public partial class Mensaje
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Required]
    [Column("texto")]
    [StringLength(240)]
    [Unicode(false)]
    public string Texto { get; set; }

    [Column("idChat")]
    public int IdChat { get; set; }
}