<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.ResetPassword" %>
<%@ Register TagPrefix="he" TagName="resetpassword" Src="~/controls/resetpassword.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strResetPassword").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <br />
            <he:resetpassword id="heResetPassword" runat="server"/>
        </div>
    </div>
</asp:Content>

