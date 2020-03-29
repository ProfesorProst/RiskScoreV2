using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DependencyCheck.Models
{
    public class DependencyDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 1, IsUnique = true)]
        public string name { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 2, IsUnique = true)]
        public string fileName { get; set; }
        [MaxLength(256)]
        public string filePath { get; set; }
    }
}
