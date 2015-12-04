using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class UsuarioAtividade
    {
        
        //This property has autoincrementation
        public int IdUsuarioAtividade { get; set; }
        public int IdUsuario { get; set; }
        public int IdAtividade { get; set; }
        public UsuarioAtividade(int idUsuario, int idAtividade)
        {
            this.IdUsuario = idUsuario;
            this.IdAtividade = IdAtividade;
        }
    }
}
