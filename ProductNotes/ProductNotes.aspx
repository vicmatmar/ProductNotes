<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductNotes.aspx.cs" Inherits="ProductNotes.ProductNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:GridView ID="GridView_ProductNotes" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                OnRowDataBound="GridView_ProductNotes_RowDataBound" OnRowEditing="GridView_ProductNotes_RowEditing"
                OnRowCancelingEdit="GridView_ProductNotes_RowCancelingEdit" OnRowUpdating="GridView_ProductNotes_RowUpdating"
                OnRowDeleting="GridView_ProductNotes_RowDeleting" EmptyDataText="No record has been added">
                <Columns>
                    <asp:TemplateField HeaderText="EffectiveDate">
                        <ItemTemplate>
                            <asp:Label ID="Label_effectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="Text_effectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Note" ItemStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="Label_note" runat="server" Text='<%# Eval("Note") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="Text_note" runat="server" Text='<%# Eval("Note") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
                </Columns>
            </asp:GridView>

            <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                <tr>
                    <td style="width: 150px">Effiective Date:<br />
                        <asp:TextBox ID="Text_effectiveDate" runat="server" Width="140" />
                    </td>
                    <td style="width: 150px">Note:<br />
                        <asp:TextBox ID="Text_note" runat="server" Width="140" />
                    </td>
                    <td style="width: 100px">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Insert" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
