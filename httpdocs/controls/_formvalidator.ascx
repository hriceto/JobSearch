<%@ Control Language="C#" AutoEventWireup="true" CodeFile="_formvalidator.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls._formvalidator" %>

<div style="display: none;">
    Email:
    <asp:TextBox ID="txtEmailVerification" runat="server" ValidationGroup="Verification" TabIndex="-1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="reqEmailVerification" runat="server" ControlToValidate="txtEmailVerification"
        ErrorMessage="*" ValidationGroup="Verification">
    </asp:RequiredFieldValidator>
</div>
<div class="email_verification">
    Email:
    <asp:TextBox ID="txtEmailVerification2" runat="server" ValidationGroup="Verification2" TabIndex="-1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="reqEmailVerification2" runat="server" ControlToValidate="txtEmailVerification2"
        ErrorMessage="*" ValidationGroup="Verification2"></asp:RequiredFieldValidator>
</div>