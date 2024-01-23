<%@ Control Language="C#" AutoEventWireup="true" CodeFile="couponadmin.ascx.cs"
    Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.couponadmin" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>

<div class="form-group row">
    <div class="col-sm-4">
        <asp:HyperLink ID="hypAddNewCoupon" runat="server" meta:resourcekey="hypAddNewCoupon" CssClass="btn btn-primary"></asp:HyperLink>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-12">
        <h4><asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch"></asp:Label></h4>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblSearchByCompany" runat="server" meta:resourcekey="lblSearchByCompany"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchByCompany" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4"></div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblSearchByUser" runat="server" meta:resourcekey="lblSearchByUser"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchByUser" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4"></div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Button id="btnSearch" runat="server" OnClick="btnSearch_Click" meta:resourcekey="btnSearch" CssClass="btn btn-primary"/>
    </div>
    <div class="col-sm-4"></div>
    <div class="col-sm-4"></div>
</div>

<asp:Repeater ID="rptrCoupons" runat="server" OnItemDataBound="rptrCoupons_ItemDataBound">
    <HeaderTemplate>
        <div class="form-group row">
            <div class="col-sm-2"><strong><asp:Label ID="lblCouponCodeHeader" runat="server" meta:resourcekey="lblCouponCodeHeader"></asp:Label></strong></div>
            <div class="col-sm-2"><strong><asp:Label ID="lblCompanyIdHeader" runat="server" meta:resourcekey="lblCompanyIdHeader"></asp:Label></strong></div>
            <div class="col-sm-3"><strong><asp:Label ID="lblUserIdHeader" runat="server" meta:resourcekey="lblUserIdHeader"></asp:Label></strong></div>
            <div class="col-sm-2"><strong><asp:Label ID="lblCouponDiscountHeader" runat="server" meta:resourcekey="lblCouponDiscountHeader"></asp:Label></strong></div>
            <div class="col-sm-2"><strong><asp:Label ID="lblCouponStatusHeader" runat="server" meta:resourcekey="lblCouponStatusHeader"></asp:Label></strong></div>
            <div class="col-sm-1"></div>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="form-group row">
            <div class="col-sm-2"><asp:Label ID="lblCouponCode" runat="server"></asp:Label></div>
            <div class="col-sm-2"><asp:Label ID="lblCompanyId" runat="server"></asp:Label></div>
            <div class="col-sm-3"><asp:Label ID="lblUserId" runat="server"></asp:Label></div>
            <div class="col-sm-2"><asp:Label ID="lblCouponDiscount" runat="server"></asp:Label></div>
            <div class="col-sm-2"><asp:Label ID="lblCouponStatus" runat="server"></asp:Label></div>
            <div class="col-sm-1"><asp:HyperLink id="hypEditCoupon" runat="server" meta:resourcekey="hypEditCoupon" CssClass="btn btn-default"/></div>
        </div>
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
</asp:Repeater>

<div>
    <he:paging id="hePaging" runat="server"/>
</div>