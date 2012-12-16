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
using System.Web.Mvc;

namespace GeekcubedUtils.Mvc
{
    public class PagedListBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //Does the request contain the minimum parameters for sortng a PagedList?
            if (!String.IsNullOrEmpty((string)controllerContext.HttpContext.Request.Params["Column"]))
            {
                //Yup - 
                SortOption sort = new SortOption();
                sort.Column = controllerContext.HttpContext.Request.Params["Column"];

                if (Enum.IsDefined(typeof(SortOrder), controllerContext.HttpContext.Request.Params["Direction"]))
                {
                    Enum.TryParse<SortOrder>(controllerContext.HttpContext.Request.Params["Direction"], out sort.Direction);
                }
                else
                {
                    sort.Direction = SortOrder.ascending;
                }

                return sort;
            }
            else
            {
                //Nothing to bind to
                return null;
            }

        }
    }
}
