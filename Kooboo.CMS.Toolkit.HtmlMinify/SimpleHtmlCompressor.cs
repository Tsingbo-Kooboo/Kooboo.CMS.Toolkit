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
using System.Linq;
using Kooboo.Web.Mvc.WebResourceLoader;
public class SimpleHtmlCompressor
{

    //javascript and css compressor implementations
    //private Compressor javaScriptCompressor = null;
    //private Compressor cssCompressor = null;

    //default settings
    private bool _removeComments = true;
    private bool _removeMultiSpaces = true;

    public bool IsRemoveComments
    {
        get
        {
            return _removeComments;
        }
        set
        {
            _removeComments = value;
        }
    }
    public bool IsRemoveMultiSpaces
    {
        get
        {
            return _removeMultiSpaces;
        }
        set
        {
            _removeMultiSpaces = value;
        }
    }

    public bool IsCompressJs { get; set; }


    //temp replacements for preserved blocks 
    protected static string tempCondCommentBlock = "%%%~COMPRESS~COND~{0}~%%%";
    protected static string tempPreBlock = "%%%~COMPRESS~PRE~{0}~%%%";
    protected static string tempTextAreaBlock = "%%%~COMPRESS~TEXTAREA~{0}~%%%";
    protected static string tempScriptBlock = "%%%~COMPRESS~SCRIPT~{0}~%%%";
    protected static string tempStyleBlock = "%%%~COMPRESS~STYLE~{0}~%%%";
    protected static string tempEventBlock = "%%%~COMPRESS~EVENT~{0}~%%%";
    protected static string tempLineBreakBlock = "%%%~COMPRESS~LT~{0}~%%%";
    protected static string tempSkipBlock = "%%%~COMPRESS~SKIP~{0}~%%%";
    protected static string tempUserBlock = "%%%~COMPRESS~USER{0}~{1}~%%%";

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
    protected static Regex scriptPattern = new Regex("(<script[^>]*?>)([\\s\\S]*?)(</script>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
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
    public string Compress(string html)
    {
        List<string> preBlocks = new List<string>();
        List<string> taBlocks = new List<string>();
        List<string> scriptBlocks = new List<string>();
        List<string> styleBlocks = new List<string>();

        html = PreserveBlocks(html, preBlocks, taBlocks, scriptBlocks, styleBlocks);

        //process pure html
        html = ProcessHtml(html);

        ProcessPreservedBlocks(html, preBlocks, taBlocks, scriptBlocks, styleBlocks);

        html = ReturnBlocks(html, preBlocks, taBlocks, scriptBlocks, styleBlocks);

        return html;
    }

    protected void ProcessPreservedBlocks(string html, List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks)
    {
        if (IsCompressJs)
        {
            for (int i = 0; i < scriptBlocks.Count; i++)
            {
                scriptBlocks[i] = JSMinify.Minify(scriptBlocks[i]);
            }
        }
    }
    protected string PreserveBlocks(string html, List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks)
    {
        var index = 0;

        //preserve PRE tags
        index = 0;
        html = prePattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               preBlocks.Add(m.Groups[2].Value);
                               return m.Groups[1].Value + string.Format(tempPreBlock, index++) + m.Groups[3].Value;
                           }
                           return "";
                       });


        //preserve SCRIPT tags
        index = 0;
        html = scriptPattern.Replace(html,
                       m =>
                       {
                           //javascript block, preserve and compress with js compressor
                           scriptBlocks.Add(m.Groups[2].Value);
                           return m.Groups[1].Value + string.Format(tempScriptBlock, index++) + m.Groups[3].Value;
                       });


        //preserve STYLE tags
        index = 0;
        html = stylePattern.Replace(html,
                       m =>
                       {
                           if (m.Groups[2].Value.Trim().Length > 0)
                           {
                               styleBlocks.Add(m.Groups[2].Value);
                               return m.Groups[1].Value + string.Format(tempStyleBlock, index++) + m.Groups[3].Value;
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
                               return m.Groups[1].Value + string.Format(tempTextAreaBlock, index++) + m.Groups[3].Value;
                           }

                           return "";
                       });
        return html;
    }
    protected string ReturnBlocks(string html, List<string> preBlocks, List<string> taBlocks, List<string> scriptBlocks, List<string> styleBlocks)
    {

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
        return html;
    }
    protected string ProcessHtml(string html)
    {

        //remove comments
        html = RemoveComments(html);

        //remove multi whitespace characters
        html = RemoveMultiSpaces(html);

        return html.Trim();
    }
    protected string RemoveMultiSpaces(string html)
    {
        //collapse multiple spaces
        if (_removeMultiSpaces)
        {
            html = multispacePattern.Replace(html, " ");
        }
        return html;
    }
    protected string RemoveComments(string html)
    {
        //remove comments
        if (_removeComments)
        {
            html = commentPattern.Replace(html, "");
        }
        return html;
    }



}