using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DependencyCheck.Models
{
    class UserDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long id { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 1, IsUnique = true)]
        public string username { get; set; }
        [MaxLength(128)]
        public string nickname { get; set; }

        public UserDB()
        {

        }
        public UserDB(long id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}