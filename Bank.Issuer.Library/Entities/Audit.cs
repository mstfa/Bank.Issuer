using System;
using System.ComponentModel.DataAnnotations;
using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Entities
{
    public class Audit:BaseEntity
    {

        public DateTime DateTime { get; set; }
        [Required]
        [MaxLength(128)]
        public String TableName { get; set; }
        [Required]
        [MaxLength(50)]
        public String Action { get; set; }
        public String KeyValues { get; set; }
        public String OldValues { get; set; }
        public String NewValues { get; set; }
    }
}
