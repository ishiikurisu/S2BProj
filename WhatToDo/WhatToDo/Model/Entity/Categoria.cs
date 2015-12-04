using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class Categoria
    {
        
        //This property has autoincrementation
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string Icone { get; set; }
        public Categoria(string nome, string icone)
        {
            this.Nome = nome;
            this.Icone = icone;
        }
    }
}
