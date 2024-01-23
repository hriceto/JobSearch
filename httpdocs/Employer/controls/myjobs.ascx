<%@ Control Language="C#" AutoEventWireup="true" CodeFile="myjobs.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.myjobs" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>

<div class="form-group row">
    <div class="col-sm-12">
        <asp:HyperLink id="hypAddJob" runat="server" meta:resourcekey="hypAddJob" CssClass="btn btn-primary"/><asp:Label ID="lblAddJobDisabled" runat="server" meta:resourcekey="lblAddJobDisabled" Visible="false"></asp:Label>
    </div>
</div>

<asp:PlaceHolder ID="plhPendingReview" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblTablePendingReview" runat="server" meta:resourcekey="lblTablePendingReview"></asp:Label></h2>
        </div>
    </div>
    <asp:Repeater ID="rptrJobsPendingReview" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <%# ((JobPost)Container.DataItem).Title %>
                </div>
                <div class="col-sm-1"></div>
                <div class="col-sm-2"></div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group row">
        <div class="col-sm-12">
            <hr />
        </div>
    </div>
</asp:PlaceHolder>

<asp:PlaceHolder ID="plhPublished" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblTableCurrent" runat="server" meta:resourcekey="lblTableCurrent"></asp:Label></h2>
        </div>
    </div>
    <asp:Repeater ID="rptrJobsPublished" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <%# ((JobPost)Container.DataItem).Title %>
                </div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypEditJob" runat="server" meta:resourcekey="hypEditJob"></asp:HyperLink>
                </div>
                <div class="col-sm-2"></div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypViewApplications" runat="server" meta:resourcekey="hypViewApplications"></asp:HyperLink>
                </div>
                <div class="col-sm-1">
                    <asp:Panel ID="pnlSuspend" runat="server">
                        <a href="javascript:void(0);" alt="<%= GetLocalResourceObject("aSuspendPopup.alt").ToString() %>" 
                            title="<%= GetLocalResourceObject("aSuspendPopup.title").ToString() %>" 
                            onclick="javascript:jQuery('.divSuspendModal_<%# Container.ItemIndex %>').modal('show');">
                            <span class="glyphicon glyphicon-remove"></span>
                        </a>
                        <div class="modal fade divSuspendModal_<%# Container.ItemIndex %>" tabindex="-1" role="dialog">
                            <div class="modal-dialog modal-sm">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h2 class="modal-title" id="mySmallModalLabel"><%= GetLocalResourceObject("strSuspendHeader").ToString() %></h2>
                                    </div>
                                    <div class="modal-body">
                                        <%= GetLocalResourceObject("strSuspendQuestion").ToString() %>
                                        <br /><br />
                                        <asp:Button ID="btnSuspend" runat="server" meta:resourcekey="btnSuspend" 
                                            OnCommand="btnSuspend_Command" CssClass="btn btn-primary"></asp:Button>
                                        <button type="button" class="btn btn-default pull-right" data-dismiss="modal" aria-hidden="true"><%= GetLocalResourceObject("btnCancel").ToString() %></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group row">
        <div class="col-sm-12">
            <he:paging id="hePagingPublished" runat="server"/>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <hr />
        </div>
    </div>
</asp:PlaceHolder>

<asp:PlaceHolder ID="plhPublishedFuture" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblTablePublishedFuture" runat="server" meta:resourcekey="lblTablePublishedFuture"></asp:Label></h2>
        </div>
    </div>
    <asp:Repeater ID="rptrJobsPublishedFuture" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <%# ((JobPost)Container.DataItem).Title %> - <%# ((JobPost)Container.DataItem).StartDate.Value.ToString("MM/dd/yyyy") %>
                </div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypEditJob" runat="server" meta:resourcekey="hypEditJob"></asp:HyperLink>
                </div>
                <div class="col-sm-2"></div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group row">
        <div class="col-sm-12">
            <he:paging id="hePagingPublishedFuture" runat="server"/>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <hr />
        </div>
    </div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="plhUnpublished" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:Label ID="lblPublishInstructions" runat="server" meta:resourcekey="lblPublishInstructions" CssClass="text-info"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblTableUnpublished" runat="server" meta:resourcekey="lblTableUnpublished"></asp:Label></h2>
        </div>
    </div>
    <asp:Repeater ID="rptrJobsUnpublished" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <%# ((JobPost)Container.DataItem).Title %>
                </div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypEditJob" runat="server" meta:resourcekey="hypEditJob"></asp:HyperLink>
                </div>
                <div class="col-sm-2">
                    <asp:HyperLink ID="hypPublishJob" runat="server" meta:resourcekey="hypPublishJob"></asp:HyperLink>
                </div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group row">
        <div class="col-sm-12">
            <hr />
        </div>
    </div>
</asp:PlaceHolder>

<asp:PlaceHolder ID="plhExpired" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblTableExpired" runat="server" meta:resourcekey="lblTableExpired"></asp:Label></h2>
        </div>
    </div>
    <asp:Repeater ID="rptrJobsPublishedExpired" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
        <ItemTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <%# ((JobPost)Container.DataItem).Title %>
                </div>
                <div class="col-sm-1"></div>
                <div class="col-sm-2">
                    <asp:HyperLink ID="hypRepublish" runat="server" meta:resourcekey="hypRepublish"></asp:HyperLink>
                </div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypViewApplications" runat="server" meta:resourcekey="hypViewApplications"></asp:HyperLink>
                </div>
                <div class="col-sm-1"></div>
                <div class="col-sm-1">
                    <asp:HyperLink ID="hypPreviewJob" runat="server" meta:resourcekey="hypPreviewJob"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="form-group row">
        <div class="col-sm-12">
            <he:paging id="hePagingExpired" runat="server"/>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <hr />
        </div>
    </div>
</asp:PlaceHolder>