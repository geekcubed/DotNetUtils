using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GeekcubedUtils.DataAnnotations
{
    /// <summary>
    /// Validation Attribute to ensure a given number of pattern occurences are found in the property
    /// </summary>
    /// <remarks>This attribute is especially useful when validating pro-forma / templating strings.</remarks>
    /// <example>
    ///     [TokenValidation(@"\(L\)", 1, 1, ErrorMessageResourceName = "Template_TemplateText_Link", ErrorMessageResourceType = typeof(Resources.Validation))]
    ///     string TemplateText { get; set; }        
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class TokenValidationAttribute : RegularExpressionAttribute
    {
        private int minOccurences = 0;
        private int maxOccurences = 1;

        public TokenValidationAttribute(string pattern, int minOccurences, int maxOccurences)
            : base(pattern)
        {
            this.minOccurences = minOccurences;
            this.maxOccurences = maxOccurences;
        }

        public override bool IsValid(object value)
        {
            int tokenCount = Regex.Matches(value.ToString(), this.Pattern).Count;

            if (tokenCount < this.minOccurences || tokenCount > this.maxOccurences)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Override TypeId in order to allow multiple uses on Properties
        /// </summary>
        /// <see cref="http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/e6bb4146-eb1a-4c1b-a5b1-f3528d8a7864" />
        public override object TypeId
        {
            get
            {
                return base.TypeId.ToString()
                    + "_" + this.Pattern
                    + "_" + this.minOccurences
                    + "_" + this.maxOccurences;
            }
        }
    }
}
