<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobDetail.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.JobDetail" %>
<%@ Register TagPrefix="he" TagName="jobdetail" Src="~/Controls/JobDetail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="robots" content="NOARCHIVE,NOFOLLOW" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:jobdetail id="heJobDetail" runat="server"/>
        </div>
    </div>
</asp:Content>

