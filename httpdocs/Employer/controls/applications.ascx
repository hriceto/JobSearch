<%@ Control Language="C#" AutoEventWireup="true" CodeFile="applications.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.applications" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>

<div class="row">
    <div class="col-sm-12">
        <h1 class="text-center"><asp:Label ID="lblJobApplications" runat="server"></asp:Label></h1>
    </div>
</div>

<br />

<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblTitleText" runat="server" meta:resourcekey="lblTitleText" CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblTitle" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblBeginDateText" runat="server" meta:resourcekey="lblBeginDateText" CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblBeginDate" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblEndDateText" runat="server" meta:resourcekey="lblEndDateText" CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblEndDate" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblLocationText" runat="server" meta:resourcekey="lblLocationText" CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblLocation" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblNumberOfViewsText" runat="server" CssClass="control-label" 
            meta:resourcekey="lblNumberOfViewsText"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblNumberOfViews" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <strong><asp:Label ID="lblNumberOfApplicationsText" runat="server" CssClass="control-label" 
            meta:resourcekey="lblNumberOfApplicationsText"></asp:Label></strong>
    </div>
    <div class="col-sm-8">
        <asp:Label ID="lblNumberOfApplications" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>

<br />

<asp:Label ID="lblEmptyMessage" runat="server" meta:resourcekey="lblEmptyMessage"></asp:Label>

<asp:Repeater ID="rptrJobApplications" runat="server" OnItemDataBound="rptrJobApplications_ItemDataBound">
    <HeaderTemplate>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <tr>
                            <th><%= GetLocalResourceObject("strHeaderDate").ToString() %></th>
                            <th><%= GetLocalResourceObject("strHeaderName").ToString() %></th>
                            <th></th>
                        </tr>
    </HeaderTemplate>
    <ItemTemplate>
                        <tr>
                            <td><%# ((JobApplication)Container.DataItem).DateCreated.ToString("MM/dd/yyyy hh:mm") %></td>
                            <td><%# ((JobApplication)Container.DataItem).FirstName + " " + ((JobApplication)Container.DataItem).LastName %></td>
                            <td><asp:HyperLink ID="hypApplicationDetails" runat="server" meta:resourcekey="hypApplicationDetails"></asp:HyperLink></td>
                        </tr>
    </ItemTemplate>
    <FooterTemplate>
                    </table>
                </div>
            </div>
        </div>
    </FooterTemplate>
</asp:Repeater>

<div class="row">
    <div class="col-sm-4">
        <div>
            <he:paging id="hePaging" runat="server"/>
        </div>
    </div>
</div>