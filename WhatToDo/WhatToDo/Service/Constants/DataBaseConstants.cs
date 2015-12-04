using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;

namespace WhatToDo.Service.Constants
{
    public class DataBaseConstants
    {
        public static string MyConnectionString = @"Server=31.170.164.31;
                                                  Database=u562774431_s2b;
                                                  Uid= u562774431_s2b;
                                                  Pwd=i3Q45JuUAe;";
        public static void InsertTest()
        {
            Usuario test = new Usuario("Ygor", "11235813", "ygordanniel@gmail.com", "Longos dias e belas noites.");
            MySqlConnection
        }
    }
}
