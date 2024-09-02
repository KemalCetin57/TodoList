using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;


namespace TodoListt.Database
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TodoListDB"].ConnectionString;
        }
        public User GetUserById(int userId)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId, Username, Email FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Hata yönetimi (loglama yapılabilir)
                    throw new Exception("Veritabanı hatası: " + ex.Message);
                }
            }

            return user;
        }
        public void DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public User GetUserByEmail(string email)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, Password FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString() 
                        };
                    }
                }
                catch (Exception ex)
                {
                   
                    throw new Exception("Veritabanı hatası: " + ex.Message);
                }
            }

            return user;
        }
    }
}
