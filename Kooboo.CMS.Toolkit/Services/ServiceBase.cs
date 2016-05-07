using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Collections.Specialized;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Toolkit.Services
{
    public class ServiceBase<TEntity>
        where TEntity : TextContent, new()
    {
        public ServiceBase()
            : this(Repository.Current)
        { }

        public ServiceBase(Repository repository)
        {
            _repository = repository;
        }

        private Repository _repository;
        public Repository Repository
        {
            get
            {
                return _repository;
            }
        }

        public ITextContentProvider TextContentProvider
        {
            get
            {
                return Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>();
            }
        }

        #region Folder

        private string _folderName;
        public virtual string FolderName
        {
            get
            {
                if (String.IsNullOrEmpty(_folderName))
                {
                    _folderName = typeof(TEntity).Name;
                }
                return _folderName;
            }
        }

        private TextFolder _folder;
        public TextFolder Folder
        {
            get
            {
                if (_folder == null)
                {
                    _folder = new TextFolder(Repository, FolderName).AsActual();
                    if (_folder == null)
                    {
                        throw new ArgumentNullException(String.Format("Folder \"{0}\" not found", FolderName));
                    }
                }

                return _folder;
            }
        }

        private string _schemaName;
        public virtual string SchemaName
        {
            get
            {
                if (String.IsNullOrEmpty(_schemaName))
                {
                    _schemaName = Folder.SchemaName;
                    if (String.IsNullOrEmpty(_schemaName))
                    {
                        _schemaName = typeof(TEntity).Name;
                    }
                }
                return _schemaName;
            }
        }

        private Schema _schema;
        public Schema Schema
        {
            get
            {
                if (_schema == null)
                {
                    _schema = new Schema(Repository, SchemaName);
                    if (_schema == null)
                    {
                        throw new ArgumentNullException(String.Format("Schema \"{0}\" not found", SchemaName));
                    }
                }

                return _schema;
            }
        }

        #endregion

        #region Query

        public virtual IContentQuery<TextContent> CreateQuery()
        {
            return Folder.CreateQuery();
        }

        public virtual TEntity Get(TextContent textContent)
        {
            if (textContent != null)
            {
                return Activator.CreateInstance(typeof(TEntity), textContent) as TEntity;
            }
            return null;
        }

        public TEntity Get(ContentBase content)
        {
            if (content != null)
            {
                var textContent = new TextContent(content);
                return Get(textContent);
            }

            return null;
        }

        /// <summary>
        /// Get entity (Exclude drafts)
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public TEntity Get(string uuid)
        {
            return Get(uuid, false);
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="includeDrafts"></param>
        /// <returns></returns>
        public TEntity Get(string uuid, bool includeDrafts)
        {
            return Get(SystemFieldNames.UUID, uuid, includeDrafts);
        }

        /// <summary>
        /// Get entity (Exclude drafts)
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public TEntity Get(string fieldName, object fieldValue)
        {
            return Get(fieldName, fieldValue, false);
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="includeDrafts"></param>
        /// <returns></returns>
        public TEntity Get(string fieldName, object fieldValue, bool includeDrafts)
        {
            var query = CreateQuery().WhereEquals(fieldName, fieldValue);
            if (!includeDrafts)
            {
                query = query.Published();
            }

            var textContent = query.FirstOrDefault();
            if (textContent != null)
            {
                return Get(textContent);
            }

            return null;
        }

        /// <summary>
        /// Get all (Exclude drafts order by folder order setting)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return GetAll(false);
        }

        /// <summary>
        /// Get all (Order by folder order setting)
        /// </summary>
        /// <param name="includeDrafts"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(bool includeDrafts)
        {
            return GetAll(includeDrafts, Folder.OrderSetting.FieldName, Folder.OrderSetting.Direction);
        }

        /// <summary>
        /// Get all (Exclude drafts)
        /// </summary>
        /// <param name="orderField"></param>
        /// <param name="orderDirection"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(string orderField, OrderDirection orderDirection)
        {
            return GetAll(false, orderField, orderDirection);
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="orderField"></param>
        /// <param name="orderDirection"></param>
        /// <param name="includeDrafts"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(bool includeDrafts, string orderField, OrderDirection orderDirection)
        {

            var query = CreateQuery().WhereEquals(SystemFieldNames.ParentUUID, null);
            if (!includeDrafts)
            {
                query = query.Published();
            }

            if (orderDirection == OrderDirection.Ascending)
            {
                query = query.OrderBy(orderField);
            }
            else
            {
                query = query.OrderByDescending(orderField);
            }

            return query.Select(textContent => Get(textContent));
        }

        #endregion

        #region Add

        public virtual TEntity Add(TEntity entity)
        {
            return Add(entity, Folder);
        }

        public virtual TEntity Add(TEntity entity, TextFolder folder)
        {
            entity.Repository = Repository.Name;
            entity.FolderName = folder.FullName;
            entity.SchemaName = folder.SchemaName;

            ContentEvent.Fire(ContentAction.PreAdd, entity);
            TextContentProvider.Add(entity);
            ContentEvent.Fire(ContentAction.Add, entity);

            return entity;
        }

        public virtual TEntity Add(TEntity entity, string categoryFolderName, params string[] categoryUUIDs)
        {
            return Add(entity, Folder, categoryFolderName, categoryUUIDs);
        }

        public virtual TEntity Add(TEntity entity, TextFolder folder, string categoryFolderName, params string[] categoryUUIDs)
        {
            var categoryFolder = new TextFolder(Repository, categoryFolderName).AsActual();
            var categoryQuery = categoryFolder.CreateQuery();
            IEnumerable<TextContent> categories = null;
            foreach (var categoryUUID in categoryUUIDs)
            {
                categories = categoryQuery.WhereIn(SystemFieldNames.UUID, categoryUUIDs);
            }

            return Add(entity, folder, categories);
        }

        public virtual TEntity Add(TEntity entity, IEnumerable<TextContent> categories)
        {
            return Add(entity, Folder, categories);
        }

        public virtual TEntity Add(TEntity entity, TextFolder folder, IEnumerable<TextContent> categories)
        {
            Add(entity, folder);
            AddCategories(entity, categories.ToArray());

            return entity;
        }

        public virtual TEntity AddSubContent(TEntity entity, string parentFolderName, string parentUUID)
        {
            return AddSubContent(entity, Folder, parentFolderName, parentUUID);
        }

        public virtual TEntity AddSubContent(TEntity entity, TextFolder folder, string parentFolderName, string parentUUID)
        {
            entity.Repository = Repository.Name;
            entity.FolderName = folder.FullName;
            entity.SchemaName = folder.SchemaName;
            entity.ParentFolder = parentFolderName;
            entity.ParentUUID = parentUUID;

            ContentEvent.Fire(ContentAction.PreAdd, entity);
            TextContentProvider.Add(entity);
            ContentEvent.Fire(ContentAction.Add, entity);

            return entity;
        }

        public virtual void AddCategories(TEntity entity, params TextContent[] categories)
        {
            TextContentProvider.AddCategories(entity, categories.Select(it => new Category()
            {
                CategoryFolder = it.FolderName,
                CategoryUUID = it.UUID,
                ContentUUID = entity.UUID
            }).ToArray());
        }

        #endregion

        #region Update

        public virtual TEntity Update(TEntity entity)
        {
            return Update(entity, Folder);
        }

        public virtual TEntity Update(TEntity entity, TextFolder folder)
        {
            entity.Repository = Repository.Name;
            entity.FolderName = folder.FullName;
            entity.SchemaName = folder.SchemaName;
            ContentEvent.Fire(ContentAction.PreUpdate, entity);
            TextContentProvider.Update(entity, entity);
            ContentEvent.Fire(ContentAction.Update, entity);

            return entity;
        }

        #endregion

        #region Remove

        public virtual void Remove(string uuid)
        {
            Remove(uuid, Folder);
        }

        public virtual void Remove(string uuid, TextFolder folder)
        {
            ServiceFactory.TextContentManager.Delete(
                  Repository,
                  folder,
                  uuid);
        }

        #endregion

        #region get by same dbkey
        public TEntity GetAllSameDbKeyItems(Repository targetRepository, TEntity sourceContent)
        {
            if (sourceContent != null)
            {
                TEntity destDBContent = null;

                var destFolder = new TextFolder(targetRepository, sourceContent.FolderName);

                string dbKey = sourceContent.GetString(SystemFieldNames.DBKey);
                if (!String.IsNullOrEmpty(dbKey)) // Try to find content by DBKey
                {
                    destDBContent = destFolder.CreateQuery().WhereEquals(SystemFieldNames.DBKey, dbKey).FirstOrDefault().MapTo<TEntity>();
                }

                if (destDBContent == null) // Try to find content by OrginalUUID
                {
                    string orginalUUID = sourceContent.OriginalUUID;
                    if (!String.IsNullOrEmpty(orginalUUID))
                    {
                        destDBContent = destFolder.CreateQuery().WhereEquals(SystemFieldNames.OriginalUUID, orginalUUID).FirstOrDefault().MapTo<TEntity>();
                    }
                }

                if (destDBContent == null)
                {
                    destDBContent = destFolder.CreateQuery().WhereEquals(SystemFieldNames.UUID, sourceContent.UUID).FirstOrDefault().MapTo<TEntity>();
                }

                if (destDBContent == null)
                {
                    destDBContent = destFolder.CreateQuery().WhereEquals(SystemFieldNames.UserKey, sourceContent.UserKey).FirstOrDefault().MapTo<TEntity>();
                }

                return destDBContent;
            }
            return null;

        }
        public TEntity GetAllSameDbKeyItemsByUserKey(Repository targetRepository, string sourceUserKey)
        {
            return GetAllSameDbKeyItems(targetRepository, this.Folder.CreateQuery().WhereEquals(SystemFieldNames.UserKey, sourceUserKey).FirstOrDefault().MapTo<TEntity>());
        }
        #endregion
    }
}