<%@ Control Language="C#" AutoEventWireup="true" CodeFile="updateuser.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.JobSeeker.Controls.updateuser" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script language="javascript" type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        updateJobSeekerProfileOnLoadScript();
    });
                
    function updateJobSeekerProfileOnLoadScript() {
        jQuery('#<%= txtFirstName.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
        jQuery('#<%= txtLastName.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
        jQuery('#<%= txtPhone.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
        jQuery('#<%= chkbPublicResume.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
        jQuery('#<%= lstbUserCategories.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
        jQuery('#<%= chkbOkToEmail.ClientID %>').keypress(OnKeyPressUpdateJobSeekerProfile);
    }

    function OnKeyPressUpdateJobSeekerProfile(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnUpdateUser.ClientID %>').click();
            return false;
        }
    }
    
    function SetFocus<%= this.ClientID %>() {
        jQuery('#<%= txtFirstName.ClientID %>').focus();
        return true;
    }
</script>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" 
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="UpdateUser" MaxLength="100"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName"
            ValidationGroup="UpdateUser" meta:resourcekey="reqFirstName" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="UpdateUser" MaxLength="100"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName"
            ValidationGroup="UpdateUser" meta:resourcekey="reqLastName" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:Label ID="lblCoverLetter" runat="server" meta:resourcekey="lblCoverLetter" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox ID="txtCoverLetter" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regCoverLetter" runat="server" ControlToValidate="txtCoverLetter" 
            ValidationGroup="UpdateUser" ValidationExpression="^.{0,25000}$" meta:resourcekey="regCoverLetter"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-12">
        <asp:Label ID="lblResume" runat="server" meta:resourcekey="lblResume" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox ID="txtResume" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regResume" runat="server" ControlToValidate="txtResume" 
            ValidationGroup="UpdateUser" ValidationExpression="^.{0,25000}$" meta:resourcekey="regResume"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblPhone" runat="server" meta:resourcekey="lblPhone" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtPhone" runat="server" ValidationGroup="UpdateUser" MaxLength="20" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-12">
        <asp:CheckBox ID="chkbPublicResume" runat="server" meta:resourcekey="chkbPublicResume" CssClass="checkbox"/>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblUserCategories" runat="server" meta:resourcekey="lblUserCategories" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:ListBox ID="lstbUserCategories" runat="server" DataValueField="CategoryId" DataTextField="Name" 
            SelectionMode="Multiple" CssClass="form-control categories-listbox" Rows="6" meta:resourcekey="lstbUserCategories">
        </asp:ListBox>
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