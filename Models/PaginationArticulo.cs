namespace almacenAPI.Models
{
    public class PaginationArticulo<T>
    {
        public int total_objects { get; set; }
        public int total_pages { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public List<T> results { get; set; }
        public int previousPage { get;set;}
        public int nextPage { get;set;}

        public PaginationArticulo(
            int total_objects,
            int page,
            List<T> results,
            int limit,
            int total_pages,
            int previousPage,
            int nextPage
        ) 
        {
            this.total_objects = total_objects;
            this.page = page;
            this.limit = limit;
            this.results = results;
            this.total_pages = total_pages;
            this.previousPage = previousPage;
            this.nextPage = nextPage;
        }
    }
}