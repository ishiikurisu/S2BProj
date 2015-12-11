using System;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    class PaginaEntrarController
    {
        public int DataBaseValidateRegisterCaller(Usuario usuarioCasdatro)
        {
            return DatabaseConnection.ValidateRegister(usuarioCasdatro);
        }
        public Usuario DataBaseGetUsuarioCaller(string email)
        {
            return DatabaseConnection.GetUsuario(email);
        }
    }
}
