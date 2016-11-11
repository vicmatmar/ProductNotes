<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNote.aspx.cs" Inherits="ProductNotes.AddNote" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div>

                    <asp:Panel ID="Panel1" runat="server"></asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel_Data" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Table ID="Table_date" runat="server">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:TextBox ID="TextBox_fromDate" runat="server" AutoPostBack="True"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server">
                                <asp:TextBox ID="TextBox_toDate" runat="server" AutoPostBack="true"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                    <asp:Button ID="ButtonPreviousDay" runat="server" OnClick="ButtonPreviousDay_Click" Text="&lt;" ToolTip="Previous Day" UseSubmitBehavior="False" />

                    <asp:Panel ID="ProductsPanel" runat="server"></asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

    </form>
</body>
</html>
