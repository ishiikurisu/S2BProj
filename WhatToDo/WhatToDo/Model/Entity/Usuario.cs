using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class Usuario
    {
        
        //This property has autoincrementation
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public Usuario(string nome, string senha, string email, string perfil)
        {
            this.Nome = nome;
            this.Senha = senha;
            this.Email = email;
            this.Perfil = perfil;
        }

        public Usuario(string nome, string senha, string email)
        {
            this.Nome = nome;
            this.Senha = senha;
            this.Email = email;
        }
        public Usuario(string senha, string email)
        {
            this.Senha = senha;
            this.Email = email;
        }
        public Usuario(string email)
        {
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
