!function(){function s(s,t){return e(s||self.document.URL||self.location.href,t||l())}function l(){var s=document.getElementsByTagName("script");return s[s.length-1].src}function e(s,l){var e=l;return/^(\/|\\\\)/.test(l)?e=/^.+?\w(\/|\\\\)/.exec(s)[0]+l.replace(/^(\/|\\\\)/,""):/^[a-z]+:/i.test(l)||(s=s.split("#")[0].split("?")[0].replace(/[^\\\/]+$/,""),e=s+""+l),t(e)}function t(s){var l=/^[a-z]+:\/\//.exec(s)[0],e=null,t=[];for(s=s.replace(l,"").split("?")[0].split("#")[0],s=s.replace(/\\/g,"/").split(/\//),s[s.length-1]="";s.length;)".."===(e=s.shift())?t.pop():"."!==e&&t.push(e);return l+t.join("/")}window.UEDITOR_HOME_URL=window.UEDITOR_HOME_URL||"/Scripts/ueditor/";var a=window.UEDITOR_HOME_URL||s(),i=window.location.href,r=i.indexOf("?");window.UEDITOR_CONFIG={UEDITOR_HOME_URL:a,serverUrl:"/Contents/UEditor/Browser"+i.substr(r),toolbars:[["fullscreen","source","|","undo","redo","|","bold","italic","underline","strikethrough","formatmatch","pasteplain","|","forecolor","backcolor","|","fontfamily","fontsize","|","indent","|","justifyleft","justifycenter","justifyright","justifyjustify","|","link","unlink","|","simpleupload","insertimage","emotion","insertvideo","attachment","pagebreak","|","spechars","|","preview","searchreplace"]],langPath:a+"lang/",themePath:a+"themes/",iframeCssUrl:a+"themes/iframe.css",initialFrameWidth:1e3,initialFrameHeight:350,autoHeightEnabled:!1,codeMirrorJsUrl:"/Scripts/codemirror/lib/codemirror.js",codeMirrorCssUrl:"/Scripts/codemirror/lib/codemirror.css",xssFilterRules:!0,inputXssFilter:!0,outputXssFilter:!0,whitList:{a:["target","href","title","class","style"],abbr:["title","class","style"],address:["class","style"],area:["shape","coords","href","alt"],article:[],aside:[],audio:["autoplay","controls","loop","preload","src","class","style"],b:["class","style"],bdi:["dir"],bdo:["dir"],big:[],blockquote:["cite","class","style"],br:[],caption:["class","style"],center:[],cite:[],code:["class","style"],col:["align","valign","span","width","class","style"],colgroup:["align","valign","span","width","class","style"],dd:["class","style"],del:["datetime"],details:["open"],div:["class","style"],dl:["class","style"],dt:["class","style"],em:["class","style"],font:["color","size","face"],footer:[],h1:["class","style"],h2:["class","style"],h3:["class","style"],h4:["class","style"],h5:["class","style"],h6:["class","style"],header:[],hr:[],i:["class","style"],img:["src","alt","title","width","height","id","_src","_url","loadingclass","class","data-latex"],ins:["datetime"],li:["class","style"],mark:[],nav:[],ol:["class","style"],p:["class","style"],pre:["class","style"],s:[],section:[],small:[],span:["class","style"],sub:["class","style"],sup:["class","style"],strong:["class","style"],table:["width","border","align","valign","class","style"],tbody:["align","valign","class","style"],td:["width","rowspan","colspan","align","valign","class","style"],tfoot:["align","valign","class","style"],th:["width","rowspan","colspan","align","valign","class","style"],thead:["align","valign","class","style"],tr:["rowspan","align","valign","class","style"],tt:[],u:[],ul:["class","style"],video:["autoplay","controls","loop","preload","src","height","width","class","style"],source:["src","type"],embed:["type","class","pluginspage","src","width","height","align","style","wmode","play",NaN,"loop","menu","allowscriptaccess","allowfullscreen","controls","preload"],iframe:["src","class","height","width","max-width","max-height","align","frameborder","allowfullscreen"]}},window.UE={getUEBasePath:s}}();