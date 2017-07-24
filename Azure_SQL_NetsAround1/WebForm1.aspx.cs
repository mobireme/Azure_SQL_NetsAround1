using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DT = System.Data;            // System.Data.dll 
using QC = System.Data.SqlClient;  // System.Data.dll

namespace Azure_SQL_NetsAround1
{
    
    public partial class WebForm1 : System.Web.UI.Page

        
    {
        static string outputstring;
        static string inputstring;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "Button clicked";

            using (var connection = new QC.SqlConnection(
"Server=tcp:mobiremesql6.database.windows.net,1433;Initial Catalog=SQLTest6;Persist Security Info=False;User ID=pepaavola;Password=Socme_23092015;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
))
            {

                try
                {
                    connection.Open();
                    outputstring = "Command output:\n";
                    inputstring = TextBox2.Text;

                    SelectRows(connection);
                    connection.Close();
                }
                catch (Exception ex)
                {
                    outputstring = "Error in connect: " + ex.Message;
                }
            }

            TextBox1.Text = outputstring;

        }

        static public void SelectRows(QC.SqlConnection connection)
        {
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                /*
                                command.CommandText = @"  
                SELECT *
                FROM dbo.TodoItems 
                WHERE Strength = 24 
                ;
                ";

                                command.CommandText = @"  
                    SELECT  
                        TOP 5  
                            COUNT(Strength) AS [OrderCount],
                            Text,
                            Location
                        FROM  dbo.TodoItems
                        GROUP BY  
                            Text,  
                            Location  
                        ORDER BY  
                            [OrderCount] DESC,  
                            Text; ";
*/
                
                command.CommandText = @inputstring;


                QC.SqlDataReader reader = command.ExecuteReader();

                                while (reader.Read())
                                {
                                    // Console.WriteLine("{0}\t{1}\t{2}",
                                    //   reader.GetInt32(0),
                                    //    reader.GetInt32(1),
                                    //  reader.GetString(2));
                                    outputstring = outputstring + reader.GetString(1) + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(0).ToString() + "\n";
                                    
                                }
                            }
                        


            }
        }
}