﻿    using System;
namespace TFEatery.Entidades
{
    public class Producto
    {
        public string IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public string IdRestaurante { get; set; }

        public string NombreRestaurante { get; set; }

        public string TipoDeComida { get; set; }

        public string Descripcion { get; set; }

        public float PrecioProducto { get; set; }

        public int TiempoEntrega { get; set; }

        public int Descuento { get; set; }

        public byte[] FotoProducto { get; set; }

        public string Disponibilidad { get; set; }

    }
}
