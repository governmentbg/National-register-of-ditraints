using System;

namespace NRZ.Models.GridModels
{
    public class GridSearchModel
    {
        public string Search { get; set; }

        public string SortBy { get; set; }

        public bool SortDesc { get; set; }

        public int Page { get; set; }

        public int ItemsPerPage { get; set; }   

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
