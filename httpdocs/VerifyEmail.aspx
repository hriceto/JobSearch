<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VerifyEmail.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.VerifyEmail" %>
<%@ Register TagPrefix="he" TagName="verifyemail" Src="~/controls/verifyemail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strVerifyEmail").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <he:verifyemail id="heVerifyEmail" runat="server"/>
</asp:Content>

