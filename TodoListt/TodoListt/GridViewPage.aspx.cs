using System;
using System.Web.UI.WebControls;
using TodoListt.Services;
using TodoListt.Database;
using System.Web.UI;
using System.Data;

namespace TodoListt
{
    public partial class GridViewPage : System.Web.UI.Page
    {
        private readonly TodoItemService _todoItemService;
        private readonly UserService _userService;

        public GridViewPage()
        {
            _todoItemService = new TodoItemService();
            _userService = new UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                User user = _userService.GetUserById(userId);
                if (user != null)
                {
                    lblUserName.Text = "Hoşgeldiniz, " + user.Username;
                }

                BindTodoItems();
            }
        }

        private void BindTodoItems()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var todoItems = _todoItemService.GetTasks(userId);

            gvTodoItems.DataSource = todoItems;
            gvTodoItems.DataBind();
        }

        protected void gvTodoItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTodoItems.PageIndex = e.NewPageIndex;
            BindTodoItems();
            NotifyParent();
        }

        protected void gvTodoItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTodoItems.EditIndex = e.NewEditIndex;
            BindTodoItems();
            NotifyParent();
        }

        protected void gvTodoItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTodoItems.EditIndex = -1;
            BindTodoItems();
            NotifyParent();
        }

        protected void gvTodoItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTodoItems.DataKeys[e.RowIndex].Value);
            int userId = Convert.ToInt32(Session["UserId"]);

            TextBox txtTaskDescriptionEdit = gvTodoItems.Rows[e.RowIndex].FindControl("txtTaskDescriptionEdit") as TextBox;
            CheckBox chkCompletedEdit = gvTodoItems.Rows[e.RowIndex].FindControl("chkCompletedEdit") as CheckBox;
            TextBox txtStartDateEdit = gvTodoItems.Rows[e.RowIndex].FindControl("txtStartDateEdit") as TextBox;
            TextBox txtEndDateEdit = gvTodoItems.Rows[e.RowIndex].FindControl("txtEndDateEdit") as TextBox;
            TextBox txtAdditionalNotesEdit = gvTodoItems.Rows[e.RowIndex].FindControl("txtAdditionalNotesEdit") as TextBox;

            if (txtTaskDescriptionEdit == null || chkCompletedEdit == null || txtStartDateEdit == null || txtEndDateEdit == null || txtAdditionalNotesEdit == null)
            {
                ShowAlert("Düzeltilecek alan bulunamadı. Lütfen tüm düzenleme alanlarını kontrol edin.");
                return; 
            }

            string taskDescription = txtTaskDescriptionEdit.Text;
            bool isCompleted = chkCompletedEdit.Checked;
            DateTime? startDate = string.IsNullOrEmpty(txtStartDateEdit.Text) ? (DateTime?)null : DateTime.TryParse(txtStartDateEdit.Text, out var tempStartDate) ? tempStartDate : (DateTime?)null;
            DateTime? endDate = string.IsNullOrEmpty(txtEndDateEdit.Text) ? (DateTime?)null : DateTime.TryParse(txtEndDateEdit.Text, out var tempEndDate) ? tempEndDate : (DateTime?)null;
            string additionalNotes = txtAdditionalNotesEdit.Text;
            DateTime? completedDate = isCompleted ? (DateTime?)DateTime.Now : (DateTime?)null;

            var todoItem = new TodoItem
            {
                Id = taskId,
                TaskDescription = taskDescription,
                IsCompleted = isCompleted,
                StartDate = startDate,
                EndDate = endDate,
                AdditionalNotes = additionalNotes,
                CompletedDate = completedDate,
                UserId = userId
            };

            _todoItemService.UpdateTask(todoItem);

            gvTodoItems.EditIndex = -1;
            BindTodoItems();

            NotifyParent();
        }

        protected void gvTodoItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTodoItems.DataKeys[e.RowIndex].Value);
            _todoItemService.DeleteTask(taskId);
            BindTodoItems();
            NotifyParent();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            gvTodoItems.FooterRow.Visible = true;
            btnAddNew.Visible = false;
            newRowControls.Visible = true;
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
        
            string taskDescription = ((TextBox)gvTodoItems.FooterRow.FindControl("txtTaskDescriptionNew")).Text;

            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                // SweetAlert mesajını göstermek için JavaScript kodunu ekleyin
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert",
                    "Swal.fire({ title: 'Hata!', text: 'Görev adı boş bırakılamaz.', icon: 'error', confirmButtonText: 'Tamam' });", true);
                return;
            }

            int userId = (int)Session["UserId"];

            var newTask = new TodoItem
            {
                TaskDescription = taskDescription,
                IsCompleted = ((CheckBox)gvTodoItems.FooterRow.FindControl("chkCompletedNew")).Checked,
                StartDate = DateTime.TryParse(((TextBox)gvTodoItems.FooterRow.FindControl("txtStartDateNew")).Text, out var tempStartDate) ? tempStartDate : (DateTime?)null,
                EndDate = DateTime.TryParse(((TextBox)gvTodoItems.FooterRow.FindControl("txtEndDateNew")).Text, out var tempEndDate) ? tempEndDate : (DateTime?)null,
                AdditionalNotes = ((TextBox)gvTodoItems.FooterRow.FindControl("txtAdditionalNotesNew")).Text,
                UserId = userId
            };

            _todoItemService.AddTask(userId, newTask.TaskDescription, newTask.IsCompleted, newTask.AdditionalNotes, newTask.StartDate, newTask.EndDate);

            BindTodoItems();

            btnAddNew.Visible = true;
            newRowControls.Visible = false;

            NotifyParent();
        }


        protected void btnCancelNew_Click(object sender, EventArgs e)
        {
            gvTodoItems.FooterRow.Visible = false;
            btnAddNew.Visible = true;
            newRowControls.Visible = false;

            NotifyParent();
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

        public string GetNextTaskNo()
        {
            int nextTaskNo = _todoItemService.GetNextTaskNumber();
            return nextTaskNo.ToString();
        }
        protected void gvTodoItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
