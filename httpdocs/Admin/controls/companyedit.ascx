<%@ Control Language="C#" AutoEventWireup="true" CodeFile="companyedit.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.companyedit" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>

<script type="text/javascript">
    function ChangeAllowFreePostsOption(control) {
        var checked = jQuery(control).is(':checked');
        if (checked) {
            jQuery('#<%= divDeclineFreePostsReason.ClientID %>').hide('fast');
        }
        else {
            jQuery('#<%= divDeclineFreePostsReason.ClientID %>').show('fast');
        }
    }

    function ValidateAllowFreePostsOption() {
        if (!jQuery("#<%= chkbCompanyAllowFreePosts.ClientID %>").is(":checked")) {
            return Page_ClientValidate('DeclineFreePostsReason');
        }
        return Page_ClientValidate('UpdateCompanyReview');
    }
</script>

<asp:Panel ID="pnlReviewCompany" runat="server" Visible="false">
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyId").ToString() %>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblCompanyId" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyName").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyName" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyAddress").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyAddress" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyPhone").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyPhone" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyWebsite").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyWebsite" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyCreatedDate").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyCreatedDate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyUsers").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblCompanyUsers" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyIsRecruiter").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:CheckBox ID="chkbCompanyIsRecruiter" runat="server"></asp:CheckBox>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strAllowFreePosts").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:CheckBox ID="chkbCompanyAllowFreePosts" runat="server" onclick="javascript:ChangeAllowFreePostsOption(this);" />
        </div>
    </div>
    <div id="divDeclineFreePostsReason" runat="server" class="row">
        <div class="col-sm-12">
            <div class="form-group row">
                <div class="col-sm-12">
                    <%= GetLocalResourceObject("strDeclineFreePostsReason").ToString() %>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-9">
                    <asp:RadioButtonList ID="rbtlDeclineFreePostsReason" runat="server" ValidationGroup="DeclineFreePostsReason">
                        <asp:ListItem meta:resourcekey="rbtlDeclineFreePostsReason_Item1"></asp:ListItem>
                        <asp:ListItem meta:resourcekey="rbtlDeclineFreePostsReason_Item2"></asp:ListItem>
                        <asp:ListItem meta:resourcekey="rbtlDeclineFreePostsReason_Item3"></asp:ListItem>
                        <asp:ListItem meta:resourcekey="rbtlDeclineFreePostsReason_Item4"></asp:ListItem>
                        <asp:ListItem meta:resourcekey="rbtlDeclineFreePostsReason_Item5"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqDeclineFreePostsReason" runat="server" ControlToValidate="rbtlDeclineFreePostsReason"
                        meta:resourcekey="reqDeclineFreePostsReason" ValidationGroup="DeclineFreePostsReason" 
                        Display="Dynamic" CssClass="text-danger">
                    </asp:RequiredFieldValidator>        
                </div>
            </div>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strCompanyDomain").ToString() %>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyDomain" runat="server" MaxLength="100" ValidationGroup="UpdateCompanyReview"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RegularExpressionValidator ID="regCompanyDomain" runat="server" ControlToValidate="txtCompanyDomain"
                ValidationGroup="UpdateCompanyReview" meta:resourcekey="regCompanyDomain" ValidationExpression="\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </div>
    </div>
    <asp:Panel ID="pnlCompanyApplication" runat="server" CssClass="form-group row" Visible="false">
        <div class="col-sm-3">
            <div class="form-group row">&nbsp;</div>
            <%= GetLocalResourceObject("strCompanyApplication").ToString() %>
        </div>
        <div class="col-sm-9">
            <div class="form-group row">
                <div class="col-sm-4">
                    <%= GetLocalResourceObject("strCompanyApplicationNumberOfEmployees").ToString() %>
                </div>
                <div class="col-sm-4">
                    <%= GetLocalResourceObject("strCompanyApplicationNumberOfPosts").ToString()%>
                </div>
                <div class="col-sm-4">
                    <%= GetLocalResourceObject("strCompanyApplicationApproved").ToString() %>    
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4">
                    <asp:Label ID="lblCompanyApplicationNumberOfEmployees" runat="server"></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:Label ID="lblCompanyApplicationNumberOfPostsPerYear" runat="server"></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:CheckBox id="chkbCompanyApplicationApprove" runat="server"/>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Button ID="btnReviewCompany" runat="server" OnClick="btnReviewCompany_Click"
                meta:resourcekey="btnReviewCompany" ValidationGroup="UpdateCompanyReview" 
                OnClientClick="javascript: return ValidateAllowFreePostsOption();" CssClass="btn btn-primary" />
        </div>
        <div class="col-sm-2">
        </div>
        <div class="col-sm-3">
            <asp:HyperLink ID="hypCompanyJobs" runat="server" meta:resourcekey="hypCompanyJobs"
                CssClass="btn btn-default"></asp:HyperLink>
        </div>
        <div class="col-sm-2">
            <asp:HyperLink ID="hypViewCompanyCoupons" runat="server" meta:resourcekey="hypViewCompanyCoupons" 
                CssClass="btn btn-default"></asp:HyperLink>
        </div>
        <div class="col-sm-2">
            <asp:HyperLink ID="hypAddCompanyCoupon" runat="server" meta:resourcekey="hypAddCompanyCoupon" 
                CssClass="btn btn-default"></asp:HyperLink>
        </div>
    </div>
</asp:Panel>
