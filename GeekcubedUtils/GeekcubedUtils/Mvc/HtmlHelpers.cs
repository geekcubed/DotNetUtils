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
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GeekcubedUtils.Mvc
{
    /// <summary>
    /// Various useful little helper extension methods useful
    /// in MVC View pages.
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// Wrapper for LocalisedDisplayFor, generating a MvcHtmlString
        /// </summary>
        /// <typeparam name="TModel">Object model</typeparam>
        /// <typeparam name="TValue">Model property</typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns>The localised property display string</returns>
        public static MvcHtmlString PropertyNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return MvcHtmlString.Create(LocalisedDisplayFor(html, expression));
        }

        /// <summary>
        /// Retrieves the field name for the property of a given object, based on DataAnnotations
        /// </summary>
        /// <typeparam name="TModel">Object model</typeparam>
        /// <typeparam name="TValue">Object property</typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns>Localised display name from resources, or the supplied fieldname as a fallback</returns>
        /// <see cref="GeekcubedUtils.DataAnnotations.LocalisedDisplayNameAttribute"/>
        public static String LocalisedDisplayFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            return metaData.DisplayName ?? metaData.PropertyName ?? fieldName.Split('.').Last();
        }

        /// <summary>
        /// Generates a SelectList of items based on the options in a specific Enum
        /// </summary>
        /// <remarks>If the Enum instance is nullable, the returned options will included a blank entry.</remarks>
        /// <typeparam name="TEnum">Type of the passed Enum</typeparam>
        /// <param name="enumObj">Instance of the Enum</param>
        /// <returns>SelectList of options from the Enum, with the correct item being marked as Selected based on the passed Enum property</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        {
            bool includeNull = false;
            //Get the type of Enum
            var t = typeof(TEnum);

            //If it is a nullable? we need to get the underlying enum type
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
                includeNull = true;
            }

            var values = from TEnum e in Enum.GetValues(t)
                         select new { ID = e.ToString(), Name = e.ToString() };
            if (includeNull)
            {
                values.ToList().Insert(0, new { ID = String.Empty, Name = String.Empty });
            }

            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
}
