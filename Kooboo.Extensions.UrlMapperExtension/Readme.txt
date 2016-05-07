Example:
I have a server which hosts a Kooboo CMS application,with binds domains:
www.kooboo.com
www.kooboo.cn
www.kooboo.org
......

It has a subsite "en".In another word,we can access it by:
http://www.kooboo.com/en
http://www.kooboo.cn/en
http://www.kooboo.org/en
......

Now I want to make a url redirect,for example I want to:
http://www.kooboo.com/article/abc --> http://www.kooboo.com/en/article/abc
http://www.kooboo.cn/article/abc --> http://www.kooboo.cn/en/article/abc
http://www.kooboo.org/article/abc --> http://www.kooboo.org/en/article/abc

With this extension we can make a url redirect rule in root site like this:
Regex			Regular expression 
Input URL/Pattern	/article/(.+) 
Output URL/Pattern	~/en/article/$1 
Redirect type		Move_Permanently_301 
 
