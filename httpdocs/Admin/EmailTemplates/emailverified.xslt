<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="LoginLink"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    
    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        Your account has been verified and you can now start using our job portal 
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$LoginLink"/>
            </xsl:attribute>
            <xsl:value-of select="$LoginLink"/>
        </xsl:element>. Thank you for allowing us to server you.
        <br /><br />
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
