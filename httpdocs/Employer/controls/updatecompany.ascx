<%@ Control Language="C#" AutoEventWireup="true" CodeFile="updatecompany.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.updatecompany" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        updateCompanyOnLoadScript();
    });

    function updateCompanyOnLoadScript() {
        jQuery('#<%= txtCompanyAddress1.ClientID %>').keypress(OnKeyPressUpdateCompany);
        jQuery('#<%= txtCompanyAddress2.ClientID %>').keypress(OnKeyPressUpdateCompany);
        jQuery('#<%= txtCompanyCity.ClientID %>').keypress(OnKeyPressUpdateCompany);
        jQuery('#<%= ddlCompanyState.ClientID %>').keyup(OnKeyPressUpdateCompany);
        jQuery('#<%= txtCompanyZip.ClientID %>').keypress(OnKeyPressUpdateCompany);
        jQuery('#<%= ddlCompanyCountry.ClientID %>').keyup(OnKeyPressUpdateCompany);
        jQuery('#<%= txtCompanyPhoneNumber.ClientID %>').keypress(OnKeyPressUpdateCompany);
        jQuery('#<%= txtCompanyWebsite.ClientID %>').keypress(OnKeyPressUpdateCompany);
    }

    function OnKeyPressUpdateCompany(e) {
        if (e.which == 13) {
            jQuery('#<%= btnUpdateCompany.ClientID %>').click();
            return false;
        }
    }

    function SetFocus<%= this.ClientID %>() {
        jQuery('#<%= txtCompanyAddress1.ClientID %>').focus();
        return true;
    }
</script>


<div class="form-group row">
    <div class="col-sm-8">
        <asp:Label ID="lblCompanyIsRecruiter" runat="server" meta:resourcekey="lblCompanyIsRecruiter"
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-1">
        <asp:Label ID="lblCompanyIsRecruiterDisplay" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblCompanyName" runat="server" meta:resourcekey="lblCompanyName" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblCompanyNameDisplay" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<asp:Panel ID="pnlDisplayCompany" runat="server">
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyAddress1Label" runat="server" CssClass="control-label" 
                meta:resourcekey="lblDisplayCompanyAddress1Label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyAddress1" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyAddress2Label" runat="server" CssClass="control-label" 
                meta:resourcekey="lblDisplayCompanyAddress2Label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyAddress2" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyCityLabel" runat="server" meta:resourcekey="lblDisplayCompanyCityLabel"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyCity" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyZipLabel" runat="server" meta:resourcekey="lblDisplayCompanyZipLabel"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyZip" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyPhoneLabel" runat="server" meta:resourcekey="lblDisplayCompanyPhoneLabel"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyPhone" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <asp:Label ID="lblDisplayCompanyWebsiteLabel" runat="server" meta:resourcekey="lblDisplayCompanyWebsiteLabel"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:Label ID="lblDisplayCompanyWebsite" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlEditCompany" runat="server">
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyAddress1" runat="server" meta:resourcekey="lblCompanyAddress1"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyAddress1" runat="server" ValidationGroup="UpdateCompany" MaxLength="256"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqCompanyAddress1" runat="server" ControlToValidate="txtCompanyAddress1"
                meta:resourcekey="reqCompanyAddress1" Display="Dynamic" ValidationGroup="UpdateCompany"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyAddress2" runat="server" meta:resourcekey="lblCompanyAddress2"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyAddress2" runat="server" ValidationGroup="UpdateCompany" MaxLength="256"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyCity" runat="server" meta:resourcekey="lblCompanyCity"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyCity" runat="server" ValidationGroup="UpdateCompany" MaxLength="100"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqCompanyCity" runat="server" ControlToValidate="txtCompanyCity"
                meta:resourcekey="reqCompanyCity" Display="Dynamic" ValidationGroup="UpdateCompany"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyState" runat="server" meta:resourcekey="lblCompanyState"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:DropDownList ID="ddlCompanyState" runat="server" ValidationGroup="UpdateCompany"
                DataValueField="StateCode" DataTextField="StateName" CssClass="form-control input-sm">
            </asp:DropDownList>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqCompanyState" runat="server" ControlToValidate="ddlCompanyState"
                meta:resourcekey="reqCompanyState" Display="Dynamic" ValidationGroup="UpdateCompany"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyZip" runat="server" meta:resourcekey="lblCompanyZip"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyZip" runat="server" ValidationGroup="UpdateCompany" MaxLength="10"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqCompanyZip" runat="server" ControlToValidate="txtCompanyZip"
                meta:resourcekey="reqCompanyZip" Display="Dynamic" ValidationGroup="UpdateCompany"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regCompanyZip" runat="server" ControlToValidate="txtCompanyZip"
                meta:resourcekey="regCompanyZip" ValidationGroup="UpdateCompany" Display="Dynamic" 
                ValidationExpression="[0-9]{5}(-[0-9]{4})?" CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyCountry" runat="server" meta:resourcekey="lblCompanyCountry"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:DropDownList ID="ddlCompanyCountry" runat="server" ValidationGroup="UpdateCompany" 
                DataValueField="CountryCode" DataTextField="CountryName" CssClass="form-control input-sm">
            </asp:DropDownList>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqCompanyCountry" runat="server" ControlToValidate="ddlCompanyCountry"
                meta:resourcekey="reqCompanyCountry" Display="Dynamic" ValidationGroup="UpdateCompany"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyPhoneNumber" runat="server" meta:resourcekey="lblCompanyPhoneNumber"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyPhoneNumber" runat="server" ValidationGroup="UpdateCompany" MaxLength="20"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblCompanyWebsite" runat="server" meta:resourcekey="lblCompanyWebsite"
                CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtCompanyWebsite" runat="server" ValidationGroup="UpdateCompany" MaxLength="50"
                CssClass="form-control input-sm"></asp:TextBox>
        </div>
    </div>
    <he:formvalidator id="heFormValidator" runat="server"/>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Button id="btnUpdateCompany" runat="server" meta:resourcekey="btnUpdateCompany" ValidationGroup="UpdateCompany"
                OnClick="btnUpdateCompany_Click" CssClass="btn btn-primary"/>
        </div>
    </div>
</asp:Panel>