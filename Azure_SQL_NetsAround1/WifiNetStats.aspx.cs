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
    public partial class WifiNetStats : System.Web.UI.Page
    {
        public static System.Drawing.Color colorDef;
        public static List<wifiDisp3> wifidisplay3 = new List<wifiDisp3>();
        static DateTime date_select1 = DateTime.Now.Date;
        static DateTime date_select2 = DateTime.Now.Date;
        int selected_zoomlevel = 12;
        string dbname = "dbo.WifiScans";
        string dbdetails = "dbo.WifiScanDetails";


        protected void Page_Load(object sender, EventArgs e)
        {
            int selected_index;
            Calendar1.SelectedDate = date_select1;
            Calendar2.SelectedDate = date_select2;

            try
            {
                colorDef = GridView1.BackColor;
                if (Session["UserWifiDetail"] != null)
                {
                    selected_index = (int)Session["UserWifiDetail"];
                    DropDownList1.SelectedIndex = selected_index;
                }
                if (Session["SSIDWifiDetail"] != null)
                {
                    selected_index = (int)Session["SSIDWifiDetail"];
                    DropDownList2.SelectedIndex = selected_index;
                }
                if (Session["BSSIDWifiDetail"] != null)
                {
                    selected_index = (int)Session["BSSIDWifiDetail"];
                    DropDownList3.SelectedIndex = selected_index;
                }
                if (Session["StartDate"] != null) Calendar1.SelectedDate = (DateTime)Session["StartDate"];
                if (Session["EndDate"] != null) Calendar2.SelectedDate = (DateTime)Session["EndDate"];
                if (Session["MinStrength"] != null) TextBox5.Text = (string)Session["MinStrength"];
                if (Session["SSIDSearch"] != null) TextBox8.Text = (string)Session["SSIDSearch"];
                if (Session["WifiType"] != null) DropDownList4.SelectedIndex = (int)Session["WifiType"];
                if (Session["GridData"] != null)
                {
                    List<wifiDisp3> grid_data = (List<wifiDisp3>)Session["GridData"];
                    GridView1.DataSource = grid_data;
                    GridView1.DataBind();
                    GridView1.Visible = true;

                }
                if (Session["GridRow"] != null)
                {
                    GridView1.SelectedIndex = (int)Session["GridRow"];
                    GridView1.SelectedRow.BackColor = System.Drawing.Color.Yellow;

                }
            } catch (Exception ex)
            {
                TextBox11.Text = "Error in reading the session data";
            }
            
            clear_session();


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox11.Text = "";
            
            GridView1.Visible = false;
            
            string user_name = DropDownList1.SelectedValue;

            string sqlType = "WIFISIGNAL";
            string date_string1 = Calendar1.SelectedDate.Date.ToString();
            
            string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
            
            string testDate1 = $@"'{date_string1}'";
            string testDate2 = $@"'{date_string2}'";

            string signal_level = "";

            string username = "";
            string ssidsearch = "";
            string wifitype = "All";
            string where_string = "WHERE ";

            where_string = where_string + $@"SaveTime >= {testDate1} and SaveTime <= {testDate2}";
            bool firstCondition = false;

            if (TextBox5.Text != "")
            {
                signal_level = TextBox5.Text;
                try
                {
                    int sgn = Convert.ToInt16(signal_level);

                    if ((sgn > 0) | (sgn < -100))
                    {
                        TextBox11.Text = "Invalid signal strength value. Give a negative dBm value between -1 and -100.\n";
                        return;
                    }
                } catch (Exception ex)
                {
                    TextBox11.Text = "Invalid signal strength value: " + ex.Message + ". Give a negative dBm value between -1 and -100.\n";
                    return;
                }

                string sigstr = $@"'{signal_level}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"Level >= {sigstr}";
                }
                else
                {
                    where_string = where_string + $@" AND Level >= {sigstr}";
                }
                firstCondition = false;

            }

            if ((TextBox8.Text != "") | (DropDownList2.SelectedValue != ""))
            {
                if (DropDownList2.SelectedValue != "")
                {
                    ssidsearch = DropDownList2.SelectedValue;
                } else
                {
                    ssidsearch = TextBox8.Text;

                }

                string ssidstr = $@"'{ssidsearch}'";
                if (firstCondition)
                {
                    where_string = where_string + $@"SSID={ssidstr}";
                }
                else
                {
                    where_string = where_string + $@" AND SSID={ssidstr}";
                }
                firstCondition = false;

            }

            if (DropDownList4.SelectedValue != "All")
            {
                wifitype = DropDownList4.SelectedValue;
                if (wifitype != "Open")
                {
                    if (wifitype == "WPA") wifitype = "WPA-";
                    string typestr = $@"'%{wifitype}%'";
                    if (firstCondition)
                    {
                        where_string = where_string + $@"Capabilities LIKE {typestr}";
                    }
                    else
                    {
                        where_string = where_string + $@" AND Capabilities LIKE {typestr}";
                    }
                } else
                {
                    if (firstCondition)
                    {
                        where_string = where_string + $@"Capabilities NOT LIKE '%WEP%' AND Capabilities NOT LIKE '%WPA%' AND Capabilities NOT LIKE '%WPS%'";
                    }
                    else
                    {
                        where_string = where_string + $@" AND Capabilities NOT LIKE '%WEP%' AND Capabilities NOT LIKE '%WPA%' AND Capabilities NOT LIKE '%WPS%'";
                    }
                }
                firstCondition = false;

            } 



            string Signal_Top;
            if (where_string != "WHERE ")                                                       // WHERE conditions selected
            {
                if (DropDownList3.SelectedValue != "All")                                       // Distinct BSSID selected
                {
                    Signal_Top = $@"
                    SELECT a.SSID,a.BSSID,a.Capabilities,a.Frequency,a.Level,a.WifiItemId 
                    FROM {dbdetails} a 
                    INNER JOIN
                    (SELECT BSSID,
                    MAX(id) as id
                    FROM {dbdetails}
                    {where_string}
                    GROUP BY BSSID ) AS b
                    ON a.BSSID = b.BSSID 
                    AND a.id = b.id;";
                }
                else
                {                                                                               // All BSSIDs selected
                    Signal_Top = $@"                                                            
                    SELECT SSID,BSSID,Capabilities,Frequency,Level,WifiItemId
                    FROM {dbdetails} 
                    {where_string}
                    ORDER BY Level DESC;";

                }

            } else
            {                                                                                   // No WHERE conditions
                Signal_Top = $@"
            SELECT SSID,BSSID,Capabilities,Frequency,Level,WifiItemId
            FROM {dbdetails} 
            ORDER BY Level DESC;";
            }


            //TextBox11.Text = Signal_Top + "\n";                                       // Turn on if SQL debugging needed
            
            var sqlget = new SQL_query();
            string strength_outputstring = sqlget.SQL_read(Signal_Top, sqlType);

            if (strength_outputstring != "")
            {
                if (strength_outputstring.StartsWith("Error"))
                {
                    TextBox11.Text = strength_outputstring;
                }
                else
                {
                    show_wifi_signal(strength_outputstring);
                }

                
            }
            else
            {
                TextBox11.Text = "No matching records found\n";
            }
            

           
        }


        private void show_wifi_signal(string inputstring)
        {

            string parent_id = "";

            string sqlType = "WIFITIME";
            string[] lines;
            string[] line_entries;


            wifidisplay3.Clear();

            
            try
            {
                lines = inputstring.Split(new Char[] { '\n' });
            } catch (Exception ex)
            {
                TextBox11.Text = "Error in extracting the lines: " + ex.Message;
                return;
            }
            
            int i1 = 0;
            for (i1 = 0; i1 < lines.Length - 1; i1++)
            {
                try
                {
                    line_entries = lines[i1].Split(new Char[] { '\t' });

                    parent_id = line_entries[5];

                } catch (Exception ex)
                {
                    TextBox11.Text = "Error in extracting the parent id: " + ex.Message;
                    return;
                }

                string Wifi_byId = $@"
            SELECT WifisFound,SaveTime,Location,UserName,Id 
            FROM {dbname} 
            WHERE Id={$@"'{parent_id}'"} 
            ORDER BY WifisFound DESC;";


                var sqlget = new SQL_query();
                string strength_outputstring = sqlget.SQL_read(Wifi_byId, sqlType);

                if (strength_outputstring.StartsWith("Error"))
                {
                    TextBox11.Text = strength_outputstring;
                    return;
                }
                


                try
                {
                    string[] w_entry = strength_outputstring.Split(new Char[] { '\t' });
                    if (w_entry.Length > 4)
                    {
                        // Show entry if no specific username selected or if the entry matches with the given name

                        if ((DropDownList1.SelectedValue == "") | (w_entry[3] == DropDownList1.SelectedValue))
                        {
                            wifidisplay3.Add(new wifiDisp3 { Time = w_entry[0], Location = w_entry[1], SSID = line_entries[0], BSSID = line_entries[1], Capabilities = line_entries[2], Freq_str = line_entries[3], Level_str = line_entries[4], User = w_entry[3] });
                        }
                    }
                    else
                    {
                        wifidisplay3.Add(new wifiDisp3 { Time = "Time not found", Location = "Location not found", SSID = line_entries[0], BSSID = line_entries[1], Capabilities = line_entries[2], Freq_str = line_entries[3], Level_str = line_entries[4] });
                    }
                } catch (Exception ex)
                {
                    TextBox11.Text = "Error in creating Wifi signal list: " + ex.Message;
                    return;
                }
            }


            try
            {
               
                GridView1.DataSource = wifidisplay3;
                GridView1.DataBind();
                GridView1.Visible = true;

            }
            catch (Exception ex)
            {
                TextBox11.Text = "Gridview error: " + ex.Message;

            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            date_select1 = Calendar1.SelectedDate;
            
            GridView1.Visible = false;
            TextBox11.Text = "";
            
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            date_select2 = Calendar2.SelectedDate;

            GridView1.Visible = false;
            TextBox11.Text = "";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Show the selected place on the map

            TextBox11.Text = "";
            string loc_entry;
            List<wifi_d_map> wifiDmap;

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
                TextBox11.Text = TextBox11.Text + "\nSelected index: " + gr_index.ToString() + "\n";


                wifiDmap = new List<wifi_d_map>();


                wifiDmap.Clear();
                loc_entry = GridView1.SelectedRow.Cells[2].Text;
            } catch (Exception ex)
            {
                TextBox11.Text = "Error in gridview row command: " + ex.Message;
                return;
            }

            if (loc_entry != "NO_LOCATION")
            {
                try
                {
                    string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });
                    string time_entry = GridView1.SelectedRow.Cells[1].Text;
                    string strength_entry = GridView1.SelectedRow.Cells[7].Text;

                    wifiDmap.Add(new wifi_d_map { wtime = time_entry, wlat = double.Parse(lat_lon_str[0]), wlon = double.Parse(lat_lon_str[1]), wstrength = strength_entry });

                } catch (Exception ex)
                {
                    TextBox11.Text = "Error in creating the map list entry: " + ex.Message;
                    return;
                }

                show_d_map_image(wifiDmap);


            }
            else
            {
                TextBox11.Text = "No location info available";
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Show the locations from the Gridview on the map

            int max_map_count = 100;
            TextBox11.Text = "";

            List<wifi_d_map> wifiDmap = new List<wifi_d_map>();


            wifiDmap.Clear();
            int i1 = 0;
            int mcount = 0;

            try
            {
                for (i1 = 0; i1 < GridView1.Rows.Count; i1++)
                {
                    GridView1.SelectedIndex = i1;
                    string loc_entry = GridView1.SelectedRow.Cells[2].Text;
                    if (loc_entry != "NO_LOCATION")
                    {
                        string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });
                        string time_entry = GridView1.SelectedRow.Cells[1].Text;
                        string strength_entry = GridView1.SelectedRow.Cells[7].Text;

                        wifiDmap.Add(new wifi_d_map { wtime = time_entry, wlat = double.Parse(lat_lon_str[0]), wlon = double.Parse(lat_lon_str[1]), wstrength = strength_entry });
                        mcount = mcount + 1;
                    }
                }
            } catch (Exception ex)
            {

                TextBox11.Text = "Error in creating the Wifi map list from the current Wifi list: " + ex.Message;
                return;
            }

            if (mcount < max_map_count)
            {
                if (mcount != 0)
                {
                    
                    show_d_map_image(wifiDmap);


                }
                else
                {
                    TextBox11.Text = "No location info available";
                    
                }
            } else
            {
                TextBox11.Text = "Too many locations to show, try to shorten the list, max location count is " + max_map_count.ToString();
            }

            

        }


        // SHOW THE DATA ON MAP (BING MAPS)
        public void show_d_map_image(List<wifi_d_map> dmap)
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

            if (dmap.Count <= 10)                       // TOP 10 AND BOTTOM 10, or less than 10
            {
                int i1 = 0;

                try
                {

                    while (i1 < dmap.Count)
                    {
                        if (dmap.Count == 1)                            // Just one entry, probably for the detailed view, set only the marker color in this specific case
                        {
                            int wstrengthlevel = (Int32.Parse(dmap[i1].wstrength) + 10) / (-30);         // Map wifi dBm to 0 - 3, 0 = -10dBm and 3 = -100dBm or lower

                            // Use switch to select specific colored marker to match with the Wifi count
                            switch (wstrengthlevel)
                            {
                                case 0:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 16, Location = new Coordinate(dmap[i1].wlat, dmap[i1].wlon) });
                                    break;

                                case 1:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 15, Location = new Coordinate(dmap[i1].wlat, dmap[i1].wlon) });
                                    break;

                                case 2:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 34, Location = new Coordinate(dmap[i1].wlat, dmap[i1].wlon) });
                                    break;

                                default:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 22, Location = new Coordinate(dmap[i1].wlat, dmap[i1].wlon) });
                                    break;
                            }
                        }
                        else
                        {                                           // More than just 1 entry to show


                            pushpins.Add(new ImageryPushpin() { Label = (i1 + 1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[i1].wlat, dmap[i1].wlon) });

                        }
                        i1++;
                    }
                } catch (Exception ex)
                {
                    TextBox11.Text = "Error in creating map list 1: " + ex.Message;
                    return;
                }

                // This request used when < 10 pushpins


                Session["CenterLat"] = dmap[0].wlat;
                Session["CenterLon"] = dmap[0].wlon;
                Session["ZoomLevel"] = selected_zoomlevel;
                Session["Pushpins"] = pushpins;

            }
            else
            {                                                                           // More than 10 locations to show, use different type of request
                decimal skips = 0;
                decimal skipcounter = 0;
                int i2 = 0;
                int j2 = 0;
                try
                {
                    
                    skips = (decimal)dmap.Count / 100;
                    if (skips < 1)
                    {
                        skips = 1;                                  // If less than 1000 entries, no need to skip any entries, set to 1 to select the next entry
                    }


                }
                catch (Exception ex)
                {
                    TextBox11.Text = TextBox11.Text + "Error in map list 2 parameter init: " + ex.Message;
                    return;
                }

                try
                {
               
                    max_lat = dmap[0].wlat;
                    min_lat = dmap[0].wlat;
                    max_lon = dmap[0].wlon;
                    min_lon = dmap[0].wlon;

                    while ((j2 < dmap.Count) & (pushpins.Count < 100))
                    {
                        if (j2 == 0)                                    // If need to have a special start icon, define it here
                        {
                            pushpins.Add(new ImageryPushpin() { Label = (j2+1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[j2].wlat, dmap[j2].wlon) });

                        }
                        else
                        {
                            int wstrengthlevel = (Int32.Parse(dmap[j2].wstrength) +10) / (-30);
                            // Use switch to select specific colored marker to match with the Wifi strength
                            switch (wstrengthlevel)
                            {
                                case 0:
                                    
                                    // IconStyle: 16 = green circle, 15=blue, 22=red
                                    pushpins.Add(new ImageryPushpin() { Label = (j2 + 1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[j2].wlat, dmap[j2].wlon) });
                                    break;

                                case 1:
                                    
                                    // IconStyle: 16 = green circle, 15=blue, 22=red
                                    pushpins.Add(new ImageryPushpin() { Label = (j2 + 1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[j2].wlat, dmap[j2].wlon) });
                                    break;

                                case 2:
                                    
                                    // IconStyle: 16 = green circle, 15=blue, 22=red
                                    pushpins.Add(new ImageryPushpin() { Label = (j2 + 1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[j2].wlat, dmap[j2].wlon) });
                                    break;

                                default:
                                    
                                    // IconStyle: 16 = green circle, 15=blue, 22=red
                                    pushpins.Add(new ImageryPushpin() { Label = (j2 + 1).ToString(), IconStyle = 15, Location = new Coordinate(dmap[j2].wlat, dmap[j2].wlon) });
                                    break;
                            }

                            if (dmap[j2].wlat > max_lat) max_lat = dmap[j2].wlat;
                            if (dmap[j2].wlat < min_lat) min_lat = dmap[j2].wlat;
                            if (dmap[j2].wlon > max_lon) max_lon = dmap[j2].wlon;
                            if (dmap[j2].wlon < min_lon) min_lon = dmap[j2].wlon;

                        }
                        i2 = j2;                                    // To be used in diagnostics message below
                        skipcounter = skipcounter + skips;
                        j2 = (int)skipcounter;
                    }



                }
                catch (Exception ex)
                {
                    TextBox11.Text = TextBox11.Text + "Error in map list 2 building: " + ex.Message;
                    return;
                }


                try
                {
                 
                    center_lat = min_lat + (max_lat - min_lat) / 2;
                    center_lon = min_lon + (max_lon - min_lon) / 2;



                    // The request below when more than 10 locations to show

                    Session["CenterLat"] = center_lat;
                    Session["CenterLon"] = center_lon;
                    Session["ZoomLevel"] = selected_zoomlevel;
                    Session["Pushpins"] = pushpins;

                }
                catch (Exception ex)
                {
                    TextBox11.Text = TextBox11.Text + "Error in imagery request: " + ex.Message;
                    return;
                }

            }

            Session["Caller"] = "WifiNetStats.aspx";


            save_session();

            try
            {
                Response.Redirect("MapDisplay.aspx");
            }
            catch (Exception ex)
            {
                TextBox11.Text = "Error in starting MapDisplay: " + ex.Message;
            }



        }


        public class wifiDisp3
        {
            public string Time { get; set; }
            public string Location { get; set; }
            public string SSID { get; set; }
            public string BSSID { get; set; }
            public string Capabilities { get; set; }
            public string Freq_str { get; set; }
            public string Level_str { get; set; }
            public string User { get; set; }

        }

        public class wifi_d_map
        {
            public string wtime { get; set; }
            public double wlat { get; set; }
            public double wlon { get; set; }
            public string wstrength { get; set; }

        }



        protected void save_session ()
        {
            Session["UserWifiDetail"] = DropDownList1.SelectedIndex;
            Session["SSIDWifiDetail"] = DropDownList2.SelectedIndex;
            Session["BSSIDWifiDetail"] = DropDownList3.SelectedIndex;
            Session["StartDate"] = Calendar1.SelectedDate;
            Session["EndDate"] = Calendar2.SelectedDate;
            Session["MinStrength"] = TextBox5.Text;
            Session["SSIDSearch"] = TextBox8.Text;
            Session["WifiType"] = DropDownList4.SelectedIndex;
            Session["GridData"] = wifidisplay3;
            Session["GridRow"] = GridView1.SelectedIndex;

            

        }

        private void clear_session()
        {
            Session.Remove("UserWifiDetail");
            Session.Remove("SSIDWifiDetail");
            Session.Remove("BSSIDWifiDetail");
            Session.Remove("StartDate");
            Session.Remove("EndDate");
            Session.Remove("MinStrength");
            Session.Remove("SSIDSearch");
            Session.Remove("WifiType");
            Session.Remove("GridData");
            Session.Remove("GridRow");

            
        }

        protected void TextBox11_PreRender(object sender, EventArgs e)
        {
            if (TextBox11.Text != "")
            {

                TextBox11.Style.Add("display", "block");
            }
            else
            {
                TextBox11.Style.Add("display", "none");
            }

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBox11.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBox11.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBox11.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource2_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBox11.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }
    }
}