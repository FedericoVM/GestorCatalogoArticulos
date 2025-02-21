using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {

        public List<Articulo> listarArticulos()
        {
            BaseDatos accesoDB = new BaseDatos();
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> articulos = new List<Articulo> ();

            try
            {
                accesoDB.setConsulta("Select Codigo, Nombre,A.Id,Precio ,A.Descripcion,M.Id, M.Descripcion NombreMarca,C.Id,C.Descripcion NombreCategoria from ARTICULOS A, MARCAS M, CATEGORIAS C Where A.IdCategoria = C.Id AND A.IdMarca = M.Id");
                accesoDB.ejecutarConsulta();

                while (accesoDB.Lector.Read())
                {
                    Articulo art = new Articulo();
                    art.Codigo = (string)accesoDB.Lector["Codigo"];
                    art.Nombre = (string)accesoDB.Lector["Nombre"];
                    art.Descripcion = (string)accesoDB.Lector["Descripcion"];
                    art.Marca = new Marca();
                    art.Marca.Descripcion = (string)accesoDB.Lector["NombreMarca"];
                    art.Categoria = new Categoria();
                    art.Categoria.Descripcion = (string)accesoDB.Lector["NombreCategoria"];
                    art.Precio = (decimal)accesoDB.Lector["Precio"];

                    articulos.Add(art);
                    
                }


                return articulos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accesoDB.cerrarConexion();
            }



        }




    }
}
