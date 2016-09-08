using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mtg.Models.CardModels
{
    public class ForeignName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string language { get; set; }
        public string name { get; set; }
        public int multiverseid { get; set; }
    }
}