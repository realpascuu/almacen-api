namespace almacenAPI.Models
{
    public class DetallesArticulo<T>
    {
        public List<T> producto { get; set; }
        public DetallesArticulo(
            List<T> producto
        ) 
        {
            this.producto = producto;
        }
    }
}