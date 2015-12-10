using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    public class PageRegistrarController
    {
        public int DataBaseCaller(Usuario usuarioCasdatro)
        {
            return DatabaseConnection.InsertUsuario(usuarioCasdatro);
        }
    }
}
