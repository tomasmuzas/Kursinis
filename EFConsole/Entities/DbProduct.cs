﻿using System.ComponentModel.DataAnnotations;

namespace MigrationDemo.Entities
{
    public class DbProduct
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Isin { get; set; }
    }
}