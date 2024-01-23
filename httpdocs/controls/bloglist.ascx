<%@ Control Language="C#" AutoEventWireup="true" CodeFile="bloglist.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.bloglist" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>
<%@ Register TagPrefix="he" TagName="topiclist" Src="~/controls/topiclist.ascx" %>

<div class="row">
    <div class="col-sm-12">
        <h1><asp:Label ID="lblBlogHeading" runat="server"></asp:Label></h1>
        <h3 id="h3Description" runat="server" visible="false"><asp:Label ID="lblBlogDescription" runat="server"></asp:Label></h3>
        <hr />
    </div>
</div>

<div class="row">
    <div class="col-sm-9">
        <asp:Repeater ID="rptrBlogs" runat="server" OnItemDataBound="rptrBlogs_ItemDataBound">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                <div class="row">
                    <div class="col-sm-12">
                        <h2><asp:HyperLink ID="hypBlogTitle" runat="server"></asp:HyperLink></h2>
                    </div>  
                </div>        
                <div class="row">
                    <div class="col-sm-12">
                        <div><%# String.Format("{0} {1} {2}", GetLocalResourceObject("strAuthorBy").ToString(), ((Blog)Container.DataItem).User.FirstName, ((Blog)Container.DataItem).User.LastName)%></div>
                        <%# ((Blog)Container.DataItem).PublishDate.HasValue ? String.Format("<div>{0}</div>", ((Blog)Container.DataItem).PublishDate.Value.ToString("MM/dd/yyyy")) : ""%>
                        <div><%# ((Blog)Container.DataItem).SubTitle %></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        &nbsp;
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="col-sm-3">
        <he:topiclist id="heTopicList" runat="server"/>
    </div>
</div>

<div>
    <he:paging id="hePaging" runat="server"/>
</div>