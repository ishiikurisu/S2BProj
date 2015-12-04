using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class BancoDeDados
    {
        public List<Usuario> Usuarios { get; set; }
        public BancoDeDados(List<Usuario> usuarios)
        {
            this.Usuarios = usuarios;
        }
        public bool Add(Usuario novo)
        {
            if (Usuarios.Contains(novo))
            {
                return false;
            }
            Usuarios.Add(novo);
            return true;
        }
    }
}
