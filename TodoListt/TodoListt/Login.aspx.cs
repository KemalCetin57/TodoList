using System;
using TodoListt.Database;
using TodoListt.Services; // Services namespace'ini ekleyin

namespace TodoListt
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = mail.Text.Trim();
            string password = sifre.Text.Trim();

            try
            {
                UserService userService = new UserService();
                User user = userService.ValidateUser(email, password);

                if (user != null)
                {
                    Session["UserId"] = user.UserId; 
                    Response.Redirect("Login.aspx?result=success"); 
                }
                else
                {
                    Response.Redirect("Login.aspx?result=failure"); 
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = "Veritabanı hatası: " + ex.Message;
            }
        }

    }
}
