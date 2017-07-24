<%@ Page Title="Cellular Connection Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CellDetails.aspx.cs" Inherits="Azure_SQL_NetsAround1.CellDetails" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <p>Search the Cellular Connection Details sent by the NetsAround Android app to the Azure SQL database.</p>

            <div style="display:block">
            <asp:Button ID="Button1" runat="server" Height="75px" OnClick="Button1_Click" Text="Run Query" Width="150px" Font-Bold="True" Font-Size="Large" />
            <asp:Button ID="Button2" runat="server" Height="75px" Width="150px" Text="Show Map" OnClick="Button2_Click" Font-Bold="True" Font-Size="Large" />
            </div>
    
                <div style="display:block;width:500px;margin-top:10px">
                   <div style="float:left; vertical-align:top">
                    <asp:TextBox ID="CellCalStart" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">Start Date</asp:TextBox>
                   </div>
                    <div style="float:right; vertical-align:top"">
                   <asp:TextBox ID="CellCalEnd" runat="server" Width="200px" Font-Bold="True" ReadOnly="True">End Date</asp:TextBox>     
                    </div>
                    </div>
                    
                
            <div style="display:block;width:500px">
                <div style="float:left">
            
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
            <div style="float:right">
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
            
                       <div style="display:block;margin-top:20px">
                       <asp:TextBox ID="TextBox1" runat="server" ReadOnly="True" Width="200px" Font-Bold="True" ToolTip="Show entries with the selected signal strength or higher (strength between 0-31)">Minimum Signal Strength</asp:TextBox>
                    <asp:DropDownList ID="DropDownList2" runat="server" Width="200px">

                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>0</asp:ListItem>
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
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                    </asp:DropDownList>
                           </div>
            

                <div style="display:block;margin-top:10px">
                    <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Width="200px" Font-Bold="True" ToolTip="Show entries with the selected signal strength or lower (strength between 0-31)">Maximum Signal Strength</asp:TextBox>
                    <asp:DropDownList ID="DropDownList3" runat="server" Width="200px">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>0</asp:ListItem>
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
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>

                    </asp:DropDownList>
                    </div>

                <div style="display:block;margin-top:10px">

                <asp:TextBox ID="TextBoxUser" Width="200px" runat="server" Font-Bold="True" ReadOnly="True" ToolTip="Show the entries recorded by the selected user">User Name</asp:TextBox>
            <asp:DropDownList ID="DropDownList1" AppendDataBoundItems="true" runat="server" DataSourceID="SqlDataSource1" DataTextField="UserName" DataValueField="UserName" Width="200px">
                <asp:ListItem Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [UserName] FROM [CellDatas]" OnSelected="SqlDataSource1_Selected" OnUpdated="SqlDataSource1_Updated"></asp:SqlDataSource>
    </div>
            

                <div style="display:block;margin-top:10px">
                    <asp:TextBox ID="TextBox3" runat="server" ReadOnly="True" Width="200px" Font-Bold="True" ToolTip="Show the entries with the selected Data Connection Status">Data Connection Status</asp:TextBox>
                    <asp:DropDownList ID="DropDownList4" AppendDataBoundItems="true" runat="server" DataSourceID="SqlDataSource2" DataTextField="DataConnState" DataValueField="DataConnState" Width="200px">
                        <asp:ListItem Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [DataConnState] FROM [CellDatas]" OnSelected="SqlDataSource2_Selected" OnUpdated="SqlDataSource2_Updated"></asp:SqlDataSource>
                    </div>
                <div style="display:block;margin-top:10px">
                    <asp:TextBox ID="TextBox4" runat="server" ReadOnly="True" Width="200px" Font-Bold="True" ToolTip="Show the entries with selected cellular network type">Network Type</asp:TextBox>
                    <asp:DropDownList ID="DropDownList5" AppendDataBoundItems="true" runat="server" DataSourceID="SqlDataSource3" DataTextField="Net_Type" DataValueField="Net_Type" Width="200px">
                        <asp:ListItem Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [Net_Type] FROM [CellDatas]" OnSelected="SqlDataSource3_Selected" OnUpdated="SqlDataSource3_Updated"></asp:SqlDataSource>
                    </div>
                <div style="display:block;margin-top:10px">
                    <asp:TextBox ID="TextBox5" runat="server" ReadOnly="True" Width="200px" Font-Bold="True" ToolTip="Show the entries with the selected Data Activity state">Data Activity</asp:TextBox>
                    <asp:DropDownList ID="DropDownList6" AppendDataBoundItems="true" runat="server" DataSourceID="SqlDataSource4" DataTextField="Data_Activity" DataValueField="Data_Activity" Width="200px">
                        <asp:ListItem Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:SQLNetsAroundConnectionString %>" SelectCommand="SELECT DISTINCT [Data_Activity] FROM [CellDatas]" OnSelected="SqlDataSource4_Selected" OnUpdated="SqlDataSource4_Updated"></asp:SqlDataSource>
                    <br />
            </div>
            
           
            
                <div style="display:block;margin-top:10px">
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
                
           
            
            <div style="display:block;margin-top:10px">
              <asp:TextBox ID="TextBoxC1" runat="server" TextMode="MultiLine" Height="200px" Width="800px" OnPreRender="TextBoxC1_PreRender"></asp:TextBox>
            </div>
                

</asp:Content>

