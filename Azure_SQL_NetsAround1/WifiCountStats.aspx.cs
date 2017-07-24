using BingMapsRESTToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.ComponentModel;




namespace Azure_SQL_NetsAround1
{
    public partial class WifiCountStats : System.Web.UI.Page
    {
        static string outputstring;
        static string inputstring;
        static string mapstring;

        static DateTime date_select1 = DateTime.Now;
        static DateTime date_select2 = DateTime.Now;
        static string date_string;

        static string testDate;

        string Wifi_Top10;

        string Wifi_Low10;

        string Wifi_All;

        string datasource = "SQL";

        int selected_zoomlevel = 12;
        double[] lat = new double[10];
        double[] lon = new double[10];

        public static List<WifiData> wifilist = new List<WifiData>();
        public static List<wifiDisp1> wifidisplay1 = new List<wifiDisp1>();
        public static List<wifiDisp2> wifidisplay2 = new List<wifiDisp2>();
        public static List<string> wifi_ids = new List<string>();

        public static System.Drawing.Color colorDef;
        static string curr_status = "";
        string dbname = "dbo.WifiScans";
        string dbdetails = "dbo.WifiScanDetails";




        protected void Page_Load(object sender, EventArgs e)
        {
         
            Calendar1.SelectedDate = date_select1;
            Calendar2.SelectedDate = date_select2;

            colorDef = GridView1.BackColor;
            
            try
            {

                if (Session["UserWifiCount"] != null) DropDownList1.SelectedIndex = (int)Session["UserWifiCount"];
                if (Session["StartDateWifiCount"] != null) Calendar1.SelectedDate = (DateTime)Session["StartDateWifiCount"];
                if (Session["EndDateWifiCount"] != null) Calendar2.SelectedDate = (DateTime)Session["EndDateWifiCount"];
                if (Session["Grid1DataWifiCount"] != null)
                {
                    List<wifiDisp1> grid1_data = (List<wifiDisp1>)Session["Grid1DataWifiCount"];
                    GridView1.DataSource = grid1_data;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                }
                if (Session["Grid2DataWifiCount"] != null)
                {
                    List<wifiDisp2> grid2_data = (List<wifiDisp2>)Session["Grid2DataWifiCount"];
                    GridView2.DataSource = grid2_data;
                    GridView2.DataBind();
                    GridView2.Visible = true;

                }
                if (Session["Grid1SelectedRowWifiCount"] != null)
                {
                    int grid1row = (int)Session["Grid1SelectedRowWifiCount"];
                    GridView1.SelectedIndex = grid1row;
                    GridView1.SelectedRow.BackColor = System.Drawing.Color.Yellow;


                }
            } catch (Exception ex)
            {
                TextBoxW1.Text = "Error in restoring sesson data.\n";
            }
            clear_session();


        }

        // SHOW TOP 10
        protected void ButtonW1_Click(object sender, EventArgs e)
        {
            TextBoxW1.Text = "";
            curr_status = "TOP";
            GridView1.Visible = false;
            GridView2.Visible = false;
            string user_name = DropDownList1.SelectedValue;

            
            if (datasource == "SQL")
            {
                string date_string1 = Calendar1.SelectedDate.Date.ToString();
                string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
                string testDate1 = $@"'{date_string1}'";
                string testDate2 = $@"'{date_string2}'";

                string sqlType = "WIFICOUNT";

                if (user_name == "")                                // If no username given, search with all users
                {
                    Wifi_Top10 = $@"
            SELECT TOP(10) WifisFound,SaveTime,Location,UserName,Id 
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY WifisFound DESC;";
                }
                else
                {                                                   // Username given, filter with username
                    Wifi_Top10 = $@"
            SELECT TOP(10) WifisFound,SaveTime,Location,UserName,Id 
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY WifisFound DESC;";
                }

                inputstring = Wifi_Top10;
                var sqlget = new SQL_query();
                outputstring = sqlget.SQL_read(inputstring, sqlType);


            }
            else
            {
                outputstring = read_data_from_blob(curr_status);
                mapstring = read_map_from_blob(curr_status);
            }

            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxW1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxW1.Text = "No matching records found.\n";
            }
            

        }

