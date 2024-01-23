<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strError").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <h1><asp:Label ID="lblHeader1" runat="server" meta:resourcekey="lblHeader1"></asp:Label></h1>
            <div class="text-muted"><asp:Label ID="lblHeader2" runat="server"></asp:Label></div>
        </div>
    </div>
</asp:Content>

