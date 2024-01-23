<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BlogEdit.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.BlogEdit" ValidateRequest="false" %>
<%@ Register TagPrefix="he" TagName="blogedit" Src="~/Admin/controls/blogedit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/controls/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="/js/bootstrap-datepicker.js" type="text/javascript" language="javascript"></script>
    <link rel="Stylesheet" type="text/css" href="/css/datepicker3.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:blogedit id="heBlogEdit" runat="server"/>
        </div>
    </div>
</asp:Content>

