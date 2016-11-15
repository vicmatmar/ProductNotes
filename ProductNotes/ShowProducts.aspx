<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowProducts.aspx.cs" Inherits="ProductNotes.AddNote" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel_Data" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table ID="Table_date" runat="server">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:TextBox ID="TextBox_fromDate" runat="server" AutoPostBack="True" OnKeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:Button ID="Button_fromCalendar" runat="server" OnClick="Button_Calendar_Click" Text="c" ToolTip="Calendar" UseSubmitBehavior="False" />
                        </asp:TableCell>
                        <asp:TableCell runat="server">
                            <asp:TextBox ID="TextBox_toDate" runat="server" AutoPostBack="true" OnKeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:Button ID="Button_toCalendar" runat="server" OnClick="Button_Calendar_Click" Text="c" ToolTip="Calendar" UseSubmitBehavior="False" />
                        </asp:TableCell>
                        <asp:TableCell runat="server">
                            <asp:Button ID="ButtonPreviousDay" runat="server" OnClick="ButtonPreviousDay_Click" Text="&lt;" ToolTip="Previous Day" UseSubmitBehavior="False" />
                            <asp:Button ID="ButtonNextDay" runat="server" OnClick="ButtonNextDay_Click" Text="&gt;" ToolTip="Next Day" UseSubmitBehavior="False" />
                        </asp:TableCell>
                        <asp:TableCell runat="server">
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <asp:Calendar ID="Calendar_Date" runat="server" Visible="false" OnSelectionChanged="Calendar_Date_SelectionChanged" Requester="Button_fromCalendar"></asp:Calendar>

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
    </form>
</body>
</html>
