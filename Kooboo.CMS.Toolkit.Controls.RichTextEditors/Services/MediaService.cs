using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Web;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.IO;
using System.Web;
using System.Net;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Services
{
    public interface IMediaService
    {
        FileListViewModel GetList(
            string repositoryName,
            string siteName,
            string folderName,
            string search,
            int skip = 0,
            int size = 20);

        UEditorConfig GetConfig(string repositoryName,
            string siteName,
            string folderName);

        UploadResultViewModel Upload(Repository repository,
            string siteName,
            MediaFolder folder,
            string name,
            Stream stream);

        CrawlerViewModel Crawler(Repository repository,
            string siteName,
            MediaFolder folder,
            string[] source,
            string catcherPathFormat);
    }

    [Dependency(typeof(IMediaService))]
    public class MediaService : IMediaService
    {
        private readonly MediaContentManager _mediaContentManager;
        private readonly MediaFolderManager _mediaFolderManager;
        public MediaService(MediaContentManager mediaContentManager, MediaFolderManager mediaFolderManager)
        {
            _mediaContentManager = mediaContentManager;
            _mediaFolderManager = mediaFolderManager;
        }

        public FileListViewModel GetList(
            string repositoryName,
            string siteName,
            string folderName,
            string search,
            int skip = 0,
            int size = 20)
        {
            if (size < 1)
            {
                size = 20;
            }
            var repository = new Repository(repositoryName).AsActual();
            var currentFolder = _mediaFolderManager.Get(repository, folderName);
            if (currentFolder == null)
            {
                currentFolder = new MediaFolder(repository, folderName);
                _mediaFolderManager.Add(repository, currentFolder);
            }
            var contentQuery = currentFolder.CreateQuery();
            if (!string.IsNullOrEmpty(search))
            {
                IWhereExpression expression = new WhereContainsExpression(null, "FileName", search);
                expression = new OrElseExpression(expression, new WhereContainsExpression(null, "Metadata.AlternateText", search));
                expression = new OrElseExpression(expression, new WhereContainsExpression(null, "Metadata.Description", search));
                contentQuery = contentQuery
                    .Where(expression);
            }
            var page = skip / size;
            contentQuery = contentQuery.SortBy("UtcCreationDate", "desc");
            var pagedList = contentQuery.ToPageList(page, size);
            return new FileListViewModel
            {
                total = pagedList.TotalItemCount,
                start = skip,
                list = pagedList.Select(it => new FileViewModel
                {
                    url = it.Url,
                    mtime = DateTime.UtcNow.Ticks
                }),
                state = "SUCCESS"
            };
        }

        public UEditorConfig GetConfig(
            string repositoryName,
            string siteName,
            string folderName)
        {
            var config = new UEditorConfig();

            return config;
        }

        public UploadResultViewModel Upload(Repository repository,
            string siteName,
            MediaFolder folder,
            string name,
            Stream stream)
        {
            var result = new UploadResultViewModel();
            try
            {
                stream.Position = 0;
                var content = _mediaContentManager.Add(repository, folder, name, stream, true);
                result.state = "SUCCESS";
                result.url = content.Url;
                result.title = content.FileName;
                result.original = content.FileName;
            }
            catch (Exception ex)
            {
                result.state = ex.Message;
                result.error = ex.Message;
            }
            return result;
        }

        public CrawlerViewModel Crawler(
            Repository repository,
            string siteName,
            MediaFolder folder,
            string[] source,
            string catcherPathFormat)
        {
            var result = new CrawlerViewModel
            {
                state = "SUCCESS"
            };
            if (source == null || source.Length == 0)
            {
                result.state = "参数错误：没有指定抓取源";
                return result;
            }
            result.list = source.Select(item =>
            {
                var crawler = new CrawlerItemViewModel();
                if (!IsExternalIPAddress(item))
                {
                    crawler.state = "INVALID_URL";
                    return crawler;
                }
                crawler.source = item;
                var request = HttpWebRequest.Create(item) as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        crawler.state = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                        return crawler;
                    }
                    if (response.ContentType.IndexOf("image") == -1)
                    {
                        crawler.state = "Url is not an image";
                        return crawler;
                    }
                    try
                    {
                        var name = Path.GetFileName(item);
                        if (!Path.HasExtension(item))
                        {
                            name += ".jpg";
                        }
                        var fileName = PathFormatter.Format(name, catcherPathFormat);
                        var stream = response.GetResponseStream();
                        var reader = new BinaryReader(stream);
                        using (var ms = new MemoryStream())
                        {
                            byte[] buffer = new byte[4096];
                            int count;
                            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, count);
                            }
                            ms.Flush();
                            var res = Upload(repository, siteName, folder, fileName, ms);
                            crawler.url = res.url;
                        }

                        crawler.state = "SUCCESS";
                    }
                    catch (Exception e)
                    {
                        crawler.state = "抓取错误：" + e.Message;
                    }
                    return crawler;
                }
                return crawler;
            });

            return result;
        }

        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
