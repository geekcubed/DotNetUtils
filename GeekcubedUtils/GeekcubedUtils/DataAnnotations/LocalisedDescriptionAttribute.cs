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
