namespace PeopleNetCoreBackend.Models
{
    /// <summary>
    /// Represents a paginated result with data and pagination metadata
    /// </summary>
    /// <typeparam name="T">The type of data being paginated</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The data for the current page
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// The current page number (1-based)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Indicates if there is a previous page
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// Indicates if there is a next page
        /// </summary>
        public bool HasNext => Page < TotalPages;
    }
}
