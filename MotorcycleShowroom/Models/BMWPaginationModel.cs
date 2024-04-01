using System.Collections.Generic;
using MotorcycleShowroom.Models;

namespace MotorcycleShowroom.Models
{
    public class BMWPaginationModel
    {
        public IEnumerable<BMW> BMWs { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set;}

        public int PageNumber { get; set; }
    }
}