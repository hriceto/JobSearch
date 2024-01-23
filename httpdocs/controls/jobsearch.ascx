<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jobsearch.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.jobsearch" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        jobSearchOnLoadScript();
    });

    function jobSearchOnLoadScript() {
        jQuery('#<%= txtSearch.ClientID %>').keypress(OnKeyDownJobSearch);
        jQuery('#<%= ddlMilesDistance.ClientID %>').keyup(OnKeyDownJobSearch);
        jQuery('#<%= txtMilesZip.ClientID %>').keypress(OnKeyDownJobSearch);
        jQuery('#<%= ddlOrderBy.ClientID %>').keyup(OnKeyDownJobSearch);
        jQuery('#<%= ddlCategory.ClientID %>').keyup(OnKeyDownJobSearch);

        jQuery('#<%= txtSearch.ClientID %>').focus();
        jQuery('#<%= txtSearch.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
    }

    function OnKeyDownJobSearch(e) 
    {
        if (e.which == 13) {
            jQuery('#<%= btnSearch.ClientID %>').click();
            return false;
        }
    }
</script>

<div class="form-group row">
    <div class="col-sm-12">
        <h1><%= GetLocalResourceObject("strHeaderJobSearch").ToString() %></h1>
    </div>
</div>    

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label id="lblJobSearch" runat="server" meta:resourcekey="lblJobSearch" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-7">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm" meta:resourcekey="txtSearch" MaxLength="200"></asp:TextBox>
    </div>
    <div class="col-sm-2">
        <asp:Button id="btnSearch" runat="server" OnClick="btnSearch_Click" meta:resourcekey="btnSearch" CssClass="btn btn-default"/>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblFilterMilesZip1" runat="server" meta:resourcekey="lblFilterMilesZip1" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-2">
        <asp:DropDownList ID="ddlMilesDistance" runat="server" CssClass="form-control input-sm">
            <asp:ListItem Value="5">5 miles</asp:ListItem>
            <asp:ListItem Value="10">10 miles</asp:ListItem>
            <asp:ListItem Value="15">15 miles</asp:ListItem>
            <asp:ListItem Value="20">20 miles</asp:ListItem>
            <asp:ListItem Value="25">25 miles</asp:ListItem>
            <asp:ListItem Value="30">30 miles</asp:ListItem>
            <asp:ListItem Value="35">35 miles</asp:ListItem>
            <asp:ListItem Value="40">40 miles</asp:ListItem>
            <asp:ListItem Value="45">45 miles</asp:ListItem>        
            <asp:ListItem Value="50">50 miles</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="col-sm-2">
        <asp:Label ID="lblFilterMilesZip2" runat="server" meta:resourcekey="lblFilterMilesZip2" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-3">
        <asp:TextBox ID="txtMilesZip" runat="server" CssClass="form-control input-sm" MaxLength="12"></asp:TextBox>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblOrderBy" runat="server" meta:resourcekey="lblOrderBy"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" CssClass="form-control input-sm" 
            OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
            <asp:ListItem Value="Relevance" meta:resourcekey="ddlOrderBy_Item1"></asp:ListItem>
            <asp:ListItem Value="StartDate" meta:resourcekey="ddlOrderBy_Item2"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="col-sm-3">
        <asp:DropDownList ID="ddlCategory" runat="server" AppendDataBoundItems="true" CssClass="form-control input-sm" 
            DataTextField="Name" DataValueField="CategoryId" AutoPostBack="true" >
            <asp:ListItem meta:resourcekey="ddlCategory_Item1"></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        <br />
        <asp:Label ID="lblNumberOfSearchResults" runat="server" CssClass="help-block"></asp:Label>
    </div>
</div>

<asp:Repeater ID="rptrJobResults" runat="server" OnItemDataBound="rptrJobResults_ItemDataBound">
    <HeaderTemplate><hr /></HeaderTemplate>
    <ItemTemplate>
    <div class="row">
        <div class="col-sm-3">
            <%# ((JobSearchResult)Container.DataItem).StartDate.ToString("MM/dd/yyyy") %> 
        </div>
        <div class="<%# String.IsNullOrEmpty(((JobSearchResult)Container.DataItem).Location) ? "col-sm-9" : "col-sm-6" %>">
            <asp:HyperLink ID="hypJobDetail" runat="server"></asp:HyperLink>
        </div>
        <div class="<%# String.IsNullOrEmpty(((JobSearchResult)Container.DataItem).Location) ? "" : "col-sm-3" %>">
            <span class="pull-right text-right"><%# ((JobSearchResult)Container.DataItem).Location %></span> 
        </div>
    </div>
    <br />
    </ItemTemplate>
    <FooterTemplate><hr /></FooterTemplate>
</asp:Repeater>

<div>
    <he:paging id="hePaging" runat="server"/>
</div>