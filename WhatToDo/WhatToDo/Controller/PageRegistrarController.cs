using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    public class PageRegistrarController
    {
        public static bool DataBaseCaller(Usuario UsuarioCasdatro)
        {
            return DatabaseConnection.InsertUsuario(UsuarioCasdatro);
        }
    }
}
