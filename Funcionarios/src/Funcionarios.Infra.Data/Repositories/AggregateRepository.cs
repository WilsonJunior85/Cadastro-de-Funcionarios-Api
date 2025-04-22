using Funcionarios.Domain.Models;
using Funcionarios.Infra.Data.Context;
using Funcionarios.Infra.Data.Repositories.Contracts;
using Funcionarios.Infra.Data.Selectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Repositories
{
    public abstract class AggregateRepository<T> : IAggregateRepository<T> where T : Model
    {
        protected readonly ApiContext _apiContext;
        protected readonly DbSet<T> _dbSet;

        public AggregateRepository(ApiContext apiContext)
        {
            _apiContext = apiContext;
            _dbSet = _apiContext.Set<T>();
        }

        public void Dispose() => _apiContext?.Dispose();
        public IUnitOfWork UnitOfWork => _apiContext;
        protected IQueryable<T> Track(IQueryable<T> table, bool track) => !track ? table.AsNoTracking() : table;
        protected IQueryable<TM> Track<TM>(IQueryable<TM> table, bool track) where TM : Model => !track ? table.AsNoTracking() : table;

        protected virtual IQueryable<T> Limit(Selector sel, IQueryable<T> query)
        {
            if (!sel.Limit.Equals(0))
            {
                sel.Page = sel.Page.Equals(0) ? 1 : sel.Page;
                query = query.Skip((sel.Page - 1) * sel.Limit).Take(sel.Limit);
            }
            return query;
        }

        protected virtual IQueryable<T> Order(Selector seletor, IQueryable<T> query)
        {
            if (!string.IsNullOrEmpty(seletor.OrderBy))
            {
                query = query.OrderBy(y => 1);
                string[] fields = seletor.OrderBy.Split(',');
                foreach (string fieldWithOrder in fields)
                {
                    string[] fieldParam = fieldWithOrder.Split(' ');
                    string orderBy = "ThenBy";
                    if (seletor.OrderByOrder.ToUpper().Equals("DESC"))
                    {
                        orderBy = "ThenByDescending";
                    }
                    ParameterExpression x = Expression.Parameter(query.ElementType, "x");
                    LambdaExpression exp = Expression.Lambda(Expression.PropertyOrField(x, fieldParam[0].Trim()), x);
                    query = (IQueryable<T>)query.Provider.CreateQuery(Expression.Call(typeof(Queryable), orderBy,
                        new Type[] { query.ElementType, exp.Body.Type }, query.Expression, exp));
                }
            }

            return query;
        }
    }
}
