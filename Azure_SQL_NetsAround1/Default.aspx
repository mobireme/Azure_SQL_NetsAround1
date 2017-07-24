<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Azure_SQL_NetsAround1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>NetsAround Web App</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>WiFi Data Analysis</h2>
            <p>
                For more details of the WiFi data collected by the mobile device, visit the pages below:
            </p>
            <p>
                &nbsp;</p>
            <p>
                &nbsp;<a class="btn btn-default" href="WifiCountStats.aspx">WiFi Statistics</a></p>
            <p>
                <a class="btn btn-default" href="WifiNetStats.aspx">WiFi Network Detail Information</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Cellular Network Data Analysis</h2>
            <p>
                For more details of the cellular network data, visit the page below:
            </p>
            <p>
                &nbsp;</p>
            <p>
                <a class="btn btn-default" href="CellStrengthStats.aspx">Cellular network statistics</a>
            </p>
            <p>
                <a class="btn btn-default" href="CellDetails.aspx">Cellular network details</a>
            </p>
            <p>
                &nbsp;</p>
        </div>
    </div>

</asp:Content>
