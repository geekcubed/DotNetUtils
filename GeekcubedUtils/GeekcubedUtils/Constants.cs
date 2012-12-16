using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeekcubedUtils
{

    /// <summary>
    /// Struct to simplify passing around ordering requests
    /// </summary>
    public struct SortOption
    {
        public string Column;
        public SortOrder Direction;
    }

    /// <summary>
    /// Struct to simplfy passing around query filtering
    /// </summary>
    public struct FilterOption
    {
        public string Param;
        public string Value;
    }

    /// <summary>
    /// Enum to define query sorting
    /// </summary>
    public enum SortOrder { ascending, descending }
}
