namespace almacenAPI.Models
{
    public class DetallesArticulo<T>
    {

        public List<T> results { get; set; }

        public DetallesArticulo(
            int total_objects,
            int page,
            List<T> results,
            int limit,
            int total_pages,
            int previousPage,
            int nextPage
        ) 
        {
            this.results = results;
        }
    }
}