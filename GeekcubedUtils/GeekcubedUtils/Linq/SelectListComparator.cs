﻿// Copyright 2012 Ian Stapleton
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
using System.Web.Mvc;

namespace GeekcubedUtils.Linq
{
    /// <summary>
    /// Simple class to allow two SelectListItem objects to be compared
    /// </summary>
    public class SelectListItemComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem left, SelectListItem right)
        {
            return (left.Value == right.Value);
        }

        public int GetHashCode(SelectListItem obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}
