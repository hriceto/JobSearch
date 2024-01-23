<%@ Control Language="C#" AutoEventWireup="true" CodeFile="topiclist.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.topiclist" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>

<asp:Repeater ID="rptrTopics" runat="server" OnItemDataBound="rptrTopics_ItemDataBound">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate>
        <div class="row" id="divTopicGroup" runat="server" visible="false">
            <div class="col-sm-12">
                <h2><asp:Label ID="lblTopicGroup" runat="server"></asp:Label></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <strong><asp:HyperLink ID="hypTopicName" runat="server"></asp:HyperLink></strong>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
</asp:Repeater>