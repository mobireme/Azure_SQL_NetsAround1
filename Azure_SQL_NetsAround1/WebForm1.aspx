<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Azure_SQL_NetsAround1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="RUN SQL" />
        </p>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Rows="20" Height="296px" Width="696px"></asp:TextBox>
        <p>
            <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="120px" Width="700px"></asp:TextBox>
        </p>
    </form>
</body>
</html>
