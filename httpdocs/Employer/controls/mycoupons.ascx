<%@ Control Language="C#" AutoEventWireup="true" CodeFile="mycoupons.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.mycoupons" %>

<div class="form-group row">
    <div class="col-sm-12">
        <h2>
            <asp:Label ID="lblHeadingCouponsYes" runat="server" meta:resourcekey="lblHeadingCouponsYes"></asp:Label>
            <asp:Label ID="lblHeadingCouponsNo" runat="server" meta:resourcekey="lblHeadingCouponsNo"></asp:Label>
        </h2>
    </div>
</div>

<asp:PlaceHolder ID="plhMyCoupons" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h1><%= GetLocalResourceObject("strMyCoupons").ToString() %></h1>
            <hr />
        </div>
    </div>
            
    <asp:Repeater ID="rptrMyCoupons" runat="server" OnItemDataBound="rptrCoupons_ItemDataBound">
        <HeaderTemplate>
            <div class="form-group row">
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponCode" runat="server" meta:resourcekey="lblHeadingCouponCode"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingNumberOfUsesLeft" runat="server" meta:resourcekey="lblHeadingNumberOfUsesLeft"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponDiscount" runat="server" meta:resourcekey="lblHeadingCouponDiscount"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponExpiration" runat="server" meta:resourcekey="lblHeadingCouponExpiration"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponStatus" runat="server" meta:resourcekey="lblHeadingCouponStatus"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                </div>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponCode" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblNumberOfUsesLeft" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponDiscount" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponExpiration" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponStatus" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Button id="btnUseCoupon" runat="server" meta:resourcekey="btnUseCoupon" OnCommand="btnUseCoupon_Command" 
                        CssClass="btn btn-primary"/>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
    <br /><br /><br />
</asp:PlaceHolder>

<asp:PlaceHolder ID="plhSiteCoupons" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h1><%= GetLocalResourceObject("strSiteCoupons").ToString() %></h1>
            <hr />
        </div>
    </div>

    <asp:Repeater ID="rptrSiteCoupons" runat="server" OnItemDataBound="rptrCoupons_ItemDataBound">
        <HeaderTemplate>
            <div class="form-group row">
                <div class="col-sm-4">
                    <strong><asp:Label ID="lblHeadingCouponCode" runat="server" meta:resourcekey="lblHeadingCouponCode"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponDiscount" runat="server" meta:resourcekey="lblHeadingCouponDiscount"></asp:Label></strong>
                </div>
                <div class="col-sm-2">
                    <strong><asp:Label ID="lblHeadingCouponExpiration" runat="server" meta:resourcekey="lblHeadingCouponExpiration"></asp:Label></strong>
                </div>
                <div class="col-sm-2"></div>
                <div class="col-sm-2"></div>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-4">
                    <asp:Label ID="lblCouponCode" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponDiscount" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lblCouponExpiration" runat="server" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-2"></div>
                <div class="col-sm-2">
                    <asp:Button id="btnUseCoupon" runat="server" meta:resourcekey="btnUseCoupon" OnCommand="btnUseCoupon_Command" 
                        CssClass="btn btn-primary"/>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
</asp:PlaceHolder>