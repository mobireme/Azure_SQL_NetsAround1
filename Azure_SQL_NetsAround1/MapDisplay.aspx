<%@ Page Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MapDisplay.aspx.cs" Inherits="Azure_SQL_NetsAround1.MapDisplay" %>

    
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    

        <div style="display:block;margin-bottom:10px">
            <asp:Button ID="BackButton" runat="server" Text="BACK" OnClick="BackButton_Click" Height="75px" Width="150px" Font-Bold="True" Font-Size="Large" />
            </div>

        <div style="display:block">
            <asp:TextBox ID="TextBox2" runat="server" Width="150px" ReadOnly="True" Font-Bold="True" Font-Size="Large" Wrap="False">Zoom level :</asp:TextBox>
        </div>
        
     
        <div style="display:block;width:1000px">
            
        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" Font-Bold="True" Font-Size="Large" Width="1000px" RepeatDirection="Horizontal">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
        </asp:RadioButtonList>
                </div>
            <div style="display:block;width:1000px">
        <asp:Image ID="ImageMap" runat="server" Height="1000px" Width="1000px" />
                </div>
        

        <asp:TextBox ID="TextBox1" runat="server" Height="500px" Width="1000px" TextMode="MultiLine" OnPreRender="TextBox1_PreRender"></asp:TextBox>
</asp:Content>