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
        public int DataBaseCaller(Usuario UsuarioCasdatro)
        {
            return DatabaseConnection.InsertUsuario(UsuarioCasdatro);
        }
    }
}
