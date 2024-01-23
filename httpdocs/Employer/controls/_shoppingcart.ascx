<%@ Control Language="C#" AutoEventWireup="true" CodeFile="_shoppingcart.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls._shoppingcart" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>

<asp:Repeater ID="rptrShoppingCart" runat="server" OnItemDataBound="rptrShoppingCart_ItemDataBound">
    <HeaderTemplate>
        <div class="form-group row">
            <div class="col-sm-5">
                <strong><%# GetLocalResourceObject("strJobNameHeader").ToString() %></strong>
            </div>
            <div class="col-sm-4"></div>
            <div class="col-sm-1"></div>
            <div class="col-sm-2">
                <div class="pull-right"><strong><%# GetLocalResourceObject("strJobPrice").ToString() %></strong></div>
            </div>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="form-group row">
            <div class="col-sm-5"><%# ((JobPost)Container.DataItem).Title %></div>
            <div class="col-sm-4">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:LinkButton id="lnkbDelete" runat="server" meta:resourcekey="lnkbDelete" OnCommand="lnkbDelete_Command" CommandArgument='<%# ((JobPost)Container.DataItem).JobPostId%>'/>
                        </td>
                        <td>
                            <asp:HyperLink ID="hypEditJob" runat="server" meta:resourcekey="hypEditJob"></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="hypRepublishJob" runat="server" meta:resourcekey="hypRepublishJob"></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-sm-1"></div>
            <div class="col-sm-2">
                <div class="pull-right">
                    <%# ((JobPost)Container.DataItem).PriceTotal != null ? String.Format("{0:C}", ((Decimal)((JobPost)Container.DataItem).PriceTotal)) : "" %>
                </div>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        <div class="row">
            <div class="col-sm-5"></div>
            <div class="col-sm-4"></div>
            <div class="col-sm-1"><%# GetLocalResourceObject("strSubtotal").ToString() %></div>
            <div class="col-sm-2"><asp:Label ID="lblSubtotal" runat="server" CssClass="pull-right"></asp:Label></div>
        </div>
        <div class="row">
            <div class="col-sm-5"></div>
            <div class="col-sm-4"></div>
            <div class="col-sm-1"><%# GetLocalResourceObject("strTax").ToString() %></div>
            <div class="col-sm-2"><asp:Label ID="lblTax" runat="server" CssClass="pull-right"></asp:Label></div>
        </div>
        <div id="divCouponDiscount" runat="server" class="row" visible="false">
            <div class="col-sm-5"></div>
            <div class="col-sm-4"></div>
            <div class="col-sm-1"><%# GetLocalResourceObject("strCouponDiscount").ToString()%></div>
            <div class="col-sm-2"><asp:Label ID="lblCouponDiscount" runat="server" CssClass="pull-right"></asp:Label></div>
        </div>
        <div class="row">
            <div class="col-sm-5"></div>
            <div class="col-sm-4"></div>
            <div class="col-sm-1"><%# GetLocalResourceObject("strTotal").ToString() %></div>
            <div class="col-sm-2"><asp:Label ID="lblTotal" runat="server" CssClass="pull-right"></asp:Label></div>
        </div>
    </FooterTemplate>
</asp:Repeater>