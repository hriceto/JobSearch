<%@ Control Language="C#" AutoEventWireup="true" CodeFile="updatepassword.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.updatepassword" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        updatePasswordOnLoadScript();
    });
                
    function updatePasswordOnLoadScript() {
        jQuery('#<%= txtCurrentPassword.ClientID %>').keypress(OnKeyPressUpdatePassword);
        jQuery('#<%= txtNewPassword.ClientID %>').keypress(OnKeyPressUpdatePassword);
        jQuery('#<%= txtNewPasswordRepeat.ClientID %>').keypress(OnKeyPressUpdatePassword);
        jQuery('#<%= txtCurrentPassword.ClientID %>').focus();
        jQuery('#<%= txtNewPassword.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
    }

    function OnKeyPressUpdatePassword(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnUpdatePassword.ClientID %>').click();
            return false;
        }
    }

    function SetFocus<%= this.ClientID %>() {
        jQuery('#<%= txtCurrentPassword.ClientID %>').focus();
        return true;
    }
</script>

<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblCurrentPassword" runat="server" CssClass="control-label" 
            meta:resourcekey="lblCurrentPassword"></asp:Label>
    </div>
    <div class="col-sm-5">
        <asp:TextBox ID="txtCurrentPassword" runat="server" MaxLength="100" TextMode="Password" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword"
            meta:resourcekey="reqCurrentPassword" Display="Dynamic" ValidationGroup="UpdatePassword"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblNewPassword" runat="server" meta:resourcekey="lblNewPassword" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-5">
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" MaxLength="100" 
            CssClass="form-control input-sm" ValidationGroup="UpdatePassword"
            meta:resourcekey="txtNewPassword"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqNewPassword" runat="server" ControlToValidate="txtNewPassword"
            meta:resourcekey="reqNewPassword" Display="Dynamic" ValidationGroup="UpdatePassword" CssClass="text-danger">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regNewPassword" runat="server" ControlToValidate="txtNewPassword"
            meta:resourcekey="regNewPassword" ValidationGroup="UpdatePassword" Display="Dynamic"
            ValidationExpression="^((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,})$" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblNewPasswordRepeat" runat="server" meta:resourcekey="lblNewPasswordRepeat" 
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-5">
        <asp:TextBox ID="txtNewPasswordRepeat" runat="server" TextMode="Password" MaxLength="100" 
            CssClass="form-control input-sm" ValidationGroup="UpdatePassword"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqNewPasswordRepeat" runat="server" ControlToValidate="txtNewPasswordRepeat"
            meta:resourcekey="reqNewPasswordRepeat" Display="Dynamic" ValidationGroup="UpdatePassword"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
        <asp:CompareValidator ID="compNewPasswordRepeat" runat="server" ControlToValidate="txtNewPasswordRepeat"
            ControlToCompare="txtNewPassword" meta:resourcekey="compNewPasswordRepeat" Display="Dynamic"
            ValidationGroup="UpdatePassword" CssClass="text-danger">
        </asp:CompareValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Button id="btnUpdatePassword" runat="server" OnClick="btnUpdatePassword_Click" CssClass="btn btn-primary"
            meta:resourcekey="btnUpdatePassword" ValidationGroup="UpdatePassword"/>
    </div>
</div>

<he:formvalidator id="heFormValidator" runat="server"/>