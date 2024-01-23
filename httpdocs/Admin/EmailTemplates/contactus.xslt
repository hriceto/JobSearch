<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="Name"/>
    <xsl:param name="Email"/>
    <xsl:param name="Message"/>
    <xsl:param name="UserId"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    
    <xsl:template match="/">
        Dear Administrator,
        <br /><br />
        A new message has been submitted from the contact us form on <xsl:value-of select="$Domain"/>
        <br /><br />
        <b>Name:</b> <xsl:value-of select="$Name"/><br />
        <b>Email:</b> <xsl:value-of select="$Email"/><br />
        <b>Message:</b> <xsl:value-of select="$Message"/><br />
        <b>UserId:</b> <xsl:value-of select="$UserId"/><br />

        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
