using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using TodoListt.Database;
using System;

namespace TodoListt.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;
        private string connectionString;

        public UserService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TodoListDB"].ConnectionString;
            userRepository = new UserRepository();
        }
        public User GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }

        public void DeleteUser(int userId)
        {
            userRepository.DeleteUser(userId);
        }

        public User ValidateUser(string email, string password)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId, Username, Email, Password FROM Users WHERE Email = @Email AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
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
