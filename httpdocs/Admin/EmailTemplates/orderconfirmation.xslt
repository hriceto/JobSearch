<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    <xsl:param name="JobsPage"/>
    
    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        Your order has been placed successfully. The order number is: <xsl:value-of select="root/Order/OrderId"/><br />
        Order date: <xsl:value-of select="msxsl:format-date(root/Order/CreatedDate, 'MM/dd/yyyy')"/><br />

        <table width="100%">
            <tr>
                <td colspan="2">
                    <hr></hr>
                </td>
            </tr>
            <tr>
                <td colspan="2">Order Items:</td>
            </tr>
        <xsl:for-each select="root/ArrayOfJobPost/JobPost">
            <tr>
                <td>
                    <xsl:value-of select="Title"/>
                </td>
                <td align="right">
                    $<xsl:value-of select="format-number(PriceTotal, '#,##0.00')"/>
                </td>
            </tr>
        </xsl:for-each>
            <tr>
                <td colspan="2">
                    <hr></hr>
                </td>
            </tr>
            <tr>
                <td>Subtotal:</td>
                <td align="right">
                    $<xsl:value-of select="format-number(root/Order/Subtotal,'#,##0.00')"/>
                </td>
            </tr>
            <tr>
                <td>Tax:</td>
                <td align="right">
                    $<xsl:value-of select="format-number(root/Order/Tax,'#,##0.00')"/>
                </td>
            </tr>
            <xsl:if test="number(root/Order/CouponDiscount) > 0">
                <tr>
                    <td>Discount:</td>
                    <td align="right">
                        ($<xsl:value-of select="format-number(root/Order/CouponDiscount,'#,##0.00')"/>)
                    </td>
                </tr>
            </xsl:if>
            <tr>
                <td>Total:</td>
                <td align="right">
                    $<xsl:value-of select="format-number(root/Order/Total,'#,##0.00')"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr></hr>
                </td>
            </tr>
        </table>
        <xsl:if test="number(root/Order/Total) > 0 and string-length(root/Order/GWAuthorizationCode) > 0">
            <br/>
            <xsl:if test="string-length(root/Coupon/CouponCode) > 0">
                Coupon Code: <xsl:value-of select="root/Coupon/CouponCode"/><br />
            </xsl:if>            
            Authorization Code: <xsl:value-of select="root/Order/GWAuthorizationCode"/><br />
            Transaction ID: <xsl:value-of select="root/Order/GWTransactionID"/><br />
            Card: <xsl:value-of select="root/Order/CardType"/> - <xsl:value-of select="root/Order/CardLastFour"/><br />
            Billing Address: <br /><xsl:value-of select="root/Order/BillingAddress1"/>&#160;<xsl:value-of select="root/Order/BillingAddress2"/>
            <br /><xsl:value-of select="root/Order/BillingCity"/>&#160;<xsl:value-of select="root/Order/BillingState"/>&#160;<xsl:value-of select="root/Order/BillingZip"/>
            <br /><xsl:value-of select="root/Order/BillingCountry"/><br />
        </xsl:if>
        <br /><br />
        To manage your jobs and add new ones please go to:
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$JobsPage"/>
            </xsl:attribute>
            <xsl:value-of select="$JobsPage"/>
        </xsl:element><br /><br />
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />
        Thank you for your business and we look forward to serving you.
        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
