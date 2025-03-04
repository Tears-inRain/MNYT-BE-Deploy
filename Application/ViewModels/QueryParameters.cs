using Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class QueryParameters
    {
        private const int MaxPageSize = PaginationDefaults.MaxPageSize;
        private int _pageSize = PaginationDefaults.DefaultPageSize;

        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
        public int PageNumber { get; set; } = PaginationDefaults.DefaultPageNumber;

        [Range(1, MaxPageSize, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
