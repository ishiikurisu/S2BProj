using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Constants;

namespace WhatToDo.Service.Connection
{
    public class DatabaseConnection
    {
        public static bool InsertUsuario(Usuario UsuarioCasdatro)
        {
            //Verify if UsuarioCadastro already exists on database, if it does exist return false else insert UsuarioCadastro into database and return true.
            return true;
        }
        //Insert Command Model
        public static void ConnectionTest()
        {
            Usuario test = new Usuario("Ygor", "11235813", "ygordanniel@gmail.com", "Longos dias e belas noites.");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
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
            catch (MySqlException mse)
            {

            }
            catch (NotImplementedException nie)
            {

            }
        }
    }
}
