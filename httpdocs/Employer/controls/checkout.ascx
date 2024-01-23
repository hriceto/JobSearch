<%@ Control Language="C#" AutoEventWireup="true" CodeFile="checkout.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.checkout" %>
<%@ Register TagPrefix="he" TagName="shoppingcart" Src="~/Employer/controls/_shoppingcart.ascx" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        checkoutOnLoadScript();
    });

    function checkoutOnLoadScript() {
        jQuery('#<%= txtEnterCoupon.ClientID %>').keypress(OnKeyCoupon);

        jQuery('#<%= rdblBillingAddresses.ClientID %> input').keypress(OnKeyCheckout);
        jQuery('#<%= txtBillingFirstName.ClientID %> input').keypress(OnKeyCheckout);
        jQuery('#<%= txtBillingLastName.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= txtBillingAddress1.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= txtBillingAddress2.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= txtBillingCity.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= ddlBillingState.ClientID %> input').keyup(OnKeyCheckout);
        jQuery('#<%= txtBillingZip.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= ddlBillingCountry.ClientID %> input').keyup(OnKeyCheckout);
        jQuery('#<%= chkbSaveBillingAddress.ClientID %> input').keyup(OnKeyCheckout);
        jQuery('#<%= txtCreditCardNumber.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= ddlCreditCardExpirationMonth.ClientID %> input').keyup(OnKeyCheckout);
        jQuery('#<%= ddlCreditCardExpirationYear.ClientID %> input').keyup(OnKeyCheckout);
        jQuery('#<%= txtCreditCardCVV.ClientID %>').keypress(OnKeyCheckout);
        jQuery('#<%= imgSecure.ClientID %>').popover({ animation: true,
            content: '<%= GetLocalResourceObject("imgSecure.datacontent").ToString() %>',
            html: true,
            placement:'left',
            trigger: 'hover'
        });
    }

    function OnKeyCheckout(e) {
        if (e.which == 13) {
            return false;
        }
    }

    function OnKeyCoupon(e) {
        if (e.which == 13) {
            jQuery('#<%= btnEnterCoupon.ClientID %>').click();
            return false;
        }
    }

    function PublishClick(button) {
        if (ValidateBillingAddress()) {
            button.disabled = true;
            button.value = '<%= GetLocalResourceObject("strPublishButtonWorking").ToString() %>';
            return true;
        } 
        else { return false; }
    }
</script>


<asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
    <h2 class="text-center"><asp:Label ID="lblEmptyCart" runat="server"></asp:Label></h2>
</asp:Panel>

