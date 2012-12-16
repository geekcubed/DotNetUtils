#region Copyright and license information
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
#endregion

using System;
using System.ComponentModel;
using System.Reflection;

namespace GeekcubedUtils.DataAnnotations
{
    /// <summary>
    /// DataAnnotation to allow a DisplayName Attribute to be localised via Resource files
    /// </summary>
    /// <example>
    ///     [LocalisedDisplayName("User_Email", NameResouceType = typeof(Resources.Fields))]
    ///     public string Email { get; set; }
    /// </example>
    /// <see cref="http://adamyan.blogspot.com/2010/02/aspnet-mvc-2-localization-complete.html"/>
    public class LocalisedDisplayNameAttribute : DisplayNameAttribute
    {
        private Type resourceType;
        public Type NameResourceType
        {
            get { return resourceType; }
            set
            {
                resourceType = value;
                //Init the name property
                nameProperty = resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public);
            }
        }

        private PropertyInfo nameProperty;
        public override string DisplayName
        {
            get
            {
                if (nameProperty == null)
                {
                    return base.DisplayName;
                }
                else
                {
                    return (string)nameProperty.GetValue(nameProperty.DeclaringType, null);
                }
            }
        }

        public LocalisedDisplayNameAttribute(string displayNameKey) 
            : base(displayNameKey) 
        { }
    }
}
