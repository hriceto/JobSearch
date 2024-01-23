<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BlogList.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.BlogList" %>
<%@ Register TagPrefix="he" TagName="bloglist" Src="~/Controls/bloglist.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:bloglist id="heBlogList" runat="server"/>
        </div>
    </div>
</asp:Content>

