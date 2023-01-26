namespace almacenAPI.Models
{
    public class DetallesArticulo<T>
    {
        public string nombre { get; set; }
        public string especificaciones { get; set; }
        public float pvp { get; set; }
        public int cod { get; set; }
        public string categoria { get; set; }

        public DetallesArticulo(
           string nombre,
           string especificaciones ,
           float pvp ,
           int cod ,
           string categoria 
        ) 
        {
            this.nombre = nombre;
            this.especificaciones = especificaciones;
            this.pvp = pvp;
            this.cod = cod;
            this.categoria = categoria;
        }
    }
}