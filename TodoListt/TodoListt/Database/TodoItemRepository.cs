using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace TodoListt.Database
{
    public class TodoItemRepository
    {
        private readonly string connectionString;

        public TodoItemRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TodoListDB"].ConnectionString;
        }

        public void AddTodoItem(TodoItem item)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO TodoItems (TaskDescription, IsCompleted, UserId, AdditionalNotes, StartDate, EndDate) VALUES (@TaskDescription, @IsCompleted, @UserId, @AdditionalNotes, @StartDate, @EndDate)", connection);
                    command.Parameters.AddWithValue("@TaskDescription", (object)item.TaskDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                    command.Parameters.AddWithValue("@UserId", item.UserId);
                    command.Parameters.AddWithValue("@AdditionalNotes", (object)item.AdditionalNotes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", (object)item.StartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object)item.EndDate ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void UpdateTodoItem(TodoItem todoItem)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "UPDATE TodoItems SET TaskDescription = @TaskDescription, AdditionalNotes = @AdditionalNotes, StartDate = @StartDate, EndDate = @EndDate, IsCompleted = @IsCompleted, CompletedDate = @CompletedDate WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@TaskDescription", (object)todoItem.TaskDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AdditionalNotes", (object)todoItem.AdditionalNotes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", (object)todoItem.StartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object)todoItem.EndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", todoItem.IsCompleted);
                    command.Parameters.AddWithValue("@CompletedDate", todoItem.IsCompleted ? (object)todoItem.CompletedDate ?? DBNull.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Id", todoItem.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                  
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void DeleteTodoItem(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TodoItems WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<TodoItem> GetAllTodoItems(int userId, bool? isCompleted = null)
        {
            List<TodoItem> todoItems = new List<TodoItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM TodoItems WHERE UserId = @UserId";

                    if (isCompleted.HasValue)
                    {
                        query += " AND IsCompleted = @IsCompleted";
                    }

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (isCompleted.HasValue)
                    {
                        command.Parameters.AddWithValue("@IsCompleted", isCompleted.Value);
                    }

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TodoItem todoItem = new TodoItem
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TaskDescription = reader["TaskDescription"].ToString(),
                            IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                            StartDate = reader["StartDate"] as DateTime?,
                            EndDate = reader["EndDate"] as DateTime?,
                            AdditionalNotes = reader["AdditionalNotes"].ToString(),
                            UserId = Convert.ToInt32(reader["UserId"])
                        };

                        todoItems.Add(todoItem);
                    }
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine(ex.Message);
                }
            }

            return todoItems;
        }







        public TodoItem GetTaskById(int taskId)
        {
            TodoItem todoItem = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM TodoItems WHERE Id = @TaskId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        todoItem = new TodoItem
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TaskDescription = reader["TaskDescription"].ToString(),
                            IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                            StartDate = reader["StartDate"] as DateTime?,
                            EndDate = reader["EndDate"] as DateTime?,
                            AdditionalNotes = reader["AdditionalNotes"].ToString(),
                            CompletedDate = reader["CompletedDate"] as DateTime?,
                            UserId = Convert.ToInt32(reader["UserId"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message);
                }
            }

            return todoItem;
        }
        public int GetNextTaskNumber()
        {
            int nextNumber = 1; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM TodoItems";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    nextNumber = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return nextNumber;
        }


    }
}
