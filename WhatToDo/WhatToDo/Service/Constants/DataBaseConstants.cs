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
        public static string MyConnectionString = @"server=31.170.164.31;
                                                  database=u562774431_s2b;
                                                  uid= u562774431_s2b;
                                                  password=i3Q45JuUAe;";

        //mysql -u u562774431_s2b -p i3Q45JuUAe -h 31.170.164.31 u562774431_s2b 
        public static void InsertTest()
        {
            Usuario test = new Usuario("Ygor", "11235813", "ygordanniel@gmail.com", "Longos dias e belas noites.");
        }

        public static void ConnectionTest()
        {

        }
    }
}
