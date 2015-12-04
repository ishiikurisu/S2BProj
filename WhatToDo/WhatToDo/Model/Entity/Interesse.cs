using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class Interesse
    {
        public int IdInteresse { get; set; }
        public int IdUsuario { get; set; }
        public string PalavraChave { get; set; }
        public Interesse(int idUsuario, string palavraChave)
        {
            this.IdUsuario = idUsuario;
            this.PalavraChave = palavraChave;
        }
    }
}
