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
            <asp:UpdatePanel ID="UpdatePanel_Data" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Table ID="Table_date" runat="server">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:TextBox ID="TextBox_fromDate" runat="server" AutoPostBack="True" OnKeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server">
                                <asp:TextBox ID="TextBox_toDate" runat="server" AutoPostBack="true" OnKeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server">
                                <asp:Button ID="ButtonPreviousDay" runat="server" OnClick="ButtonPreviousDay_Click" Text="&lt;" ToolTip="Previous Day" UseSubmitBehavior="False" />
                                <asp:Button ID="ButtonNextDay" runat="server" OnClick="ButtonNextDay_Click" Text="&gt;" ToolTip="Next Day" UseSubmitBehavior="False" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                    <asp:Panel ID="ProductsPanel" runat="server"></asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdateProgress ID="UpdateProgress_Data" runat="server" AssociatedUpdatePanelID="UpdatePanel_Data" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                        <span style="border-width: 10px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Loading ...</span>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

    </form>
</body>
</html>
