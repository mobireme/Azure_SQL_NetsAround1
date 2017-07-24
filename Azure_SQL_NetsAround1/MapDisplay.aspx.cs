using BingMapsRESTToolkit;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Azure_SQL_NetsAround1
{
    public partial class MapDisplay : System.Web.UI.Page
    {
        public static double center_lat;
        public static double center_lon;
        public static int selected_zoomlevel;
        public static List<ImageryPushpin> pushpins;
        public static List<ImageryPushpin> pushpin1;
        public static string caller;
        public static System.Drawing.Image img1;
        public static System.Drawing.Bitmap img2;
        public static System.IO.Stream imgStream;

        protected void Page_Load(object sender, EventArgs e)
        {

         {
                
                try
                {
                    RegisterAsyncTask(new PageAsyncTask(show_map));
                } catch (Exception ex)
                {
                    TextBox1.Text = "Error in showing the map: " + ex.Message;
                    //TextBox1.Visible = true;
                    TextBox1.Style.Add("display", "block");
                    ImageMap.Visible = false;
                }
             
          }




            ImageMap.Visible = true;

            if (Session["CenterLat"] != null) center_lat = (double)Session["CenterLat"];
            if (Session["CenterLon"] != null) center_lon = (double)Session["CenterLon"];
            if (Session["Pushpins"] != null) pushpins = (List<ImageryPushpin>)Session["Pushpins"];

            if (RadioButtonList1.SelectedValue == "")
            {
                if ((Session["ZoomLevel"]) != null) selected_zoomlevel = (int)Session["ZoomLevel"];
                int rbmax = RadioButtonList1.Items.Count;
                for (int i1 = 0; i1 < rbmax; i1++)
                {
                    if (RadioButtonList1.Items[i1].Value == selected_zoomlevel.ToString()) RadioButtonList1.Items[i1].Selected = true;
                }
            }

 
        }


        public async Task show_map()
        //public async void show_map()
        {
            string bing_key = CloudConfigurationManager.GetSetting("BingMapsKey");
            if ((bing_key == "") | (bing_key == null)) 
            {
                TextBox1.Style.Add("display", "block");
                ImageMap.Visible = false;
                TextBox1.Text = TextBox1.Text + "\nNo BingMapsKey in application settings, cannot show the maps. Configure the key via the Azure portal in the App Services > Settings > Application menu by adding a key with name BingMapsKey.";
                return;
            }


            ImageryRequest request = new ImageryRequest()
            {
                CenterPoint = new Coordinate(center_lat, center_lon),
                ZoomLevel = selected_zoomlevel,
                ImagerySet = ImageryType.Road,
                DeclutterPins = true,
                Pushpins = pushpins,
                MapHeight = 1000,
                MapWidth = 1000,
                BingMapsKey = bing_key
            };


            CloudStorageAccount storageAccount;
            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlockBlob blockBlob;


            try
            {
                using (var imageStream = await ServiceManager.GetImageAsync(request))
                {
                    //blockBlob.UploadFromStream(imageStream);                      // Use this if need to store file in blob storage

                    img1 = System.Drawing.Image.FromStream(imageStream);
                    // img2 = (System.Drawing.Bitmap)imageStream;
                    // System.IO.Stream imgStream = imageStream;

                }
            }
            catch (Exception ex)
            {
                TextBox1.Text = TextBox1.Text + "\nError in getting the map stream: " + ex.Message;
                //TextBox1.Visible = true;
                TextBox1.Style.Add("display", "block");
                ImageMap.Visible = false;
                
                return;
            }



            ImageMap.ImageUrl = "~/ImageHandler1.ashx";                        // Call the HTTP handler


        }


        protected async void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_zoomlevel = Int32.Parse(RadioButtonList1.SelectedValue);
            
            if (ImageMap.ImageUrl != "")
            {
                await show_map();
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["Caller"] != null)
            {
                caller = (string)Session["Caller"];
                Response.Redirect(caller);
            }

        }

        protected void TextBox1_PreRender(object sender, EventArgs e)
        {
            if (TextBox1.Text != "")
            {

                TextBox1.Style.Add("display", "block");
                ImageMap.Visible = false;
                //TextBox1.Visible = true;
            }
            else
            {
                TextBox1.Style.Add("display", "none");
                ImageMap.Visible = true;
            }

        }

    }
}