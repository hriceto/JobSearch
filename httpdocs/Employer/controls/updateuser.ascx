<%@ Control Language="C#" AutoEventWireup="true" CodeFile="updateuser.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.updateuser" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        updateProfileOnLoadScript();
    });

    function updateProfileOnLoadScript() {
        jQuery('#<%= txtUserFirstName.ClientID %>').keypress(OnKeyPressUpdateProfile);
        jQuery('#<%= txtUserLastName.ClientID %>').keypress(OnKeyPressUpdateProfile);
        jQuery('#<%= chkbOkToEmail.ClientID %>').keyup(OnKeyPressUpdateProfile);
    }

    function OnKeyPressUpdateProfile(e) {
        if (e.which == 13) {
            jQuery('#<%= btnUpdateUser.ClientID %>').click();
            return false;
        }
    }

    function SetFocus<%= this.ClientID %>() {
        jQuery('#<%= txtUserFirstName.ClientID %>').focus();
        return true;
    }
</script>


<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblUserFirstName" runat="server" meta:resourcekey="lblUserFirstName"
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtUserFirstName" runat="server" ValidationGroup="UpdateUser" MaxLength="100"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqUserFirstName" runat="server" ControlToValidate="txtUserFirstName"
            meta:resourcekey="reqUserFirstName" Display="Dynamic" ValidationGroup="UpdateUser" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblUserLastName" runat="server" meta:resourcekey="lblUserLastName"
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtUserLastName" runat="server" ValidationGroup="UpdateUser" MaxLength="100"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqUserLastName" runat="server" ControlToValidate="txtUserLastName"
            meta:resourcekey="reqUserLastName" Display="Dynamic" ValidationGroup="UpdateUser" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:CheckBox ID="chkbOkToEmail" runat="server" meta:resourcekey="chkbOkToEmail" CssClass="checkbox"/>
    </div>
</div>
    
<he:formvalidator id="heFormValidator" runat="server"/>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Button id="btnUpdateUser" runat="server" meta:resourcekey="btnUpdateUser" ValidationGroup="UpdateUser"
            OnClick="btnUpdateUser_Click" CssClass="btn btn-primary"/>
    </div>
</div>