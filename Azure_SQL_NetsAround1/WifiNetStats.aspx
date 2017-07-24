<%@ Page Title="WiFi Network Details" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WifiNetStats.aspx.cs" Inherits="Azure_SQL_NetsAround1.WifiNetStats" EnableViewState="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <p>Search the WiFi Network details from the data sent by the NetsAround Android app to the Azure SQL database.</p>

        <div style="display:inline">
        <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Size="Large" Height="75px" OnClick="Button1_Click" Text="Run Query" Width="150px" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Show on Map" Height="75px" Width="150px" Font-Bold="True" Font-Size="Large" />
            </div>

    
        <div style="width:500px;margin-top:10px">
                <div style="float:left;width:200px">
            <asp:TextBox ID="TextBox2" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">Start Date</asp:TextBox>
                    </div>
            <div style="float:right;width:200px">
                <asp:TextBox ID="TextBox1" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">End Date</asp:TextBox>
                </div>
            </div>


            <div style="width:500px">
                <div style="float:left;width:200px">
            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar1_SelectionChanged">
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
            <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar2_SelectionChanged">
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
        <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox4" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Show only WiFis with the given signal strength or stronger, given as negative dBm value between 0 - (-100), e.g. -50">Minimum signal strength</asp:TextBox>
        <asp:TextBox ID="TextBox5" runat="server" Width="200px"></asp:TextBox>
        </div>

        <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox6" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Show only WiFis detected by this user">User Name</asp:TextBox>
        <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource1" DataTextField="UserName" DataValueField="UserName" Width="200px">
            <asp:ListItem Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [UserName] FROM [WifiScans]" OnSelected="SqlDataSource1_Selected" OnUpdated="SqlDataSource1_Updated"></asp:SqlDataSource>
            </div>
        
        <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox7" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Select the WiFi SSID name to search">Select SSID</asp:TextBox>
            <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource2" Width="200px" DataTextField="SSID" DataValueField="SSID">
                <asp:ListItem Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [SSID] FROM [WifiScanDetails]" OnSelected="SqlDataSource2_Selected" OnUpdated="SqlDataSource2_Updated"></asp:SqlDataSource>
        </div>

        <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox3" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Enter the WiFi SSID name to search">Search SSID</asp:TextBox>
        <asp:TextBox ID="TextBox8" runat="server" Width="200px"></asp:TextBox>
        </div>

        <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox12" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Show all occurances or only the WiFis with a distinct BSSID">BSSID</asp:TextBox>
        <asp:DropDownList ID="DropDownList3" runat="server" Width="200px">
            <asp:ListItem Selected="True">All</asp:ListItem>
            <asp:ListItem>Distinct BSSID</asp:ListItem>
            </asp:DropDownList>
        </div>
            <div style="display:block;margin-top:10px">
        <asp:TextBox ID="TextBox9" runat="server" Width="200px" Font-Bold="True" ReadOnly="True" ToolTip="Select the WiFi type to search">Search by Wifi Type</asp:TextBox>


        <asp:DropDownList ID="DropDownList4" runat="server" Width="200px">
            <asp:ListItem Selected="True">All</asp:ListItem>
            <asp:ListItem>ESS</asp:ListItem>
            <asp:ListItem>WPA</asp:ListItem>
            <asp:ListItem>WPA2</asp:ListItem>
            <asp:ListItem>WEP</asp:ListItem>
            <asp:ListItem>BLE</asp:ListItem>
            <asp:ListItem>WPS</asp:ListItem>
            <asp:ListItem>Open</asp:ListItem>
            </asp:DropDownList>
                </div>
        
            
            <div style="margin-top:10px">
            <asp:GridView ID="GridView1" runat="server" Height="200px" Width="793px" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
            <Columns>
                    <asp:TemplateField HeaderText="ROW">
                        <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="TIME" DataField="Time" />
                    <asp:BoundField HeaderText="LOCATION" DataField="Location" />
                    <asp:BoundField DataField="SSID" HeaderText="SSID" />
                    <asp:BoundField DataField="BSSID" HeaderText="BSSID" />
                    <asp:BoundField DataField="Capabilities" HeaderText="Capabilities" />
                    <asp:BoundField DataField="Freq_str" HeaderText="Frequency" />
                    <asp:BoundField DataField="Level_str" HeaderText="Signal Level" />
                    <asp:BoundField DataField="User" HeaderText="User" />
                    <asp:ButtonField Text="Show on map" />

            </Columns>
                <RowStyle HorizontalAlign="Center" />
        </asp:GridView>
                </div>
        
            <div style="margin-top:10px">
            <asp:TextBox ID="TextBox11" runat="server" Height="200px" Width="800px" Rows="0" TextMode="MultiLine" OnPreRender="TextBox11_PreRender"></asp:TextBox>
                </div>
</asp:Content>