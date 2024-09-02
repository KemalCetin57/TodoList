<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TodoListt.Login" MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="login-page section-b-space">
        <div class="container">
            <div class="row justify-content-center align-items-center min-vh-100">
                <div class="col-lg-6">
                    <h3 class="text-center">Login</h3>
                    <div class="theme-card mx-auto">
                        <form id="form1" runat="server" class="theme-form">
                            <div class="form-group">
                                <label for="mail">Email</label>
                                <asp:TextBox ID="mail" runat="server" CssClass="form-control" Placeholder="Mailinizi giriniz" />
                            </div>
                            <div class="form-group">
                                <label for="sifre">Şifre</label>
                                <asp:TextBox ID="sifre" runat="server" TextMode="Password" CssClass="form-control" Placeholder="Şifrenizi giriniz" />
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-solid w-50 align-items-center" Text="Login" OnClick="btnLogin_Click" />
                            </div>
                            <asp:Label ID="litMessage" runat="server" CssClass="text-danger mt-2" />
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section>
 

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="scriptcontent" runat="server">
    <script type="text/javascript">
        function showAlert(type, title, text) {
            Swal.fire({
                position: 'center',
                icon: type,
                title: title,
                text: text,
                showConfirmButton: false,
                timer: 1500
            });
        }

        function handleLoginResult(isSuccess) {
            if (isSuccess) {
                showAlert('success', 'Başarılı!', 'Giriş başarılı!');

                
                setTimeout(function () {
                   
                    window.location.href = 'TodoList.aspx';

                    
                    var newWindow = window.open('GridViewPage.aspx', '_blank');
                    if (newWindow) {
                        newWindow.focus();
                    }
                }, 1500); 
            } else {
                showAlert('error', 'Hata', 'Geçersiz kullanıcı adı veya şifre.');
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            var loginResult = '<%= Request.QueryString["result"] %>';
      if (loginResult === 'success') {
          handleLoginResult(true);
      } else if (loginResult === 'failure') {
          handleLoginResult(false);
      }
  });
    </script>
</asp:Content>