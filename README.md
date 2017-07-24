# Azure_NetsAround

Azure Mobile and Web App for syncing Wifi and cellular network diagnostics from Android phone to Azure

## Introduction

This is an Azure app that syncs the WiFi and cellular network diagnostics information from the Android phones to the Azure SQL database. The app consists of an Azure Mobile App that syncs the data to the SQL database and a Web App for viewing the data. In addition, a separate app (NetsAround) needs to be installed in the Android phone. The Android app can be found in Google Play at https://play.google.com/store/apps/details?id=netsAround.netsAround.

The NetsAround app in the Android phone scans the nearby Wifi networks and monitors the cellular signal strength while the user is moving. The data is stored locally in the phone and there is also an option to sync the data to the Azure SQL Database. The user can be authenticated via Azure Active Directory before any data is sent. After the authentication the data sync works in the background until the user stops the collecting of the data. The phone app scans the Wifi networks and collects the network information, e.g. the location, the number of Wifi networks found in each place, the Wifi network names and signal strengths. The cellular connection data consists of the time, location and the observed cellular signal strength. The cellular network data is stored separately from the Wifi data in an own database table.

The Android app uses Azure Mobile Client and SQLiteStore for syncing the data to Azure SQL. The data is first stored locally in SQLiteStore from where it is synced to Azure SQL via the Azure NetsAround Mobile App app when the internet connection is available. The following database tables will be created in the destination SQL database:
* dbo:CellDatas (cellular network data)
* dbo:WifiScans (WiFi statistics)
* dbo:WifiScanDetails (WiFi network details)

The database tables are automatically created when the first entries are added. The tables don't need to be created manually, it's enough that an SQL server and database has been created and the connection string to the SQL database is defined for the Azure NetsAround app via the Azure portal.

The Azure NetsAround Web App uses Bing Maps RESTToolKit for displaying the location information. For this purpose, a valid Bing Maps API key needs to be provided in the App Settings via the portal.

The Azure NetsAround Web App includes the menus below for viewing the data. The time values are shown as UTC time.

**WiFi Statistics:**

Shows the number and list of the WiFi networks detected at each site. The number of Wifi networks can be used as an indication of the expected Wifi signal quality. If there’s a high number of Wifi networks concentrated in small areas, the chances for interference and connectivity issues increases.

The Web app lets the user to select the Top 10 and Bottom 10 places with the highest and lowest number of WiFi networks, or All places listed in the chronological order. When viewing All places on the map, the poster color shows the number of the WiFi networks detected at each place:
* green = 0-9 WiFi networks
* blue = 10-19 WiFi networks
* purple = 20-29 Wifi networks
* red = more than 30 WiFi networks

WiFi statistics can be filtered to cover selected days and show the entries from a selected user. The User Name is an optional identifier that the users can configure in their phone app.

**WiFi Network Detail information:**

Lets the user to search the WiFi networks based on different criterias, e.g the following:
* Minimum Signal Strength: given in negative dBm value between 0 and -100 (e.g. -50). The closer the value is zero, the stronger the signal is.
* User Name: optional identifier that the users can configure in their phone app.
* SSID: WiFi network name
* BSSID: Select "Distinct" if you want the same WiFi access point to appear only once in the search results.
* WiFi Type: WiFi security type (or open).

**Cellular Network Statistics:**

Shows a list of the Top 10 and Bottom 10 places ordered by the cellular network signal strength observed at each place, or All places in the chronological order. The strength is expressed with a value between 0 - 31 where 31 means the highest signal strength. With low signal strengths the chances for poor reception and connectivity issues increases.

When selecting "All" and showing all entries on the map, the following color codes are used:
* green = signal strength 20-31
* purple = signal strength 10-19
* red = signal strength 0-9

The statistics can be filtered to cover selected days and show the entries from a selected user. The User Name is an optional identifier that the users can configure in their phone app.

**Cellular Network Details:**

Lets the user to search the cellular network data based on different criterias:
* Minimum Signal Strength: Show the networks with higher or equal to the given signal strength (0 - 31)
* Maximum Signal Strength: Show the networks with lower or equal to the given signal strength (0 - 31)
* User Name: Optional identifier that the users can configure in their phone app.
* Data Connection Status: Shows if the cellular data connection is established.
* Network Type: Shows the type of network to which the phone is connected (e.g. Umts, Lte).
* Data Activity: Shows if there's any data activity going on (In, Out, Inout, None).

## Configuring the Azure Mobile and Web Apps

### Azure NetsAround Mobile App

The following connection string needs to be configured in the Connection Strings section for accessing the SQL database. Open the Azure portal and select the NetsAroundMobileApp service and go to Settings > Application Settings menu:
* Key Name: SQLNetsAroundConnectionString
* Value: Copy the string from SQL Database > Show database connection strings > ADO.NET and add the user name and password in the string.

### Azure NetsAround Web App

The following connection string needs to be configured in the Connection Strings section for accessing the SQL database. Open the Azure portal and select the AzureSQLNetsAround App service and go to Settings > Application Settings menu:
* Key Name: SQLNetsAroundConnectionString
* Value: Copy the string from SQL Database > Show database connection strings > ADO.NET and add the user name and password in the string.

The following key for Bing Maps needs to be configured via the portal in the App Settings section of the AzureSQLNetsAround app service:
* Key Name: BingMapsKey
* Value: Bing Maps key obtained from https://www.bingmapsportal.com

### Configuring the NetsAround Android app

Install the Android app from Google Play at https://play.google.com/store/apps/details?id=netsAround.netsAround.

The following settings need to be configured in the Android app to sync the data to the Azure cloud:
* Select Azure Sync ON/OFF = ON
* Set Azure Sync Service Address = URL address of the Azure NetsAround Mobile App (check the address via the Azure portal)
* Define an optional User Name to be included in the synced data (can be used for filtering purposes)
* Select Azure Authentication = Azure Active Directory or None

### Enabling authentication

The Android app supports Azure Active Directory (AD) authentication and it can be enabled in the client settings as shown above.

For the Azure Mobile App and Web App the authentication is turned on via the Azure portal in the Settings > Authentication menu.

Azure NetsAround Mobile App
* In the Azure portal, select the Azure Mobile App (NetsAroundMobileApp)
* Select Authentication / Authorization
* Select App service authentication = ON
* Select Action to take when request is not authenticated = Log in with Azure Active Directory
* Select Authentication Providers = Azure Active Directory
* Select the Azure NetsAround Mobile App to be an Azure Active Directory App

Azure NetsAround Web App
* In the Azure portal, select the Azure Web App (AzureSQLNetsAround)
* Select Authentication / Authorization
* Select App service authentication = ON
* Select Action to take when request is not authenticated = Log in with Azure Active Directory
* Select Authentication Providers = Azure Active Directory
* Select the Azure NetsAround Web App to be an Azure Active Directory App
