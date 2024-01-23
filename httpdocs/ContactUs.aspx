<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.ContactUs" %>
<%@ Register TagPrefix="he" TagName="contactus" Src="~/controls/contactus.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strContactUs").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:contactus id="heContactUs" runat="server"/>
        </div>
    </div>
</asp:Content>

