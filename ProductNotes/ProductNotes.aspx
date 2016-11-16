<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductNotes.aspx.cs" Inherits="ProductNotes.ProductNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" BackColor="LightBlue">
            <table style="width:100%" border="1" cellpadding="1" cellspacing="1" style="border-collapse: collapse">
                <tr >
                    <td style="width:200px">
                        Effiective Date:<br />
                        <asp:TextBox ID="Text_effectiveDate" runat="server" />
                        <asp:Button ID="Button_calendar" runat="server" Text="c"/>
                    </td>
                    <td>
                        Note:<br />
                        <asp:TextBox ID="Text_note" runat="server" Width="99%"/>
                    </td>
                    <td style="width: 100px; text-align: center;">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Insert" ToolTip="Add a note" UseSubmitBehavior="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel runat="server" BackColor="LightGray">

            <asp:GridView ID="GridView_ProductNotes" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                OnRowDataBound="GridView_ProductNotes_RowDataBound" OnRowEditing="GridView_ProductNotes_RowEditing"
                OnRowCancelingEdit="GridView_ProductNotes_RowCancelingEdit" OnRowUpdating="GridView_ProductNotes_RowUpdating"
                OnRowDeleting="GridView_ProductNotes_RowDeleting" EmptyDataText="No record has been added"
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="EffectiveDate" ItemStyle-Width="100" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="Label_effectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>' />
                            <asp:Button ID="Button_nextDate" runat="server" Text=">" OnClick="Button_nextDate_Click" Enabled="false"></asp:Button>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="Text_effectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Note" ItemStyle-Width="80%">
                        <ItemTemplate>
                            <asp:Button ID="Button_expandNote" runat="server" Text="+" OnClick="Button_expandNote_Click"></asp:Button>
                            <asp:Label ID="Label_note" runat="server" Text='<%# Eval("Note") %>'></asp:Label>
                            <asp:Panel ID="Panel_noteInfo" runat="server" Visible="false">
                            </asp:Panel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="Text_note" runat="server" Text='<%# Eval("Note") %>' Width="98%"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Button" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top"/>
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
