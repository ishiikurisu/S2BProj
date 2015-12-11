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
        public List<Atividade> DataBaseCaller()
        {
            return DatabaseConnection.GetAtividades();
        }
    }
}
