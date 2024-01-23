<%@ Control Language="C#" AutoEventWireup="true" CodeFile="verifyemail.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.verifyemail" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<% if(pnlResetEmailVerification.Visible) { %>
<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        resetEmailVerificationOnLoadScript();
    });

    function resetEmailVerificationOnLoadScript() {
        jQuery('#<%= txtResetEmailVerificationEmail.ClientID %>').keypress(OnEnterKeyResetEmailVerificationChange);
        jQuery('#<%= txtResetEmailVerificationFirstName.ClientID %>').keypress(OnEnterKeyResetEmailVerificationChange);

        jQuery('#<%= txtResetEmailVerificationEmail.ClientID %>').focus();
    }

    function OnEnterKeyResetEmailVerificationChange(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnResetEmailVerification.ClientID %>').click();
            return false;
        }
    }

</script>
<% } %>

<asp:Panel ID="pnlResetEmailVerification" runat="server" Visible="false">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblResetEmailVerification" runat="server" CssClass="control-label" 
                meta:resourcekey="lblResetEmailVerification"></asp:Label></h2>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblResetEmailVerificationEmail" runat="server" CssClass="control-label" 
                meta:resourcekey="lblResetEmailVerificationEmail"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtResetEmailVerificationEmail" runat="server" CssClass="form-control"
                ValidationGroup="ResetEmailVerification" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqResetEmailVerificationEmail" runat="server" ControlToValidate="txtResetEmailVerificationEmail"
                meta:resourcekey="reqResetEmailVerificationEmail" Display="Dynamic" ValidationGroup="ResetEmailVerification"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regResetEmailVerificationEmail" runat="server" ControlToValidate="txtResetEmailVerificationEmail" 
                meta:resourcekey="regResetEmailVerificationEmail" ValidationGroup="ResetEmailVerification" 
                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblResetEmailVerificationFirstName" runat="server" CssClass="control-label" 
                meta:resourcekey="lblResetEmailVerificationFirstName"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtResetEmailVerificationFirstName" runat="server" CssClass="form-control" 
                ValidationGroup="ResetEmailVerification" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqResetEmailVerificationFirstName" runat="server" ControlToValidate="txtResetEmailVerificationFirstName"
                meta:resourcekey="reqResetEmailVerificationFirstName" Display="Dynamic" ValidationGroup="ResetEmailVerification"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button ID="btnResetEmailVerification" runat="server" meta:resourcekey="btnResetEmailVerification" 
                OnClick="btnResetEmailVerification_Click" CssClass="btn btn-primary" ValidationGroup="ResetEmailVerification" />
        </div>
    </div>
</asp:Panel>

<he:formvalidator id="heFormValidator" runat="server"/>