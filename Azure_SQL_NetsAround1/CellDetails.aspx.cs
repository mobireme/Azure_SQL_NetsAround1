using BingMapsRESTToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Azure_SQL_NetsAround1
{
    public partial class CellDetails : System.Web.UI.Page
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

        List<CellStrengthStats.CellData> cell_list = new List<CellStrengthStats.CellData>();
        static List<CellStrengthStats.cellDisp1> celldisplay1 = new List<CellStrengthStats.cellDisp1>();
        


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

                if (Session["UserCellDetails"] != null) DropDownList1.SelectedIndex = (int)Session["UserCellDetails"];
                if (Session["StartDateCellDetails"] != null) Calendar1.SelectedDate = (DateTime)Session["StartDateCellDetails"];
                if (Session["EndDateCellDetails"] != null) Calendar2.SelectedDate = (DateTime)Session["EndDateCellDetails"];
                if (Session["MinStrengthCellDetails"] != null) DropDownList2.SelectedIndex = (int)Session["MinStrengthCellDetails"];
                if (Session["MaxStrengthCellDetails"] != null) DropDownList3.SelectedIndex = (int)Session["MaxStrengthCellDetails"];
                if (Session["DataConnCellDetails"] != null) DropDownList4.SelectedIndex = (int)Session["DataConnCellDetails"];
                if (Session["NetTypeCellDetails"] != null) DropDownList5.SelectedIndex = (int)Session["NetTypeCellDetails"];
                if (Session["DataActCellDetails"] != null) DropDownList6.SelectedIndex = (int)Session["DataActCellDetails"];

                if (Session["Grid1DataCellDetails"] != null)
                {
                    List<CellStrengthStats.cellDisp1> grid1_data = (List<CellStrengthStats.cellDisp1>)Session["Grid1DataCellDetails"];
                    GridView1.DataSource = grid1_data;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                }
                if (Session["Grid1SelectedRowCellDetails"] != null)
                {
                    int grid1row = (int)Session["Grid1SelectedRowCellDetails"];
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBoxC1.Text = "";
            GridView1.Visible = false;
            string user_name = DropDownList1.SelectedValue;

            string date_string1 = Calendar1.SelectedDate.Date.ToString();
            string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
            string testDate1 = $@"'{date_string1}'";
            string testDate2 = $@"'{date_string2}'";


            string sqlType = "CELLSTRENGTH";

            string where_string = "WHERE ";

            where_string = where_string + $@"SaveTime >= {testDate1} AND SaveTime <= {testDate2}";
            bool firstCondition = false;

            if (DropDownList2.SelectedValue != "")
            {
                string min_signal = DropDownList2.SelectedValue;
                string sqlstr = $@"'{min_signal}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"Strength >= {sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND Strength >= {sqlstr}";
                }
                firstCondition = false;

            }

            if (DropDownList3.SelectedValue != "")
            {
                string max_signal = DropDownList3.SelectedValue;
                string sqlstr = $@"'{max_signal}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"Strength <= {sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND Strength <= {sqlstr}";
                }
                firstCondition = false;

            }


            if (user_name != "")                                // If no username given, search with all users
            {
                
                string sqlstr = $@"'{user_name}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"UserName={sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND UserName={sqlstr}";
                }
                firstCondition = false;

            }

            if (DropDownList4.SelectedValue != "")                                // Data Connection Status
            {
                string dconn = DropDownList4.SelectedValue;
                string sqlstr = $@"'{dconn}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"DataConnState={sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND DataConnState={sqlstr}";
                }
                firstCondition = false;

            }

            if (DropDownList5.SelectedValue != "")                                // Network Type
            {
                string ntype = DropDownList5.SelectedValue;
                string sqlstr = $@"'{ntype}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"Net_Type={sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND Net_Type={sqlstr}";
                }
                firstCondition = false;

            }

            if (DropDownList6.SelectedValue != "")                                // Data Activity Status
            {
                string dact = DropDownList6.SelectedValue;
                string sqlstr = $@"'{dact}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"Data_Activity={sqlstr}";
                }
                else
                {
                    where_string = where_string + $@" AND Data_Activity={sqlstr}";
                }
                firstCondition = false;

            }

            string CellDetails = $@"
            SELECT Strength,SaveTime,Location,UserName,DataConnState,Net_Type,Data_Activity
            FROM {dbname}
            {where_string}
            ORDER BY SaveTime ASC;";
            inputstring = CellDetails;
            var sqlget = new SQL_query();
            outputstring = sqlget.SQL_read(inputstring, sqlType);

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
                TextBoxC1.Text = "No matching records.\n";
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
                    celldisplay1.Add(new CellStrengthStats.cellDisp1 { Time = line_entries[1], Location = line_entries[2], Cell_Strength = line_entries[0], DataConnState = line_entries[4], Net_Type = line_entries[5], Data_Activity = line_entries[6], User = line_entries[3] });
            
                }
            } catch (Exception ex)
            {
                TextBoxC1.Text = "Error in extracting the SQL response data\n";
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
                TextBoxC1.Text = "Error in extracting the Gridview command: " + ex.Message;
            }


            if (e.CommandName == "Details")
            {                                                       // No "Details" command at the moment, just the "Show Map" command available
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
                    TextBoxC1.Text = "Error in extracting the location data from the Gridview: " + ex.Message;
                    return;
                }
                if (loc_entry != "NO_LOCATION")
                {
                    try
                    {
                        string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });
                        string time_entry = GridView1.SelectedRow.Cells[1].Text;
                        string strength_entry = GridView1.SelectedRow.Cells[3].Text;

                        cell_list.Add(new CellStrengthStats.CellData { ctime = time_entry, clat = double.Parse(lat_lon_str[0]), clon = double.Parse(lat_lon_str[1]), cstrength = strength_entry });
                    } catch (Exception ex)
                    {
                        TextBoxC1.Text = "Error in building the Celldata list for map view: " + ex.Message;
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
                    }
                    catch (Exception ex)
                    {
                        TextBoxC1.Text = "Error in creating the map list nr 1: " + ex.Message;
                        return;
                    }

                }

                // These will be passed together with pushpins to MapDisplay at the end of this module

                try
                {
                    center_lat = cell_list[0].clat;
                    center_lon = cell_list[0].clon;
                }
                catch (Exception ex)
                {
                    TextBoxC1.Text = "Error in the map center coordinates: " + ex.Message;
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
                    TextBoxC1.Text = TextBoxC1.Text + "Error in list nr 2 parameter init: " + ex.Message;
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

                        }
                        else
                        {
                            int siglevel = Int32.Parse(cell_list[j2].cstrength) / 10;
                            switch (siglevel)
                            {
                                // IconStyles 28 = red, 27 = purple, 26 = green
                                case 0:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 28, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });
                                    break;

                                case 1:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 27, Location = new Coordinate(cell_list[j2].clat, cell_list[j2].clon) });
                                    break;

                                default:
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
                    TextBoxC1.Text = TextBoxC1.Text + "Error in list nr 2 creation: " + ex.Message;
                    return;
                }

                try
                {
                    center_lat = min_lat + (max_lat - min_lat) / 2;
                    center_lon = min_lon + (max_lon - min_lon) / 2;

                }
                catch (Exception ex)
                {
                    TextBoxC1.Text = TextBoxC1.Text + "Error in map list nr 2 center coordinates: " + ex.Message;
                    return;
                }



            }

            // Save the imageryrequest parameters for passing to MapDisplay routine

            Session["CenterLat"] = center_lat;
            Session["CenterLon"] = center_lon;
            Session["ZoomLevel"] = selected_zoomlevel;
            Session["Pushpins"] = pushpins;

            Session["Caller"] = "CellDetails.aspx";
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
            Session["UserCellDetails"] = DropDownList1.SelectedIndex;
            Session["StartDateCellDetails"] = Calendar1.SelectedDate;
            Session["EndDateCellDetails"] = Calendar2.SelectedDate;
            Session["Grid1DataCellDetails"] = celldisplay1;
            Session["MinStrengthCellDetails"] = DropDownList2.SelectedIndex;
            Session["MaxStrengthCellDetails"] = DropDownList3.SelectedIndex;
            Session["DataConnCellDetails"] = DropDownList4.SelectedIndex;
            Session["NetTypeCellDetails"] = DropDownList5.SelectedIndex;
            Session["DataActCellDetails"] = DropDownList6.SelectedIndex;
            
            if (GridView1.SelectedIndex != -1)
            {
                Session["Grid1SelectedRowCellDetails"] = GridView1.SelectedIndex;
            }


        }

        private void clear_session()
        {
            Session.Remove("UserCellDetails");
            Session.Remove("StartDateCellDetails");
            Session.Remove("EndDateCellDetails");
            Session.Remove("Grid1DataCellDetails");
            Session.Remove("Grid1SelectedRowCellDetails");
            Session.Remove("MinStrengthCellDetails");
            Session.Remove("MaxStrengthCellDetails");
            Session.Remove("DataConnCellDetails");
            Session.Remove("NetTypeCellDetails");
            Session.Remove("DataActCellDetails");

        }

        protected void Button2_Click(object sender, EventArgs e)                // Show Map clicked
        {
            TextBoxC1.Text = "";
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
                        cell_list.Add(new CellStrengthStats.CellData { ctime = line_entries[1], clat = double.Parse(lat_lon_str[0]), clon = double.Parse(lat_lon_str[1]), cstrength = line_entries[0] });
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
        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource2_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource3_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }


        }

        protected void SqlDataSource3_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource4_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource4_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxC1.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }
    }
}