<%@ Control Language="C#" AutoEventWireup="true" CodeFile="latestjobs.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.latestjobs" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects" %>

<asp:Panel ID="pnlEmpty" runat="server" Visible="false">
    <div class="row">
        <div class="col-sm-12">
            <p>&nbsp;</p>
            <p><asp:Label ID="lblEmptyList" runat="server" CssClass="text-info"></asp:Label></p>
        </div>
    </div>
</asp:Panel>

<asp:Repeater ID="rptrJobs" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate>
        <div class="row">
            <div class="col-sm-12">
                <asp:HyperLink ID="hypJobDetail" runat="server"></asp:HyperLink>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <%# ((JobSearchResult)Container.DataItem).StartDate.ToString("MM/dd/yyyy") %> 
            </div>
            <div class="col-sm-7">
                <span class="pull-right text-right"><%# ((JobSearchResult)Container.DataItem).Location %></span> 
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">&nbsp;</div>
        </div>
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
</asp:Repeater>