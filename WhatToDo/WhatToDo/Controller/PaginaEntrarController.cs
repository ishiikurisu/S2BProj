using System;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    class PaginaEntrarController
    {
        public int DataBaseCaller(Usuario usuarioCasdatro)
        {
            return DatabaseConnection.ValidateRegister(usuarioCasdatro);
        }
        public Usuario DataBaseCaller(string email)
        {
            return DatabaseConnection.GetUsuario(email);
        }
    }
}
