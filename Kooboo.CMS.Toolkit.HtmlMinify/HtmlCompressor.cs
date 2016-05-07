//http://code.google.com/p/htmlcompressor
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * Class that compresses given HTML source by removing comments, extra spaces and 
 * line breaks while preserving content within &lt;pre>, &lt;textarea>, &lt;script> 
 * and &lt;style> tags. 
 * <p>Blocks that should be additionally preserved could be marked with:
 * <br><code>&lt;!-- {{{ -->
 * <br>&nbsp;&nbsp;&nbsp;&nbsp;...
 * <br>&lt;!-- }}} --></code> 
 * <br>or any number of user defined patterns. 
 * <p>Content inside &lt;script> or &lt;style> tags could be optionally compressed using 
 * <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a> or <a href="http://code.google.com/closure/compiler/">Google Closure Compiler</a>
 * libraries.
 * 
 * @author <a href="mailto:serg472@gmail.com">Sergiy Kovalchuk</a>
 */
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
public class HtmlCompressor
{

    /**
     * Predefined pattern that matches <code>&lt;?php ... ?></code> tags. 
     * Could be passed inside a list to {@link #setPreservePatterns(List) setPreservePatterns} method.
     */
    public static Regex PHP_TAG_PATTERN = new Regex("<\\?php.*?\\?>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

    /**
     * Predefined pattern that matches <code>&lt;% ... %></code> tags. 
     * Could be passed inside a list to {@link #setPreservePatterns(List) setPreservePatterns} method.
     */
    public static Regex SERVER_SCRIPT_TAG_PATTERN = new Regex("<%.*?%>", RegexOptions.Compiled | RegexOptions.Multiline);

    /**
     * Predefined pattern that matches <code>&lt;--# ... --></code> tags. 
     * Could be passed inside a list to {@link #setPreservePatterns(List) setPreservePatterns} method.
     */
    public static Regex SERVER_SIDE_INCLUDE_PATTERN = new Regex("<!--\\s*#.*?-->", RegexOptions.Compiled | RegexOptions.Multiline);

    /**
     * Predefined list of tags that are very likely to be block-level. 
     * Could be passed to {@link #setRemoveSurroundingSpaces(string) setRemoveSurroundingSpaces} method.
     */
    public static string BLOCK_TAGS_MIN = "html,head,body,br,p";

    /**
     * Predefined list of tags that are block-level by default, excluding <code>&lt;div></code> and <code>&lt;li></code> tags. 
     * Table tags are also included.
     * Could be passed to {@link #setRemoveSurroundingSpaces(string) setRemoveSurroundingSpaces} method.
     */
    public static string BLOCK_TAGS_MAX = BLOCK_TAGS_MIN + ",h1,h2,h3,h4,h5,h6,blockquote,center,dl,fieldset,form,frame,frameset,hr,noframes,ol,table,tbody,tr,td,th,tfoot,thead,ul";

    /**
     * Could be passed to {@link #setRemoveSurroundingSpaces(string) setRemoveSurroundingSpaces} method 
     * to remove all surrounding spaces (not recommended).
     */
    public static string ALL_TAGS = "all";

    private bool enabled = true;

    //javascript and css compressor implementations
    //private Compressor javaScriptCompressor = null;
    //private Compressor cssCompressor = null;

    //default settings
    private bool _removeComments = true;
    private bool _removeMultiSpaces = true;

    //optional settings
    private bool _removeIntertagSpaces = false;
    private bool removeQuotes = false;
    private bool _compressJavaScript = false;
    private bool compressCss = false;
    private bool _simpleDoctype = false;
    private bool _removeScriptAttributes = false;
    private bool _removeStyleAttributes = false;
    private bool _removeLinkAttributes = false;
    private bool _removeFormAttributes = false;
    private bool _removeInputAttributes = false;
    private bool _simpleBooleanAttributes = false;
    private bool _removeJavaScriptProtocol = false;
    private bool _removeHttpProtocol = false;
    private bool _removeHttpsProtocol = false;
    private bool preserveLineBreaks = false;
    private string _removeSurroundingSpaces = null;

    private List<Regex> preservePatterns = null;

    //statistics
    private bool generateStatistics = false;
    //private HtmlCompressorStatistics statistics = null;

    //YUICompressor settings
    private bool yuiJsNoMunge = false;
    private bool yuiJsPreserveAllSemiColons = false;
    private bool yuiJsDisableOptimizations = false;
    private int yuiJsLineBreak = -1;
    private int yuiCssLineBreak = -1;

    //error reporter implementation for YUI compressor
    //private ErrorReporter yuiErrorReporter = null;

    //temp replacements for preserved blocks 
    protected static string tempCondCommentBlock = "%%%~COMPRESS~COND~{0,number,#}~%%%";
    protected static string tempPreBlock = "%%%~COMPRESS~PRE~{0,number,#}~%%%";
    protected static string tempTextAreaBlock = "%%%~COMPRESS~TEXTAREA~{0,number,#}~%%%";
    protected static string tempScriptBlock = "%%%~COMPRESS~SCRIPT~{0,number,#}~%%%";
    protected static string tempStyleBlock = "%%%~COMPRESS~STYLE~{0,number,#}~%%%";
    protected static string tempEventBlock = "%%%~COMPRESS~EVENT~{0,number,#}~%%%";
    protected static string tempLineBreakBlock = "%%%~COMPRESS~LT~{0,number,#}~%%%";
    protected static string tempSkipBlock = "%%%~COMPRESS~SKIP~{0,number,#}~%%%";
    protected static string tempUserBlock = "%%%~COMPRESS~USER{0,number,#}~{1,number,#}~%%%";

    //compiled regex patterns
    protected static Regex emptyPattern = new Regex("\\s");
    protected static Regex skipPattern = new Regex("<!--\\s*\\{\\{\\{\\s*-->(.*?)<!--\\s*\\}\\}\\}\\s*-->", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex condCommentPattern = new Regex("(<!(?:--)?\\[[^\\]]+?]>)(.*?)(<!\\[[^\\]]+]-->)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex commentPattern = new Regex("<!---->|<!--[^\\[].*?-->", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex intertagPattern_TagTag = new Regex(">\\s+<", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex intertagPattern_TagCustom = new Regex(">\\s+%%%~", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex intertagPattern_CustomTag = new Regex("~%%%\\s+<", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex intertagPattern_CustomCustom = new Regex("~%%%\\s+%%%~", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex multispacePattern = new Regex("\\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex tagEndSpacePattern = new Regex("(<(?:[^>]+?))(?:\\s+?)(/?>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex tagLastUnquotedValuePattern = new Regex("=\\s*[a-z0-9-_]+$", RegexOptions.IgnoreCase);
    protected static Regex tagQuotePattern = new Regex("\\s*=\\s*([\"'])([a-z0-9-_]+?)\\1(/?)(?=[^<]*?>)", RegexOptions.IgnoreCase);
    protected static Regex prePattern = new Regex("(<pre[^>]*?>)(.*?)(</pre>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex taPattern = new Regex("(<textarea[^>]*?>)(.*?)(</textarea>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex scriptPattern = new Regex("(<script[^>]*?>)(.*?)(</script>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex stylePattern = new Regex("(<style[^>]*?>)(.*?)(</style>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex tagPropertyPattern = new Regex("(\\s\\w+)\\s*=\\s*(?=[^<]*?>)", RegexOptions.IgnoreCase);
    protected static Regex cdataPattern = new Regex("\\s*<!\\[CDATA\\[(.*?)\\]\\]>\\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex doctypePattern = new Regex("<!DOCTYPE[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex typeAttrPattern = new Regex("type\\s*=\\s*([\\\"']*)(.+?)\\1", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex jsTypeAttrPattern = new Regex("(<script[^>]*)type\\s*=\\s*([\"']*)(?:text|application)/javascript\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex jsLangAttrPattern = new Regex("(<script[^>]*)language\\s*=\\s*([\"']*)javascript\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex styleTypeAttrPattern = new Regex("(<style[^>]*)type\\s*=\\s*([\"']*)text/style\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex linkTypeAttrPattern = new Regex("(<link[^>]*)type\\s*=\\s*([\"']*)text/(?:css|plain)\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex linkRelAttrPattern = new Regex("<link(?:[^>]*)rel\\s*=\\s*([\"']*)(?:alternate\\s+)?stylesheet\\1(?:[^>]*)>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex formMethodAttrPattern = new Regex("(<form[^>]*)method\\s*=\\s*([\"']*)get\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex inputTypeAttrPattern = new Regex("(<input[^>]*)type\\s*=\\s*([\"']*)text\\2([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex booleanAttrPattern = new Regex("(<\\w+[^>]*)(checked|selected|disabled|readonly)\\s*=\\s*([\"']*)\\w*\\3([^>]*>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex eventJsProtocolPattern = new Regex("^javascript:\\s*(.+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex httpProtocolPattern = new Regex("(<[^>]+?(?:href|src|cite|action)\\s*=\\s*['\"])http:(//[^>]+?>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex httpsProtocolPattern = new Regex("(<[^>]+?(?:href|src|cite|action)\\s*=\\s*['\"])https:(//[^>]+?>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex relExternalPattern = new Regex("<(?:[^>]*)rel\\s*=\\s*([\"']*)(?:alternate\\s+)?external\\1(?:[^>]*)>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex eventPattern1 = new Regex("(\\son[a-z]+\\s*=\\s*\")([^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*)(\")", RegexOptions.IgnoreCase); //unmasked: \son[a-z]+\s*=\s*"[^"\\\r\n]*(?:\\.[^"\\\r\n]*)*"
    protected static Regex eventPattern2 = new Regex("(\\son[a-z]+\\s*=\\s*')([^'\\\\\\r\\n]*(?:\\\\.[^'\\\\\\r\\n]*)*)(')", RegexOptions.IgnoreCase);
    protected static Regex lineBreakPattern = new Regex("(?:[ \t]*(\\r?\\n)[ \t]*)+");
    protected static Regex surroundingSpacesMinPattern = new Regex("\\s*(</?(?:" + BLOCK_TAGS_MIN.Replace(",", "|") + ")(?:>|[\\s/][^>]*>))\\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex surroundingSpacesMaxPattern = new Regex("\\s*(</?(?:" + BLOCK_TAGS_MAX.Replace(",", "|") + ")(?:>|[\\s/][^>]*>))\\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);
    protected static Regex surroundingSpacesAllPattern = new Regex("\\s*(<[^>]+>)\\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);

    //patterns for searching for temporary replacements
    protected static Regex tempCondCommentPattern = new Regex("%%%~COMPRESS~COND~(\\d+?)~%%%");
    protected static Regex tempPrePattern = new Regex("%%%~COMPRESS~PRE~(\\d+?)~%%%");
    protected static Regex tempTextAreaPattern = new Regex("%%%~COMPRESS~TEXTAREA~(\\d+?)~%%%");
    protected static Regex tempScriptPattern = new Regex("%%%~COMPRESS~SCRIPT~(\\d+?)~%%%");
    protected static Regex tempStylePattern = new Regex("%%%~COMPRESS~STYLE~(\\d+?)~%%%");
    protected static Regex tempEventPattern = new Regex("%%%~COMPRESS~EVENT~(\\d+?)~%%%");
    protected static Regex tempSkipPattern = new Regex("%%%~COMPRESS~SKIP~(\\d+?)~%%%");
    protected static Regex tempLineBreakPattern = new Regex("%%%~COMPRESS~LT~(\\d+?)~%%%");

    /**
     * The main method that compresses given HTML source and returns compressed
     * result.
     * 
     * @param html HTML content to compress
     * @return compressed content.
     */
    public string compress(string html)
    {
        if (!enabled || html == null || html.Length == 0)
        {
            return html;
        }

        //calculate uncompressed statistics
        initStatistics(html);

        //preserved block containers
        List<string> condCommentBlocks = new List<string>();
        List<string> preBlocks = new List<string>();
        List<string> taBlocks = new List<string>();
        List<string> scriptBlocks = new List<string>();
        List<string> styleBlocks = new List<string>();
        List<string> eventBlocks = new List<string>();
        List<string> skipBlocks = new List<string>();
        List<string> lineBreakBlocks = new List<string>();
        List<List<string>> userBlocks = new List<List<string>>();

        //preserve blocks
        html = preserveBlocks(html, preBlocks, taBlocks, scriptBlocks, styleBlocks, eventBlocks, condCommentBlocks, skipBlocks, lineBreakBlocks, userBlocks);

        //process pure html
        html = processHtml(html);

        //process preserved blocks
        processPreservedBlocks(preBlocks, taBlocks, scriptBlocks, styleBlocks, eventBlocks, condCommentBlocks, skipBlocks, lineBreakBlocks, userBlocks);

        //put preserved blocks back
        html = returnBlocks(html, preBlocks, taBlocks, scriptBlocks, styleBlocks, eventBlocks, condCommentBlocks, skipBlocks, lineBreakBlocks, userBlocks);

        //calculate compressed statistics
        endStatistics(html);

        return html;
    }

    protected void initStatistics(string html)
    {
        ////create stats
        //if (generateStatistics)
        //{
        //    statistics = new HtmlCompressorStatistics();
        //    statistics.setTime((new Date()).getTime());
        //    statistics.getOriginalMetrics().setFilesize(html.length());

        //    //calculate number of empty chars
        //    Matcher matcher = emptyPattern.matcher(html);
        //    while (matcher.find())
        //    {
        //        statistics.getOriginalMetrics().setEmptyChars(statistics.getOriginalMetrics().getEmptyChars() + 1);
        //    }
        //}
        //else
        //{
        //    statistics = null;
        //}
    }

    protected void endStatistics(string html)
    {
        ////calculate compression time
        //if (generateStatistics)
        //{
        //    statistics.setTime((new Date()).getTime() - statistics.getTime());
        //    statistics.getCompressedMetrics().setFilesize(html.length());

        //    //calculate number of empty chars
        //    Matcher matcher = emptyPattern.matcher(html);
        //    while (matcher.find())
        //    {
        //        statistics.getCompressedMetrics().setEmptyChars(statistics.getCompressedMetrics().getEmptyChars() + 1);
        //    }
        //}
    }

    protected string preserveBlocks(string html, List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks, List<string> eventBlocks, List<string> condCommentBlocks, List<string> skipBlocks, List<string> lineBreakBlocks, List<List<string>> userBlocks)
    {
        var index = 0;
        //preserve user blocks
        if (preservePatterns != null)
        {
            for (int p = 0; p < preservePatterns.Count; p++)
            {
                List<string> userBlock = new List<string>();

                index = 0;

                html = preservePatterns[p].Replace(html,
                         m =>
                         {
                             userBlock.Add(m.Groups[0].Value);
                             return string.Format(tempUserBlock, p, index++);
                         });


                userBlocks.Add(userBlock);
            }
        }

        //preserve <!-- {{{ ---><!-- }}} ---> skip blocks
        int skipBlockIndex = 0;
        html = skipPattern.Replace(html,
                      m =>
                      {
                          skipBlocks.Add(m.Groups[1].Value);
                          return string.Format(tempSkipBlock, skipBlockIndex++);
                      });

        //preserve conditional comments
        HtmlCompressor condCommentCompressor = createCompressorClone();
        index = 0;
        html = skipPattern.Replace(html,
                      m =>
                      {
                          if (m.Groups[2].Value.Trim().Length > 0)
                          {
                              condCommentBlocks.Add(m.Groups[1].Value + condCommentCompressor.compress(m.Groups[2].Value) + m.Groups[3].Value);
                              return string.Format(tempCondCommentBlock, index++);
                          }
                          return "";
                      });

        //preserve inline events
        index = 0;
        html = eventPattern1.Replace(html,
                      m =>
                      {
                          if (m.Groups[2].Value.Trim().Length > 0)
                          {
                              eventBlocks.Add(m.Groups[2].Value);
                              return "$1" + string.Format(tempEventBlock, index++) + "$3";
                          }
                          return "";
                      });

        html = eventPattern2.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               eventBlocks.Add(m.Groups[2].Value);
                               return "$1" + string.Format(tempEventBlock, index++) + "$3";
                           }
                           return "";
                       });

        //preserve PRE tags
        index = 0;
        html = prePattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               preBlocks.Add(m.Groups[2].Value);
                               return "$1" + string.Format(tempPreBlock, index++) + "$3";
                           }
                           return "";
                       });


        //preserve SCRIPT tags
        index = 0;
        html = scriptPattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               //check type
                               string type = "";
                               var typeMatcher = typeAttrPattern.Match(m.Groups[1].Value);
                               if (typeMatcher.Success)
                               {
                                   type = typeMatcher.Groups[2].Value.ToLower();
                               }

                               if (type.Length == 0 || type == "text/javascript" || type == "application/javascript")
                               {
                                   //javascript block, preserve and compress with js compressor
                                   scriptBlocks.Add(m.Groups[2].Value);
                                   return "$1" + string.Format(tempScriptBlock, index++) + "$3"; ;
                               }
                               else if (type == "text/x-jquery-tmpl")
                               {
                                   //jquery template, ignore so it gets compressed with the rest of html
                               }
                               else
                               {
                                   //some custom script, preserve it inside "skip blocks" so it won't be compressed with js compressor 
                                   skipBlocks.Add(m.Groups[2].Value);
                                   return "$1" + string.Format(tempSkipBlock, skipBlockIndex++) + "$3";
                               }
                           }
                           return "";
                       });


        //preserve STYLE tags

        index = 0;
        html = stylePattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               styleBlocks.Add(m.Groups[2].Value);
                               return "$1" + string.Format(tempStyleBlock, index++) + "$3";
                           }
                           return "";
                       });



        //preserve TEXTAREA tags
        index = 0;
        html = taPattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               taBlocks.Add(m.Groups[2].Value);
                               return "$1" + string.Format(tempTextAreaBlock, index++) + "$3";
                           }

                           return "";
                       });



        //preserve line breaks
        if (preserveLineBreaks)
        {
            index = 0;
            html = lineBreakPattern.Replace(html,
                           m =>
                           {
                               lineBreakBlocks.Add(m.Groups[1].Value);
                               return string.Format(tempLineBreakBlock, index++);
                           });
        }

        return html;
    }

    protected string returnBlocks(string html, List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks, List<string> eventBlocks, List<string> condCommentBlocks, List<string> skipBlocks, List<string> lineBreakBlocks, List<List<string>> userBlocks)
    {
        //put line breaks back
        if (preserveLineBreaks)
        {

            html = tempLineBreakPattern.Replace(html,
                          m =>
                          {
                              int i = int.Parse(m.Groups[1].Value);
                              if (lineBreakBlocks.Count > i)
                              {
                                  return lineBreakBlocks[i];
                              }

                              return "";
                          });

        }

        //put TEXTAREA blocks back
        html = tempTextAreaPattern.Replace(html,
                         m =>
                         {
                             int i = int.Parse(m.Groups[1].Value);
                             if (taBlocks.Count > i)
                             {
                                 return taBlocks[i];
                             }
                             return "";
                         });


        //put STYLE blocks back
        html = tempStylePattern.Replace(html,
                         m =>
                         {
                             int i = int.Parse(m.Groups[1].Value);
                             if (styleBlocks.Count > i)
                             {
                                 return styleBlocks[i];
                             }
                             return "";
                         });

        //put SCRIPT blocks back
        html = tempScriptPattern.Replace(html,
                       m =>
                       {
                           int i = int.Parse(m.Groups[1].Value);
                           if (scriptBlocks.Count > i)
                           {
                               return scriptBlocks[i];
                           }
                           return "";
                       });

        //put PRE blocks back
        html = tempPrePattern.Replace(html,
                        m =>
                        {
                            int i = int.Parse(m.Groups[1].Value);
                            if (preBlocks.Count > i)
                            {
                                return preBlocks[i];
                            }
                            return "";
                        });


        //put event blocks back
        html = tempEventPattern.Replace(html,
                         m =>
                         {
                             int i = int.Parse(m.Groups[1].Value);
                             if (eventBlocks.Count > i)
                             {
                                 return eventBlocks[i];
                             }

                             return "";
                         });


        //put conditional comments back
        html = tempCondCommentPattern.Replace(html,
                           m =>
                           {
                               int i = int.Parse(m.Groups[1].Value);
                               if (condCommentBlocks.Count > i)
                               {
                                   return condCommentBlocks[i];
                               }

                               return "";
                           });


        //put skip blocks back
        html = tempSkipPattern.Replace(html,
                             m =>
                             {
                                 int i = int.Parse(m.Groups[1].Value);
                                 if (skipBlocks.Count > i)
                                 {
                                     return skipBlocks[i];
                                 }

                                 return "";
                             });


        //put user blocks back
        if (preservePatterns != null)
        {
            for (int p = preservePatterns.Count - 1; p >= 0; p--)
            {
                Regex tempUserPattern = new Regex("%%%~COMPRESS~USER" + p + "~(\\d+?)~%%%");
                html = tempUserPattern.Replace(html,
                               m =>
                               {
                                   int i = int.Parse(m.Groups[1].Value);
                                   if (userBlocks.Count > p && userBlocks[p].Count > i)
                                   {
                                       return userBlocks[p][i];
                                   }

                                   return "";
                               });
            }
        }

        return html;
    }

    protected string processHtml(string html)
    {

        //remove comments
        html = removeComments(html);

        //simplify doctype
        html = simpleDoctype(html);

        //remove script attributes
        html = removeScriptAttributes(html);

        //remove style attributes
        html = removeStyleAttributes(html);

        //remove link attributes
        html = removeLinkAttributes(html);

        //remove form attributes
        html = removeFormAttributes(html);

        //remove input attributes
        html = removeInputAttributes(html);

        //simplify bool attributes
        html = simpleBooleanAttributes(html);

        //remove http from attributes
        html = removeHttpProtocol(html);

        //remove https from attributes
        html = removeHttpsProtocol(html);

        //remove inter-tag spaces
        html = removeIntertagSpaces(html);

        //remove multi whitespace characters
        html = removeMultiSpaces(html);

        //remove spaces around equals sign and ending spaces
        html = removeSpacesInsideTags(html);

        //remove quotes from tag attributes
        html = removeQuotesInsideTags(html);

        //remove surrounding spaces
        html = removeSurroundingSpaces(html);

        return html.Trim();
    }

    protected string removeSurroundingSpaces(string html)
    {
        //remove spaces around provided tags
        if (_removeSurroundingSpaces != null)
        {
            Regex pattern;
            if (_removeSurroundingSpaces.Equals(BLOCK_TAGS_MIN, System.StringComparison.OrdinalIgnoreCase))
            {
                pattern = surroundingSpacesMinPattern;
            }
            else if (_removeSurroundingSpaces.Equals(BLOCK_TAGS_MAX, System.StringComparison.OrdinalIgnoreCase))
            {
                pattern = surroundingSpacesMaxPattern;
            } if (_removeSurroundingSpaces.Equals(ALL_TAGS, System.StringComparison.OrdinalIgnoreCase))
            {
                pattern = surroundingSpacesAllPattern;
            }
            else
            {
                pattern = new Regex("\\s*(</?(?:" + _removeSurroundingSpaces.Replace(",", "|") + ")(?:>|[\\s/][^>]*>))\\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }

            html = pattern.Replace(html, m => { return "$1"; });

        }
        return html;
    }

    protected string removeQuotesInsideTags(string html)
    {
        //remove quotes from tag attributes
        if (removeQuotes)
        {
            html = tagQuotePattern.Replace(html, m =>
            {
                if (m.Groups[3].Value.Trim().Length == 0)
                {
                    return "=$2";
                }
                else
                {
                    return "=$2 $3";
                }
            });


        }
        return html;
    }

    protected string removeSpacesInsideTags(string html)
    {
        //remove spaces around equals sign inside tags
        html = tagPropertyPattern.Replace(html, "$1=");

        //remove ending spaces inside tags
        //html = tagEndSpacePattern.matcher(html).replaceAll("$1$2");

        html = tagEndSpacePattern.Replace(html, m =>
        {
            //keep space if attribute value is unquoted before trailing slash
            if (m.Groups[2].Value.StartsWith("/") && tagLastUnquotedValuePattern.Match(m.Groups[1].Value).Success)
            {
                return "$1 $2";
            }
            else
            {
                return "$1$2";
            }
        });


        return html;
    }

    protected string removeMultiSpaces(string html)
    {
        //collapse multiple spaces
        if (_removeMultiSpaces)
        {
            html = multispacePattern.Replace(html, " ");
        }
        return html;
    }

    protected string removeIntertagSpaces(string html)
    {
        //remove inter-tag spaces
        if (_removeIntertagSpaces)
        {
            html = intertagPattern_TagTag.Replace(html, "><");
            html = intertagPattern_TagCustom.Replace(html, ">%%%~");
            html = intertagPattern_CustomTag.Replace(html, "~%%%<");
            html = intertagPattern_CustomCustom.Replace(html, "~%%%%%%~");
        }
        return html;
    }

    protected string removeComments(string html)
    {
        //remove comments
        if (_removeComments)
        {
            html = commentPattern.Replace(html, "");
        }
        return html;
    }

    protected string simpleDoctype(string html)
    {
        //simplify doctype
        if (_simpleDoctype)
        {
            html = doctypePattern.Replace(html, "<!DOCTYPE html>");
        }
        return html;
    }

    protected string removeScriptAttributes(string html)
    {

        if (_removeScriptAttributes)
        {
            //remove type from script tags
            html = jsTypeAttrPattern.Replace(html, "$1$3");

            //remove language from script tags
            html = jsLangAttrPattern.Replace(html, "$1$3");
        }
        return html;
    }

    protected string removeStyleAttributes(string html)
    {
        //remove type from style tags
        if (_removeStyleAttributes)
        {
            html = styleTypeAttrPattern.Replace(html, "$1$3");
        }
        return html;
    }

    protected string removeLinkAttributes(string html)
    {
        //remove type from link tags with rel=stylesheet
        if (_removeLinkAttributes)
        {
            html = linkTypeAttrPattern.Replace(html, m =>
            {
                //if rel=stylesheet
                if (linkRelAttrPattern.Match(m.Groups[0].Value).Success)
                {
                    return "$1$3";
                }
                else
                {
                    return "$0";
                }
            });
        }
        return html;
    }

    protected string removeFormAttributes(string html)
    {
        //remove method from form tags
        if (_removeFormAttributes)
        {
            html = formMethodAttrPattern.Replace(html, "$1$3");
        }
        return html;
    }

    protected string removeInputAttributes(string html)
    {
        //remove type from input tags
        if (_removeInputAttributes)
        {
            html = inputTypeAttrPattern.Replace(html, "$1$3");
        }
        return html;
    }

    protected string simpleBooleanAttributes(string html)
    {
        //simplify bool attributes
        if (_simpleBooleanAttributes)
        {
            html = booleanAttrPattern.Replace(html, "$1$2$4");
        }
        return html;
    }

    protected string removeHttpProtocol(string html)
    {
        //remove http protocol from tag attributes
        if (_removeHttpProtocol)
        {
            html = httpProtocolPattern.Replace(html, m =>
            {
                //if rel!=external
                if (!relExternalPattern.Match(m.Groups[0].Value).Success)
                {
                    return "$1$2";
                }
                else
                {
                    return "$0";
                }
            });
        }
        return html;
    }

    protected string removeHttpsProtocol(string html)
    {
        //remove https protocol from tag attributes
        if (_removeHttpsProtocol)
        {
            html = httpsProtocolPattern.Replace(html, m =>
            {
                //if rel!=external
                if (!relExternalPattern.Match(m.Groups[0].Value).Success)
                {
                    return "$1$2";
                }
                else
                {
                    return "$0";
                }
            });
        }
        return html;
    }

    protected void processPreservedBlocks(List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks, List<string> eventBlocks, List<string> condCommentBlocks, List<string> skipBlocks, List<string> lineBreakBlocks, List<List<string>> userBlocks)
    {
        processPreBlocks(preBlocks);
        processTextAreaBlocks(taBlocks);
        processScriptBlocks(scriptBlocks);
        processStyleBlocks(styleBlocks);
        processEventBlocks(eventBlocks);
        processCondCommentBlocks(condCommentBlocks);
        processSkipBlocks(skipBlocks);
        processUserBlocks(userBlocks);
        processLineBreakBlocks(lineBreakBlocks);
    }

    protected void processPreBlocks(List<string> preBlocks)
    {
        //if (generateStatistics)
        //{
        //    foreach (string block in preBlocks)
        //    {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}
    }

    protected void processTextAreaBlocks(List<string> taBlocks)
    {
        //if(generateStatistics) {
        //    for(string block : taBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}
    }

    protected void processCondCommentBlocks(List<string> condCommentBlocks)
    {
        //if(generateStatistics) {
        //    for(string block : condCommentBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}
    }

    protected void processSkipBlocks(List<string> skipBlocks)
    {
        //if(generateStatistics) {
        //    for(string block : skipBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}
    }

    protected void processLineBreakBlocks(List<string> lineBreakBlocks)
    {
        //if(generateStatistics) {
        //    for(string block : lineBreakBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}
    }

    protected void processUserBlocks(List<List<string>> userBlocks)
    {
        //if(generateStatistics) {
        //    for(List<string> blockList : userBlocks) {
        //        for(string block : blockList) {
        //            statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //        }
        //    }
        //}
    }

    protected void processEventBlocks(List<string> eventBlocks)
    {

        //if(generateStatistics) {
        //    for(string block : eventBlocks) {
        //        statistics.getOriginalMetrics().setInlineEventSize(statistics.getOriginalMetrics().getInlineEventSize() + block.length());
        //    }
        //}

        //if(removeJavaScriptProtocol) {
        //    for(int i = 0; i < eventBlocks.size(); i++) {
        //        eventBlocks.set(i, removeJavaScriptProtocol(eventBlocks.get(i)));
        //    }
        //} else if(generateStatistics) {
        //    for(string block : eventBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}

        //if(generateStatistics) {
        //    for(string block : eventBlocks) {
        //        statistics.getCompressedMetrics().setInlineEventSize(statistics.getCompressedMetrics().getInlineEventSize() + block.length());
        //    }
        //}
    }

    protected string removeJavaScriptProtocol(string source)
    {
        //remove javascript: from inline events
        //string result = source;

        //var match = eventJsProtocolPattern.Match(source);
        //if (matcher.matches())
        //{
        //    result = matcher.replaceFirst("$1");
        //}

        //if (generateStatistics)
        //{
        //    statistics.setPreservedSize(statistics.getPreservedSize() + result.length());
        //}

        return source;
    }

    protected void processScriptBlocks(List<string> scriptBlocks)
    {

        //if(generateStatistics) {
        //    for(string block : scriptBlocks) {
        //        statistics.getOriginalMetrics().setInlineScriptSize(statistics.getOriginalMetrics().getInlineScriptSize() + block.length());
        //    }
        //}

        //if(compressJavaScript) {
        //    for(int i = 0; i < scriptBlocks.size(); i++) {
        //        scriptBlocks.set(i, compressJavaScript(scriptBlocks.get(i)));
        //    }
        //} else if(generateStatistics) {
        //    for(string block : scriptBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}

        //if(generateStatistics) {
        //    for(string block : scriptBlocks) {
        //        statistics.getCompressedMetrics().setInlineScriptSize(statistics.getCompressedMetrics().getInlineScriptSize() + block.length());
        //    }
        //}
    }

    protected void processStyleBlocks(List<string> styleBlocks)
    {

        //if(generateStatistics) {
        //    for(string block : styleBlocks) {
        //        statistics.getOriginalMetrics().setInlineStyleSize(statistics.getOriginalMetrics().getInlineStyleSize() + block.length());
        //    }
        //}

        //if(compressCss) {
        //    for(int i = 0; i < styleBlocks.size(); i++) {
        //        styleBlocks.set(i, compressCssStyles(styleBlocks.get(i)));
        //    }
        //} else if(generateStatistics) {
        //    for(string block : styleBlocks) {
        //        statistics.setPreservedSize(statistics.getPreservedSize() + block.length());
        //    }
        //}

        //if(generateStatistics) {
        //    for(string block : styleBlocks) {
        //        statistics.getCompressedMetrics().setInlineStyleSize(statistics.getCompressedMetrics().getInlineStyleSize() + block.length());
        //    }
        //}
    }

    protected string compressJavaScript(string source)
    {

        ////set default javascript compressor
        //if (javaScriptCompressor == null)
        //{
        //    YuiJavaScriptCompressor yuiJsCompressor = new YuiJavaScriptCompressor();
        //    yuiJsCompressor.setNoMunge(yuiJsNoMunge);
        //    yuiJsCompressor.setPreserveAllSemiColons(yuiJsPreserveAllSemiColons);
        //    yuiJsCompressor.setDisableOptimizations(yuiJsDisableOptimizations);
        //    yuiJsCompressor.setLineBreak(yuiJsLineBreak);

        //    if (yuiErrorReporter != null)
        //    {
        //        yuiJsCompressor.setErrorReporter(yuiErrorReporter);
        //    }

        //    javaScriptCompressor = yuiJsCompressor;
        //}

        ////detect CDATA wrapper
        //bool cdataWrapper = false;
        //Matcher matcher = cdataPattern.matcher(source);
        //if (matcher.matches())
        //{
        //    cdataWrapper = true;
        //    source = matcher.group(1);
        //}

        //string result = javaScriptCompressor.compress(source);

        //if (cdataWrapper)
        //{
        //    result = "<![CDATA[" + result + "]]>";
        //}

        return source;

    }

    protected string compressCssStyles(string source)
    {

        ////set default css compressor
        //if (cssCompressor == null)
        //{
        //    YuiCssCompressor yuiCssCompressor = new YuiCssCompressor();
        //    yuiCssCompressor.setLineBreak(yuiCssLineBreak);

        //    cssCompressor = yuiCssCompressor;
        //}

        ////detect CDATA wrapper
        //bool cdataWrapper = false;
        //Matcher matcher = cdataPattern.matcher(source);
        //if (matcher.matches())
        //{
        //    cdataWrapper = true;
        //    source = matcher.group(1);
        //}

        //string result = cssCompressor.compress(source);

        //if (cdataWrapper)
        //{
        //    result = "<![CDATA[" + result + "]]>";
        //}

        return source;

    }

    protected HtmlCompressor createCompressorClone()
    {
        HtmlCompressor clone = new HtmlCompressor();
        //clone.setJavaScriptCompressor(javaScriptCompressor);
        //clone.setCssCompressor(cssCompressor);
        clone.setRemoveComments(_removeComments);
        clone.setRemoveMultiSpaces(_removeMultiSpaces);
        clone.setRemoveIntertagSpaces(_removeIntertagSpaces);
        clone.setRemoveQuotes(removeQuotes);
        //clone.setCompressJavaScript(compressJavaScript);
        clone.setCompressCss(compressCss);
        clone.setSimpleDoctype(_simpleDoctype);
        clone.setRemoveScriptAttributes(_removeScriptAttributes);
        clone.setRemoveStyleAttributes(_removeStyleAttributes);
        clone.setRemoveLinkAttributes(_removeLinkAttributes);
        clone.setRemoveFormAttributes(_removeFormAttributes);
        clone.setRemoveInputAttributes(_removeInputAttributes);
        clone.setSimpleBooleanAttributes(_simpleBooleanAttributes);
        //clone.setRemoveJavaScriptProtocol(removeJavaScriptProtocol);
        clone.setRemoveHttpProtocol(_removeHttpProtocol);
        clone.setRemoveHttpsProtocol(_removeHttpsProtocol);
        clone.setPreservePatterns(preservePatterns);
        clone.setYuiJsNoMunge(yuiJsNoMunge);
        clone.setYuiJsPreserveAllSemiColons(yuiJsPreserveAllSemiColons);
        clone.setYuiJsDisableOptimizations(yuiJsDisableOptimizations);
        clone.setYuiJsLineBreak(yuiJsLineBreak);
        clone.setYuiCssLineBreak(yuiCssLineBreak);
        //clone.setYuiErrorReporter(yuiErrorReporter);

        return clone;

    }

    /**
     * Returns <code>true</code> if JavaScript compression is enabled.
     * 
     * @return current state of JavaScript compression.
     */
    public bool isCompressJavaScript()
    {
        return _compressJavaScript;
    }

    /**
     * Enables JavaScript compression within &lt;script> tags using 
     * <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a> 
     * if set to <code>true</code>. Default is <code>false</code> for performance reasons.
     *  
     * <p><b>Note:</b> Compressing JavaScript is not recommended if pages are 
     * compressed dynamically on-the-fly because of performance impact. 
     * You should consider putting JavaScript into a separate file and
     * compressing it using standalone YUICompressor for example.</p>
     * 
     * @param compressJavaScript set <code>true</code> to enable JavaScript compression. 
     * Default is <code>false</code>
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * 
     */
    public void setCompressJavaScript(bool compressJavaScript)
    {
        this._compressJavaScript = compressJavaScript;
    }

    /**
     * Returns <code>true</code> if CSS compression is enabled.
     * 
     * @return current state of CSS compression.
     */
    public bool isCompressCss()
    {
        return compressCss;
    }

    /**
     * Enables CSS compression within &lt;style> tags using 
     * <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a> 
     * if set to <code>true</code>. Default is <code>false</code> for performance reasons.
     *  
     * <p><b>Note:</b> Compressing CSS is not recommended if pages are 
     * compressed dynamically on-the-fly because of performance impact. 
     * You should consider putting CSS into a separate file and
     * compressing it using standalone YUICompressor for example.</p>
     * 
     * @param compressCss set <code>true</code> to enable CSS compression. 
     * Default is <code>false</code>
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * 
     */
    public void setCompressCss(bool compressCss)
    {
        this.compressCss = compressCss;
    }

    /**
     * Returns <code>true</code> if Yahoo YUI Compressor
     * will only minify javascript without obfuscating local symbols. 
     * This corresponds to <code>--nomunge</code> command line option.  
     *   
     * @return <code>nomunge</code> parameter value used for JavaScript compression.
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public bool isYuiJsNoMunge()
    {
        return yuiJsNoMunge;
    }

    /**
     * Tells Yahoo YUI Compressor to only minify javascript without obfuscating 
     * local symbols. This corresponds to <code>--nomunge</code> command line option. 
     * This option has effect only if JavaScript compression is enabled. 
     * Default is <code>false</code>.
     * 
     * @param yuiJsNoMunge set <code>true</code> to enable <code>nomunge</code> mode
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public void setYuiJsNoMunge(bool yuiJsNoMunge)
    {
        this.yuiJsNoMunge = yuiJsNoMunge;
    }

    /**
     * Returns <code>true</code> if Yahoo YUI Compressor
     * will preserve unnecessary semicolons during JavaScript compression. 
     * This corresponds to <code>--preserve-semi</code> command line option.
     *   
     * @return <code>preserve-semi</code> parameter value used for JavaScript compression.
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public bool isYuiJsPreserveAllSemiColons()
    {
        return yuiJsPreserveAllSemiColons;
    }

    /**
     * Tells Yahoo YUI Compressor to preserve unnecessary semicolons 
     * during JavaScript compression. This corresponds to 
     * <code>--preserve-semi</code> command line option. 
     * This option has effect only if JavaScript compression is enabled.
     * Default is <code>false</code>.
     * 
     * @param yuiJsPreserveAllSemiColons set <code>true<code> to enable <code>preserve-semi</code> mode
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public void setYuiJsPreserveAllSemiColons(bool yuiJsPreserveAllSemiColons)
    {
        this.yuiJsPreserveAllSemiColons = yuiJsPreserveAllSemiColons;
    }

    /**
     * Returns <code>true</code> if Yahoo YUI Compressor
     * will disable all the built-in micro optimizations during JavaScript compression. 
     * This corresponds to <code>--disable-optimizations</code> command line option.
     *   
     * @return <code>disable-optimizations</code> parameter value used for JavaScript compression.
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public bool isYuiJsDisableOptimizations()
    {
        return yuiJsDisableOptimizations;
    }

    /**
     * Tells Yahoo YUI Compressor to disable all the built-in micro optimizations
     * during JavaScript compression. This corresponds to 
     * <code>--disable-optimizations</code> command line option. 
     * This option has effect only if JavaScript compression is enabled.
     * Default is <code>false</code>.
     * 
     * @param yuiJsDisableOptimizations set <code>true<code> to enable 
     * <code>disable-optimizations</code> mode
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public void setYuiJsDisableOptimizations(bool yuiJsDisableOptimizations)
    {
        this.yuiJsDisableOptimizations = yuiJsDisableOptimizations;
    }

    /**
     * Returns number of symbols per line Yahoo YUI Compressor
     * will use during JavaScript compression. 
     * This corresponds to <code>--line-break</code> command line option.
     *   
     * @return <code>line-break</code> parameter value used for JavaScript compression.
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public int getYuiJsLineBreak()
    {
        return yuiJsLineBreak;
    }

    /**
     * Tells Yahoo YUI Compressor to break lines after the specified number of symbols 
     * during JavaScript compression. This corresponds to 
     * <code>--line-break</code> command line option. 
     * This option has effect only if JavaScript compression is enabled.
     * Default is <code>-1</code> to disable line breaks.
     * 
     * @param yuiJsLineBreak set number of symbols per line
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public void setYuiJsLineBreak(int yuiJsLineBreak)
    {
        this.yuiJsLineBreak = yuiJsLineBreak;
    }

    /**
     * Returns number of symbols per line Yahoo YUI Compressor
     * will use during CSS compression. 
     * This corresponds to <code>--line-break</code> command line option.
     *   
     * @return <code>line-break</code> parameter value used for CSS compression.
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public int getYuiCssLineBreak()
    {
        return yuiCssLineBreak;
    }

    /**
     * Tells Yahoo YUI Compressor to break lines after the specified number of symbols 
     * during CSS compression. This corresponds to 
     * <code>--line-break</code> command line option. 
     * This option has effect only if CSS compression is enabled.
     * Default is <code>-1</code> to disable line breaks.
     * 
     * @param yuiCssLineBreak set number of symbols per line
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    public void setYuiCssLineBreak(int yuiCssLineBreak)
    {
        this.yuiCssLineBreak = yuiCssLineBreak;
    }

    /**
     * Returns <code>true</code> if all unnecessary quotes will be removed 
     * from tag attributes. 
     *   
     */
    public bool isRemoveQuotes()
    {
        return removeQuotes;
    }

    /**
     * If set to <code>true</code> all unnecessary quotes will be removed  
     * from tag attributes. Default is <code>false</code>.
     * 
     * <p><b>Note:</b> Even though quotes are removed only when it is safe to do so, 
     * it still might break strict HTML validation. Turn this option on only if 
     * a page validation is not very important or to squeeze the most out of the compression.
     * This option has no performance impact. 
     * 
     * @param removeQuotes set <code>true</code> to remove unnecessary quotes from tag attributes
     */
    public void setRemoveQuotes(bool removeQuotes)
    {
        this.removeQuotes = removeQuotes;
    }

    /**
     * Returns <code>true</code> if compression is enabled.  
     * 
     * @return <code>true</code> if compression is enabled.
     */
    public bool isEnabled()
    {
        return enabled;
    }

    /**
     * If set to <code>false</code> all compression will be bypassed. Might be useful for testing purposes. 
     * Default is <code>true</code>.
     * 
     * @param enabled set <code>false</code> to bypass all compression
     */
    public void setEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    /**
     * Returns <code>true</code> if all HTML comments will be removed.
     * 
     * @return <code>true</code> if all HTML comments will be removed
     */
    public bool isRemoveComments()
    {
        return _removeComments;
    }

    /**
     * If set to <code>true</code> all HTML comments will be removed.   
     * Default is <code>true</code>.
     * 
     * @param removeComments set <code>true</code> to remove all HTML comments
     */
    public void setRemoveComments(bool removeComments)
    {
        this._removeComments = removeComments;
    }

    /**
     * Returns <code>true</code> if all multiple whitespace characters will be replaced with single spaces.
     * 
     * @return <code>true</code> if all multiple whitespace characters will be replaced with single spaces.
     */
    public bool isRemoveMultiSpaces()
    {
        return _removeMultiSpaces;
    }

    /**
     * If set to <code>true</code> all multiple whitespace characters will be replaced with single spaces.
     * Default is <code>true</code>.
     * 
     * @param removeMultiSpaces set <code>true</code> to replace all multiple whitespace characters 
     * will single spaces.
     */
    public void setRemoveMultiSpaces(bool removeMultiSpaces)
    {
        this._removeMultiSpaces = removeMultiSpaces;
    }

    /**
     * Returns <code>true</code> if all inter-tag whitespace characters will be removed.
     * 
     * @return <code>true</code> if all inter-tag whitespace characters will be removed.
     */
    public bool isRemoveIntertagSpaces()
    {
        return _removeIntertagSpaces;
    }

    /**
     * If set to <code>true</code> all inter-tag whitespace characters will be removed.
     * Default is <code>false</code>.
     * 
     * <p><b>Note:</b> It is fairly safe to turn this option on unless you 
     * rely on spaces for page formatting. Even if you do, you can always preserve 
     * required spaces with <code>&amp;nbsp;</code>. This option has no performance impact.    
     * 
     * @param removeIntertagSpaces set <code>true</code> to remove all inter-tag whitespace characters
     */
    public void setRemoveIntertagSpaces(bool removeIntertagSpaces)
    {
        this._removeIntertagSpaces = removeIntertagSpaces;
    }

    /**
     * Returns a list of Patterns defining custom preserving block rules  
     * 
     * @return list of <code>Regex</code> objects defining rules for preserving block rules
     */
    public List<Regex> getPreservePatterns()
    {
        return preservePatterns;
    }

    /**
     * This method allows setting custom block preservation rules defined by regular 
     * expression patterns. Blocks that match provided patterns will be skipped during HTML compression. 
     * 
     * <p>Custom preservation rules have higher priority than default rules.
     * Priority between custom rules are defined by their position in a list 
     * (beginning of a list has higher priority).
     * 
     * <p>Besides custom patterns, you can use 3 predefined patterns: 
     * {@link #PHP_TAG_PATTERN PHP_TAG_PATTERN},
     * {@link #SERVER_SCRIPT_TAG_PATTERN SERVER_SCRIPT_TAG_PATTERN},
     * {@link #SERVER_SIDE_INCLUDE_PATTERN SERVER_SIDE_INCLUDE_PATTERN}.
     * 
     * @param preservePatterns List of <code>Regex</code> objects that will be 
     * used to skip matched blocks during compression  
     */
    public void setPreservePatterns(List<Regex> preservePatterns)
    {
        this.preservePatterns = preservePatterns;
    }

    /**
     * Returns <code>ErrorReporter</code> used by YUI Compressor to log error messages 
     * during JavasSript compression 
     * 
     * @return <code>ErrorReporter</code> used by YUI Compressor to log error messages 
     * during JavasSript compression
     * 
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * @see <a href="http://www.mozilla.org/rhino/apidocs/org/mozilla/javascript/ErrorReporter.html">Error Reporter Interface</a>
     */
    //public ErrorReporter getYuiErrorReporter()
    //{
    //    return yuiErrorReporter;
    //}

    /**
     * Sets <code>ErrorReporter</code> that YUI Compressor will use for reporting errors during 
     * JavaScript compression. If no <code>ErrorReporter</code> was provided 
     * {@link YuiJavaScriptCompressor.DefaultErrorReporter} will be used 
     * which reports errors to <code>System.err</code> stream. 
     * 
     * @param yuiErrorReporter <code>ErrorReporter<code> that will be used by YUI Compressor
     * 
     * @see YuiJavaScriptCompressor.DefaultErrorReporter
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * @see <a href="http://www.mozilla.org/rhino/apidocs/org/mozilla/javascript/ErrorReporter.html">ErrorReporter Interface</a>
     */
    //public void setYuiErrorReporter(ErrorReporter yuiErrorReporter)
    //{
    //    this.yuiErrorReporter = yuiErrorReporter;
    //}

    /**
     * Returns JavaScript compressor implementation that will be used 
     * to compress inline JavaScript in HTML.
     * 
     * @return <code>Compressor</code> implementation that will be used 
     * to compress inline JavaScript in HTML.
     * 
     * @see YuiJavaScriptCompressor
     * @see ClosureJavaScriptCompressor
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * @see <a href="http://code.google.com/closure/compiler/">Google Closure Compiler</a>
     */
    //public Compressor getJavaScriptCompressor()
    //{
    //    return javaScriptCompressor;
    //}

    /**
     * Sets JavaScript compressor implementation that will be used 
     * to compress inline JavaScript in HTML. 
     * 
     * <p>HtmlCompressor currently 
     * comes with basic implementations for <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a> (called {@link YuiJavaScriptCompressor})
     * and <a href="http://code.google.com/closure/compiler/">Google Closure Compiler</a> (called {@link ClosureJavaScriptCompressor}) that should be enough for most cases, 
     * but users can also create their own JavaScript compressors for custom needs.
     * 
     * <p>If no compressor is set {@link YuiJavaScriptCompressor} will be used by default.  
     * 
     * @param javaScriptCompressor {@link Compressor} implementation that will be used for inline JavaScript compression
     * 
     * @see YuiJavaScriptCompressor
     * @see ClosureJavaScriptCompressor
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     * @see <a href="http://code.google.com/closure/compiler/">Google Closure Compiler</a>
     */
    //public void setJavaScriptCompressor(Compressor javaScriptCompressor)
    //{
    //    this.javaScriptCompressor = javaScriptCompressor;
    //}

    /**
     * Returns CSS compressor implementation that will be used 
     * to compress inline CSS in HTML.
     * 
     * @return <code>Compressor</code> implementation that will be used 
     * to compress inline CSS in HTML.
     * 
     * @see YuiCssCompressor
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    //public Compressor getCssCompressor()
    //{
    //    return cssCompressor;
    //}

    /**
     * Sets CSS compressor implementation that will be used 
     * to compress inline CSS in HTML. 
     * 
     * <p>HtmlCompressor currently 
     * comes with basic implementation for <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a> (called {@link YuiCssCompressor}), 
     * but users can also create their own CSS compressors for custom needs. 
     * 
     * <p>If no compressor is set {@link YuiCssCompressor} will be used by default.  
     * 
     * @param cssCompressor {@link Compressor} implementation that will be used for inline CSS compression
     * 
     * @see YuiCssCompressor
     * @see <a href="http://developer.yahoo.com/yui/compressor/">Yahoo YUI Compressor</a>
     */
    //public void setCssCompressor(Compressor cssCompressor)
    //{
    //    this.cssCompressor = cssCompressor;
    //}

    /**
     * Returns <code>true</code> if existing DOCTYPE declaration will be replaced with simple <code><!DOCTYPE html></code> declaration.
     * 
     * @return <code>true</code> if existing DOCTYPE declaration will be replaced with simple <code><!DOCTYPE html></code> declaration.
     */
    public bool isSimpleDoctype()
    {
        return _simpleDoctype;
    }

    /**
     * If set to <code>true</code>, existing DOCTYPE declaration will be replaced with simple <code>&lt;!DOCTYPE html></code> declaration.
     * Default is <code>false</code>.
     * 
     * @param simpleDoctype set <code>true</code> to replace existing DOCTYPE declaration with <code>&lt;!DOCTYPE html></code>
     */
    public void setSimpleDoctype(bool simpleDoctype)
    {
        this._simpleDoctype = simpleDoctype;
    }

    /**
     * Returns <code>true</code> if unnecessary attributes wil be removed from <code>&lt;script></code> tags 
     * 
     * @return <code>true</code> if unnecessary attributes wil be removed from <code>&lt;script></code> tags
     */
    public bool isRemoveScriptAttributes()
    {
        return _removeScriptAttributes;
    }

    /**
     * If set to <code>true</code>, following attributes will be removed from <code>&lt;script></code> tags: 
     * <ul>
     * <li>type="text/javascript"</li>
     * <li>type="application/javascript"</li>
     * <li>language="javascript"</li>
     * </ul>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param removeScriptAttributes set <code>true</code> to remove unnecessary attributes from <code>&lt;script></code> tags 
     */
    public void setRemoveScriptAttributes(bool removeScriptAttributes)
    {
        this._removeScriptAttributes = removeScriptAttributes;
    }

    /**
     * Returns <code>true</code> if <code>type="text/style"</code> attributes will be removed from <code>&lt;style></code> tags
     * 
     * @return <code>true</code> if <code>type="text/style"</code> attributes will be removed from <code>&lt;style></code> tags
     */
    public bool isRemoveStyleAttributes()
    {
        return _removeStyleAttributes;
    }

    /**
     * If set to <code>true</code>, <code>type="text/style"</code> attributes will be removed from <code>&lt;style></code> tags. Default is <code>false</code>.
     * 
     * @param removeStyleAttributes set <code>true</code> to remove <code>type="text/style"</code> attributes from <code>&lt;style></code> tags
     */
    public void setRemoveStyleAttributes(bool removeStyleAttributes)
    {
        this._removeStyleAttributes = removeStyleAttributes;
    }

    /**
     * Returns <code>true</code> if unnecessary attributes will be removed from <code>&lt;link></code> tags
     * 
     * @return <code>true</code> if unnecessary attributes will be removed from <code>&lt;link></code> tags
     */
    public bool isRemoveLinkAttributes()
    {
        return _removeLinkAttributes;
    }

    /**
     * If set to <code>true</code>, following attributes will be removed from <code>&lt;link rel="stylesheet"></code> and <code>&lt;link rel="alternate stylesheet"></code> tags: 
     * <ul>
     * <li>type="text/css"</li>
     * <li>type="text/plain"</li>
     * </ul>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param removeLinkAttributes set <code>true</code> to remove unnecessary attributes from <code>&lt;link></code> tags
     */
    public void setRemoveLinkAttributes(bool removeLinkAttributes)
    {
        this._removeLinkAttributes = removeLinkAttributes;
    }

    /**
     * Returns <code>true</code> if <code>method="get"</code> attributes will be removed from <code>&lt;form></code> tags
     * 
     * @return <code>true</code> if <code>method="get"</code> attributes will be removed from <code>&lt;form></code> tags
     */
    public bool isRemoveFormAttributes()
    {
        return _removeFormAttributes;
    }

    /**
     * If set to <code>true</code>, <code>method="get"</code> attributes will be removed from <code>&lt;form></code> tags. Default is <code>false</code>.
     * 
     * @param removeFormAttributes set <code>true</code> to remove <code>method="get"</code> attributes from <code>&lt;form></code> tags
     */
    public void setRemoveFormAttributes(bool removeFormAttributes)
    {
        this._removeFormAttributes = removeFormAttributes;
    }

    /**
     * Returns <code>true</code> if <code>type="text"</code> attributes will be removed from <code>&lt;input></code> tags
     * @return <code>true</code> if <code>type="text"</code> attributes will be removed from <code>&lt;input></code> tags
     */
    public bool isRemoveInputAttributes()
    {
        return _removeInputAttributes;
    }

    /**
     * If set to <code>true</code>, <code>type="text"</code> attributes will be removed from <code>&lt;input></code> tags. Default is <code>false</code>.
     * 
     * @param removeInputAttributes set <code>true</code> to remove <code>type="text"</code> attributes from <code>&lt;input></code> tags
     */
    public void setRemoveInputAttributes(bool removeInputAttributes)
    {
        this._removeInputAttributes = removeInputAttributes;
    }

    /**
     * Returns <code>true</code> if bool attributes will be simplified
     * 
     * @return <code>true</code> if bool attributes will be simplified
     */
    public bool isSimpleBooleanAttributes()
    {
        return _simpleBooleanAttributes;
    }

    /**
     * If set to <code>true</code>, any values of following bool attributes will be removed:
     * <ul>
     * <li>checked</li>
     * <li>selected</li>
     * <li>disabled</li>
     * <li>readonly</li>
     * </ul>
     * 
     * <p>For example, <code>&ltinput readonly="readonly"></code> would become <code>&ltinput readonly></code>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param simpleBooleanAttributes set <code>true</code> to simplify bool attributes
     */
    public void setSimpleBooleanAttributes(bool simpleBooleanAttributes)
    {
        this._simpleBooleanAttributes = simpleBooleanAttributes;
    }

    /**
     * Returns <code>true</code> if <code>javascript:</code> pseudo-protocol will be removed from inline event handlers.
     * 
     * @return <code>true</code> if <code>javascript:</code> pseudo-protocol will be removed from inline event handlers.
     */
    public bool isRemoveJavaScriptProtocol()
    {
        return _removeJavaScriptProtocol;
    }

    /**
     * If set to <code>true</code>, <code>javascript:</code> pseudo-protocol will be removed from inline event handlers.
     * 
     * <p>For example, <code>&lta onclick="javascript:alert()"></code> would become <code>&lta onclick="alert()"></code>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param removeJavaScriptProtocol set <code>true</code> to remove <code>javascript:</code> pseudo-protocol from inline event handlers.
     */
    public void setRemoveJavaScriptProtocol(bool removeJavaScriptProtocol)
    {
        this._removeJavaScriptProtocol = removeJavaScriptProtocol;
    }

    /**
     * Returns <code>true</code> if <code>HTTP</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     * 
     * @return <code>true</code> if <code>HTTP</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     */
    public bool isRemoveHttpProtocol()
    {
        return _removeHttpProtocol;
    }

    /**
     * If set to <code>true</code>, <code>HTTP</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     * URL without a protocol would make a browser use document's current protocol instead. 
     * 
     * <p>Tags marked with <code>rel="external"</code> will be skipped.
     * 
     * <p>For example: 
     * <p><code>&lta href="http://example.com"> &ltscript src="http://google.com/js.js" rel="external"></code> 
     * <p>would become: 
     * <p><code>&lta href="//example.com"> &ltscript src="http://google.com/js.js" rel="external"></code>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param removeHttpProtocol set <code>true</code> to remove <code>HTTP</code> protocol from tag attributes
     */
    public void setRemoveHttpProtocol(bool removeHttpProtocol)
    {
        this._removeHttpProtocol = removeHttpProtocol;
    }

    /**
     * Returns <code>true</code> if <code>HTTPS</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     * 
     * @return <code>true</code> if <code>HTTPS</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     */
    public bool isRemoveHttpsProtocol()
    {
        return _removeHttpsProtocol;
    }

    /**
     * If set to <code>true</code>, <code>HTTPS</code> protocol will be removed from <code>href</code>, <code>src</code>, <code>cite</code>, and <code>action</code> tag attributes.
     * URL without a protocol would make a browser use document's current protocol instead.
     * 
     * <p>Tags marked with <code>rel="external"</code> will be skipped.
     * 
     * <p>For example: 
     * <p><code>&lta href="https://example.com"> &ltscript src="https://google.com/js.js" rel="external"></code> 
     * <p>would become: 
     * <p><code>&lta href="//example.com"> &ltscript src="https://google.com/js.js" rel="external"></code>
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param removeHttpsProtocol set <code>true</code> to remove <code>HTTP</code> protocol from tag attributes
     */
    public void setRemoveHttpsProtocol(bool removeHttpsProtocol)
    {
        this._removeHttpsProtocol = removeHttpsProtocol;
    }

    /**
     * Returns <code>true</code> if HTML compression statistics is generated
     * 
     * @return <code>true</code> if HTML compression statistics is generated
     */
    public bool isGenerateStatistics()
    {
        return generateStatistics;
    }

    /**
     * If set to <code>true</code>, HTML compression statistics will be generated. 
     * 
     * <p><strong>Important:</strong> Enabling statistics makes HTML compressor not thread safe. 
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param generateStatistics set <code>true</code> to generate HTML compression statistics 
     * 
     * @see #getStatistics()
     */
    public void setGenerateStatistics(bool generateStatistics)
    {
        this.generateStatistics = generateStatistics;
    }

    /**
     * Returns {@link HtmlCompressorStatistics} object containing statistics of the last HTML compression, if enabled. 
     * Should be called after {@link #compress(string)}
     * 
     * @return {@link HtmlCompressorStatistics} object containing last HTML compression statistics
     * 
     * @see HtmlCompressorStatistics
     * @see #setGenerateStatistics(bool)
     */
    //public HtmlCompressorStatistics getStatistics()
    //{
    //    return statistics;
    //}

    /**
     * Returns <code>true</code> if line breaks will be preserved.
     * 
     * @return <code>true</code> if line breaks will be preserved. 
     */
    public bool isPreserveLineBreaks()
    {
        return preserveLineBreaks;
    }

    /**
     * If set to <code>true</code>, line breaks will be preserved. 
     * 
     * <p>Default is <code>false</code>.
     * 
     * @param preserveLineBreaks set <code>true</code> to preserve line breaks
     */
    public void setPreserveLineBreaks(bool preserveLineBreaks)
    {
        this.preserveLineBreaks = preserveLineBreaks;
    }

    /**
     * Returns a comma separated list of tags around which spaces will be removed. 
     * 
     * @return a comma separated list of tags around which spaces will be removed. 
     */
    public string getRemoveSurroundingSpaces()
    {
        return _removeSurroundingSpaces;
    }

    /**
     * Enables surrounding spaces removal around provided comma separated list of tags.
     * 
     * <p>Besides custom defined lists, you can pass one of 3 predefined lists of tags: 
     * {@link #BLOCK_TAGS_MIN BLOCK_TAGS_MIN},
     * {@link #BLOCK_TAGS_MAX BLOCK_TAGS_MAX},
     * {@link #ALL_TAGS ALL_TAGS}.
     * 
     * @param tagList a comma separated list of tags around which spaces will be removed
     */
    public void setRemoveSurroundingSpaces(string tagList)
    {
        if (tagList != null && tagList.Length == 0)
        {
            tagList = null;
        }
        this._removeSurroundingSpaces = tagList;
    }

}