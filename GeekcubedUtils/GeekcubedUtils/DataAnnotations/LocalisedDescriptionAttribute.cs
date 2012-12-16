// Copyright 2012 Ian Stapleton
// Based on original code samples by Alex Adamyan
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
using System.ComponentModel;
using System.Reflection;

namespace GeekcubedUtils.DataAnnotations
{
    /// <summary>
    /// DataAnnotation to allow a Description Attribute to be localised via Resource files
    /// </summary>
    /// <example>
    ///     [LocalisedDescriptionAttribute("User_Email", NameResouceType = typeof(Resources.Descriptions))]
    ///     public string Email { get; set; }
    /// </example>
    /// <see cref="http://adamyan.blogspot.com/2010/02/aspnet-mvc-2-localization-complete.html"/>
    public class LocalisedDescriptionAttribute : DescriptionAttribute
    {
        private Type resourceType;
        public Type NameResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }

        private PropertyInfo nameProperty;
        public override string Description
        {
            get
            {
                if (nameProperty == null)
                {
                    return base.Description;
                }
                else
                {
                    return (string)nameProperty.GetValue(nameProperty.DeclaringType, null);
                }
            }
        }

        public LocalisedDescriptionAttribute(string description)
            : base(description)
        { }
    }
}