        // SHOW BOTTOM 10
        protected void ButtonW2_Click(object sender, EventArgs e)
        {
            TextBoxW1.Text = "";
            curr_status = "BOTTOM";
            GridView1.Visible = false;
            GridView2.Visible = false;
            string sqlType = "WIFICOUNT";

            string user_name = DropDownList1.SelectedValue;

        
            if (datasource == "SQL")
            {
                string date_string1 = Calendar1.SelectedDate.Date.ToString();
                string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
                string testDate1 = $@"'{date_string1}'";
                string testDate2 = $@"'{date_string2}'";

                if (user_name == "")                                // If no username given, search with all users
                {
                    Wifi_Low10 = $@"
            SELECT TOP(10) WifisFound,SaveTime,Location,UserName,Id
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY WifisFound ASC;";
                }
                else
                {                                                   // Filter with user name
                    Wifi_Low10 = $@"
            SELECT TOP(10) WifisFound,SaveTime,Location,UserName,Id
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY WifisFound ASC;";

                }


                inputstring = Wifi_Low10;
                var sqlget = new SQL_query();
                outputstring = sqlget.SQL_read(inputstring, sqlType);


            }
            else
            {
                outputstring = read_data_from_blob(curr_status);
                mapstring = read_map_from_blob(curr_status);

            }

            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxW1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxW1.Text = "No matching records found.\n";
            }



        }

        // SHOW ALL
        protected void ButtonW3_Click(object sender, EventArgs e)
        {
            TextBoxW1.Text = "";
            curr_status = "ALL";
            GridView1.Visible = false;
            GridView2.Visible = false;
            string sqlType = "WIFICOUNT";

            string user_name = DropDownList1.SelectedValue;

           
            if (datasource == "SQL")
            {
                string date_string1 = Calendar1.SelectedDate.Date.ToString();
                string date_string2 = Calendar2.SelectedDate.Date.AddDays(1).ToString();
                string testDate1 = $@"'{date_string1}'";
                string testDate2 = $@"'{date_string2}'";

                if (user_name == "")                                // If no username given, search with all users
                {
                    Wifi_All = $@"
            SELECT WifisFound,SaveTime,Location,UserName,Id 
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY Text;";
                }
                else
                {                                                   // Filter with user name
                    Wifi_All = $@"
            SELECT WifisFound,SaveTime,Location,UserName,Id 
            FROM {dbname} 
            WHERE SaveTime >= {testDate1} AND SaveTime <= {testDate2} AND UserName={$@"'{user_name}'"} AND Location NOT LIKE '%NO_LOCATION%' 
            ORDER BY Text;";

                }

                inputstring = Wifi_All;
                var sqlget = new SQL_query();
                outputstring = sqlget.SQL_read(inputstring, sqlType);


            }
            else
            {
                outputstring = read_data_from_blob(curr_status);
                mapstring = read_map_from_blob(curr_status);
            }
            if (outputstring != "")
            {
                if (outputstring.StartsWith("Error"))
                {
                    TextBoxW1.Text = outputstring;
                }
                else
                {
                    show_table(outputstring);
                }

            }
            else
            {
                TextBoxW1.Text = "No matching records found.\n";
            }
        }

        // CALENDAR DATE CHANGED
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            date_select1 = Calendar1.SelectedDate;
            TextBoxW1.Text = "";
            
            GridView1.Visible = false;
            GridView2.Visible = false;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            date_select2 = Calendar2.SelectedDate;
            TextBoxW1.Text = "";

            GridView1.Visible = false;
            GridView2.Visible = false;
        }
        // SHOW ON MAP
        protected void ButtonW4_Click(object sender, EventArgs e)
        {
            TextBoxW1.Text = "";
            string[] wificount = new string[10];
            string[] locstr = new string[10];
            wifilist.Clear();

            try
            {
                string[] lines = outputstring.Split(new Char[] { '\n' });
                int i1 = 0;
                for (i1 = 0; i1 < lines.Length - 1; i1++)
                {

                    string[] line_entries = lines[i1].Split(new Char[] { '\t' });
                    string loc_entry = line_entries[1];
                    string[] lat_lon_str = loc_entry.Split(new Char[] { ',' });

                    wifilist.Add(new WifiData { wtime = line_entries[0], wlat = double.Parse(lat_lon_str[0]), wlon = double.Parse(lat_lon_str[1]), wcount = line_entries[2] });

                }
            } catch (Exception ex)
            {
                TextBoxW1.Text = "Error in creating the Wifi list: " + ex.Message;
                return;
            }
            if (wifilist.Count > 0)
            {
                show_map_image();
            }
            else
            {
                TextBoxW1.Text = "No location data available";
            }

        }


        // SHOW THE DATA ON MAP (BING MAPS)
        public void show_map_image()
        {

            //Create an image request.

            ImageryRequest request;

            List<ImageryPushpin> pushpins = new List<ImageryPushpin>();
            pushpins.Clear();

            double max_lat;
            double max_lon;
            double min_lat;
            double min_lon;
            double center_lat = 0;
            double center_lon = 0;

            if (wifilist.Count <= 10)                       // TOP 10 AND BOTTOM 10, or less than 10
            {
                int i1 = 0;

                while (i1 < wifilist.Count)
                {
                    try
                    {
                        if (wifilist.Count == 1)                            // Just one entry, probably for the detailed view, set only the marker color in this specific case
                        {
                            int wcountlevel = Int32.Parse(wifilist[i1].wcount) / 10;

                            // Use switch to select specific colored marker to match with the Wifi count
                            switch (wcountlevel)
                            {
                                case 0:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 16, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                                    break;

                                case 1:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 15, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                                    break;

                                case 2:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 34, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                                    break;

                                default:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 22, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                                    break;
                            }
                        }
                        else
                        {                                           // More than just 1 entry to show

                            if (curr_status == "BOTTOM")
                            {
                                pushpins.Add(new ImageryPushpin() { Label = (i1 + 1).ToString(), IconStyle = 16, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                            }
                            else
                            {
                                pushpins.Add(new ImageryPushpin() { Label = (i1 + 1).ToString(), IconStyle = 15, Location = new Coordinate(wifilist[i1].wlat, wifilist[i1].wlon) });
                            }
                        }
                        i1++;
                    } catch (Exception ex)
                    {
                        TextBoxW1.Text = "Error in creating the map list nr 1: " + ex.Message;
                        return;
                    }
                }


                Session["CenterLat"] = wifilist[0].wlat;
                Session["CenterLon"] = wifilist[0].wlon;
                Session["ZoomLevel"] = selected_zoomlevel;
                Session["Pushpins"] = pushpins;

            }
            else
            {
                decimal skips = 0;
                decimal skipcounter = 0;
                int i2 = 0;
                int j2 = 0;
                try
                {
                    skips = (decimal)wifilist.Count / 100;
                    if (skips < 1)
                    {
                        skips = 1;                                  // If less than 1000 entries, no need to skip any entries, set to 1 to select the next entry
                    }


                }
                catch (Exception ex)
                {
                    TextBoxW1.Text = TextBoxW1.Text + "Error in parameter init in map list nr 2: " + ex.Message;
                    return;
                }

                try
                {

                    max_lat = wifilist[0].wlat;
                    min_lat = wifilist[0].wlat;
                    max_lon = wifilist[0].wlon;
                    min_lon = wifilist[0].wlon;

                    while ((j2 < wifilist.Count) & (pushpins.Count < 100))
                    {
                        if (j2 == 0)
                        {
                            pushpins.Add(new ImageryPushpin() { Label = "START", IconStyle = 0, Location = new Coordinate(wifilist[j2].wlat, wifilist[j2].wlon) });

                        }
                        else
                        {
                            int wcountlevel = Int32.Parse(wifilist[j2].wcount) / 10;
                            // Use switch to select specific colored marker to match with the Wifi count
                            switch (wcountlevel)
                            {
                                case 0:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 26, Location = new Coordinate(wifilist[j2].wlat, wifilist[j2].wlon) });
                                    break;

                                case 1:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 20, Location = new Coordinate(wifilist[j2].wlat, wifilist[j2].wlon) });
                                    break;

                                case 2:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 27, Location = new Coordinate(wifilist[j2].wlat, wifilist[j2].wlon) });
                                    break;

                                default:
                                    pushpins.Add(new ImageryPushpin() { Label = "", IconStyle = 28, Location = new Coordinate(wifilist[j2].wlat, wifilist[j2].wlon) });
                                    break;
                            }

                            if (wifilist[j2].wlat > max_lat) max_lat = wifilist[j2].wlat;
                            if (wifilist[j2].wlat < min_lat) min_lat = wifilist[j2].wlat;
                            if (wifilist[j2].wlon > max_lon) max_lon = wifilist[j2].wlon;
                            if (wifilist[j2].wlon < min_lon) min_lon = wifilist[j2].wlon;

                        }
                        i2 = j2;                                    // To be used in diagnostics message below
                        skipcounter = skipcounter + skips;
                        j2 = (int)skipcounter;
                    }



                }
                catch (Exception ex)
                {
                    TextBoxW1.Text = TextBoxW1.Text + "Error in map list nr 2 creation: " + ex.Message;
                    return;
                }

                try
                {
                    center_lat = min_lat + (max_lat - min_lat) / 2;
                    center_lon = min_lon + (max_lon - min_lon) / 2;

                    Session["CenterLat"] = center_lat;
                    Session["CenterLon"] = center_lon;
                    Session["ZoomLevel"] = selected_zoomlevel;
                    Session["Pushpins"] = pushpins;

                }
                catch (Exception ex)
                {
                    TextBoxW1.Text = TextBoxW1.Text + "Error in imagery request: " + ex.Message;
                    return;
                }

            }


            Session["Caller"] = "WifiCountStats.aspx";
            try
            {
                save_session();                                     // Save the settings to restore them when returning back
                Response.Redirect("MapDisplay.aspx");
            }
            catch (Exception ex)
            {
                TextBoxW1.Text = "Error in starting MapDisplay: " + ex.Message;
            }


        }


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var wifi_index = DropDownList1.SelectedIndex;
            
            TextBoxW1.Text = "";
            GridView1.Visible = false;
            GridView2.Visible = false;
        }


        private string read_data_from_blob(string searchtype)
        {
            CloudStorageAccount storageAccount;
            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlockBlob blockBlob;
            string dfile;
            string readstring;

            string[] splitname = DropDownList1.SelectedValue.Split(' ');
            var citypart = splitname[0].Substring(0, 3).ToLower();
            var datepart = splitname[1].Replace("-", "");
            dfile = "wifi_" + citypart + "_" + datepart + "_" + searchtype + ".txt";

            try
            {
                storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobConnectionString1"));

                // Create the blob client.
                blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                container = blobClient.GetContainerReference("images1");
                blockBlob = container.GetBlockBlobReference(dfile);
                readstring = "Data read from blob: " + dfile + ":\n\n" + blockBlob.DownloadText();
            }
            catch (Exception ex)
            {
                readstring = "Error in reading blob data file: " + ex.Message + " (Filename: " + dfile + ")\n";
            }

            return (readstring);
        }

        private string read_map_from_blob(string searchtype)
        {
            CloudStorageAccount storageAccount;
            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlockBlob blockBlob;
            string mfile;
            string readstring;



            string[] splitname = DropDownList1.SelectedValue.Split(' ');
            var citypart = splitname[0].Substring(0, 3).ToLower();
            var datepart = splitname[1].Replace("-", "");
            mfile = "map" + selected_zoomlevel.ToString() + "_wifi_" + citypart + "_" + datepart + "_" + searchtype + ".bmp";

            try
            {
                storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobConnectionString1"));

                // Create the blob client.
                blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                container = blobClient.GetContainerReference("images1");
                blockBlob = container.GetBlockBlobReference(mfile);
                readstring = "Map data read from blob: " + mfile + "\n";

            }
            catch (Exception ex)
            {
                readstring = "Error in reading blob data file: " + ex.Message + " (Filename: " + mfile + ")\n";
            }

            return (readstring);

        }

        public class WifiData
        {
            public string wtime { get; set; }
            public double wlat { get; set; }
            public double wlon { get; set; }

            public string wcount { get; set; }
        }

        public class wifiDisp1
        {
            public string Time { get; set; }
            public string Location { get; set; }
            public string Wifi_Count { get; set; }
            public string User { get; set; }
        }

        public class WifiDetail
        {
            public string Id { get; set; }
            public string SaveTime { get; set; }
            public string WifiItemId { get; set; }
            public string SSID { get; set; }
            public string BSSID { get; set; }
            public string Capabilities { get; set; }
            public int Frequency { get; set; }
            public int Level { get; set; }
        }


        public class wifiDisp2
        {
            public string SaveTime { get; set; }
            public string SSID { get; set; }
            public string BSSID { get; set; }
            public string Capabilities { get; set; }
            public string Freq_str { get; set; }
            public string Level_str { get; set; }

        }


        private void show_table(string inputstring)
        {

            wifidisplay1.Clear();
            wifi_ids.Clear();

            try
            {
                string[] lines = inputstring.Split(new Char[] { '\n' });
                int i1 = 0;
                for (i1 = 0; i1 < lines.Length - 1; i1++)
                {

                    string[] line_entries = lines[i1].Split(new Char[] { '\t' });
                    wifidisplay1.Add(new wifiDisp1 { Time = line_entries[0], Location = line_entries[1], Wifi_Count = line_entries[2], User = line_entries[3] });
                    wifi_ids.Add(line_entries[4]);                                                                                          // Add id's to separate table

                }

            } catch (Exception ex)
            {
                TextBoxW1.Text = "Error in parsing the SQL response data: " + ex.Message;
                return;
            }


            try
            {
                GridView1.DataSource = wifidisplay1;
                GridView1.DataBind();
                GridView1.Visible = true;

            }
            catch (Exception ex)
            {
                TextBoxW1.Text = "\nGridview error: " + ex.Message;

            }
        }

        private void show_wifi_details(string inputstring)
        {

            wifidisplay2.Clear();

            try
            {
                string[] lines = inputstring.Split(new Char[] { '\n' });
                int i1 = 0;
                for (i1 = 0; i1 < lines.Length - 1; i1++)
                {

                    string[] line_entries = lines[i1].Split(new Char[] { '\t' });

                    wifidisplay2.Add(new wifiDisp2 { SSID = line_entries[0], BSSID = line_entries[1], Capabilities = line_entries[2], Freq_str = line_entries[3], Level_str = line_entries[4] });

                }
            } catch (Exception ex)
            {
                TextBoxW1.Text = "Error in creating the Wifi details list: " + ex.Message;
            }
            

            try
            {
                GridView2.DataSource = wifidisplay2;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            catch (Exception ex)
            {
                TextBoxW1.Text = TextBoxW1.Text + "\nGridview error: " + ex.Message;

            }
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gr_index = GridView1.SelectedIndex;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int gr_index;

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

                gr_index = Convert.ToInt32(e.CommandArgument);
                GridView1.SelectedIndex = gr_index;
                GridView1.SelectedRow.BackColor = System.Drawing.Color.Yellow;
            } catch (Exception ex)
            {
                TextBoxW1.Text = "Error in reading the gridview row: " + ex.Message;
                return;
            }

            if (e.CommandName == "Details")
            {
                string wifi_id = wifi_ids[gr_index];
                string wifi_id_sql = $@"'{wifi_id}'";
                string sqlType = "WIFIDETAIL1";

                string Wifi_details = $@"
            SELECT SSID,BSSID,Capabilities,Frequency,Level
            FROM {dbdetails} 
            WHERE WifiItemId = {wifi_id_sql}
            ORDER BY Level DESC;";

                inputstring = Wifi_details;

                var sqlget = new SQL_query();
                string wifidetail_outputstring = sqlget.SQL_read(inputstring, sqlType);

                //TextBoxW1.Text = TextBoxW1.Text + "SQL Read completed\n";

                if (wifidetail_outputstring != "")
                {
                    if (wifidetail_outputstring.StartsWith("Error"))
                    {
                        TextBoxW1.Text = outputstring;
                    }
                    else
                    {
                        show_wifi_details(wifidetail_outputstring);
                    }


                }
                else
                {
                    TextBoxW1.Text = "No matching Wifi details record\n";
                }
            }
            else
            {

                // Show the selected place on the map

                wifilist.Clear();

                string loc_entry;
                string[] lat_lon_str;
                string time_entry;
                string count_entry;

                try
                {
                    loc_entry = GridView1.SelectedRow.Cells[2].Text;
                    lat_lon_str = loc_entry.Split(new Char[] { ',' });
                    time_entry = GridView1.SelectedRow.Cells[1].Text;
                    count_entry = GridView1.SelectedRow.Cells[3].Text;

                    wifilist.Add(new WifiData { wtime = time_entry, wlat = double.Parse(lat_lon_str[0]), wlon = double.Parse(lat_lon_str[1]), wcount = count_entry });
                } catch (Exception ex)
                {
                    TextBoxW1.Text = "Error in creating the map request for a single Wifi record: " + ex.Message;
                    return;
                }
                if (loc_entry != "NO_LOCATION")
                {
                    show_map_image();
                }
                else
                {
                    TextBoxW1.Text = "No location data available\n";
                }
            }

        }


        protected void save_session()
        {
            Session["UserWifiCount"] = DropDownList1.SelectedIndex;
            Session["StartDateWifiCount"] = Calendar1.SelectedDate;
            Session["EndDateWifiCount"] = Calendar2.SelectedDate;
            Session["Grid1DataWifiCount"] = wifidisplay1;
            Session["Grid2DataWifiCount"] = wifidisplay2;
            if (GridView1.SelectedIndex != -1)
            {
                Session["Grid1SelectedRowWifiCount"] = GridView1.SelectedIndex;
            }

            
        }

        private void clear_session()
        {
            Session.Remove("UserWifiCount");
            Session.Remove("StartDateWifiCount");
            Session.Remove("EndDateWifiCount");
            Session.Remove("Grid1DataWifiCount");
            Session.Remove("Grid2DataWifiCount");
            Session.Remove("Grid1SelectedRowWifiCount");

        }


        protected void TextBoxW1_PreRender(object sender, EventArgs e)
        {
            if (TextBoxW1.Text != "")
            {
                
                TextBoxW1.Style.Add("display", "block");
            }
            else
            {
                TextBoxW1.Style.Add("display", "none");
            }

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxW1.Text = "Error in selecting the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                TextBoxW1.Text = "Error in updating the SQL data binding: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }
    }
}
