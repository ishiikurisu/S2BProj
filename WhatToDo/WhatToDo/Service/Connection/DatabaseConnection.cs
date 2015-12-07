using MySql.Data.MySqlClient;
using System;
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
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

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

		// Validate a user on the database, checking if email and password is correct
		// Returns:
		// 0: Email and Password check
		// 1: Email not found
		// 2: Wrong password
		// TODO: More error codes
		// TODO: Improve security
		public static int ValidateRegister(Usuario usuario)
        {
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

				string sql;
				MySqlCommand cmd;

				// Query to look for email on user table
				sql = "SELECT senha FROM TB_Usuario WHERE email = @email";
				cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@email", usuario.Email);

				var reader = cmd.ExecuteReader();

				string password;
				// Checks if email and password is correct
				if (reader.Read() && ((password = reader.GetString(0)) != null))
				{
					if(password == usuario.Senha)
					{
						reader.Close();
						connection.Close();

						return 0;
					}
					else
					{
						reader.Close();
						connection.Close();

						return 2;
					}
						
				}
				else
				{
					reader.Close();
					connection.Close();

					return 1;
				}

			}
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
