// Copyright 2012 Ian Stapleton
//
//Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using GeekcubedUtils.Linq;

namespace GeekcubedUtils
{
    /// <summary>
    /// An extension of the default List object to provide enhancments for rendering data in GUI.
    /// 
    /// PagedList provides support for not only paging results, but also filtering and sorting.
    /// </summary>
    public class PagedList<T> : List<T>
    {
        public const int FirstPage = 1;
        public const int ShowAll = int.MaxValue;
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public SortOption? Sorting {get; private set;}
        public FilterOption? Filtered { get; private set; }

        public PagedList(IQueryable<T> src, int pageIndex, int pageSize, SortOption? sort = null, FilterOption? filter = null)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = src.Count();
            this.TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Sorting = sort;
            Filtered = filter;

            //Apply any sorting
            if (Sorting.HasValue)
            {
                if (Sorting.Value.Direction == SortOrder.ascending)
                {
                    src = src.OrderBy(sort.Value.Column);
                }
                else
                {
                    src = src.OrderByDescending(sort.Value.Column);
                }
            }

            //We work with a none-zero based paging system
            if (pageSize != ShowAll)
            {
                this.AddRange(src.Skip((PageIndex - FirstPage) * PageSize).Take(PageSize));
            }
            else
            {
                this.AddRange(src);
            }
        }

        /// <summary>
        /// Generates a RouteValueDictionary of query parameters for the next page of results in this set.
        /// Propagates any applied Sort and Filter
        /// </summary>
        /// <remarks>Returns an empty Dictionary if there are no more pages. <see cref="HasNextPage"/></remarks>
        /// <returns>Routing dictionary of parameters to link to the next page of results in this set.</returns>        
        public RouteValueDictionary NextPageParams() 
        {
            RouteValueDictionary routeParams = new RouteValueDictionary();

            //Only generate route params if there is a next page
            if (this.HasNextPage)
            {
                //The page entry
                routeParams.Add("page", this.NextPage);

                //Carry forward any sort
                if (this.IsSorted)
                {
                    routeParams.Add("Column", this.Sorting.Value.Column);
                    routeParams.Add("Direction", this.Sorting.Value.Direction);
                }

                //Likewise with any filter
                if (IsFiltered)
                {
                    routeParams.Add(this.Filtered.Value.Param, this.Filtered.Value.Value);
                }
            }
            //else we return an empty dictionary

            return routeParams;
        }

        /// <summary>
        /// Generates a RouteValueDictionary of query parameters for the previous page of results in this set.
        /// Propagates any applied Sort and Filter
        /// </summary>
        /// <remarks>Returns an empty Dictionary if there is no previous page. <see cref="HasPreviousPage"/></remarks>
        /// <returns>Routing dictionary of parameters to link to the previous page of results in this set.</returns>
        public RouteValueDictionary PrevPageParams()
        {
            RouteValueDictionary routeParams = new RouteValueDictionary();

            //Only generate route params if there is a previous page
            if (this.HasPreviousPage)
            {
                //The page entry
                routeParams.Add("page", this.PreviousPage);

                //Carry forward any sort
                if (this.IsSorted)
                {
                    routeParams.Add("Column", this.Sorting.Value.Column);
                    routeParams.Add("Direction", this.Sorting.Value.Direction);
                }

                //Likewise with any filter
                if (IsFiltered)
                {
                    routeParams.Add(this.Filtered.Value.Param, this.Filtered.Value.Value);
                }
            }
            //else we return an empty dictionary

            return routeParams;
        }

        #region Utlity properties
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex - 1 > 0);
            }
        }

        public int PreviousPage
        {
            get
            {
                return (PageIndex - 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public int NextPage
        {
            get
            {
                return (PageIndex + 1);
            }
        }

        public bool IsSorted
        {
            get
            {
                return Sorting.HasValue;
            }
        }

        public bool IsFiltered
        {
            get
            {
                return Filtered.HasValue;

            }
        }
        #endregion
    }
}
