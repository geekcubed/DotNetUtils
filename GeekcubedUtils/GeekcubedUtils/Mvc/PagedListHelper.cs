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
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace GeekcubedUtils.Mvc
{
    public static class PagedListHelper
    {
        public static MvcHtmlString SortableLinkFor<TModel, TValue>
            (this HtmlHelper<PagedList<TModel>> html, Expression<Func<PagedList<TModel>, TValue>> expression, 
                Type parentModel = null, string action = null, string controller = null, string routeName = null)
        {
            //Init some internal variables - some html attributes to make things pretty, and the params for our link
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            RouteValueDictionary urlParams = new RouteValueDictionary();
            
            //Load the MetaData for the model
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            //Step 1. Determine the column name for the sorting.
            string sortColName = metaData.PropertyName;
            
            //Handle any forgein-field sorting
            if (parentModel != null && parentModel != metaData.ContainerType)
            {
                //We have been passed a reference to a forgein field
                //All we really need is to grab the object name                
                sortColName = metaData.ContainerType.Name + "." + metaData.PropertyName;

                /***** Warning ******/
                // The above will only work with Linq2SQL models 
                // where the object names remain as geneated by the L2S Designer
            }

            //Now we have the correct name, stash it into the params
            urlParams.Add("Column", sortColName);

            //Step 2. Determine the direction for sorting.
            //Are we sorting by this field already?
            if (html.ViewData.Model.IsSorted && html.ViewData.Model.Sorting.Value.Column == sortColName)
            {
                //We are, but in which direction
                //Arrows show the current direction of sorting. 
                //The links however, should reverse the sort

                if (html.ViewData.Model.Sorting.Value.Direction == SortOrder.ascending)
                {
                    urlParams.Add("Direction", SortOrder.descending);
                    htmlAttributes.Add("class", "sortable asc");
                }
                else
                {
                    urlParams.Add("Direction", SortOrder.ascending);
                    htmlAttributes.Add("class", "sortable desc");
                }
            }
            else
            {
                //Not sorting by this field/property
                //Force to first page, and default to sort ASC
                urlParams.Add("Direction", SortOrder.ascending);
                urlParams.Add("Page", 1);
                htmlAttributes.Add("class", "sortable");
            }

            //Step 3. Are we filtering?
            if (html.ViewData.Model.IsFiltered)
            {
                urlParams.Add(html.ViewData.Model.Filtered.Value.Param, html.ViewData.Model.Filtered.Value.Value);
            }

            //Step 4. Generate (and return) our link
            return MvcHtmlString.Create(HtmlHelper.GenerateLink(
                                        html.ViewContext.RequestContext,
                                        html.RouteCollection,
                                        HtmlHelpers.LocalisedDisplayFor(html, expression),
                                        routeName,
                                        action,
                                        controller,
                                        urlParams,
                                        htmlAttributes));
        }
    }
}
