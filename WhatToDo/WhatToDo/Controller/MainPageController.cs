﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;

namespace WhatToDo.Controller
{
    class MainPageController
    {
        public List<Atividade> DataBaseGetAtividadeCaller()
        {
            return DatabaseConnection.GetAtividades();
        }
    }
}
