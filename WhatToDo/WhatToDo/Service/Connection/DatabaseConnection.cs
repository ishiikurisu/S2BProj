using MySql.Data.MySqlClient;
using System.Collections.Generic;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Auxiliar;
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
				sql = "SELECT senha FROM TB_Usuario WHERE email = @email LIMIT 1";
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

						return 0;
					}
					else
					{
						reader.Close();

						return 2;
					}
						
				}
				else
				{
					reader.Close();

					return 1;
				}

			}
        }

		// Search for a Usuario on the database by an email and return it
		// Return null if the user was not found
		public static Usuario GetUsuario(string email)
		{
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

				string sql;
				MySqlCommand cmd;

				// Query to look for email on user table
				sql = "SELECT * FROM TB_Usuario WHERE email = @email LIMIT 1";
				cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@email", email);

				var reader = cmd.ExecuteReader();

				if(reader.Read())
				{
					Usuario usuario = new Usuario(email);

					usuario.Nome = reader.GetString("nome");
					usuario.Senha = "";
					usuario.Perfil = reader.GetString("perfil");

					reader.Close();

					return usuario;
				}
			}

			return null;
		}

		// Insert an Atividade on the database
		public static void InsertAtividade(Atividade atividade)
		{
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

				var sql = "INSERT INTO TB_Atividade(nome, id_categoria, localGPS, local, descricao, data) VALUES (@nome, @id_categoria, @localGPS, @local, @descricao, @data)";

				var cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@nome", atividade.Nome);
				cmd.Parameters.AddWithValue("@id_categoria", atividade.IdCategoria);
				cmd.Parameters.AddWithValue("@localGPS", atividade.LocalGPS);
				cmd.Parameters.AddWithValue("@local", atividade.Local);
				cmd.Parameters.AddWithValue("@descricao", atividade.Descricao);
				cmd.Parameters.AddWithValue("@data", atividade.Data);

				cmd.ExecuteNonQuery();
			}

		}

        // Search for all activities
        // Return a IEnumerable containing these activities
        public static List<Atividade> GetAtividades()
        {
            using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;

                sql = "SELECT * FROM TB_Atividade";
                cmd = new MySqlCommand(sql, connection);
                var reader = cmd.ExecuteReader();

                List<Atividade> listAtividade = new List<Atividade>();

                while (reader.Read())
                {
                    Atividade newAtividade = new Atividade();
                    newAtividade.Nome = reader.GetString("nome");
                    newAtividade.IdCategoria = reader.GetInt16("id_categoria");
                    newAtividade.LocalGPS = reader.GetString("localGPS");
                    newAtividade.Local = reader.GetString("local");
                    newAtividade.Descricao = reader.GetString("descricao");
                    newAtividade.Data = reader.GetDateTime("data");

                    listAtividade.Add(newAtividade);
                }

                return listAtividade;
            }
        }

        // Search for all activities near a location given a radius(in km)
        // Return a IEnumerable containing these activities
        public static List<Atividade> GetAtividades(string location, double radius)
        {
            using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;

                sql = "SELECT * FROM TB_Atividade";
                cmd = new MySqlCommand(sql, connection);
                var reader = cmd.ExecuteReader();

                List<Atividade> listAtividade = new List<Atividade>();

                while (reader.Read())
                {
                    var registerLocation = reader.GetString("localGPS");

                    if (!Geo.checkInsideRadius(location, registerLocation, radius))
                        continue;

                    Atividade newAtividade = new Atividade();
                    newAtividade.Nome = reader.GetString("nome");
                    newAtividade.IdCategoria = reader.GetInt16("id_categoria");
                    newAtividade.LocalGPS = registerLocation;
                    newAtividade.Local = reader.GetString("local");
                    newAtividade.Descricao = reader.GetString("descricao");
                    newAtividade.Data = reader.GetDateTime("data");

                    listAtividade.Add(newAtividade);
                }

                return listAtividade;
            }
        }

        // Search for all activities of a given category and near a location given a radius(in km)
        // Return a IEnumerable containing these activities
        public static List<Atividade> GetAtividadesByCategoria(string location, double radius, int idCategoria)
		{
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

				string sql;
				MySqlCommand cmd;

				sql = "SELECT * FROM TB_Atividade WHERE id_categoria = @id_categoria";
				cmd = new MySqlCommand(sql, connection);
				cmd.Parameters.AddWithValue("@id_categoria", idCategoria);
				var reader = cmd.ExecuteReader();

				List<Atividade> listAtividade = new List<Atividade>();

				while(reader.Read())
				{
					var registerLocation = reader.GetString("localGPS");

					if (!Geo.checkInsideRadius(location, registerLocation, 10))
						continue;

					Atividade newAtividade = new Atividade();
					newAtividade.Nome = reader.GetString("nome");
					newAtividade.IdCategoria = reader.GetInt16("id_categoria");
					newAtividade.LocalGPS = registerLocation;
					newAtividade.Local = reader.GetString("local");
                    newAtividade.Descricao = reader.GetString("descricao");
					newAtividade.Data = reader.GetDateTime("data");

					listAtividade.Add(newAtividade);
				}

				return listAtividade;
			}
		}

		// Return a list of all Categorias on the database
		public static List<Categoria> GetCategorias()
		{
			using (var connection = new MySqlConnection(DataBaseConstants.MyConnectionString))
			{
				connection.Open();

				string sql;
				MySqlCommand cmd;

				// Query to look for email on user table
				sql = "SELECT * FROM TB_Categoria";
				cmd = new MySqlCommand(sql, connection);
				var reader = cmd.ExecuteReader();

				List<Categoria> listCategorias = new List<Categoria>();

				while (reader.Read())
				{
					Categoria newCategoria = new Categoria();

					newCategoria.IdCategoria = reader.GetInt16("id");
					newCategoria.Nome = reader.GetString("nome");
					newCategoria.Icone = reader.GetString("icone");

					listCategorias.Add(newCategoria);
				}

				reader.Close();
				return listCategorias;
			}
		}
    }
}
