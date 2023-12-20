using Practico.Types;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Practico.Models
{
    public class Instancia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Nombre debe tener entre 4 y 20 caracteres.")]
        public string Nombre { get; set; }
		[Required]
		public string URL { get; set; }
        [Required]
        public Tematica Tematica { get; set; }
        [Required]
        [DisplayName("Paleta de Colores")]
        public Paleta PaletaColor { get; set; }
        [Required]
        [DisplayName("Tipo de Registro")]
        public TipoRegistro TipoRegistro { get; set; }
        public bool Estado { get; set; }
    }
}
