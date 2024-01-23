<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jobedit.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.jobedit" %>
<script type="text/javascript">
    function ChangeSuspendJobPostOption(control) {
        var checked = jQuery(control).is(':checked');
        if (checked) {
            jQuery('#<%= divSuspendJobPostReason.ClientID %>').css('display', 'block');
        }
        else {
            jQuery('#<%= divSuspendJobPostReason.ClientID %>').css('display', 'none');
        }
    }

    function ValidateSuspendJobPostOption() {
        if (jQuery("#<%= chkbJobSuspended.ClientID %>").is(":checked")) {
            return Page_ClientValidate('SuspendJobPostReason');
        }
    }
</script>

<asp:Panel ID="pnlEditJob" runat="server" Visible="false">
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobPostIdLabel" runat="server" meta:resourcekey="lblJobPostIdLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobPostId" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblReviewRequiredLabel" runat="server" meta:resourcekey="lblReviewRequiredLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblReviewRequired" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblStartDateLabel" runat="server" meta:resourcekey="lblStartDateLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblStartDate" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblEndDateLabel" runat="server" meta:resourcekey="lblEndDateLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblEndDate" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblIsFreeLabel" runat="server" meta:resourcekey="lblIsFreeLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblIsFree" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblIsPaidLabel" runat="server" meta:resourcekey="lblIsPaidLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblIsPaid" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblIsAnonymousLabel" runat="server" meta:resourcekey="lblIsAnonymousLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblIsAnonymous" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobTitleLabel" runat="server" meta:resourcekey="lblJobTitleLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobTitle" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobDescriptionLabel" runat="server" meta:resourcekey="lblJobDescriptionLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobDescription" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobRequirementsLabel" runat="server" meta:resourcekey="lblJobRequirementsLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobRequirements" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobBenefitsLabel" runat="server" meta:resourcekey="lblJobBenefitsLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobBenefits" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobPositionLabel" runat="server" meta:resourcekey="lblJobPositionLabel" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblJobPosition" runat="server"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobSuspended" runat="server" meta:resourcekey="lblJobSuspended" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-9">
            <asp:CheckBox ID="chkbJobSuspended" runat="server" onclick="javascript:ChangeSuspendJobPostOption(this);" />
        </div>
    </div>
    <div class="form-group row" id="divSuspendJobPostReason" runat="server">
        <div class="col-sm-3">
            <%= GetLocalResourceObject("strSuspendJobPostReason").ToString() %>
        </div>
        <div class="col-sm-9">
            <asp:RadioButtonList ID="rbtlSuspendJobPostReason" runat="server" ValidationGroup="SuspendJobPostReason">
                <asp:ListItem meta:resourcekey="rbtlSuspendJobPostReason_Item1"></asp:ListItem>
                <asp:ListItem meta:resourcekey="rbtlSuspendJobPostReason_Item2"></asp:ListItem>
                <asp:ListItem meta:resourcekey="rbtlSuspendJobPostReason_Item3"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="reqSuspendJobPostReason" runat="server" ControlToValidate="rbtlSuspendJobPostReason"
                meta:resourcekey="reqSuspendJobPostReason" ValidationGroup="SuspendJobPostReason"
                Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Button ID="btnSubmitReview" runat="server" OnClick="btnSubmitReview_Click" meta:resourcekey="btnSubmitReview"
                OnClientClick="javascript: return ValidateSuspendJobPostOption();" CssClass="btn btn-primary"/>
        </div>
        <div class="col-sm-3"> 
        </div>
        <div class="col-sm-3"> 
            <asp:HyperLink ID="hypCompanyLink" runat="server" meta:resourcekey="hypCompanyLink" CssClass="btn btn-default"></asp:HyperLink>
        </div>
    </div>
    
</asp:Panel>
