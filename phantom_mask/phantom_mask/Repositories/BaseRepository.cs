using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using phantom_mask.Data;
using phantom_mask.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace phantom_mask.Repositories
{

    public interface IBaseRepository<T> where T : class
    {
        public ReplyCode Update(T obj);
        public ReplyCode Save();        // this function probably isn't needed, b/c SaveChanges is in Context, not the specific table
        public IQueryable<T> GetAll();
        public IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] include = null);
        //public ReplyCode Add(T obj);
        public T GetByID(int id);
        //public List<T> Paging(List<T> model, PadingModel paging);
        public string ConcatenatedString(string[] Params);
    }

    //public class Reply
    //{
    //    public Reply(int result = 0)
    //    {
    //        r = result;
    //    }

    //    public Reply(ReplyCode code)
    //    {
    //        r = (int)code;
    //    }

    //    [JsonProperty(Order = -10)]
    //    public int r { get; set; }

    //    public static Reply New()
    //    {
    //        return new Reply(ReplyCode.SUCCESS);
    //    }

    //    public static Reply New(ReplyCode code, string langID = "en")
    //    {
    //        if (code == ReplyCode.SUCCESS)
    //            return new Reply(code);
    //        else
    //            return new ErrorReply(code);
    //    }
    //}

    public enum ReplyCode : int
    {
        [Display(Name = "SUCCESS")]
        SUCCESS = 0,

        [Display(Name = "BAD_TOKEN")]
        BAD_TOKEN = 1,

        [Display(Name = "BAD_USER")]
        BAD_USER = 2,

        [Display(Name = "USER_EXISTS")]
        USER_EXISTS = 3,

        [Display(Name = "PHONE_NOT_FOUND")]
        PHONE_NOT_FOUND = 4,        // phone # not found

        [Display(Name = "USER_CAN_RESET")]
        USER_CAN_RESET = 5,

        // login issues

        [Display(Name = "BAD_TFA_STATUS")]
        BAD_TFA_STATUS = 20,       // bad TFA mode

        [Display(Name = "BAD_TFA_CODE")]
        BAD_TFA_CODE = 21,       // bad SMS code

        [Display(Name = "BAD_TFA_EXPIRED")]
        BAD_TFA_EXPIRED = 22,       // TFA code expired already; must send new one   

        [Display(Name = "BAD_TFA_RESEND_WAIT")]
        BAD_TFA_RESEND_WAIT = 23,       // Resend time hasn't expired yet.

        [Display(Name = "BAD_TFA_RETRY_EXCEEDED")]
        BAD_TFA_RETRY_EXCEEDED = 24,       // Can't retry until a new code is sent

        [Display(Name = "BAD_LOGIN_RETRY_WAIT")]
        BAD_LOGIN_RETRY_WAIT = 25,       // can't login until timer expires

        [Display(Name = "BAD_LOGIN")]
        BAD_LOGIN = 30,        // bad user/password combination

        [Display(Name = "USER_DISABLED")]
        USER_DISABLED = 31,

        // formatting issues

        [Display(Name = "BAD_PHONE")]
        BAD_PHONE = 40,       // bad phone number format

        [Display(Name = "BAD_EMAIL")]
        BAD_EMAIL = 41,       // bad email format

        [Display(Name = "BAD_GENDER")]
        BAD_GENDER = 42,       // bad gender value

        [Display(Name = "BAD_IP")]
        BAD_IP = 43,

        [Display(Name = "SMS_FAILED")]
        SMS_FAILED = 80,


        // DB issues
        [Display(Name = "DB_Error")]
        DB_Error = 50,
        [Display(Name = "DB_NotFound")]
        DB_NotFound = 51,

        // Object issues
        [Display(Name = "Object_Null")]
        Object_Null = 60,
        [Display(Name = "unknown")]
        unknown = 70
    }

    /// <summary>
    /// Base class for basic repository functionality
    /// </summary>
    /// <typeparam name="T">Data model that this repository is working with.</typeparam>
    /// <remarks>
    /// Because EF Core does not support table per concrete (TPC), we won't be able to implement
    /// generic versions of the CRUD functions in this class right now.
    /// </remarks>
    public abstract class BaseRepository<T> where T : class
    {
        internal readonly DbContext _context;
        internal readonly DbSet<T> _dbSet;

        protected BaseRepository(Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Validates and updates an investor record
        /// </summary>
        /// <param name="obj">An updated record to save.</param>
        /// <remarks>
        /// Must call <c>context.SaveChanges() after success</c>
        /// </remarks>
        public ReplyCode Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            return ReplyCode.SUCCESS;
        }

        /// <summary>
        /// Save changes to database.
        /// </summary>
        public ReplyCode Save()
        {
            _context.SaveChanges();
            return ReplyCode.SUCCESS;
        }

        /// <summary>
        /// Lists records.
        /// </summary>
        /// <returns>
        /// A queryable list of records.
        /// </returns>
        /// <remarks>
        /// Paging can be done on the returned queryable, by using <c>PagedList</c>.
        /// </remarks>
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Performs a get on the table using LINQ filter + orderBy.
        /// 
        /// example:
        /// IQueryable<Users> users = repo.Get( 
        ///                     s => s.firstname = "bob", 
        ///                     q => q.OrderBy( s => s.lastname ), 
        ///                     new string [] { "firstname", "lastname" } );
        /// 
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="orderBy">Additional query to perform on resultant query</param>
        /// <param name="include">Array of properties to include</param>
        /// <returns>A queryable for the given parameters</returns>
        public IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] include = null
            )
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include!=null)
            {
                foreach (var includeProperty in include)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        /// <summary>
        /// Adds a new record
        /// </summary>
        /// <param name="obj">The record to add</param>
        /// <returns>Success if record was added successfully.</returns>
        /// <remarks>
        /// Must call <c>context.SaveChanges() after success</c>
        /// </remarks>
        public virtual ReplyCode Add(T obj)
        {
            _dbSet.Add(obj);
            return ReplyCode.SUCCESS;
        }

        /// <summary>
        /// Returns a record matching the given ID
        /// </summary>
        /// <param name="id">ID to match</param>
        /// <returns>Matching record, or null if record is not found</returns>
        public abstract T GetByID(int id);

        ///// <summary>
        ///// Information is paged
        ///// </summary>
        ///// <param name="model">Which you want pagin model</param>
        ///// <param name="paging">the paging info</param>
        ///// <returns>the model you want with paged</returns>
        //public List<T> Paging(List<T> model, PadingModel paging)
        //{
        //    if (paging != null)
        //    {
        //        //分頁
        //        if (paging.currentPageNumber > 0 && paging.numberOfRecords > 0)
        //        {
        //            model = model
        //                .Skip(Convert.ToInt32(paging.numberOfRecords) * (Convert.ToInt32(paging.currentPageNumber) - 1))
        //                .Take(Convert.ToInt32(paging.numberOfRecords))
        //                .ToList();
        //        }
        //    }
        //    return model;
        //}

        /// <summary>
        /// Concatenated string
        /// </summary>
        /// <param name="Params">the strings you want concatenate</param>
        /// <returns>string</returns>
        public string ConcatenatedString (string[] Params)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                sb.Append(Params[i]);
            }
            string s = sb.ToString();
            sb = null;
            return s;
        }

        // disposable
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
