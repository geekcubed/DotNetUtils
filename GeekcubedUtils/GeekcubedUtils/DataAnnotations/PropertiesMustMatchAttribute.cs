using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GeekcubedUtils.DataAnnotations
{
    /// <summary>
    /// Provides Server Side validation to ensure two properties of an object are equal
    /// </summary>
    /// <see cref="http://blogs.msdn.com/b/stuartleeks/archive/2010/12/17/asp-net-mvc-validatepasswordlength-and-propertiesmustmatch-in-asp-net-mvc-3-rc2.aspx"/>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.web.mvc.compareattribute(v=vs.98).aspx"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string defaultErrorMessage = "'{0}' and '{1}' do not match";
        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }
        
        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        private readonly object typeId = new object();        
        public override object TypeId
        {
            get
            {
                return typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, 
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /*ignore case*/).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /*ignore case*/).GetValue(value);

            return Object.Equals(originalValue, confirmValue);
        }

    }
}
