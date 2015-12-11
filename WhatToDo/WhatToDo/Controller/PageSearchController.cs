using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    class PageSearchController
    {
        public List<Atividade> DataBaseGetAtividadesCaller()
        {
            return DatabaseConnection.GetAtividades();
        }
        public List<Categoria> DataBaseGetCategoriasCaller()
        {
            return DatabaseConnection.GetCategorias();
        }
    }
}
