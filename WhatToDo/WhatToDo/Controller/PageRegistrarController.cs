using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    public class PageRegistrarController
    {
        public int DataBaseInsertUsuarioCaller(Usuario usuarioCasdatro)
        {
            return DatabaseConnection.InsertUsuario(usuarioCasdatro);
        }
    }
}
