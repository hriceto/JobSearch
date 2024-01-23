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
    
    <xsl:template match="/">
        Dear Administrator,
        <br /><br />
        A new job posting order was just created by <xsl:value-of select="root/User/Email"/><br />

        <table width="100%">
            <tr>
                <td colspan="2">
                    <hr></hr>
                </td>
            </tr>
            <tr>
                <td colspan="2">Jobs:</td>
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
        </xsl:if>
        
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
