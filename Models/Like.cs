using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Like
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required int SampleId { get; set; }
        public User User { get; set; }
        public Sample Sample { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}