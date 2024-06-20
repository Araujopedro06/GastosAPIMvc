using System;

namespace GastosAPIMvc.Models
{
    public class GastoViewModel
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime? Data { get; set; }
        public int CategoriaGastoId { get; set; }  
          
    }
}
