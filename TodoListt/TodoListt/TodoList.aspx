<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="TodoListt.TodoList" MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlecontent" runat="server">
    To Do List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="tema/assets/css/todolist.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.6.0/css/all.min.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container">
                    <h1>Todo List</h1>
                    <div class="input-container">
                        <asp:TextBox ID="txtTaskDescription" CssClass="todo-input" placeholder="Yeni görev..." runat="server"></asp:TextBox>
                        <asp:LinkButton ID="btnAddTask" CssClass="add-button" runat="server" OnClick="btnAddTask_Click">
                            <i class="fas fa-plus-circle"></i>
                        </asp:LinkButton>
                    </div>

                    <!-- Modal -->
                    <asp:Panel ID="pnlModal" CssClass="modal fade show" Style="display: none;" runat="server" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="myModalLabel">Ek Bilgi</h5>
                                    <asp:LinkButton ID="btnCloseModal" CssClass="btn-close" runat="server" OnClick="btnCloseModal_Click" aria-label="Close"></asp:LinkButton>
                                </div>
                                <div class="modal-body">
                                    <div class="mb-3">
                                        <label for="txtAdditionalNotes" class="form-label">Ek Notlar:</label>
                                        <asp:TextBox ID="txtAdditionalNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
                                    </div>
                                    <div class="mb-3">
                                        <label for="txtStartDate" class="form-label">Başlangıç Tarihi:</label>
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                    <div class="mb-3">
                                        <label for="txtEndDate" class="form-label">Bitiş Tarihi:</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnClose" CssClass="btn btn-secondary" runat="server" Text="Kapat" OnClick="btnCloseModal_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="filters">
                        <asp:LinkButton ID="btnCompleteFilter" CssClass="filter" CommandArgument="completed" runat="server" OnClick="FilterTasks_Click">Tamamlandı</asp:LinkButton>
                        <asp:LinkButton ID="btnIncompleteFilter" CssClass="filter" CommandArgument="pending" runat="server" OnClick="FilterTasks_Click">Tamamlanmadı</asp:LinkButton>
                        <asp:Button ID="btnDeleteAll" CssClass="delete-all" Text="Hepsini Sil" OnClick="btnDeleteAll_Click" runat="server" />
                    </div>

                    <div class="todos-container">
                        <asp:Repeater ID="rptTasks" runat="server">
                            <ItemTemplate>
                                <ul class="todo <%# Convert.ToBoolean(Eval("IsCompleted")) ? "completed-task" : "" %>" data-taskid='<%# Eval("Id") %>'>
                                    <li>
                                        <asp:CheckBox ID="chkCompleted" runat="server"
                                            Checked='<%# Convert.ToBoolean(Eval("IsCompleted")) %>'
                                            AutoPostBack="True"
                                            OnCheckedChanged="chkCompleted_CheckedChanged" />
                                        <span class='<%# Convert.ToBoolean(Eval("IsCompleted")) ? "completed" : "" %>'>
                                            <%# Eval("TaskDescription") %>
                                        </span>
                                        <asp:LinkButton ID="btnDelete" CssClass="delete-btn" runat="server"
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClick="btnDelete_Click">
                    <i class="fa-solid fa-trash"></i>
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Image ID="emptyImage" CssClass="empty-image" ImageUrl="tema/assets/img/empty.svg" runat="server" Visible="false" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>




<asp:Content ID="Content4" ContentPlaceHolderID="scriptcontent" runat="server">
    <script type="text/javascript">
        window.addEventListener('storage', function (event) {
            if (event.key === 'refreshTodoList' && event.newValue === 'true') {
                window.location.reload();
                localStorage.removeItem('refreshTodoList');
            }
        }, false);
    </script>
</asp:Content>
