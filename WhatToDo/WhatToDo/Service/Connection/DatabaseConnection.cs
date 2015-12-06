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
		// Insert a Usuario on the database
		// Returns:
		// 0: Usuario was successfully inserted
		// 1: Usuario already exists
		// TODO: More error codes
		public static int InsertUsuario(Usuario usuario)
        {
			//Verify if UsuarioCadastro already exists on database, if it does exist return false else insert UsuarioCadastro into database and return true.
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();
				//cmd.Parameters.Add

				string sql;
				MySqlCommand cmd;

				// Query to look for email on user table
				sql = "SELECT COUNT(1) FROM TB_Usuario WHERE email = @email";
				cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@email", usuario.Email);
				var reader = cmd.ExecuteReader();
				reader.Read();
				// Checks if register already exists
				if (reader.GetInt16(0) == 1)
				{
					reader.Close();
					connection.Close();
					return 1;
				}
				reader.Close();

				// Insert new user on user table
				sql = "INSERT INTO TB_Usuario(nome, senha, email) VALUES (@nome, @senha, @email)";
				cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@nome", usuario.Nome);
				cmd.Parameters.AddWithValue("@senha", usuario.Senha);
				cmd.Parameters.AddWithValue("@email", usuario.Email);

				cmd.ExecuteNonQuery();

				connection.Close();
			}

			return 0;
        }
        //Insert Command Model
        public static void ConnectionTest()
        {
            Usuario test = new Usuario("Ygor", "11235813", "ygordanniel@gmail.com", "Longos dias e belas noites.");
            try
            {
                using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
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
