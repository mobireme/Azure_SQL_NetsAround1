<%@ Page Title="WiFi Network Statistics" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WifiCountStats.aspx.cs" Inherits="Azure_SQL_NetsAround1.WifiCountStats" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <p>Shows the number of WiFi Networks detected by the NetsAround Android app.</p>


            <br />

            <div style="display:block">
            <asp:Button ID="ButtonW1" runat="server" Text="Top 10" OnClick="ButtonW1_Click" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Top 10 sites with the highest number of WiFi networks detected" Width="150px" />
            <asp:Button ID="ButtonW2" runat="server" OnClick="ButtonW2_Click" Text="Bottom 10" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="The sites with the lowest number of detected WiFi networks" Width="150px" />
            <asp:Button ID="ButtonW3" runat="server" OnClick="ButtonW3_Click" Text="All" Width="150px" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show all sites with the number of WiFi networks detected at each site" />
            <asp:Button ID="ButtonW4" runat="server" OnClick="ButtonW4_Click" Text="Show Map" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show all listed sites on map" Width="150px" />
            </div>


            <div style="width:500px;margin-top:10px">
                <div style="float:left;width:200px">
            <asp:TextBox ID="TextBoxCal1" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">Start Date</asp:TextBox> 
                </div>
                <div style="float:right;width:200px">
                    <asp:TextBox ID="TextBoxCal2" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">End Date</asp:TextBox>
                    </div>
                </div>

            <div style="width:500px">
                <div style="float:left;width:200px">
            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar1_SelectionChanged" >

                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#CCCCCC" />
                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
           </div>
                <div style="float:right;width:200px">
            
            <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar2_SelectionChanged" >
                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#CCCCCC" />
                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />

                </asp:Calendar>
            </div>
                </div>

            <div style="display:block;margin-top:40px">
            <asp:TextBox ID="TextBoxUser" runat="server" Font-Bold="True" ReadOnly="True" ToolTip="Show only the sites of the given user" Width="200px">User Name</asp:TextBox>
            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="UserName" DataValueField="UserName" Width="200px">
                <asp:ListItem Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [UserName] FROM [WifiScans]" OnSelected="SqlDataSource1_Selected" OnUpdated="SqlDataSource1_Updated"></asp:SqlDataSource>
            </div>

           
            <div style="margin-top:10px">
            <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" Height="200px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="800px" AutoGenerateColumns="False" AllowSorting="True"> 
                <Columns>
                    <asp:TemplateField HeaderText="ROW">
                        <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="TIME" DataField="Time" />
                    <asp:BoundField HeaderText="LOCATION" DataField="Location" />
                    <asp:BoundField HeaderText="WIFI COUNT" DataField="Wifi_Count" />
                    <asp:BoundField HeaderText="USER" DataField="User" />
                    <asp:ButtonField Text="Details" CommandName="Details" />
                    <asp:ButtonField CommandName="Map" Text="Show map" />
                </Columns>
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
            </div>
            
            <div style="margin-top:10px">
            <asp:GridView ID="GridView2" runat="server" Height="200px" Width="793px" AutoGenerateColumns="False"> 
                <Columns>
                    <asp:TemplateField HeaderText="ROW">
                        <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="SSID" HeaderText="SSID" />
                    <asp:BoundField DataField="BSSID" HeaderText="BSSID" />
                    <asp:BoundField DataField="Capabilities" HeaderText="CAPABILITIES" />
                    <asp:BoundField DataField="Freq_str" HeaderText="FREQUENCY" HtmlEncode="False" />
                    <asp:BoundField DataField="Level_str" HeaderText="SIGNAL LEVEL" />
                    
                </Columns>
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
            </div>
            




            


        <div style="margin-top:10px">

        <asp:TextBox ID="TextBoxW1" runat="server" TextMode="MultiLine" Height="200px" Width="800px" BorderStyle="Solid" OnPreRender="TextBoxW1_PreRender"></asp:TextBox>
        </div>
        

        <p></p>

                
        
</asp:Content>