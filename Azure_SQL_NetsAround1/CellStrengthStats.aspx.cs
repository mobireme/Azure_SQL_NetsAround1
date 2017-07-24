using BingMapsRESTToolkit;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Azure_SQL_NetsAround1
{
    public partial class CellStrengthStats : System.Web.UI.Page
    {
        static string outputstring;
        static string inputstring;
        static DateTime date_select1 = DateTime.Now;
        static DateTime date_select2 = DateTime.Now;
        static string date_string;

        static string testDate;

        string Cell_Top10;

        string Cell_Low10;

        string Cell_All;

        int selected_zoomlevel = 12;
        double[] lat = new double[10];
        double[] lon = new double[10];

        static List<CellData> cell_list = new List<CellData>();
        static List<cellDisp1> celldisplay1 = new List<cellDisp1>();

        static string curr_status = "";
        string dbname = "dbo.CellDatas";

        public static System.Drawing.Color colorDef;


        protected void Page_Load(object sender, EventArgs e)
        {
            Calendar1.SelectedDate = date_select1;
            Calendar2.SelectedDate = date_select2;

            try
            {
                colorDef = GridView1.BackColor;

                if (Session["UserCellStats"] != null) DropDownList1.SelectedIndex = (int)Session["UserCellStats"];
                if (Session["StartDateCellStats"] != null) Calendar1.SelectedDate = (DateTime)Session["StartDateCellStats"];
                if (Session["EndDateCellStats"] != null) Calendar2.SelectedDate = (DateTime)Session["EndDateCellStats"];
                if (Session["Grid1DataCellStats"] != null)
                {
                    List<cellDisp1> grid1_data = (List<cellDisp1>)Session["Grid1DataCellStats"];
                    GridView1.DataSource = grid1_data;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                }

                if (Session["Grid1SelectedRowCellStats"] != null)
                {

                    int grid1row = (int)Session["Grid1SelectedRowCellStats"];
                    GridView1.SelectedIndex = grid1row;
                    GridView1.SelectedRow.BackColor = System.Drawing.Color.Yellow;
                    GridView1.SelectRow(grid1row);
                }
            }
            catch (Exception ex)
            {
                TextBoxC1.Text = "Error in restoring sesson data.\n";
            }

            clear_session();

        }

        // SHOW TOP 10
        protected void ButtonC1_Click(object sender, EventArgs e)
        {
            TextBoxC1.Text = "";
            GridView1.Visible = false;

            string user_name = DropDownList1.SelectedValue;

            string date_string1 = Calendar1.SelectedDate.Date.ToString();
            string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
            string testDate1 = $@"'{date_string1}'";
            string testDate2 = $@"'{date_string2}'";

            
            string sqlType = "CELLSTRENGTH";

            if (user_name == "")                                // If no username given, search with all users
            {
                Cell_Top10 = $@"
            SELECT TOP(10) Strength,SaveTime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2}
            ORDER BY Strength DESC;";

            }
            else
            {                                                   // Username given, filter with username
                Cell_Top10 = $@"
            SELECT TOP(10) Strength,Savetime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"}
            ORDER BY Strength DESC;";
            }


            inputstring = Cell_Top10;
            var sqlget = new SQL_query();
            outputstring = sqlget.SQL_read(inputstring, sqlType);

            curr_status = "TOP";

            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxC1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxC1.Text = "No matching records found.\n";
            }
            

        }

        // SHOW BOTTOM 10
        protected void ButtonC2_Click(object sender, EventArgs e)
        {
            TextBoxC1.Text = "";
            GridView1.Visible = false;

            string user_name = DropDownList1.SelectedValue;

            string date_string1 = Calendar1.SelectedDate.Date.ToString();
            string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
            string testDate1 = $@"'{date_string1}'";
            string testDate2 = $@"'{date_string2}'";

            string sqlType = "CELLSTRENGTH";

            if (user_name == "")                                // If no username given, search with all users
            {
                Cell_Low10 = $@"
            SELECT TOP(10) Strength,Savetime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2}
            ORDER BY Strength ASC;";
            }
            else
            {                                                   // Username given, filter with username
                Cell_Low10 = $@"
            SELECT TOP(10) Strength,Savetime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"}
            ORDER BY Strength ASC;";
            }

            inputstring = Cell_Low10;
            var sqlget = new SQL_query();
            outputstring = sqlget.SQL_read(inputstring, sqlType);

            curr_status = "BOTTOM";

            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxC1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxC1.Text = "No matching records found.\n";
            }
            

        }

        //SHOW ALL
        protected void ButtonC3_Click(object sender, EventArgs e)
        {
            TextBoxC1.Text = "";
            GridView1.Visible = false;
            
            string user_name = DropDownList1.SelectedValue;

            string date_string1 = Calendar1.SelectedDate.Date.ToString();
            string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
            string testDate1 = $@"'{date_string1}'";
            string testDate2 = $@"'{date_string2}'";

            
            string sqlType = "CELLSTRENGTH";

            if (user_name == "")                                // If no username given, search with all users
            {
                Cell_All = $@"
            SELECT Strength,SaveTime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2}
            ORDER BY Text;";
            }
            else
            {                                                   // Username given, filter with username
                Cell_All = $@"
            SELECT Strength,SaveTime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"}
            ORDER BY Text;";
            }

            inputstring = Cell_All;
            var sqlget = new SQL_query();
            outputstring = sqlget.SQL_read(inputstring, sqlType);

            //TextBoxC1.Text = outputstring;
         
            curr_status = "ALL";

            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxC1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxC1.Text = "No matching records found.\n";
            }
        }

        private void show_table(string inputstring)
        {

            celldisplay1.Clear();
            
            try
            {
                string[] lines = inputstring.Split(new Char[] { '\n' });
                int i1 = 0;
                for (i1 = 0; i1 < lines.Length - 1; i1++)
                {

                    string[] line_entries = lines[i1].Split(new Char[] { '\t' });
                    celldisplay1.Add(new cellDisp1 { Time = line_entries[1], Location = line_entries[2], Cell_Strength = line_entries[0], DataConnState = line_entries[4], Net_Type = line_entries[5], Data_Activity = line_entries[6], User = line_entries[3] });
                 

                }
            } catch (Exception ex)
            {
                TextBoxC1.Text = "Error in building the cell display list: " + ex.Message;
                return;
            }

            try
            {
                GridView1.DataSource = celldisplay1;
                GridView1.DataBind();
                GridView1.Visible = true;

                
            }
            catch (Exception ex)
            {
                TextBoxC1.Text = "Gridview error: " + ex.Message;

            }
        }


        // CALENDAR DATE CHANGED
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            date_select1 = Calendar1.SelectedDate;
            TextBoxC1.Text = "";

        }


        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            date_select2 = Calendar2.SelectedDate;
            TextBoxC1.Text = "";

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            TextBoxC1.Text = "";
            string loc_entry;

            try
            {
                if (GridView1.SelectedIndex == -1)
                {
                    colorDef = GridView1.BackColor;
                }
                else
                {
                    GridView1.SelectedRow.BackColor = colorDef;
                }

                var gr_index = Convert.ToInt32(e.CommandArgument);
                GridView1.SelectedIndex = gr_index;
                GridView1.SelectedRow.BackColor = System.Drawing.Color.Yellow;
            } catch (Exception ex)
            {
                TextBoxC1.Text = "Error in extracting the gridview row command: " + ex.Message;
                return;
            }

            if (e.CommandName == "Details")
            {                                           // No "Details" command currently, only the "Show Map" command available
            }
            else
            {

                // Show the selected place on the map

                cell_list.Clear();
                try
                {
                    loc_entry = GridView1.SelectedRow.Cells[2].Text;
                } catch (Exception ex)
                {
                    TextBoxC1.Text = "Error in reading the selected row cell: " + ex.Message;
                    return;
                }
                
                if (loc_entry != "NO_LOCATION")
                {
                    try
                    {
                        string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });
                        string time_entry = GridView1.SelectedRow.Cells[1].Text;
                        string strength_entry = GridView1.SelectedRow.Cells[3].Text;

                        cell_list.Add(new CellData { ctime = time_entry, clat = double.Parse(lat_lon_str[0]), clon = double.Parse(lat_lon_str[1]), cstrength = strength_entry });
                    } catch (Exception ex)
                    {
                        TextBoxC1.Text = "Error in creating the CellData list: " + ex.Message;
                        return;
                    }
                    show_map_image();
                }
                else
                {
                    TextBoxC1.Text = "No location info available\n";
                }
            }

        }


        // SHOW ON MAP CLICKED
        protected void ButtonC4_Click(object sender, EventArgs e)
        {
            TextBoxC1.Text = "";
            string[] cellStrength = new string[10];
            string[] locstr = new string[10];

            cell_list.Clear();

            try
            {
                string[] lines = outputstring.Split(new Char[] { '\n' });
                int i1 = 0;
                for (i1 = 0; i1 < lines.Length - 1; i1++)
                {

                    string[] line_entries = lines[i1].Split(new Char[] { '\t' });
                    string loc_entry = line_entries[2];
                    string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });

                    if (loc_entry != "NO_LOCATION")
                    {
                        cell_list.Add(new CellData { ctime = line_entries[1], clat = double.Parse(lat_lon_str[0]), clon = double.Parse(lat_lon_str[1]), cstrength = line_entries[0] });
                    }


                }
            } catch (Exception ex)
            {
                TextBoxC1.Text = "Error in building the CellData list: " + ex.Message;
                return;
            }

            if (cell_list.Count > 0)
            {
                show_map_image();
            }
            else
            {
                TextBoxC1.Text = "No location info available\n";
            }

        }

        // SHOW MAP 
        public void show_map_image()
        {
         
            //Create an image request.

            //ImageryRequest request;


            List<ImageryPushpin> pushpins = new List<ImageryPushpin>();
            pushpins.Clear();

            double max_lat;
            double max_lon;
            double min_lat;
            double min_lon;
            double center_lat = 0;
            double center_lon = 0;


            if (cell_list.Count <= 10)
            {
                int i1 = 0;

                while (i1 < cell_list.Count)
                {
                    try
                    {
                        if (curr_status == "BOTTOM")
                        {
                            pushpins.Add(new ImageryPushpin() { Label = (i1 + 1).ToString(), IconStyle = 16, Location = new Coordinate(cell_list[i1].clat, cell_list[i1].clon) });
                        }
                        else
                        {
                            pushpins.Add(new ImageryPushpin() { Label = (i1 + 1).ToString(), IconStyle = 15, Location = new Coordinate(cell_list[i1].clat, cell_list[i1].clon) });
                        }
                        i1++;
                    } catch (Exception ex)
                    {
                        TextBoxC1.Text = "Error in creating the Pushpins list: " + ex.Message;
                        return;
                    }
                }

                // These will be passed together with pushpins to MapDisplay at the end of this module

                try
                {
                    center_lat = cell_list[0].clat;
                    center_lon = cell_list[0].clon;
                } catch (Exception ex)
                {
                    TextBoxC1.Text = "Error in the center coordinates: " + ex.Message;
                    return;
                }
            }
            else
            {
                
                decimal skips = 0;
                decimal skipcounter = 0;
                int i2 = 0;
                int j2 = 0;
                try
                {
                    skips = (decimal)cell_list.Count / 100;
                    if (skips < 1)
                    {
                        skips = 1;                                  // If less than 100 entries, no need to skip any entries, set to 1 to select the next entry
                    }

                }
                catch (Exception ex)
                {
                    TextBoxC1.Text = "Error in list nr 2 parameter init: " + ex.Message;
                    return;
                }



                     
                try
                {
                    max_lat = cell_list[0].clat;
                    min_lat = cell_list[0].clat;
                    max_lon = cell_list[0].clon;
                    min_lon = cell_list[0].clon;

                    while ((j2 < cell_list.Count) & (pushpins.Count < 100))
                    {
                        if (j2 == 0)
                        {
                            pushpins.Add(new ImageryPushpin() { Label = "START", IconStyle = 0, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });

                        } else
                        {
                            int siglevel = Int32.Parse(cell_list[j2].cstrength) / 10;
                            switch (siglevel)
                            {
                                // IconStyles 28 = red, 27 = purple, 26 = green
                                case 0:
                                    // Strength 0 - 9, red
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 28, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });
                                    break;

                                case 1:
                                    // Strength 10 - 19, purple
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 27, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });
                                    break;

                                default:
                                    // Strength 20 - 31, green
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 26, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });
                                    break;


                            }

                            if (cell_list[j2].clat > max_lat) max_lat = cell_list[j2].clat;
                            if (cell_list[j2].clat < min_lat) min_lat = cell_list[j2].clat;
                            if (cell_list[j2].clon > max_lon) max_lon = cell_list[j2].clon;
                            if (cell_list[j2].clon < min_lon) min_lon = cell_list[j2].clon;

                        }
                        i2 = j2;                                            // Save the latest entry for diagnostics message below
                        skipcounter = skipcounter + skips;
                        j2 = (int)skipcounter;
                    }
                    

                }
                catch (Exception ex)
                {
                    TextBoxC1.Text = TextBoxC1.Text + "Error in map list nr2 creation: " + ex.Message;
                    return;
                }

                try
                {
                    center_lat = min_lat + (max_lat - min_lat) / 2;
                    center_lon = min_lon + (max_lon - min_lon) / 2;

                }
                catch (Exception ex)
                {
                    TextBoxC1.Text = "Error in the center coordinates: " + ex.Message;
                    return;
                }



            }

            // Save the imageryrequest parameters for passing to MapDisplay routine

            Session["CenterLat"] = center_lat;
            Session["CenterLon"] = center_lon;
            Session["ZoomLevel"] = selected_zoomlevel;
            Session["Pushpins"] = pushpins;

            Session["Caller"] = "CellStrengthStats.aspx";
            try
            {
                save_session();                                     // Save the settings to restore them when returning back
                Response.Redirect("MapDisplay.aspx");
            }
            catch (Exception ex)
            {
                TextBoxC1.Text = "Error in starting MapDisplay: " + ex.Message;
            }


        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gr_index = GridView1.SelectedIndex;
            
        }


        protected void save_session()
        {
            Session["UserCellStats"] = DropDownList1.SelectedIndex;
            Session["StartDateCellStats"] = Calendar1.SelectedDate;
            Session["EndDateCellStats"] = Calendar2.SelectedDate;
            Session["Grid1DataCellStats"] = celldisplay1;
            if (GridView1.SelectedIndex != -1)
            {
                Session["Grid1SelectedRowCellStats"] = GridView1.SelectedIndex;
            }


        }

        private void clear_session()
        {
            Session.Remove("UserCellStats");
            Session.Remove("StartDateCellStats");
            Session.Remove("EndDateCellStats");
            Session.Remove("Grid1DataCellStats");
            Session.Remove("Grid1SelectedRowCellStats");

        }


        public class CellData
        {
            public string ctime { get; set; }
            public double clat { get; set; }
            public double clon { get; set; }

            public string cstrength { get; set; }
        }

        public class cellDisp1
        {
            public string Time { get; set; }
            public string Location { get; set; }
            public string Cell_Strength { get; set; }
            public string DataConnState { get; set; }
            public string Net_Type { get; set; }
            public string Data_Activity { get; set; }
            public string User { get; set; }

        }


        public class cellDisp2
        {
            public string SSID { get; set; }
            public string BSSID { get; set; }
            public string Capabilities { get; set; }
            public string Freq_str { get; set; }
            public string Level_str { get; set; }

        }

        protected void TextBoxC1_PreRender(object sender, EventArgs e)
        {
            if (TextBoxC1.Text != "")
            {

                TextBoxC1.Style.Add("display", "block");
            }
            else
            {
                TextBoxC1.Style.Add("display", "none");
            }

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
         
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }
            
        }

  
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }
        }
    }
}