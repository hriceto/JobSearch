<%@ Control Language="C#" AutoEventWireup="true" CodeFile="couponedit.ascx.cs"
    Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.couponedit" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        adminCouponAddEditLoadScript();
    });

    function adminCouponAddEditLoadScript() {
        jQuery('#<%= txtStartDate.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true,
            autoclose: true, todayHighlight: true
        });
        jQuery('#<%= txtEndDate.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true,
            autoclose: true, todayHighlight: true
        });
    }
</script>

<div class="form-group row">
    <div class="col-sm-4">
        <asp:HyperLink ID="hypCouponSearch" runat="server" meta:resourcekey="hypCouponSearch" CssClass="btn btn-primary"></asp:HyperLink>
    </div>
</div>


<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblCouponIdLabel" runat="server" meta:resourcekey="lblCouponIdLabel"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:Label ID="lblCouponId" runat="server"></asp:Label>
    </div>
    <div class="col-sm-4"></div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblCouponUserId" runat="server" meta:resourcekey="lblCouponUserId"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtCouponUserId" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regCouponUserId" runat="server" meta:resourcekey="regCouponUserId"
            ControlToValidate="txtCouponUserId" Display="Dynamic" CssClass="text-danger" 
            ValidationGroup="AddEditCoupon" ValidationExpression="[0-9]+">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblCouponCompanyId" runat="server" meta:resourcekey="lblCouponCompanyId"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtCouponCompanyId" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regCouponCompanyId" runat="server" meta:resourcekey="regCouponCompanyId"
            ControlToValidate="txtCouponCompanyId" Display="Dynamic" CssClass="text-danger" 
            ValidationGroup="AddEditCoupon" ValidationExpression="[0-9]+">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblCouponCode" runat="server" meta:resourcekey="lblCouponCode"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtCouponCode" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RequiredFieldValidator ID="reqCouponCode" runat="server" ControlToValidate="txtCouponCode"
            Display="Dynamic" CssClass="text-danger" ValidationGroup="AddEditCoupon"
            meta:resourcekey="reqCouponCode">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblNumberOfUsesLabel" runat="server" meta:resourcekey="lblNumberOfUsesLabel"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:Label ID="lblNumberOfUses" runat="server"></asp:Label>
    </div>
    <div class="col-sm-4"></div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblNumberOfUsesLimit" runat="server" meta:resourcekey="lblNumberOfUsesLimit"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtNumberOfUsesLimit" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RequiredFieldValidator ID="reqNumberOfUsesLimit" runat="server" ControlToValidate="txtNumberOfUsesLimit"
            Display="Dynamic" CssClass="text-danger" ValidationGroup="AddEditCoupon"
            meta:resourcekey="reqNumberOfUsesLimit">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regNumberOfUsesLimit" runat="server" meta:resourcekey="regNumberOfUsesLimit"
            ControlToValidate="txtNumberOfUsesLimit" Display="Dynamic" CssClass="text-danger" 
            ValidationGroup="AddEditCoupon" ValidationExpression="[0-9]+">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblDiscountPercentage" runat="server" meta:resourcekey="lblDiscountPercentage"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtDiscountPercentage" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regDiscountPercentage" runat="server" meta:resourcekey="regDiscountPercentage"
            ControlToValidate="txtDiscountPercentage" Display="Dynamic" CssClass="text-danger" 
            ValidationGroup="AddEditCoupon" ValidationExpression="([0-9]+)(\.[0-9]+)?">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblDiscountAmount" runat="server" meta:resourcekey="lblDiscountAmount"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regDiscountAmount" runat="server" meta:resourcekey="regDiscountAmount"
            ControlToValidate="txtDiscountAmount" Display="Dynamic" CssClass="text-danger" 
            ValidationGroup="AddEditCoupon" ValidationExpression="([0-9]+)(\.[0-9]+)?">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblStartDate" runat="server" meta:resourcekey="lblStartDate"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regStartDate" runat="server" ControlToValidate="txtStartDate"
            ValidationGroup="AddEditCoupon" meta:resourcekey="regStartDate" Display="Dynamic"
            ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblEndDate" runat="server" meta:resourcekey="lblEndDate"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regEndDate" runat="server" ControlToValidate="txtEndDate"
            ValidationGroup="AddEditCoupon" meta:resourcekey="regEndDate" Display="Dynamic"
            ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Button id="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-primary" ValidationGroup="AddEditCoupon"/>
    </div>
    <div class="col-sm-4">
    </div>
    <div class="col-sm-4">
        <asp:Button ID="btnDeactivate" runat="server" OnClick="btnDeactivate_Click" CssClass="btn btn-secondary" meta:resourcekey="btnDeactivate"/>
    </div>
</div>