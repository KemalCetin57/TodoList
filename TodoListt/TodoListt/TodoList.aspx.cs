using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TodoListt.Database;
using TodoListt.Services;

namespace TodoListt
{
    public partial class TodoList : Page
    {
        private TodoItemService todoService;

        protected void Page_Load(object sender, EventArgs e)
        {
            todoService = new TodoItemService();

            if (!IsPostBack)
            {
                LoadTasks();
            }
        }
        private void NotifyParent()
        {
            string scripts = @"
    localStorage.setItem('refreshTodoList', 'true');
    localStorage.removeItem('refreshTodoList');
";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "NotifyParent", scripts, true);
        }
        private void ShowAlert(string message)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", script, true);
        }
        protected void chkCompleted_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkCompleted = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkCompleted.NamingContainer;
                int taskId = Convert.ToInt32(((LinkButton)item.FindControl("btnDelete")).CommandArgument);

                var task = todoService.GetTasks(Convert.ToInt32(Session["UserId"]))
                    .Find(t => t.Id == taskId);

                if (task != null)
                {
                    task.IsCompleted = chkCompleted.Checked;
                    todoService.UpdateTask(task);

                    // UpdatePanel'i güncellemek için
                    UpdatePanel1.Update();

                    LoadTasks();

                    NotifyParent();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Seçim yaplırken hata oluştu.");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var description = txtTaskDescription.Text;
                var isCompleted = false;  
                var additionalNotes = txtAdditionalNotes.Text;
                var startDate = DateTime.TryParse(txtStartDate.Text, out var start) ? (DateTime?)start : null;
                var endDate = DateTime.TryParse(txtEndDate.Text, out var end) ? (DateTime?)end : null;


                todoService.AddTask(userId, description, isCompleted, additionalNotes, startDate, endDate);


                pnlModal.Style["display"] = "none";
                pnlModal.Visible = false;
                txtTaskDescription.Text = string.Empty;
                txtAdditionalNotes.Text = string.Empty;
                txtStartDate.Text = string.Empty;
                txtEndDate.Text = string.Empty;

                LoadTasks(); 

                ViewState["SelectedTaskId"] = null;
                //sayfayı yenilemek için
                NotifyParent();

            }
            catch (Exception ex)
            {

                ShowAlert("Task gönderiminde hata oluştu");
            }
        }

       
        protected void btnAddTask_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtTaskDescription.Text))
            {
               
                pnlModal.Style["display"] = "block";
                pnlModal.Visible = true;
            }
        }
        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            txtTaskDescription.Text = string.Empty;
            txtAdditionalNotes.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;

            pnlModal.Style["display"] = "none";
            pnlModal.Visible = false;
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var tasks = todoService.GetTasks(userId);
                foreach (var task in tasks)
                {
                    todoService.DeleteTask(task.Id);
                }
                LoadTasks();
                NotifyParent();

            }
            catch (Exception ex)
            {

                ShowAlert("Görevler silinirken bir Hata Oluştu.");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnDelete = (LinkButton)sender;
                int taskId = int.Parse(btnDelete.CommandArgument);
                todoService.DeleteTask(taskId);
                LoadTasks();
            

            }

            catch (Exception ex)
            {

                ShowAlert("Görev silinirken Hata oluştu");
            }
           
        }

        protected void FilterTasks_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnFilter = (LinkButton)sender;
                string filter = btnFilter.CommandArgument;

                // Görevleri filtrele
                int userId = Convert.ToInt32(Session["UserId"]);
                var tasks = todoService.GetTasks(userId);
                if (filter == "completed")
                {
                    tasks = tasks.FindAll(t => t.IsCompleted);
                }
                else if (filter == "pending")
                {
                    tasks = tasks.FindAll(t => !t.IsCompleted);
                }

                rptTasks.DataSource = tasks;
                rptTasks.DataBind();

                emptyImage.Visible = tasks.Count == 0;
            }
            catch (Exception ex)
            {
                ShowAlert("Görevler listelenirken bir hata oluştu.");
            }
        }

        private void LoadTasks()
        {
            try
            {
                
                int userId = Convert.ToInt32(Session["UserId"]);
                var tasks = todoService.GetTasks(userId);

                rptTasks.DataSource = tasks;
                rptTasks.DataBind();

                emptyImage.Visible = tasks.Count == 0;
            }
            catch (Exception ex)
            {
                ShowAlert("Görevler Yüklenirken Hata oluştu.");
            }
        }
    }
}
