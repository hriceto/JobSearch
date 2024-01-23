<%@ Control Language="C#" AutoEventWireup="true" CodeFile="industrylist.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.industrylist" %>

<div class="row">
    <div class="col-sm-12">
        <h1>
            <ol class="breadcrumb">
                <li class="active"><%= GetLocalResourceObject("strIndustryListHeader").ToString() %></li>
            </ol>
        </h1>
    </div>
</div>

<asp:Repeater ID="rptrIndustries" runat="server" OnItemDataBound="rptrIndustries_ItemDataBound">
    <HeaderTemplate>
        <div class="row">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="col-sm-6">
            <h5><asp:HyperLink ID="hypIndustry" runat="server"></asp:HyperLink></h5>
        </div>
        <%# ((Container.ItemIndex + 1) % 2) == 0 ? "</div><div class=\"row\">" : "" %>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>