using System;
using System.Collections.Generic;
using System.Text;

namespace WilderExperience.Services.Data
{
    public interface IPaginatableService
    {
        bool HasNextPage { get; }

        int PageNumber { get;  set; }

        int PageSize { get;  set; }
    }
}
