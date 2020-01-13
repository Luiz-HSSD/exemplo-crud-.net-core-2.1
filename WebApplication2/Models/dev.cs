using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class dev
    {
        public IList<Fruit> YourCustomSearchFunc(WebApplication2Context _context,DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                return new List<Fruit>();
            }
            return result;
        }
        public List<Fruit> GetDataFromDbase(WebApplication2Context _context, string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            // the example datatable used is not supporting multi column ordering
            // so we only need get the column order from the first column passed to us.        
            var whereClause = BuildDynamicWhereClause(_context, searchBy);

            if (String.IsNullOrEmpty(searchBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "Id";
                sortDir = true;
            }

            var result = _context.Fruit
                           .AsExpandable()
                           .Where(whereClause)
                           .Select(m => new Fruit
                           {
                               id = m.id,
                               Nome = m.Nome


                           })
                           .OrderBy(sortBy) // have to give a default order when skipping .. so use the PK
                           .Skip(skip)
                           .Take(take)
                           .ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            filteredResultsCount = _context.Fruit.AsExpandable().Where(whereClause).Count();
            totalResultsCount = _context.Fruit.Count();

            return result;
        }

        private Expression<Func<Fruit, bool>> BuildDynamicWhereClause(WebApplication2Context entities, string searchValue)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<Fruit>(true); // true -where(true) return all
            if (String.IsNullOrWhiteSpace(searchValue) == false)
            {
                // as we only have 2 cols allow the user type in name 'firstname lastname' then use the list to search the first and last name of dbase
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());

                predicate = predicate.Or(s => searchTerms.Any(srch => s.id.ToString().ToLower().Contains(srch)));
                predicate = predicate.Or(s => searchTerms.Any(srch => s.Nome.ToLower().Contains(srch)));
            }
            return predicate;
        }
    }
}
