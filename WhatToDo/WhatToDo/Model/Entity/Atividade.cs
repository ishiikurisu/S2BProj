using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model.Entity
{
    public class Atividade
    {
        //This property has autoincrementation
        public int IdAtividade { get; set; }
        public string Nome { get; set; }
        public int IdCategoria { get; set; }
        public string Localizacao { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public Atividade(string nome, int idCategoria, string localizacao, string descricao, DateTime data)
        {
            this.Nome = nome;
            this.IdAtividade = idCategoria;
            this.Localizacao = localizacao;
            this.Descricao = descricao;
            this.Data = data;
        }
    }
}
