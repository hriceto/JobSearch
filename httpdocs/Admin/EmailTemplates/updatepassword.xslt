<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    
    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        Your password for your http://<xsl:value-of select="$Domain"/> account was successfully updated. 
        <br />If you did not initiate a password change then please contact us at
        <xsl:element name="a">
            <xsl:attribute name="href">
                mailto:<xsl:value-of select="$CustomerServiceEmail"/>
            </xsl:attribute>
            <xsl:value-of select="$CustomerServiceEmail"/>
        </xsl:element>
        as this might be an attempt to break into your account.
        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
