using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAlertAPI.Models
{
    [Table("dica")]
    public class Dica
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("nivel_risco")]
        public string? NivelRisco { get; set; }

        [Column("mensagem")]
        public string? Mensagem { get; set; }
    }
}

