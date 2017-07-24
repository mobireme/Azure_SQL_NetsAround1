using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Azure_SQL_NetsAround1
{
    /// <summary>
    /// Summary description for ImageHandler1
    /// </summary>
    public class ImageHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            try
            {


                context.Response.Clear();

                //Image image = MapDisplay.img1;
                //Bitmap bmap = MapDisplay.img2;
                //Image img = Image.FromStream(MapDisplay.imgStream);
                Bitmap bmap = new Bitmap(MapDisplay.img1);
                //System.IO.Stream smap = MapDisplay.img3;


                // Of course set this to whatever your format is of the image
                context.Response.ContentType = "image/bmp";
                // Save the image to the OutputStream
                //image.Save(context.Response.OutputStream, ImageFormat.MemoryBmp);
                bmap.Save(context.Response.OutputStream, ImageFormat.Bmp);
            } catch (Exception ex)
            {

            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}