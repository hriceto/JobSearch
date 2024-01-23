<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strLogout").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <h2><%= GetGlobalResourceObject("GlobalResources", "strLoggedOut").ToString() %></h2>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6"></div>
        <div class="col-sm-6">
            <asp:Literal ID="litLogin" runat="server"></asp:Literal>
        </div>
    </div>
    
</asp:Content>

