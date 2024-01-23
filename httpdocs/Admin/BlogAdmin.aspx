<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BlogAdmin.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.BlogAdmin" %>
<%@ Register TagPrefix="he" TagName="blogadmin" Src="~/Admin/controls/blogadmin.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:blogadmin id="heBlogAdmin" runat="server"/>
        </div>
    </div>
</asp:Content>

