using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Specialized;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Toolkit
{
    public static class TextContentExtensions
    {
        #region Query


        /// <summary>
        /// create express like: ((a=1) or (a=2) or (b=3))
        /// </summary>
        public static IContentQuery<TextContent> WhereOrs(this IContentQuery<TextContent> textContents, Dictionary<string, object> args)
        {
            var textContent = textContents.FirstOrDefault();
            Repository repository = textContent == null ? Repository.Current : new Repository(textContent.Repository);
            var folder = new TextFolder(repository, textContent.FolderName);
            folder = folder.AsActual();
            var query = folder.CreateQuery();
            foreach (var arg in args)
            {
                query = query.Or(new WhereContainsExpression(null, arg.Key, arg.Value));
            }
            return textContents.Where((IWhereExpression)query.Expression);
        }


        public static IContentQuery<TextContent> WhereCategory(this IContentQuery<TextContent> textContents, string categoryFolderName, string categoryUUID)
        {
            return WhereCategory(textContents, categoryFolderName, new string[] { categoryUUID });
        }

        public static IContentQuery<TextContent> WhereCategory(this IContentQuery<TextContent> textContents, string categoryFolderName, string[] categoryUUIDs)
        {
            var textContent = textContents.FirstOrDefault();
            Repository repository = null;
            if (textContent != null)
            {
                repository = new Repository(textContent.Repository);
            }
            else
            {
                repository = Repository.Current;
            }

            var categoryFolder = new TextFolder(repository, categoryFolderName);
            var categoryQuery = categoryFolder.CreateQuery().WhereIn(SystemFieldNames.UUID, categoryUUIDs);
            return textContents.WhereCategory(categoryQuery);
        }

        /// <summary>
        /// Instead of Categories API
        /// </summary>
        /// <param name="textContent"></param>
        /// <param name="categoryFolderName"></param>
        /// <returns></returns>
        public static IContentQuery<TextContent> QueryCategories(this TextContent textContent, string categoryFolderName)
        {
            Repository repository = null;
            if (textContent != null)
            {
                repository = new Repository(textContent.Repository);
            }
            else
            {
                repository = Repository.Current;
            }
            var categoryFolder = new TextFolder(repository, categoryFolderName);
            var categorySchema = categoryFolder.GetSchema();

            var categories = Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().QueryCategories(textContent);
            var categoryUUIDs = categories
                .Where(it => it.CategoryFolder.StartsWith(categoryFolder.FullName, StringComparison.OrdinalIgnoreCase))
                .Select(it => it.CategoryUUID)
                .ToArray();

            return categorySchema.CreateQuery()
                .WhereIn(SystemFieldNames.UUID, categoryUUIDs)
                .DefaultOrder(categoryFolder);
        }

        public static IContentQuery<TextContent> Published(this IContentQuery<TextContent> textContents)
        {
            return textContents.WhereEquals(SystemFieldNames.Published, true);
        }

        [Obsolete("Not support in Kooboo 4,this method is same with  public static IContentQuery<TextContent> DefaultOrder(this IContentQuery<TextContent> contentQuery)")]
        public static IContentQuery<TextContent> DefaultOrder(this IContentQuery<TextContent> textContents, TextFolder textFolder)
        {
            //if (textContents is TextContentQueryBase)
            //{
            //    string orderField = SystemFieldNames.UtcCreationDate;
            //    OrderDirection orderDirection = OrderDirection.Descending;

            //    if (textFolder.Sortable.HasValue && textFolder.Sortable.Value)
            //    {
            //        orderField = "Sequence";
            //        orderDirection = OrderDirection.Descending;
            //    }

            //    if (orderDirection == OrderDirection.Ascending)
            //    {
            //        return textContents.OrderBy(orderField);
            //    }
            //    else
            //    {
            //        return textContents.OrderByDescending(orderField);
            //    }
            //}
            //return textContents;
            throw new NotImplementedException("Not support in Kooboo 4");
        }

        public static IContentQuery<TextContent> DefaultOrder(this IContentQuery<TextContent> textContents, string orderField, OrderDirection orderDirection = OrderDirection.Descending)
        {
            if (textContents is TextContentQueryBase)
            {
                var textFolder = ((TextContentQueryBase)textContents).Folder.AsActual();
                if (textFolder.Sortable.HasValue && textFolder.Sortable.Value)
                {
                    orderField = "Sequence";
                    orderDirection = OrderDirection.Descending;
                }

                if (orderDirection == OrderDirection.Ascending)
                {
                    return textContents.OrderBy(orderField);
                }
                else
                {
                    return textContents.OrderByDescending(orderField);
                }
            }
            return textContents;
        }

        #region Date

        /// <summary>
        /// WhereEquals date
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="date">Local date</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereEqualsDate(this IContentQuery<TextContent> textContents, string fieldName, DateTime date)
        {
            return textContents.WhereEquals(fieldName, date.ToShortDateString().AsDateTime().ToUniversalTime());
        }

        /// <summary>
        /// WhereGreaterThanOrEqual date
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="date">Local date</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereGreaterThanOrEqualDate(this IContentQuery<TextContent> textContents, string fieldName, DateTime date)
        {
            return textContents.WhereGreaterThanOrEqual(fieldName, date.ToShortDateString().AsDateTime().ToUniversalTime());
        }

        /// <summary>
        /// WhereGreaterThan date
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="date">Local date</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereGreaterThanDate(this IContentQuery<TextContent> textContents, string fieldName, DateTime date)
        {
            return textContents.WhereGreaterThan(fieldName, date.ToShortDateString().AsDateTime().ToUniversalTime());
        }

        /// <summary>
        /// WhereLessThanOrEqual date
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="date">Local date</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereLessThanOrEqualDate(this IContentQuery<TextContent> textContents, string fieldName, DateTime date)
        {
            return textContents.WhereLessThanOrEqual(fieldName, date.ToShortDateString().AsDateTime().ToUniversalTime());
        }

        /// <summary>
        /// WhereLessThan date
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="date">Local date</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereLessThanDate(this IContentQuery<TextContent> textContents, string fieldName, DateTime date)
        {
            return textContents.WhereLessThan(fieldName, date.ToShortDateString().AsDateTime().ToUniversalTime());
        }

        #endregion

        #region DateTime

        /// <summary>
        /// WhereGreaterThanOrEqualDateTime dateTime
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime">Local dateTime</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereGreaterThanOrEqualDateTime(this IContentQuery<TextContent> textContents, string fieldName, DateTime dateTime)
        {
            return textContents.WhereGreaterThanOrEqual(fieldName, dateTime.ToUniversalTime());
        }

        /// <summary>
        /// WhereGreaterThanDateTime dateTime
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime">Local dateTime</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereGreaterThanDateTime(this IContentQuery<TextContent> textContents, string fieldName, DateTime dateTime)
        {
            return textContents.WhereGreaterThan(fieldName, dateTime.ToUniversalTime());
        }

        /// <summary>
        /// WhereLessThanOrEqualDateTime dateTime
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime">Local dateTime</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereLessThanOrEqualDateTime(this IContentQuery<TextContent> textContents, string fieldName, DateTime dateTime)
        {
            return textContents.WhereLessThanOrEqual(fieldName, dateTime.ToUniversalTime());
        }

        /// <summary>
        /// WhereLessThanDateTime dateTime
        /// </summary>
        /// <param name="textContents"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime">Local dateTime</param>
        /// <returns></returns>
        public static IContentQuery<TextContent> WhereLessThanDateTime(this IContentQuery<TextContent> textContents, string fieldName, DateTime dateTime)
        {
            return textContents.WhereLessThan(fieldName, dateTime.ToUniversalTime());
        }

        #endregion

        #endregion

        #region ToNameValueCollection

        public static NameValueCollection ToNameValueCollection(this TextContent textContent)
        {
            NameValueCollection collection = new NameValueCollection();
            foreach (var item in textContent)
            {
                if (item.Value != null)
                {
                    collection.Add(item.Key, item.Value.ToString());
                }
                else
                {
                    collection.Add(item.Key, String.Empty);
                }
            }

            return collection;
        }

        #endregion

        #region MapTo

        public static TEntity MapTo<TEntity>(this TextContent textContent)
        {
            if (textContent != null)
            {
                return (TEntity)Activator.CreateInstance(typeof(TEntity), textContent);
            }

            return default(TEntity);
        }

        public static IEnumerable<TEntity> MapTo<TEntity>(this IEnumerable<TextContent> textContents)
        {
            return textContents.Select(it => MapTo<TEntity>(it));
        }

        public static TEntity MapTo<TEntity>(this ContentBase content)
        {
            if (content != null)
            {
                return (TEntity)Activator.CreateInstance(typeof(TEntity), content);
            }

            return default(TEntity);
        }

        public static IEnumerable<TEntity> MapTo<TEntity>(this IEnumerable<ContentBase> contents)
        {
            return contents.Select(it => MapTo<TEntity>(it));
        }

        #endregion
    }
}