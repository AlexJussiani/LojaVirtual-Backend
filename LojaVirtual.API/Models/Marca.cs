﻿using Loja.Core.Data;
using System.Collections.Generic;

namespace LojaVirtual.API.Models
{
    public class Marca : Entity
    {
        public string Nome { get; set; }
        public bool removido { get; set; }

        /* EF Relations */
        public IEnumerable<Produto> Produtos { get; set; }
    }
}
