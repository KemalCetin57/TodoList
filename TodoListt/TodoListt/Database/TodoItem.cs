using System;

namespace TodoListt.Database
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int UserId { get; set; }
        public string AdditionalNotes { get; set; } 
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
    }
}
