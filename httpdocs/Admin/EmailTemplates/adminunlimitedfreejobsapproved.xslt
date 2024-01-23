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
        Your account has been reviewed by a site administrator on <xsl:value-of select="msxsl:format-date(root/Company/ReviewedDate, 'MM/dd/yyyy')"/>.
        <br />
        <br />

        <hr></hr>
        <xsl:if test="root/Company/AllowUnlimitedFreeJobPosts='true'">
            <b>Unlimited Job Posts</b>
            <br /><xsl:value-of select="root/Company/Name"/> can now post an unlimited number of job advertisements at no cost.
        </xsl:if>
        <hr></hr>

        <br /><br />
        To start posting jobs go to:
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$JobsPage"/>
            </xsl:attribute>
            <xsl:value-of select="$JobsPage"/>
        </xsl:element>
        <br /><br />

        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />

        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.

</xsl:template>
</xsl:stylesheet>
