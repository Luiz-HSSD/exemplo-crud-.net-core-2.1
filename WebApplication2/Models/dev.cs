using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication2.Entities;

namespace WebApplication2.Models
{
    public class dev
    {
        public IList<Fruitdev> YourCustomSearchFunc(WebApplication2Context _context,DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            var result = GetDataFromDbase(_context,searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<Fruitdev>();
            }
            return result;
        }
        public List<Fruitdev> GetDataFromDbase(WebApplication2Context _context, string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            // the example datatable used is not supporting multi column ordering
            // so we only need get the column order from the first column passed to us.        
            var whereClause = BuildDynamicWhereClause(_context, searchBy);

            if (String.IsNullOrEmpty(searchBy) && false)
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "id";
                sortDir = true;
            }

            var result = _context.Getsp(1)
                           .AsExpandable()
                           .Where(whereClause)
                           .Select(m => new Fruitdev
                           {
                               id = m.id,
                               Nome = m.Nome,
                               dev= m.dev

                           })
                           .OrderByAD(sortBy,sortDir) // have to give a default order when skipping .. so use the PK
                           .Skip(skip)
                           .Take(take)
                           .ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            filteredResultsCount = _context.Getsp(1).ToList().AsQueryable().AsExpandable().Where(whereClause).Count();
            totalResultsCount = _context.Getsp(1).ToList().AsQueryable().Count();

            return result;
        }

        private Expression<Func<Fruitdev, bool>> BuildDynamicWhereClause(WebApplication2Context entities, string searchValue)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<Fruitdev>(true); // true -where(true) return all
            if (String.IsNullOrWhiteSpace(searchValue) == false)
            {
                // as we only have 2 cols allow the user type in name 'firstname lastname' then use the list to search the first and last name of dbase
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());

                predicate = predicate.Or(s => s.id.ToString().ToLower().Contains(searchValue));
                predicate = predicate.Or(s => s.Nome.ToLower().Contains(searchValue));//searchValue.Any(srch => s.Nome.ToLower().Contains(srch)));
            }
            return predicate;
        }
    }
}
