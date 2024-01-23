<%@ Control Language="C#" AutoEventWireup="true" CodeFile="blogdetail.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.blogdetail" %>
<%@ Register TagPrefix="he" TagName="topiclist" Src="~/controls/topiclist.ascx" %>

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
    <div class="col-sm-9">
        <div class="row">
            <div class="col-sm-12">
                <div><asp:Label ID="lblAuthor" runat="server"></asp:Label></div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <h2 class="text-primary"><asp:Label ID="lblPublishDate" runat="server"></asp:Label></h2>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <h2 class="text-primary"><asp:Label ID="lblSubTitle" runat="server"></asp:Label></h2>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <h4 class="text-primary"><asp:Label ID="lblSummary" runat="server"></asp:Label></h4>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblContent" runat="server"></asp:Label>
            </div>
        </div>

        <%--facebook, twitter, etc. share--%>
    </div>
    <div class="col-sm-3">
        <he:topiclist id="heTopicList" runat="server"/>
    </div>
</div>
</asp:PlaceHolder>