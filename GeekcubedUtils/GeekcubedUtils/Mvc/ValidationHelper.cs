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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GeekcubedUtils.Mvc
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Static utility function to bind the results of third-partry/manual validation back onto the 
        /// ViewModel.  Allows for more complex validation (such as business logic) to be integrated with
        /// automatic model validation using Data Annotations
        /// </summary>
        /// <param name="modelState">The current ModelState for the MVC Action</param>
        /// <param name="validationResults">Enumerable list of the results of any validation process</param>
        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<ValidationResult> validationResults)
        {
            foreach (var vr in validationResults)
            {
                foreach (string member in vr.MemberNames)
                {
                    modelState.AddModelError(member, vr.ErrorMessage);
                }
            }
        }
    }
}