<asp:PlaceHolder ID="plhCheckout" runat="server">
    <script type="text/javascript">
        function ChangeBillingOption(value) {
            value = parseInt(value);
            if (value > 0) {
                jQuery('#<%= divNewBillingAddress.ClientID %>').css('display', 'none');
            }
            else {
                jQuery('#<%= divNewBillingAddress.ClientID %>').css('display', 'block');
            }
        }

        function ValidateBillingAddress() {
            if (jQuery("#<%= rdblBillingAddresses.ClientID %> input:radio:checked").val() == -1)
            {
                return Page_ClientValidate('NewBillingAddress');
            }
            return Page_ClientValidate('Checkout');
        }
    </script>

    <div class="form-group row">
        <div class="col-sm-12">
            <h2><%= GetLocalResourceObject("strShoppingCart").ToString() %></h2>
        </div>
    </div>
    
    <he:shoppingcart id="heShoppingCart" runat="server"></he:shoppingcart>

    <div class="form-group row">
        <div class="col-sm-12">
            <asp:HyperLink ID="hypBackToMyJobs" runat="server" meta:resourcekey="hypBackToMyJobs"></asp:HyperLink>
            &nbsp;
            <asp:Label ID="lblCheckoutBelow" runat="server" meta:resourcekey="lblCheckoutBelow"></asp:Label>
        </div>
    </div>

    <br /><br />
    <asp:Panel ID="pnlCoupon" runat="server">
        <div class="form-group row">
            <div class="col-sm-4">
                <asp:TextBox ID="txtEnterCoupon" runat="server" MaxLength="100" 
                    CssClass="form-control input-sm" ValidationGroup="CouponValidation" meta:resourcekey="txtEnterCoupon"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                <asp:Button id="btnEnterCoupon" runat="server" ValidationGroup="CouponValidation"
                    OnClick="btnEnterCoupon_Click" CssClass="btn btn-secondary" meta:resourcekey="btnEnterCoupon"/>
                <asp:Button id="btnRemoveCoupon" runat="server" ValidationGroup="CouponValidation"
                    OnClick="btnRemoveCoupon_Click" CssClass="btn btn-secondary" meta:resourcekey="btnRemoveCoupon"/>
            </div>
            <div class="col-sm-6">
                <asp:RequiredFieldValidator ID="reqEnterCoupon" runat="server" ControlToValidate="txtEnterCoupon"
                    ValidationGroup="CouponValidation" meta:resourcekey="reqEnterCoupon" Display="Dynamic"
                        CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <br /><br />
    </asp:Panel>
    
    <div class="form-group row">
        <div class="col-sm-6">
            <h2><%= GetLocalResourceObject("strCheckout").ToString() %></h2>
        </div>
        <div class="col-sm-6">
            <img src="/images/secure.png" class="pull-right popover-dismiss secureimage" resourcekey="imgSecure" runat="server" id="imgSecure"/>
        </div>
    </div>

    <asp:Panel ID="pnlFreeCheckout" runat="server">
        <hr />
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:Label ID="lblNoActionRequired" runat="server" meta:resourcekey="lblNoActionRequired" 
                    CssClass="control-label"></asp:Label>
            </div>
        </div>
        <hr />
    </asp:Panel>

    <asp:Panel ID="pnlBillingAddress" runat="server">
        <hr />
        <div class="form-group row">
            <div class="col-sm-9">
                <asp:RadioButtonList ID="rdblBillingAddresses" runat="server" ValidationGroup="Checkout" CssClass="radio"></asp:RadioButtonList>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqBillingAddresses" runat="server" ControlToValidate="rdblBillingAddresses"
                    meta:resourcekey="reqBillingAddresses" ValidationGroup="Checkout" Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>

        <div id="divNewBillingAddress" runat="server" style="display:none;">
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingFirstName" runat="server" meta:resourcekey="lblBillingFirstName" 
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingFirstName" runat="server" ValidationGroup="NewBillingAddress" 
                        MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingFirstName" runat="server" 
                        ControlToValidate="txtBillingFirstName" meta:resourcekey="reqBillingFirstName" 
                        ValidationGroup="NewBillingAddress" Display="Dynamic" CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingLastName" runat="server" meta:resourcekey="lblBillingLastName" 
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingLastName" runat="server" ValidationGroup="NewBillingAddress" 
                        MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingLastName" runat="server" ControlToValidate="txtBillingLastName"
                        meta:resourcekey="reqBillingLastName" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingAddress1" runat="server" meta:resourcekey="lblBillingAddress1"
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingAddress1" runat="server" ValidationGroup="NewBillingAddress" 
                        MaxLength="256" CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingAddress1" runat="server" ControlToValidate="txtBillingAddress1"
                        meta:resourcekey="reqBillingAddress1" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingAddress2" runat="server" meta:resourcekey="lblBillingAddress2"
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingAddress2" runat="server" MaxLength="256" CssClass="form-control input-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingCity" runat="server" meta:resourcekey="lblBillingCity"
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingCity" runat="server" ValidationGroup="NewBillingAddress" MaxLength="100"
                        CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingCity" runat="server" ControlToValidate="txtBillingCity"
                        meta:resourcekey="reqBillingCity" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingState" runat="server" meta:resourcekey="lblBillingState"
                        CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:DropDownList ID="ddlBillingState" runat="server" ValidationGroup="NewBillingAddress"
                        CssClass="form-control input-sm" DataValueField="StateCode" DataTextField="StateName">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingState" runat="server" ControlToValidate="ddlBillingState"
                        meta:resourcekey="reqBillingState" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingZip" runat="server" meta:resourcekey="lblBillingZip" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtBillingZip" runat="server" ValidationGroup="NewBillingAddress" MaxLength="12"
                        CssClass="form-control input-sm"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingZip" runat="server" ControlToValidate="txtBillingZip"
                        meta:resourcekey="reqBillingZip" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regBillingZip" runat="server" ControlToValidate="txtBillingZip"
                        meta:resourcekey="regBillingZip" ValidationGroup="NewBillingAddress" Display="Dynamic" 
                        ValidationExpression="[0-9]{5}(-[0-9]{4})?" CssClass="text-danger">
                    </asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblBillingCountry" runat="server" meta:resourcekey="lblBillingCountry" CssClass="control-label"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:DropDownList ID="ddlBillingCountry" runat="server" ValidationGroup="NewBillingAddress"
                        CssClass="form-control input-sm" DataValueField="CountryCode" DataTextField="CountryName">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:RequiredFieldValidator ID="reqBillingCountry" runat="server" ControlToValidate="ddlBillingCountry"
                        meta:resourcekey="reqBillingCountry" ValidationGroup="NewBillingAddress" Display="Dynamic"
                        CssClass="text-danger">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:CheckBox ID="chkbSaveBillingAddress" runat="server" CssClass="checkbox"
                        meta:resourcekey="chkbSaveBillingAddress"></asp:CheckBox>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlPayment" runat="server">
        <hr />
        <div class="form-group row">
            <div class="col-sm-2">
                <asp:Label ID="lblCreditCardNumber" runat="server" meta:resourcekey="lblCreditCardNumber" 
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:TextBox ID="txtCreditCardNumber" runat="server" CssClass="form-control input-sm"
                    MaxLength="30"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <img src="/images/cc_visa.png" alt="Visa" title="Visa" class="logo_visa pull-left"/>
                <img src="/images/cc_master.png" alt="Mastercard" title="Mastercard" class="logo_mastercard pull-left"/>
                <img src="/images/cc_discover.png" alt="Discover" title="Discover" class="logo_discover pull-left"/>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCreditCardNumber" runat="server" ControlToValidate="txtCreditCardNumber"
                    meta:resourcekey="reqCreditCardNumber" ValidationGroup="Checkout" Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvalCreditCardNumber" runat="server" meta:resourcekey="cvalCreditCardNumber" 
                    ValidationGroup="CheckoutCreditCardNumber" Display="Dynamic"
                    OnServerValidate="cvalCreditCardNumber_ServerValidate" CssClass="text-danger">
                </asp:CustomValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-2">
                <asp:Label ID="lblCreditCardExpiration" runat="server" meta:resourcekey="lblCreditCardExpiration"
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:DropDownList ID="ddlCreditCardExpirationMonth" runat="server" CssClass="form-control input-sm">
                    <asp:ListItem meta:resourcekey="ddlCreditCardExpirationMonth_Item1"></asp:ListItem>
                    <asp:ListItem Text="01" Value="01"></asp:ListItem>
                    <asp:ListItem Text="02" Value="02"></asp:ListItem>
                    <asp:ListItem Text="03" Value="03"></asp:ListItem>
                    <asp:ListItem Text="04" Value="04"></asp:ListItem>
                    <asp:ListItem Text="05" Value="05"></asp:ListItem>
                    <asp:ListItem Text="06" Value="06"></asp:ListItem>
                    <asp:ListItem Text="07" Value="07"></asp:ListItem>
                    <asp:ListItem Text="08" Value="08"></asp:ListItem>
                    <asp:ListItem Text="09" Value="09"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-2">
                <asp:DropDownList ID="ddlCreditCardExpirationYear" runat="server" CssClass="form-control input-sm">
                </asp:DropDownList>
            </div>
            <div class="col-sm-6">
                <asp:RequiredFieldValidator ID="reqCreditCardExpirationMonth" runat="server" ControlToValidate="ddlCreditCardExpirationMonth"
                    meta:resourcekey="reqCreditCardExpirationMonth" ValidationGroup="Checkout" Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="reqCreditCardExpirationYear" runat="server" ControlToValidate="ddlCreditCardExpirationYear"
                    meta:resourcekey="reqCreditCardExpirationYear" ValidationGroup="Checkout" Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvalCreditCardExpirationMonth" runat="server" meta:resourcekey="cvalCreditCardExpirationMonth" 
                    ValidationGroup="CheckoutCreditCardNumber" Display="Dynamic"
                    OnServerValidate="cvalCreditCardExpirationMonth_ServerValidate" CssClass="text-danger">
                </asp:CustomValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-2">
                <asp:Label ID="lblCreditCardCVV" runat="server" meta:resourcekey="lblCreditCardCVV"
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtCreditCardCVV" runat="server" CssClass="form-control input-sm"
                    MaxLength="6"></asp:TextBox>
            </div>
            <div class="col-sm-2"></div>
            <div class="col-sm-6">
                <asp:RequiredFieldValidator ID="reqCreditCardCVV" runat="server" ControlToValidate="txtCreditCardCVV"
                    meta:resourcekey="reqCreditCardCVV" ValidationGroup="Checkout" Display="Dynamic"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
    </asp:Panel>

    <he:formvalidator id="heFormValidator" runat="server"/>

    <asp:Panel ID="pnlCheckout" runat="server">
        <div class="form-group row">
            <div class="col-sm-6">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:Label ID="lblTermsAccept" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-6">
                        <asp:Button id="btnPublish" runat="server" OnClick="btnPublish_Click" meta:resourcekey="btnPublish" 
                            CssClass="btn btn-primary" OnClientClick="javascript:if(PublishClick(this)==false){return false;}" 
                            ValidationGroup="Checkout" UseSubmitBehavior="false"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <asp:Literal ID="litAuthDotNetSeal" runat="server"></asp:Literal>
            </div>
            <div class="col-sm-3">
                
            </div>
        </div>
    </asp:Panel>
</asp:PlaceHolder>