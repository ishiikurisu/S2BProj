using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class Usuario
    {
        public Usuario(int idUsuario, string nome, string senha, string email)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
            this.Senha = senha;
            this.Email = email;
        }
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }

        //Temporary implementation - Validate if Usuario already exists
        public override bool Equals(object obj)
        {
            Usuario elem = (Usuario)obj;
            return elem.Email.ToUpper().Equals(Email.ToUpper());
        }
    }
}
