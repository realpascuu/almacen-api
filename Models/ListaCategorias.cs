namespace almacenAPI.Models
{
    public class ListaCategorias<T>
    {

        public List<T> results { get; set; }     
        public ListaCategorias(
            List<T> results
        ) 
        {
            this.results = results;
        }
    }
}