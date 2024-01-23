using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using System.Web.Configuration;

namespace HristoEvtimov.Websites.Work.WorkLibrary.Validation
{
    public class HtmlParser
    {
        public bool AddTargetBlankToAnchors { get; set; }

        private List<HtmlAllowedTag> oAllowedHtmlTags;
        public enum ParseRules { AllowSomeHtml, FilterOutHtml };
        public bool InvalidHtmlWasPresent { get; set; }
        public Dictionary<string, string> AllowedHtmlCharacters = new System.Collections.Generic.Dictionary<string, string>() { 
            {"&amp;middot;", "&middot;"},
            {"&amp;sdot;", "&sdot;"},
            {"&amp;bull;", "&bull;"},
            {"&amp;minus;", "&minus;"},
            {"&amp;times;", "&times;"},
            {"&amp;divide;", "&divide;"},
            {"&amp;frasl;", "&frasl;"},
            {"&amp;plusmn;", "&plusmn;"},
            {"&amp;deg;", "&deg;"},
            {"&amp;cent;", "&cent;"},
            {"&amp;pound;", "&pound;"},
            {"&amp;euro;", "&euro;"},
            {"&amp;lsquo;", "&lsquo;"},
            {"&amp;rsquo;", "&rsquo;"},
            {"&amp;#x263a;", "&#x263a;"}, //smiley face
            {"&amp;#x2605;", "&#x2605;"}, //black star
            {"&amp;#x2606;", "&#x2606;"}, //white star
            {"&amp;copy;", "&copy;"},
            {"&amp;reg;", "&reg;"},
            {"&amp;trade;", "&trade;"},
            {"&amp;nbsp;", "&nbsp;"},
            {"&amp;emsp;", "&emsp;"},
            {"&amp;ensp;", "&ensp;"},
            {"&amp;thinsp;", "&thinsp;"},
            {"&amp;mdash;", "&mdash;"},
            {"&amp;ndash;", "&ndash;"},
            {"&amp;oline;", "&oline;"},
            {"&amp;quot;", "&quot;"},
            {"&amp;amp; ", "&amp; "},
            {"&amp;#38; ", "&#38; "}, //ampersand
            {"&amp;#162;", "&#162;" }, //cent
            {"&amp;#163;", "&#163;"}, //pound
            {"&amp;#169;", "&#169;"}, //copyright
            {"&amp;#174;", "&#174;"}, //registered
            {"&amp;#176;", "&#176;"}, //degree
            {"&amp;#8211;", "&#8211;"}, //en dash
            {"&amp;#8212;", "&#8212;"}, //em dash
            {"&amp;#8216;", "&#8216;"}, //left single quote
            {"&amp;#8217;", "&#8217;"}, //right single quote
            {"&amp;#8218;", "&#8218;"}, //low quote
            {"&amp;#8220;", "&#8220;"}, //left double quote
            {"&amp;#8221;", "&#8221;"}, //right double quote
            {"&amp;#8222;", "&#8222;"}, //low double quote
            {"&amp;#8226;", "&#8226;"}, //bullet
            {"&amp;#8230;", "&#8230;"}, //ellipsis
            {"&amp;#8364;", "&#8364;"}, //euro
            {"&amp;#8482;", "&#8482;"}, //trade mark
            {"&amp;hellip;", "&hellip;"},
            {"&amp;ldquot;", "&ldquot;"},
            {"&amp;rdquot;", "&rdquot;"},
        };

        public Dictionary<int, int> AllowedAsciiRanges = new System.Collections.Generic.Dictionary<int, int>() { 
            {32, 37},
            {39, 47},
            {48, 59},
            {61, 61},
            {63, 96},
            {97, 126},
        };

