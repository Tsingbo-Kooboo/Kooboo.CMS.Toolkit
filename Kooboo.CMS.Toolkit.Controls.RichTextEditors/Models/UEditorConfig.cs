using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models
{
    [DataContract]
    public class UEditorConfig
    {
        #region --- 上传图片配置项 ---
        [DataMember(Name ="imageActionName")]
        
        public string ImageActionName { get; set; } = "uploadimage";

        [DataMember(Name ="imageFieldName")]
        public string ImageFieldName { get; set; } = "upfile";

        [DataMember(Name ="imageMaxSize")]
        public int ImageMaxSize { get; set; } = 2048000;
        [DataMember(Name ="imageAllowFiles")]
        public List<string> ImageAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        [DataMember(Name ="imageCompressEnable")]
        public bool ImageCompressEnable { get; set; } = true;

        [DataMember(Name ="imageCompressBorder")]
        public int ImageCompressBorder { get; set; } = 1600;

        [DataMember(Name ="imageInsertAlign")]
        public string ImageInsertAlign { get; set; } = "none";

        [DataMember(Name ="imageUrlPrefix")]
        public string ImageUrlPrefix { get; set; } = "";

        [DataMember(Name ="imagePathFormat")]
        public string ImagePathFormat { get; set; } = "{yyyy}{mm}{dd}-{hh}{ii}{ss}-{rand:3}";
        #endregion

        #region --- 涂鸦图片上传配置项 ---
        [DataMember(Name ="scrawlActionName")]
        public string ScrawlActionName { get; set; } = "uploadscrawl";

        [DataMember(Name ="scrawlFieldName")]
        public string ScrawlFieldName { get; set; } = "upfile";

        [DataMember(Name ="scrawlPathFormat")]
        public string ScrawlPathFormat { get; set; } = "{yyyy}{mm}{dd}-{hh}{ii}{ss}-{rand:3}";

        [DataMember(Name ="scrawlMaxSize")]
        public int ScrawlMaxSize { get; set; } = 2048000;

        [DataMember(Name ="scrawlUrlPrefix")]
        public string ScrawlUrlPrefix { get; set; } = "";

        [DataMember(Name ="scrawlInsertAlign")]
        public string ScrawlInsertAlign { get; set; } = "none";
        #endregion

        #region --- 截图工具上传 ---
        [DataMember(Name ="snapscreenActionName")]
        public string SnapscreenActionName { get; set; } = "uploadimage";
        [DataMember(Name ="snapscreenPathFormat")]
        public string SnapscreenPathFormat { get; set; } = "{yyyy}{mm}{dd}-{hh}{ii}{ss}-{rand:3}";
        [DataMember(Name ="snapscreenUrlPrefix")]
        public string SnapscreenUrlPrefix { get; set; } = "";
        [DataMember(Name ="snapscreenInsertAlign")]
        public string SnapscreenInsertAlign { get; set; } = "none";
        #endregion

        #region --- 抓取远程图片配置 ---
        [DataMember(Name ="catcherLocalDomain")]
        public List<string> CatcherLocalDomain { get; set; } = new List<string> { "127.0.0.1", "localhost"};
        [DataMember(Name ="catcherActionName")]
        public string CatcherActionName { get; set; } = "catchimage";

        [DataMember(Name ="catcherFieldName")]
        public string CatcherFieldName { get; set; } = "source";
        [DataMember(Name ="catcherPathFormat")]
        public string CatcherPathFormat { get; set; } = "{yyyy}{mm}{dd}-{hh}{ii}{ss}-{rand:3}";

        [DataMember(Name ="catcherUrlPrefix")]
        public string CatcherUrlPrefix { get; set; } = "";
        [DataMember(Name ="catcherMaxSize")]
        public int CatcherMaxSize { get; set; } = 2048000;
        [DataMember(Name ="catcherAllowFiles")]
        public List<string> CatcherAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        #endregion

        #region --- 上传视频配置 ---
        [DataMember(Name ="videoActionName")]
        public string VideoActionName { get; set; } = "uploadvideo";
        [DataMember(Name ="videoFieldName")]
        public string VideoFieldName { get; set; } = "upfile";
        [DataMember(Name ="videoPathFormat")]
        public string VideoPathFormat { get; set; } = "upload/video/{yyyy}{mm}{dd}-{time}{rand:6}";
        [DataMember(Name ="videoUrlPrefix")]
        public string VideoUrlPrefix { get; set; } = "";
        [DataMember(Name ="videoMaxSize")]
        public int VideoMaxSize { get; set; } = 102400000;
        [DataMember(Name ="videoAllowFiles")]
        public List<string> VideoAllowFiles { get; set; } = new List<string> {".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid" };
        #endregion

        #region --- 上传文件配置 ---
        [DataMember(Name ="fileActionName")]
        public string FileActionName { get; set; } = "uploadfile";
        [DataMember(Name ="fileFieldName")]
        public string FileFieldName { get; set; } = "upfile";
        [DataMember(Name ="filePathFormat")]
        public string FilePathFormat { get; set; } = "upload/file/{yyyy}{mm}{dd}-{time}{rand:6}";
        [DataMember(Name ="fileUrlPrefix")]
        public string FileUrlPrefix { get; set; } = "";
        [DataMember(Name ="fileMaxSize")]
        public int FileMaxSize { get; set; } = 51200000;
        [DataMember(Name ="fileAllowFiles")]
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
        [DataMember(Name ="imageManagerActionName")]
        public string ImageManagerActionName { get; set; } = "listimage";
        [DataMember(Name ="imageManagerListPath")]
        public string ImageManagerListPath { get; set; } = "upload/image";
        [DataMember(Name ="imageManagerListSize")]
        public int ImageManagerListSize { get; set; } = 20;
        [DataMember(Name ="imageManagerUrlPrefix")]
        public string ImageManagerUrlPrefix { get; set; } = "";
        [DataMember(Name ="imageManagerInsertAlign")]
        public string ImageManagerInsertAlign { get; set; } = "none";
        [DataMember(Name ="imageManagerAllowFiles")]
        public List<string> ImageManagerAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        #endregion

        #region --- 列出指定目录下的文件 ---
        [DataMember(Name ="fileManagerActionName")]
        public string FileManagerActionName { get; set; } = "listfile";
        [DataMember(Name ="fileManagerListPath")]
        public string FileManagerListPath { get; set; } = "upload/file";
        [DataMember(Name ="fileManagerUrlPrefix")]
        public string FileManagerUrlPrefix { get; set; } = "";
        [DataMember(Name ="fileManagerListSize")]
        public int FileManagerListSize { get; set; } = 20;
        [DataMember(Name ="fileManagerAllowFiles")]
        public List<string> FileManagerAllowFiles { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp",
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
        ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
        ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"}; 
        #endregion
    }
}
