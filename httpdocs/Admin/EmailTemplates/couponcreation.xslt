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
    <xsl:param name="MyCouponsPage"/>
    <xsl:param name="CouponDiscount"/>
    <xsl:param name="AddJobPage"/>
    <xsl:param name="CheckoutPage"/>
    <xsl:param name="unsubscribeUrl"/>

    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        A new promotional code has been issued for your account on <xsl:value-of select="$Domain"></xsl:value-of>.
        <br /><br />
        <table>
            <tr>
                <td>Promotional Code:</td>
                <td>
                    <xsl:value-of select="root/Coupon/CouponCode"></xsl:value-of>
                </td>                
            </tr>
            <xsl:if test="number(root/Coupon/NumberOfUsesLimit) > 1">
                <tr>
                    <td>Number of uses:</td>
                    <td>
                        <xsl:value-of select="root/Coupon/NumberOfUsesLimit"></xsl:value-of>
                    </td>
                </tr>
            </xsl:if>
            <tr>
                <td>Discount:</td>
                <td>
                    <xsl:value-of select="$CouponDiscount"/>
                </td>
            </tr>
            <xsl:if test="string-length(root/Coupon/EndDate) > 0">
                <tr>
                    <td>Expires:</td>
                    <td>
                        <xsl:value-of select="msxsl:format-date(root/Coupon/EndDate, 'MM/dd/yyyy')"/>
                    </td>
                </tr>
            </xsl:if>
        </table>
        <br /><br />

        This code is valid for paid job post advertisements. To use this coupon:
        <ol>
            <li>Create a <xsl:element name="a">
                <xsl:attribute name="href">
                    <xsl:value-of select="$AddJobPage"/>
                </xsl:attribute>
                new job posting 
                </xsl:element> and add the job posting to your cart using one of the paid options.
            </li>
            <li>
                Go to the <xsl:element name="a">
                <xsl:attribute name="href">
                    <xsl:value-of select="$CheckoutPage"/>
                </xsl:attribute>
                checkout page 
                </xsl:element> and enter the coupon code in the Coupon Code field.
            </li>
            <li>
                The coupon discount will be immediately subtracted from your total. Proceed with the checkout.
            </li>
        </ol>
        <br /><br />

        You can view all of your coupons on the
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$MyCouponsPage"/>
            </xsl:attribute>
            my coupons
        </xsl:element> page.
        
        <br /><br />
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />
        Thank you for your business and we look forward to serving you.
        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a">
        <xsl:attribute name="href">
            mailto:<xsl:value-of select="$CustomerServiceEmail"/>
        </xsl:attribute>
        <xsl:value-of select="$CustomerServiceEmail"/>
        </xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
        <br /><br /><small>No longer wish to receive emails about website news, services, and promotions? <xsl:element name="a"><xsl:attribute name="href"><xsl:value-of select="$unsubscribeUrl"/></xsl:attribute>Unsubscribe.</xsl:element></small>
</xsl:template>
</xsl:stylesheet>
