<%@ Page Title="Cellular Strength" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CellStrengthStats.aspx.cs" Inherits="Azure_SQL_NetsAround1.CellStrengthStats" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <p>View the cellular signal strength measurements sent by the NetsAround Android app to the Azure SQL database.</p>
            <div style="margin-bottom:10px">
            <asp:Button ID="ButtonC1" runat="server" Text="Top 10" OnClick="ButtonC1_Click" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show the places with the best signal strength level (max 31)" Width="150px" />
            <asp:Button ID="ButtonC2" runat="server" Text="Bottom 10" OnClick="ButtonC2_Click" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show the places with the lowest signal strength" Width="150px" />
            <asp:Button ID="ButtonC3" runat="server" Text="All" Width="150px" OnClick="ButtonC3_Click" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show all the signal strength measurements" />
            <asp:Button ID="ButtonC4" runat="server" Text="Show Map" OnClick="ButtonC4_Click" Font-Bold="True" Font-Size="Large" Height="75px" ToolTip="Show the site list on the map" Width="150px" />
                
            </div>

            <div style="width:500px">
                <div style="float:left;width:200px">
            <asp:TextBox ID="CellCalStart" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">Start Date</asp:TextBox>
                    </div>
                <div style="float:right;width:200px">
            <asp:TextBox ID="CellCalEnd" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">End Date</asp:TextBox>
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


            
            <div style="display:block;margin-top:10px;margin-bottom:10px">
                
            <asp:TextBox ID="TextBoxUser" Width="200px" runat="server" Font-Bold="True" Font-Italic="False" ToolTip="Search the entries recorded by the user">User Name</asp:TextBox>
                
                
            <asp:DropDownList ID="DropDownList1" AppendDataBoundItems="true" runat="server" DataSourceID="SqlDataSource1" DataTextField="UserName" DataValueField="UserName" Width="200px">
            <asp:ListItem Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [UserName] FROM [CellDatas]" OnSelected="SqlDataSource1_Selected" OnUpdated="SqlDataSource1_Updated"></asp:SqlDataSource>
                
            </div>

           
            <div style="display:block;width:800px;margin-bottom:10px">
            <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" Height="200px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="800px" AutoGenerateColumns="False"> 
                <Columns>
                    <asp:TemplateField HeaderText="ROW">
                        <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="TIME" DataField="Time" />
                    <asp:BoundField HeaderText="LOCATION" DataField="Location" />
                    <asp:BoundField HeaderText="CELL STRENGTH" DataField="Cell_Strength" />
                    <asp:BoundField HeaderText="DATA CONNECTION" DataField="DataConnState" />
                    <asp:BoundField HeaderText="NETWORK TYPE" DataField="Net_Type" />
                    <asp:BoundField HeaderText="DATA ACTIVITY" DataField="Data_Activity" />
                    <asp:BoundField HeaderText="USER" DataField="User" />
                    <asp:ButtonField CommandName="Map" Text="Show map" />
                </Columns>
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
            </div>
            
            
        <div>
              <asp:TextBox ID="TextBoxC1" runat="server" TextMode="MultiLine" Height="200px" Width="800px" OnPreRender="TextBoxC1_PreRender"></asp:TextBox>
        </div>
        


</asp:Content>