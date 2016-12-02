using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models
{
    public class UEditorConfig
    {
        #region --- 上传图片配置项 ---
        [JsonProperty("imageActionName")]
        public string ImageActionName { get; set; } = "uploadimage";

        [JsonProperty("imageFieldName")]
        public string ImageFieldName { get; set; } = "upfile";

        [JsonProperty("imageMaxSize")]
        public int ImageMaxSize { get; set; } = 2048000;
        [JsonProperty("imageAllowFiles")]
        public List<string> ImageAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        [JsonProperty("imageCompressEnable")]
        public bool ImageCompressEnable { get; set; } = true;

        [JsonProperty("imageCompressBorder")]
        public int ImageCompressBorder { get; set; } = 1600;

        [JsonProperty("imageInsertAlign")]
        public string ImageInsertAlign { get; set; } = "none";

        [JsonProperty("imageUrlPrefix")]
        public string ImageUrlPrefix { get; set; } = "/ueditor/net/";

        [JsonProperty("imagePathFormat")]
        public string ImagePathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";
        #endregion

        #region --- 涂鸦图片上传配置项 ---
        [JsonProperty("scrawlActionName")]
        public string ScrawlActionName { get; set; } = "uploadscrawl";

        [JsonProperty("scrawlFieldName")]
        public string ScrawlFieldName { get; set; } = "upfile";

        [JsonProperty("scrawlPathFormat")]
        public string ScrawlPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";

        [JsonProperty("scrawlMaxSize")]
        public int ScrawlMaxSize { get; set; } = 2048000;

        [JsonProperty("scrawlUrlPrefix")]
        public string ScrawlUrlPrefix { get; set; } = "/ueditor/net/";

        [JsonProperty("scrawlInsertAlign")]
        public string ScrawlInsertAlign { get; set; } = "none";
        #endregion

        #region --- 截图工具上传 ---
        [JsonProperty("snapscreenActionName")]
        public string SnapscreenActionName { get; set; } = "uploadimage";
        [JsonProperty("snapscreenPathFormat")]
        public string SnapscreenPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";
        [JsonProperty("snapscreenUrlPrefix")]
        public string SnapscreenUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("snapscreenInsertAlign")]
        public string SnapscreenInsertAlign { get; set; } = "none";
        #endregion

        #region --- 抓取远程图片配置 ---
        [JsonProperty("catcherLocalDomain")]
        public List<string> CatcherLocalDomain { get; set; } = new List<string> { "127.0.0.1", "localhost", "img.baidu.com" };
        [JsonProperty("catcherActionName")]
        public string CatcherActionName { get; set; } = "catchimage";

        [JsonProperty("catcherFieldName")]
        public string CatcherFieldName { get; set; } = "source";
        [JsonProperty("catcherPathFormat")]
        public string CatcherPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";

        [JsonProperty("catcherUrlPrefix")]
        public string CatcherUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("catcherMaxSize")]
        public int CatcherMaxSize { get; set; } = 2048000;
        [JsonProperty("catcherAllowFiles")]
        public List<string> CatcherAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        #endregion

        #region --- 上传视频配置 ---
        [JsonProperty("videoActionName")]
        public string VideoActionName { get; set; } = "uploadvideo";
        [JsonProperty("videoFieldName")]
        public string VideoFieldName { get; set; } = "upfile";
        [JsonProperty("videoPathFormat")]
        public string VideoPathFormat { get; set; } = "upload/video/{yyyy}{mm}{dd}/{time}{rand:6}";
        [JsonProperty("videoUrlPrefix")]
        public string VideoUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("videoMaxSize")]
        public int VideoMaxSize { get; set; } = 102400000;
        [JsonProperty("videoAllowFiles")]
        public List<string> VideoAllowFiles { get; set; } = new List<string> {".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid" };
        #endregion

        #region --- 上传文件配置 ---
        [JsonProperty("fileActionName")]
        public string FileActionName { get; set; } = "uploadfile";
        [JsonProperty("fileFieldName")]
        public string FileFieldName { get; set; } = "upfile";
        [JsonProperty("filePathFormat")]
        public string FilePathFormat { get; set; } = "upload/file/{yyyy}{mm}{dd}/{time}{rand:6}";
        [JsonProperty("fileUrlPrefix")]
        public string FileUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("fileMaxSize")]
        public int FileMaxSize { get; set; } = 51200000;
        [JsonProperty("fileAllowFiles")]
        public List<string> FileAllowFiles { get; set; } = new List<string>
        {
             ".png", ".jpg", ".jpeg", ".gif", ".bmp",
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
        ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
        ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"
        };
        #endregion

        #region --- 列出指定目录下的图片 ---
        [JsonProperty("imageManagerActionName")]
        public string ImageManagerActionName { get; set; } = "listimage";
        [JsonProperty("imageManagerListPath")]
        public string ImageManagerListPath { get; set; } = "upload/image";
        [JsonProperty("imageManagerListSize")]
        public int ImageManagerListSize { get; set; } = 20;
        [JsonProperty("imageManagerUrlPrefix")]
        public string ImageManagerUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("imageManagerInsertAlign")]
        public string ImageManagerInsertAlign { get; set; } = "none";
        [JsonProperty("imageManagerAllowFiles")]
        public List<string> ImageManagerAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        #endregion

        #region --- 列出指定目录下的文件 ---
        [JsonProperty("fileManagerActionName")]
        public string FileManagerActionName { get; set; } = "listfile";
        [JsonProperty("fileManagerListPath")]
        public string FileManagerListPath { get; set; } = "upload/file";
        [JsonProperty("fileManagerUrlPrefix")]
        public string FileManagerUrlPrefix { get; set; } = "/ueditor/net/";
        [JsonProperty("fileManagerListSize")]
        public int FileManagerListSize { get; set; } = 20;
        [JsonProperty("fileManagerAllowFiles")]
        public List<string> FileManagerAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp",
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
        ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
        ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"}; 
        #endregion
    }
}
