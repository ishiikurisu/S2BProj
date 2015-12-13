using System;

namespace WhatToDo.Model.Entity
{
    public class Atividade
    {
        //This property has autoincrementation
        public int IdAtividade { get; set; }
        public string Nome { get; set; }
        public int IdCategoria { get; set; }
        public string LocalGPS { get; set; }
		public string Local { get; set; }
		public string Descricao { get; set; }
        public DateTime Data { get; set; }

		public Atividade()
		{}

        public Atividade(string nome, int idCategoria, string localGPS, string local, string descricao, DateTime data)
        {
            this.Nome = nome;
            this.IdCategoria = idCategoria;
            this.LocalGPS = localGPS;
			this.Local = local;
			this.Descricao = descricao;
            this.Data = data;
        }
    }
}
