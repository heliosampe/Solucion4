using System;

namespace Models
{
    public class Continente
    {
        public int IdContinente { get; set; }
        public string NombreContinente { get; set; }
        public bool Activo { get; set; }

        //Relationships with other entities, i use a number to diferentiate entites with the same name       

        //To manage search paged
        //public int TotalRecords { get; set; }

    }
}
