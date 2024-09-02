<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridViewPage.aspx.cs" Inherits="TodoListt.GridViewPage" MasterPageFile="~/Site.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlecontent" runat="server">
    Admin Panel
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="tema/assets/css/gridview.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <div class="container">
            <h1>Kullanıcı To Do List Kontrol Paneli</h1>

            <!-- ScriptManager Ekleme -->
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <!-- Kullanıcı Adı Gösterimi -->
            <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>

            <!-- GridView ile Veri Listeleme -->
            <asp:GridView ID="gvTodoItems" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered"
                AllowPaging="True" PageSize="10"
                OnPageIndexChanging="gvTodoItems_PageIndexChanging"
                OnRowEditing="gvTodoItems_RowEditing"
                OnRowCancelingEdit="gvTodoItems_RowCancelingEdit"
                OnRowUpdating="gvTodoItems_RowUpdating"
                OnRowDeleting="gvTodoItems_RowDeleting" OnSelectedIndexChanged="gvTodoItems_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Task No" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Görev Adı">
                        <ItemTemplate>
                            <asp:Label ID="lblTaskDescription" runat="server" Text='<%# Bind("TaskDescription") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTaskDescriptionEdit" runat="server" Text='<%# Bind("TaskDescription") %>' CssClass="form-control" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tamamlanma Durumu">
                        <ItemTemplate>
                            <div class="checkbox-cell">
                                <asp:CheckBox ID="chkCompleted" runat="server" Enabled="false" Checked='<%# Bind("IsCompleted") %>' />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="checkbox-cell">
                                <asp:CheckBox ID="chkCompletedEdit" runat="server" Checked='<%# Bind("IsCompleted") %>' />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Başlangıç Tarihi">
                        <ItemTemplate>
                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStartDateEdit" runat="server" Text='<%# Bind("StartDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDateEdit" Format="yyyy-MM-dd" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bitiş Tarihi">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEndDateEdit" runat="server" Text='<%# Bind("EndDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDateEdit" Format="yyyy-MM-dd" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ek bilgiler">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAdditionalNotes" runat="server" Text='<%# Bind("AdditionalNotes") %>' Enabled="false" CssClass="form-control"></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAdditionalNotesEdit" runat="server" Text='<%# Bind("AdditionalNotes") %>' CssClass="form-control"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="İşlemler">
                        <ItemTemplate>
                            <div class="btn-group">
                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Düzenle" CssClass="btn btn-edit btn-primary" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Sil" CssClass="btn btn-delete btn-danger" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="btn-group">
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Kaydet" CssClass="btn btn-save btn-primary" />
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="İptal" CssClass="btn btn-cancel btn-danger" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <FooterTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewTaskNo" runat="server" Text='<%# GetNextTaskNo() %>' CssClass="form-control" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaskDescriptionNew" runat="server" CssClass="form-control" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkCompletedNew" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartDateNew" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="calStartDateNew" runat="server" TargetControlID="txtStartDateNew" Format="yyyy-MM-dd" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndDateNew" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="calEndDateNew" runat="server" TargetControlID="txtEndDateNew" Format="yyyy-MM-dd" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdditionalNotesNew" runat="server" CssClass="form-control" />
                                </td>
                            </tr>
                        </FooterTemplate> 
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <!-- Tablonun Altında Ekle Butonu -->
            <div class="d-flex justify-content-end mt-3">
                <asp:Button ID="btnAddNew" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
            </div>

            <!-- Kaydet ve İptal Butonları -->
            <div id="newRowControls" runat="server" visible="false" class="d-flex justify-content-end mt-3">
                <asp:Button ID="btnSaveNew" runat="server" Text="Kaydet" CssClass="btn btn-success me-2" OnClick="btnSaveNew_Click" />
                <asp:Button ID="btnCancelNew" runat="server" Text="İptal" CssClass="btn btn-danger" OnClick="btnCancelNew_Click" />
            </div>
        </div>
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

        document.addEventListener('DOMContentLoaded', function () {
            function formatDateInput(event) {
                const value = event.target.value.replace(/\D/g, '');
                let formattedValue = '';

                if (value.length > 4) {
                    formattedValue = value.substring(0, 4) + '-';
                } else {
                    formattedValue = value;
                }

                if (value.length > 6) {
                    formattedValue += value.substring(4, 6) + '-';
                } else if (value.length > 4) {
                    formattedValue += value.substring(4);
                }

                if (value.length > 8) {
                    formattedValue += value.substring(6, 8);
                } else if (value.length > 6) {
                    formattedValue += value.substring(6);
                }

                event.target.value = formattedValue;
            }

            document.querySelectorAll('input[name$="txtStartDateEdit"], input[name$="txtEndDateEdit"], input[name$="txtStartDateNew"], input[name$="txtEndDateNew"]').forEach(function (input) {
                input.addEventListener('input', formatDateInput);
            });
        });
    </script>
</asp:Content>
