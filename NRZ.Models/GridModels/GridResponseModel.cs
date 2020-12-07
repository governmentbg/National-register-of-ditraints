using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.GridModels
{
    public class GridResponseModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public IEnumerable<object> Items { get; set; }
    }
}
