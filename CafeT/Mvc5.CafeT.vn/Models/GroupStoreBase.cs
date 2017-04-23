using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc5.CafeT.vn.Models
{
    public class GroupStoreBase
    {
        public DbContext Context
        {
            get;
            private set;
        }


        public DbSet<ApplicationGroup> DbEntitySet
        {
            get;
            private set;
        }


        public IQueryable<ApplicationGroup> EntitySet
        {
            get
            {
                return this.DbEntitySet;
            }
        }


        public GroupStoreBase(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<ApplicationGroup>();
        }


        public void Create(ApplicationGroup entity)
        {
            this.DbEntitySet.Add(entity);
        }


        public void Delete(ApplicationGroup entity)
        {
            this.DbEntitySet.Remove(entity);
        }


        public virtual Task<ApplicationGroup> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[] { id });
        }


        public virtual ApplicationGroup GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }


        public virtual void Update(ApplicationGroup entity)
        {
            if (entity != null)
            {
                this.Context.Entry<ApplicationGroup>(entity).State = EntityState.Modified;
            }
        }
    }
}
