using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boomers.Utilities.Linq
{
    /// <summary>
    /// Summary description for LinqCache
    /// http://code.msdn.microsoft.com/linqtosqlcache
    /// </summary>
    public static class LinqCache
    {
        /// <summary>
        /// Caches Linq query´s that is created for LinqToSql.
        /// Limitations are the same as SqlCacheDependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">The linq query</param>
        /// <param name="dc">Your LinqToSql DataContext</param>
        /// <param name="CacheId">The unique Id for the cache</param>
        /// <returns></returns>
        public static List<T> CacheSql<T>(this System.Linq.IQueryable<T> q, System.Data.Linq.DataContext dc, string CacheId)
        {
            try
            {
                List<T> objCache = (List<T>)System.Web.HttpRuntime.Cache.Get(CacheId);

                if (objCache == null)
                {
                    /////////No cache... implement new SqlCacheDependeny//////////
                    //1. Get connstring from DataContext
                    string connStr = dc.Connection.ConnectionString;
                    //2. Get SqlCommand from DataContext and the LinqQuery
                    string sqlCmd = dc.GetCommand(q).CommandText;
                    //3. Create Conn to use in SqlCacheDependency
                    using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr))
                    {
                        conn.Open();
                        //4. Create Command to use in SqlCacheDependency
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sqlCmd, conn))
                        {
                            //5.0 Add all parameters provided by the Linq Query
                            foreach (System.Data.Common.DbParameter dbp in dc.GetCommand(q).Parameters)
                            {
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(dbp.ParameterName, dbp.Value));
                            }
                            //5.1 Enable DB for Notifications... Only needed once per DB...
                            System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connStr);
                            //5.2 Get ElementType for the query
                            string NotificationTable = q.ElementType.Name;
                            //5.3 Enable the elementtype for notification (if not done!)
                            if (!System.Web.Caching.SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(connStr).Contains(NotificationTable))
                                System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connStr, NotificationTable);
                            //6. Create SqlCacheDependency
                            System.Web.Caching.SqlCacheDependency sqldep = new System.Web.Caching.SqlCacheDependency(cmd);
                            // - removed 090506 - 7. Refresh the LinqQuery from DB so that we will not use the current Linq cache
                            // - removed 090506 - dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, q);
                            //8. Execute SqlCacheDepency query...
                            cmd.ExecuteNonQuery();
                            //9. Execute LINQ-query to have something to cache...
                            objCache = q.ToList();
                            //10. Cache the result but use the already created objectCache. Or else the Linq-query will be executed once more...
                            System.Web.HttpRuntime.Cache.Insert(CacheId, objCache, sqldep);
                        }
                    }
                }
                //Return the created (or cached) List
                return objCache;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}