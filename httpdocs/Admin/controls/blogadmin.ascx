<%@ Control Language="C#" AutoEventWireup="true" CodeFile="blogadmin.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.blogadmin" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>

<div class="form-group row">
    <div class="col-sm-4">
        <asp:HyperLink ID="hypAddNewBlog" runat="server" meta:resourcekey="hypAddNewBlog" CssClass="btn btn-primary"></asp:HyperLink>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-4">
        <h1><asp:Label ID="lblExistingBlogs" runat="server" meta:resourcekey="lblExistingBlogs"></asp:Label></h1>
    </div>
</div>  


<asp:Repeater ID="rptrBlogs" runat="server" OnItemDataBound="rptrBlogs_ItemDataBound">
    <HeaderTemplate>
        <div class="form-group row">
            <div class="col-sm-4"><strong><%# GetLocalResourceObject("strHeaderTitle").ToString()%></strong></div>
            <div class="col-sm-2"><strong><%# GetLocalResourceObject("strHeaderCreatedDate").ToString()%></strong></div>
            <div class="col-sm-2"><strong><%# GetLocalResourceObject("strHeaderPublished").ToString()%></strong></div>
            <div class="col-sm-2"><strong><%# GetLocalResourceObject("strHeaderUrl").ToString()%></strong></div>
            <div class="col-sm-2"></div>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="form-group row">
            <div class="col-sm-4"><%# ((Blog)Container.DataItem).Title %></div>
            <div class="col-sm-2"><%# ((Blog)Container.DataItem).CreatedDate.ToShortDateString() %></div>
            <div class="col-sm-2"><%# ((Blog)Container.DataItem).Published.ToString() %></div>
            <div class="col-sm-2"><asp:HyperLink ID="hypViewBlog" runat="server"></asp:HyperLink></div>
            <div class="col-sm-2"><asp:HyperLink ID="hypEditBlog" runat="server" meta:resourcekey="hypEditBlog" class="btn btn-primary"></asp:HyperLink></div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>

<div>
    <he:paging id="hePaging" runat="server"/>
</div>