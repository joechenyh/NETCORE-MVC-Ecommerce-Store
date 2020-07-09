using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.Models
{
    public class Review
    {
        [MaxLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public DateTime CreationTime { get; set; }

        [MaxLength(36)]
        public string ProductId { get; set; }

        [MaxLength(36)]
        public string CustomerId { get; set; }

        [MaxLength(36)]
        public int Stars { get; set; }

        [MaxLength(360)]
        public string Comments { get; set; }

        public virtual Customer customer { get; set; }
        public virtual Product product { get; set; }


    }
}
