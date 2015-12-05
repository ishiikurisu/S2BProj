using MySql.Data.MySqlClient;
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
        //Connection String
        public static string MyConnectionString = @"server=31.170.166.58;
                                                  database=u562774431_s2b;
                                                  uid= u562774431_s2b;
                                                  password=i3Q45JuUAe;
                                                  SslMode=None";

        //Insert Command Model
        public static void ConnectionTest()
        {
            Usuario test = new Usuario("Ygor", "11235813", "ygordanniel@gmail.com", "Longos dias e belas noites.");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(MyConnectionString))
                {
                    connection.Open();
                    string InserUsuarioCommand = "INSERT INTO TB_Usuario (email) VALUES (@email)";
                    using (var command = new MySqlCommand(InserUsuarioCommand, connection))
                    {
                        command.Parameters.AddWithValue("@email", test.Email);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(MySqlException mse)
            {

            }
            catch(NotImplementedException nie)
            {

            }
        }
    }
}
