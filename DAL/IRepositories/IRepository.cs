﻿namespace DAL
{
    //public interface IRepository<T>
    /// <summary>
    /// Version: 2.1 - Date: 1392/04/09
    /// </summary>
    public interface IRepository<T> where T : Models.BaseEntity
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteByList(System.Collections.Generic.List<T> entities);
        T GetById(System.Guid id);
        bool DeleteById(System.Guid id);
        System.Linq.IQueryable<T> Get();
        System.Linq.IQueryable<T> Get
            (System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);

        System.Collections.Generic.IEnumerable<T> GetWithRawSql
            (string query, params object[] parameters);
    }
}
