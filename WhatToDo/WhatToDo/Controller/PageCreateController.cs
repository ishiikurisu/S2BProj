using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    class PageCreateController
    {
        public void DataBaseInsertAtividadeCaller(Atividade atividade)
        {
            DatabaseConnection.InsertAtividade(atividade);
        }

        public List<Categoria> DataBaseGetCategoriasCaller()
        {
            return DatabaseConnection.GetCategorias();
        }
    }
}
