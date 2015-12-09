using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;
using WhatToDo.Service.Interface;

namespace WhatToDo.Controller
{
    class PageCreateController : IDataBase
    {
        public int DataBaseCaller(Usuario usuarioCasdatro)
        {
            throw new NotImplementedException();
        }

        public void DataBaseCaller(Atividade atividade)
        {
            DatabaseConnection.InsertAtividade(atividade);
        }

        public List<Categoria> DataBaseCaller()
        {
            return DatabaseConnection.GetCategorias();
        }
    }
}
