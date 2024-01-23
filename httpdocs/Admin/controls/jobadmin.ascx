<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jobadmin.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.jobadmin" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>


<div class="row">
    <div class="col-sm-8">
        <h4>
            <%= GetLocalResourceObject("strJobsPendingReview").ToString() %>
        </h4>
    </div>
</div>
<asp:Repeater ID="rptrJobsForReview" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row">
            <div class="col-sm-8">
                <%# ((JobPost)Container.DataItem).Title %>
            </div>    
            <div class="col-sm-4">
                <asp:HyperLink ID="hypReview" runat="server" meta:resourcekey="hypReview" CssClass="btn btn-primary" />
            </div>
        </div>
    </ItemTemplate>
    <SeparatorTemplate>
        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
    </SeparatorTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>


<div class="row">
    <div class="col-sm-8">
        <h4>
            <%= GetLocalResourceObject("strJobSearch").ToString() %>
        </h4>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
    </div>
    <div class="col-sm-4">
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Button id="btnJobSearch" runat="server" OnClick="btnJobSearch_Click" CssClass="btn btn-primary"
            meta:resourcekey="btnJobSearch"/>
    </div>
</div>
<asp:Repeater ID="rptrJobSearch" runat="server" OnItemDataBound="rptrJobs_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row">
            <div class="col-sm-8">
                <%# ((JobPost)Container.DataItem).Title %>
            </div>    
            <div class="col-sm-4">
                <asp:HyperLink ID="hypReview" runat="server" meta:resourcekey="hypReview" CssClass="btn btn-primary" />
            </div>
        </div>
    </ItemTemplate>
    <SeparatorTemplate>
        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
    </SeparatorTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>
