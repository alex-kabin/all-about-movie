<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
<xsl:output method="text" indent="no" />

<xsl:template match="Movie">
  <xsl:choose>
    <xsl:when test="TranslatedTitle">
<xsl:value-of select="TranslatedTitle"/> / <xsl:value-of select="OriginalTitle"/>
    </xsl:when>
    <xsl:otherwise>
<xsl:value-of select="OriginalTitle"/>
    </xsl:otherwise>
  </xsl:choose>
&#13;
Год: <xsl:value-of select="Year"/>
&#13;
Жанр: <xsl:apply-templates select="Genres"/>
&#13;
Продолжительность: <xsl:apply-templates select="Runtime"/>
&#13;
Режиссер: <xsl:apply-templates select="Directors" />
&#13;
В ролях: <xsl:apply-templates select="Actors" />
&#13;
О фильме: <xsl:value-of select="Storyline" />
&#13;
Рейтинг: <xsl:apply-templates select="Ratings/Rating" />
<xsl:call-template name="valueOrEmpty">
  <xsl:with-param name="value" select="@MPAA"/>
</xsl:call-template>
&#13;
Ссылка: <xsl:value-of select="Url" />
</xsl:template>
  
<xsl:template name="valueOrEmpty">
  <xsl:param name="value"/>
  <xsl:if test="$value">
&#13;
MPAA: <xsl:value-of select="$value" />    
  </xsl:if>
</xsl:template>
  
<xsl:template match="Runtime">
  <xsl:call-template name="hoursMinutes">
    <xsl:with-param name="runtime" select="." />
  </xsl:call-template>
</xsl:template>
  
<xsl:template match="Rating">
* <xsl:value-of select="@RatedBy"/>: <xsl:value-of select="@Value"/> (<xsl:value-of select="@Votes"/>)</xsl:template>
  
<xsl:template match="Genres">
  <xsl:for-each select="string">
    <xsl:value-of select="."/>
    <xsl:if test="position() != last()">
      <xsl:text> / </xsl:text>
    </xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template match="Directors">
  <xsl:for-each select="Person/@Name">
    <xsl:value-of select="."/>
    <xsl:if test="position() != last()">
      <xsl:text>, </xsl:text>
    </xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template match="Actors">
  <xsl:for-each select="Person/@Name">
    <xsl:value-of select="."/>
    <xsl:if test="position() != last()">
      <xsl:text>, </xsl:text>
    </xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="hoursMinutes">
  <xsl:param name="runtime"/>
  <xsl:variable name="minutes" select=". mod 60" />
  <xsl:value-of select="floor(. div 60)"/>:<xsl:if test="string-length($minutes) &lt; 2">0</xsl:if><xsl:value-of select="$minutes"/>
</xsl:template>

</xsl:stylesheet>
