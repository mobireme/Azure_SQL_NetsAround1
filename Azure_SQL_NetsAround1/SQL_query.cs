using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DT = System.Data;            // System.Data.dll 
using QC = System.Data.SqlClient;  // System.Data.dll


namespace Azure_SQL_NetsAround1
{
    public class SQL_query
    {
        string outputstring = "";
        string sqlType = "";
        
        public string SQL_read(string inputstring, string sqlx)
        {
            sqlType = sqlx;
            string sql_string = System.Configuration.ConfigurationManager.ConnectionStrings["SQLNetsAroundConnectionString"].ToString();

            if ((sql_string == "") | (sql_string == null))
            {
                outputstring = "Error in the SQL connection. Check that the connection string is properly defined in the portal in App settings > SQLConnectionString";
                return outputstring;
            }


            using (var connection = new QC.SqlConnection(sql_string))

            {

                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    outputstring = "Error in connect: " + ex.Message;
                    return (outputstring);
                }


                SelectRows(connection, inputstring);
                connection.Close();

                return (outputstring);

            }
        }
        private void SelectRows(QC.SqlConnection connection, string inputstring)
        {
            using (var command = new QC.SqlCommand())
            {
                try
                {

                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;

                    command.CommandText = @inputstring;
                    bool dbnew = inputstring.Contains("UserName");
                    bool dbdetails = inputstring.Contains("SSID");
                    if (dbdetails) dbnew = true;

                    QC.SqlDataReader reader = command.ExecuteReader();

                    /*
                    WIFICOUNT_OLD: SELECT WifisFound, Text, Location
                    WIFICOUNT: SELECT WifisFound, SaveTime, Location, UserName, Id
                    WIFISIGNAL: SELECT SSID, BSSID, Capabilities, Frequency, Level, WifiItemId
                    WIFITIME: SELECT WifisFound, SaveTime, Location, UserName, Id
                    WIFIDETAIL1: SELECT SSID, BSSID, Capabilities, Frequency, Level
                    CELLSTRENGTH: SELECT Strength,SaveTime,Location,UserName
                    CELLSTRENGTH_OLD: SELECT Strength,Text,Location

                    */
                    while (reader.Read())
                    {
                        switch (sqlType)
                        {
                            case ("WIFICOUNT_OLD"):
                                {
                                    // WifisFound, Text, Location
                                    outputstring = outputstring + reader.GetString(1) + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(0).ToString() + "\n";
                                    break;
                                }
                            case ("WIFICOUNT"):
                                {
                                    // WifisFound, SaveTime, Location, UserName, Id
                                    outputstring = outputstring + reader.GetDateTime(1).ToString("u") + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(0).ToString() + "\t" + reader.GetString(3) + "\t" + reader.GetString(4) + "\n";
                                    break;
                                }
                            case ("WIFISIGNAL"):
                                {
                                    // SSID, BSSID, Capabilities, Frequency, Level, WifiItemId
                                    outputstring = outputstring + reader.GetString(0) + "\t" + reader.GetString(1) + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(3).ToString() + "\t" + reader.GetInt32(4).ToString() + "\t" + reader.GetString(5) + "\n";
                                    break;
                                }
                            case ("WIFIDETAIL1"):
                                {
                                    // SSID, BSSID, Capabilities, Frequency, Level
                                    outputstring = outputstring + reader.GetString(0) + "\t" + reader.GetString(1) + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(3).ToString() + "\t" + reader.GetInt32(4).ToString() + "\n";
                                    break;
                                }
                            case ("WIFITIME"):
                                {
                                    // WifisFound, SaveTime, Location, UserName, Id
                                    outputstring = outputstring + reader.GetDateTime(1).ToString("u") + "\t" + reader.GetString(2) + "\t" + reader.GetInt32(0).ToString() + "\t" + reader.GetString(3) + "\t" + reader.GetString(4) + "\n";
                                    break;
                                }
                            case ("CELLSTRENGTH"):
                                {
                                    // Strength,SaveTime,Location,UserName,DataConnState,Network_Type,Data_Activity
                                    outputstring = outputstring + reader.GetInt32(0).ToString() + "\t" +  reader.GetDateTime(1).ToString("u") + "\t" + reader.GetString(2) + "\t" + reader.GetString(3) + "\t" + reader.GetString(4) + "\t" + reader.GetString(5) + "\t" + reader.GetString(6) + "\n";
                                    break;
                                }
                            case ("CELLSTRENGTH_OLD"):
                                {
                                    // Strength,Text,Location
                                    outputstring = outputstring + reader.GetInt32(0).ToString() + "\t" + reader.GetString(1) + "\t" + reader.GetString(2) + "\n";
                                    break;
                                }
                            default:
                                outputstring = outputstring + "Error reading SQL data";
                                break;
                        }

                    }
                } catch (Exception ex)
                {
                    outputstring = "Error in SQL query: " + ex.Message;

                }
                
            }



        }
    }

}
