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
    public class PageRegistrarController : IDataBase
    {
        public int DataBaseCaller(Usuario usuarioCasdatro)
        {
            return DatabaseConnection.InsertUsuario(usuarioCasdatro);
        }

        public void DataBaseCaller(Atividade AtividadeCadastro)
        {
            throw new NotImplementedException();
        }
    }
}
