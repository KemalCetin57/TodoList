using System;
using System.Collections.Generic;
using TodoListt.Database;

namespace TodoListt.Services
{
    public class TodoItemService
    {
        private readonly TodoItemRepository repository;

        public TodoItemService()
        {
            repository = new TodoItemRepository();
        }

        public void AddTask(int userId, string description, bool isCompleted = false, string additionalNotes = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var item = new TodoItem
            {
                TaskDescription = description,
                IsCompleted = isCompleted,
                UserId = userId,
                AdditionalNotes = additionalNotes,
                StartDate = startDate,
                EndDate = endDate
            };

            repository.AddTodoItem(item);
        }




        public void DeleteTask(int id)
        {
            repository.DeleteTodoItem(id);
        }

        public List<TodoItem> GetTasks(int userId, bool? isCompleted = null)
        {
            return repository.GetAllTodoItems(userId, isCompleted);
        }

       
        public void UpdateTask(int taskId, int userId, string taskDescription, string additionalNotes, DateTime? startDate, DateTime? endDate, bool isCompleted)
        {
            var task = repository.GetTaskById(taskId);

            if (task != null)
            {
                task.TaskDescription = taskDescription;
                task.AdditionalNotes = additionalNotes;
                task.StartDate = startDate;
                task.EndDate = endDate;
                task.IsCompleted = isCompleted;
                task.CompletedDate = isCompleted ? (DateTime?)DateTime.Now : (DateTime?)null;

                repository.UpdateTodoItem(task);
            }
        }

        public int GetNextTaskNumber()
        {
            return repository.GetNextTaskNumber();
        }

        public void UpdateTask(TodoItem item)
        {
            if (item.IsCompleted)
            {
                item.CompletedDate = DateTime.Now;
            }
            else
            {
                item.CompletedDate = null;
            }
            repository.UpdateTodoItem(item);
        }
    }
}
