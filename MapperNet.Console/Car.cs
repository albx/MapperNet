using MapperNet.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapperNet.ConsoleApp
{
    [Table(Name="automobile")]
    class Car
    {
        [Column(Name="id", IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column(Name="modello")]
        public string Modello { get; set; }

        [Column(Name="cilindrata")]
        public string Cilindrata { get; set; }

        [Column(Name="porte")]
        public int Porte { get; set; }

        [Column(Name="consumo_medio")]
        public string ConsumoMedio { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}, {3} porte, {4}km/l", Id, Modello, Cilindrata, Porte, ConsumoMedio);
        }
    }
}
