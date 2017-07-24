<%@ Page Title="Help" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Azure_SQL_NetsAround1.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Azure backend for NetsAround Android phone app</h3>
    <p>This app syncs the WiFi and cellular network diagnostics information from the Android phones to the Azure SQL database. The app consists of an Azure Mobile App that syncs the data
       to the SQL database and a Web App for viewing the data. In addition, a separate app (NetsAround) needs to be installed in 
       the Android phone. The Android app can be found in Google Play at <a href="https://play.google.com/store/apps/details?id=netsAround.netsAround">https://play.google.com/store/apps/details?id=netsAround.netsAround"</a> 
        </p>
    <p>
       The NetsAround app in the Android phone scans the nearby Wifi networks and monitors the cellular signal strength while the user is moving. The data is stored locally in the phone
        and there is also an option to sync the data to the Azure SQL Database. The user can be authenticated via Azure Active Directory before any data is sent. After the authentication the data
        sync works in the background until the user stops the collecting of the data. The phone app scans the Wifi networks and collects the network information, e.g. the location, the number of Wifi networks
        found in each place, the Wifi network names and signal strengths. The cellular connection data consists of the time, location and the observed cellular signal strength. 
        The cellular network data is stored separately from the Wifi data in an own database table.
        </p>
    <p>
        The Android app uses Azure Mobile Client and SQLiteStore for syncing the data to Azure SQL. The data is first stored locally in SQLiteStore from where it is
        synced to Azure SQL via the Azure NetsAround Mobile App app when the internet connection is available. The following database tables will be created in the destination SQL database:
        </p>
    <ul>
        <li>dbo:CellDatas (cellular network data)</li>
        <li>dbo:WifiScans (WiFi statistics)</li>
        <li>dbo:WifiScanDetails (WiFi network details)</li>
    </ul>
    <p>The database tables are automatically created when the first entries are added. The tables don't need to be created manually, it's enough that an SQL server and database is
        created and the connection string to the SQL database is defined for the Azure NetsAround app via the Azure portal.
    </p>
    <p>
        The Azure NetsAround Web App uses Bing Maps RESTToolKit for displaying the location information. For this purpose, a valid Bing Maps API key needs to be provided in the App Settings via the portal.
        </p>
    <p>
        The Azure NetsAround Web App includes the menus below for viewing the data. The time values are shown as UTC time.
        </p>
        <b>WiFi Statistics: </b>
        <p>
        Shows the number and list of the WiFi networks detected at each site. The number of Wifi networks can be used as an indication of the expected Wifi signal quality. 
        If there’s a high number of Wifi networks concentrated in small areas, the chances for interference and connectivity issues increases.
        The Web app lets the user to select the Top 10 and Bottom 10 places with the highest and lowest number of WiFi networks, or All places listed in the chronological order. When viewing All places on the map, the poster color
        shows the number of the WiFi networks detected at each place:
            </p>
    <ul>
        <li>green = 0-9 WiFi networks</li>
        <li>blue = 10-19 WiFi networks</li>
        <li>purple = 20-29 Wifi networks</li>
        <li>red = more than 30 WiFi networks</li>
    </ul>
    <p>
        WiFi statistics can be filtered to cover selected days and show the entries from a selected user. The User Name is an optional identifier that the users can configure in their phone app. 
        </p>
    <p>
        <b>WiFi Network Detail information: </b>
        </p>
        Lets the user to search the WiFi networks based on different criterias, e.g the following:
        <ul>
            <li>Minimum Signal Strength: given in negative dBm value between 0 and -100 (e.g. -50). The closer the value is zero, the stronger the signal is. </li>
            <li>User Name: optional identifier that the users can configure in their phone app.</li>
            <li>SSID: WiFi network name</li>
            <li>BSSID: Select "Distinct" if you want the same WiFi access point to appear only once in the search results.</li>
            <li>WiFi Type: WiFi security type (or open).</li>
        </ul>

    <p>
        <b>Cellular Network Statistics:</b>
        </p>
    <p>
        Shows a list of the Top 10 and Bottom 10 places ordered by the cellular network signal strength observed at each place, or All places in the chronological order. The strength
        is expressed with a value between 0 - 31 where 31 means the highest signal strength. With low signal strengths the chances for poor reception and connectivity issues increases.
        When selecting "All" and showing all entries on the map, the following color codes are used:
        </p>
    <ul>
        <li>green = signal strength 20-31</li>
        <li>purple = signal strength 10-19</li>
        <li>red = signal strength 0-9</li>
    </ul>

    <p>
        The statistics can be filtered to cover selected days and show the entries from a selected user. The User Name is an optional identifier that the users can configure in their phone app. 
        </p>

    <p>
        <b>Cellular Network Details:</b>
        </p>
    
        Lets the user to search the cellular network data based on different criterias:
    <ul>
        <li>Minimum Signal Strength: Show the networks with higher or equal to the given signal strength (0 - 31)</li>
        <li>Maximum Signal Strength: Show the networks with lower or equal to the given signal strength (0 - 31)</li>
        <li>User Name: Optional identifier that the users can configure in their phone app.</li>
        <li>Data Connection Status: Shows if the cellular data connection is established.</li>
        <li>Network Type: Shows the type of network to which the phone is connected (e.g. Umts, Lte).</li>
        <li>Data Activity: Shows if there's any data activity going on (In, Out, Inout, None).</li>
    </ul>
        
    <h3>Configuring the Azure Mobile and Web Apps</h3>

    <h4>Azure NetsAround Mobile App</h4>

    <p>
        The following connection string needs to be configured in the Connection Strings section for accessing the SQL database. Open the Azure portal and select the NetsAroundMobileApp
        service and go to Settings > Application Settings menu
    </p>
    <ul>
        <li>Key Name: SQLNetsAroundConnectionString</li>
        <li>Value: Copy the string from SQL Database > Show database connection strings > ADO.NET and add the user name and password in the string.</li>
    </ul>
    
    <h4>Azure NetsAround Web App</h4>

    <p>
        The following connection string needs to be configured in the Connection Strings section for accessing the SQL database. Open the Azure portal and select the AzureSQLNetsAround App
        service and go to Settings > Application Settings menu
    </p>
    <ul>
        <li>Key Name: SQLNetsAroundConnectionString</li>
        <li>Value: Copy the string from SQL Database > Show database connection strings > ADO.NET and add the user name and password in the string.</li>
    </ul>

    <p>
        The following key for Bing Maps needs to be configured via the portal in the App Settings section of the AzureSQLNetsAround app service:
    </p>
    <ul>
        <li>Key Name: BingMapsKey</li>
        <li>Value: Bing Maps key obtained from https://www.bingmapsportal.com</li>
    </ul>
    <h3>Configuring the NetsAround Android app</h3>

    <p>The following settings need to be configured in the Android app to sync the data to the Azure cloud:</p>
    <ul>
        <li>Select Azure Sync ON/OFF = ON</li>
        <li>Set Azure Sync Service Address = URL address of the Azure NetsAround Mobile App (check the address via the Azure portal)</li>
        <li>Define an optional User Name to be included in the synced data (can be used for filtering purposes)</li>
        <li>Select Azure Authentication = Azure Active Directory or None</li>
    </ul>

    <h3>Enabling authentication</h3>

    <p>The Android app supports Azure Active Directory (AD) authentication and it can be enabled in the client settings as shown above.
        For the Azure Mobile App and Web App the authentication is turned on via the Azure portal in the Settings > Authentication menu. 
    </p>

    <h4>Azure NetsAround Mobile App</h4>

    <ul>
        <li>In the Azure portal, select the Azure Mobile App (NetsAroundMobileApp)</li>
        <li>Select Authentication / Authorization</li>
        <li>Select App service authentication = ON</li>
        <li>Select Action to take when request is not authenticated = Log in with Azure Active Directory</li>
        <li>Select Authentication Providers = Azure Active Directory</li>
        <li>Select the Azure NetsAround Mobile App to be an Azure Active Directory App</li>
    </ul>

        <h4>Azure NetsAround Web App</h4>

    <ul>
        <li>In the Azure portal, select the Azure Web App (AzureSQLNetsAround)</li>
        <li>Select Authentication / Authorization</li>
        <li>Select App service authentication = ON</li>
        <li>Select Action to take when request is not authenticated = Log in with Azure Active Directory</li>
        <li>Select Authentication Providers = Azure Active Directory</li>
        <li>Select the Azure NetsAround Web App to be an Azure Active Directory App</li>
    </ul>

</asp:Content>
