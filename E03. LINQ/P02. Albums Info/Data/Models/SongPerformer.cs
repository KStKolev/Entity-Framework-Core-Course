using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        public int SongId { get; set; }

        [Required]
        public virtual Song Song { get; set; } = null!;

        public int PerformerId { get; set; }

        [Required]
        public virtual Performer Performer { get; set; } = null!;
    }
}
