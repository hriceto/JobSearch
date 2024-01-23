<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jobdetail.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.jobdetail" %>

<asp:PlaceHolder ID="plhError" runat="server" Visible="false">
    <h1><%= GetLocalResourceObject("strUnavailable").ToString() %></h1>
</asp:PlaceHolder>

<asp:PlaceHolder ID="plhDetail" runat="server">
<div class="row">
    <div class="col-sm-12">
        <h1><asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        <h2 class="text-primary"><asp:Label ID="lblHeading" runat="server"></asp:Label></h2>
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        <asp:Label ID="lblDescription" runat="server"></asp:Label>
    </div>
</div>

<asp:Panel ID="pnlRequirements" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <strong><%= GetLocalResourceObject("strRequirements").ToString() %></strong>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Label ID="lblRequirements" runat="server"></asp:Label>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlBenefits" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <strong><%= GetLocalResourceObject("strBenefits").ToString() %></strong>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Label ID="lblBenefits" runat="server"></asp:Label>
        </div>
    </div>
</asp:Panel>


<div class="row">
    <div class="col-sm-3">
        <span><strong><%= GetLocalResourceObject("strLocation") %></strong></span> 
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblLocation" runat="server"></asp:Label>
    </div>
</div>

<div class="row">
    <div class="col-sm-3">
        <span><strong><%= GetLocalResourceObject("strEmploymentType") %></strong></span>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblEmploymentType" runat="server"></asp:Label>
    </div>
</div>

<div class="row">
    <div class="col-sm-3">
        <span><strong><%= GetLocalResourceObject("strPostedOn") %></strong></span>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblStartDate" runat="server"></asp:Label>
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        <br />
        <asp:HyperLink ID="hypJobApplication" runat="server" meta:resourcekey="hypJobApplication"></asp:HyperLink>
    </div>
</div>
</asp:PlaceHolder>