        public HtmlParser(ParseRules parseRule)
        {
            AddTargetBlankToAnchors = true;
            InvalidHtmlWasPresent = false;
            oAllowedHtmlTags = new List<HtmlAllowedTag>();

            //parse rules from web config.
            if (parseRule == ParseRules.AllowSomeHtml)
            {
                string sAllowedHtmlNodes = WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"];
                string[] arrAllowedHtmlNodes = sAllowedHtmlNodes.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sAllowedHtmlNode in arrAllowedHtmlNodes)
                {
                    HtmlAllowedTag newAllowTag = new HtmlAllowedTag();
                    if (sAllowedHtmlNode.Contains('[') && sAllowedHtmlNode.Contains(']'))
                    {
                        string sAllowedAttributes = sAllowedHtmlNode.Substring(sAllowedHtmlNode.IndexOf('['));
                        string sAllowedTag = sAllowedHtmlNode.Remove(sAllowedHtmlNode.IndexOf('['));
                        newAllowTag.TagName = sAllowedTag;
                        newAllowTag.TagAttributes = new List<string>();

                        sAllowedAttributes = sAllowedAttributes.Trim(new char[] { '[', ']' });
                        string[] arrAllowedAttributes = sAllowedAttributes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string sAllowedAttribute in arrAllowedAttributes)
                        {
                            if (!String.IsNullOrEmpty(sAllowedAttribute.Trim()))
                            {
                                newAllowTag.TagAttributes.Add(sAllowedAttribute);
                            }
                        }
                    }
                    else
                    {
                        newAllowTag.TagName = sAllowedHtmlNode;
                    }
                    oAllowedHtmlTags.Add(newAllowTag);
                }
            }
        }

        /// <summary>
        /// Filter html out according to rules
        /// </summary>
        /// <param name="htmlInput"></param>
        /// <returns></returns>
        public string FilterHtml(string htmlInput)
        {
            if (String.IsNullOrEmpty(htmlInput))
            {
                return "";
            }

            StringBuilder result = new StringBuilder();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            HtmlNodeCollection htmlNodes = doc.DocumentNode.ChildNodes;            

            foreach (HtmlNode node in htmlNodes)
            {
                result.Append(FilterHtmlNode(node));
            }

            return result.ToString();
        }

        public void RemoveAllowedTag(string tagName)
        {
            HtmlAllowedTag tagToRemove = null;
            foreach (HtmlAllowedTag oAllowedHtmlTag in oAllowedHtmlTags)
            {
                if (oAllowedHtmlTag.TagName.Equals(tagName, StringComparison.InvariantCultureIgnoreCase))
                {
                    tagToRemove = oAllowedHtmlTag;
                    break;
                }
            }

            if (tagToRemove != null)
            {
                oAllowedHtmlTags.Remove(tagToRemove);
            }
        }

        private StringBuilder FilterHtmlNode(HtmlNode htmlNode)
        {
            StringBuilder result = new System.Text.StringBuilder();

            switch (htmlNode.NodeType)
            {
                case HtmlNodeType.Element:
                    if (HtmlTagIsAllowed(htmlNode.Name))
                    {
                        result.Append(CreateCleanOpenHtmlTag(htmlNode));
                    }
                    else
                    {
                        InvalidHtmlWasPresent = true;
                    }
                    break;
                case HtmlNodeType.Text:
                    result.Append(HtmlEncodeText(htmlNode.InnerText));
                    break;
            }            

            if (htmlNode.NodeType == HtmlNodeType.Element)
            {
                foreach (HtmlNode node in htmlNode.ChildNodes)
                {
                    result.Append(FilterHtmlNode(node));
                }
            }


            switch (htmlNode.NodeType)
            {
                case HtmlNodeType.Element:
                    if (HtmlTagIsAllowed(htmlNode.Name))
                    {
                        result.Append(CreateCleanCloseHtmlTag(htmlNode));
                    } 
                    break;
            }
            
            return result;
        }

        private bool HtmlTagIsAllowed(string tagName)
        {
            bool result = false;
            foreach (HtmlAllowedTag oAllowedHtmlTag in oAllowedHtmlTags)
            {
                if (oAllowedHtmlTag.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private string CreateCleanOpenHtmlTag(HtmlNode htmlNode)
        {
            StringBuilder sb = new StringBuilder();
            foreach (HtmlAllowedTag oAllowedHtmlTag in oAllowedHtmlTags)
            {
                if (oAllowedHtmlTag.TagName.Equals(htmlNode.Name, StringComparison.OrdinalIgnoreCase))
                {
                    string attributes = CreateCleanOpenHtmlTagAttributes(htmlNode, oAllowedHtmlTag);
                    
                    sb.AppendFormat("<{0}{1}>", htmlNode.Name, attributes);
                    break;
                }
            }
            return sb.ToString();
        }

        private string CreateCleanOpenHtmlTagAttributes(HtmlNode htmlNode, HtmlAllowedTag oAllowedHtmlTag)
        {
            StringBuilder sb = new StringBuilder();

            if (oAllowedHtmlTag.TagAttributes != null)
            {
                foreach (string allowedAttribute in oAllowedHtmlTag.TagAttributes)
                {
                    int numberOfMatchedAttributes = 0;
                    foreach (HtmlAttribute htmlAttribute in htmlNode.Attributes)
                    {
                        if (htmlAttribute.Name.Equals(allowedAttribute, StringComparison.OrdinalIgnoreCase))
                        {
                            numberOfMatchedAttributes++;
                            sb.AppendFormat(" {0}", FilterHtmlAttribute(htmlAttribute));
                        }
                    }
                    if (numberOfMatchedAttributes != htmlNode.Attributes.Count)
                    {
                        InvalidHtmlWasPresent = true;
                    }
                }
            }

            //add target="_blank" to anchors
            if (AddTargetBlankToAnchors)
            {
                if (htmlNode.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append(" target=\"_blank\"");
                }
            }

            return sb.ToString();
        }

        private string CreateCleanCloseHtmlTag(HtmlNode htmlNode)
        {
            return String.Format("</{0}>", htmlNode.Name);
        }

        private string FilterHtmlAttribute(HtmlAttribute htmlAttribute)
        {
            StringBuilder sb = new StringBuilder();
            Regex attributeValueRegex = null;

            switch (htmlAttribute.Name.ToLower().Trim())
            {
                case "href":
                    if (htmlAttribute.Value.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        htmlAttribute.Value = htmlAttribute.Value.ToLower();
                        attributeValueRegex = new Regex("^mailto:\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        attributeValueRegex = new Regex("^http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?$", RegexOptions.IgnoreCase);
                    }
                    break;
                default:
                    attributeValueRegex = new Regex("^([a-zA-Z0-9])*$", RegexOptions.IgnoreCase);
                    break;
            }

            if (attributeValueRegex.IsMatch(htmlAttribute.Value))
            {
                sb.AppendFormat("{0}=\"{1}\"", htmlAttribute.Name, htmlAttribute.Value);
            }
            else
            {
                InvalidHtmlWasPresent = true;
            }

            return sb.ToString();
        }

        private string HtmlEncodeText(string text)
        {
            string result = HttpContext.Current.Server.HtmlEncode(text);
            foreach (KeyValuePair<string, string> AllowedHtmlCharacter in AllowedHtmlCharacters)
            {
                result = Regex.Replace(result, AllowedHtmlCharacter.Key, AllowedHtmlCharacter.Value, RegexOptions.IgnoreCase);
            }
            foreach (KeyValuePair<int, int> AllowedAsciiRange in AllowedAsciiRanges)
            {
                for (int charCode = AllowedAsciiRange.Key; charCode <= AllowedAsciiRange.Value; charCode++)
                {
                    result = result.Replace(String.Format("&amp;#{0};", charCode), String.Format("&#{0};", charCode)); //decimal
                    result = Regex.Replace(result, String.Format("&amp;#x{0};", charCode.ToString("X")), String.Format("&#x{0};", charCode.ToString("X")), RegexOptions.IgnoreCase); //hex
                }
            }
            return result;
        }

        private class HtmlAllowedTag
        {
            public string TagName { get; set; }
            public List<String> TagAttributes { get; set; }
        }
    }
}
