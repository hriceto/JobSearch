<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobSearch.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.JobSearch" %>
<%@ Register TagPrefix="he" TagName="jobsearch" Src="~/Controls/JobSearch.ascx" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strJobSearch").ToString()%>" />
    <meta name="ROBOTS" content="INDEX, FOLLOW" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:jobsearch id="heJobSearch" runat="server"/>
        </div>
    </div>
</asp:Content>

