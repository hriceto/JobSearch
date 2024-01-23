<%@ Control Language="C#" AutoEventWireup="true" CodeFile="industrydetail.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.industrydetail" %>
<%@ Register Src="~/controls/latestjobs.ascx" TagPrefix="he" TagName="latestjobs" %>

<h1>
    <ol class="breadcrumb">
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </ol>
</h1>

<he:latestjobs id="heLatestJobs" runat="server"/>