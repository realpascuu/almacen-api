namespace almacenAPI.Models
{
    public class Pagination<T>
    {
        public int total_objects { get; set; }
        public int total_pages { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public List<T> results { get; set; }

        public Pagination(
            int total_objects,
            int page,
            List<T> results,
            int limit,
            int total_pages
        ) 
        {
            this.total_objects = total_objects;
            this.page = page;
            this.limit = limit;
            this.results = results;
            this.total_pages = total_pages;
        }
    }
}