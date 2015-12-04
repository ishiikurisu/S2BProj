using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    //Bla
    public class Usuario
    {
        
        //This property has autoincrementation
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public Usuario(int idUsuario, string nome, string senha, string email)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
            this.Senha = senha;
            this.Email = email;
        }
        //Temporary implementation - Validate if Usuario already exists
        public override bool Equals(object obj)
        {
            Usuario elem = (Usuario)obj;
            return elem.Email.ToUpper().Equals(Email.ToUpper());
        }
    }
}
