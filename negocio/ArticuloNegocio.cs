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
            List<Articulo> articulos = new List<Articulo> ();

            try
            {
                accesoDB.setConsulta("Select Codigo, Nombre,A.Id,Precio ,A.Descripcion,M.Id MarcaId, M.Descripcion NombreMarca,C.Id CategoriaId,C.Descripcion NombreCategoria, ImagenUrl from ARTICULOS A, MARCAS M, CATEGORIAS C Where A.IdCategoria = C.Id AND A.IdMarca = M.Id");
                accesoDB.ejecutarConsulta();

                while (accesoDB.Lector.Read())
                {
                    Articulo art = new Articulo();
                    art.Id = (int)accesoDB.Lector["Id"];
                    art.Codigo = (string)accesoDB.Lector["Codigo"];
                    art.Nombre = (string)accesoDB.Lector["Nombre"];
                    art.Descripcion = (string)accesoDB.Lector["Descripcion"];
                    if (!(accesoDB.Lector["ImagenUrl"] is DBNull))
                    {
                        art.ImagenUrl = (string)accesoDB.Lector["ImagenUrl"];
                    }
                   
                    art.Marca = new Marca();
                    art.Marca.Id = (int)accesoDB.Lector["MarcaId"];
                    art.Marca.Descripcion = (string)accesoDB.Lector["NombreMarca"];
                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)accesoDB.Lector["CategoriaId"];
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

        public List<Categoria> listaCategoria()
        {
            BaseDatos accesoBD = new BaseDatos();
            List<Categoria> categorias = new List<Categoria>();
            

            try
            {
          
                accesoBD.setConsulta("Select Id, Descripcion from CATEGORIAS");
                accesoBD.ejecutarConsulta();
                while (accesoBD.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)accesoBD.Lector["Id"];
                    categoria.Descripcion = (string)accesoBD.Lector["Descripcion"];

                    categorias.Add(categoria);
                }


                return categorias;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accesoBD.cerrarConexion();
            }
        }

        public List<Marca> listaMarcas()
        {
            BaseDatos accesoBD = new BaseDatos();
            List<Marca> marcas = new List<Marca>();
      

            try
            {
                accesoBD.setConsulta("Select Id, Descripcion from MARCAS");
                accesoBD.ejecutarConsulta();
                while (accesoBD.Lector.Read())
                {
                    Marca marca = new Marca();
                    marca.Id = (int)accesoBD.Lector["Id"];
                    marca.Descripcion = (string)accesoBD.Lector["Descripcion"];

                    marcas.Add(marca);
                }


                return marcas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accesoBD.cerrarConexion();
            }
        }


    
        public void agregarArticulo(Articulo nuevoArticulo)
        {
            BaseDatos accesoBD = new BaseDatos();

            try
            {
                accesoBD.setConsulta("Insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) VALUES(@Codigo,@Nombre,@Descripcion,@IdMarca,@IdCategoria,@ImagenUrl,@Precio) ");
                accesoBD.setParametros("@Codigo",nuevoArticulo.Codigo);
                accesoBD.setParametros("@Nombre", nuevoArticulo.Nombre);
                accesoBD.setParametros("@Descripcion", nuevoArticulo.Descripcion);
                accesoBD.setParametros("@IdMarca", nuevoArticulo.Marca.Id);
                accesoBD.setParametros("@IdCategoria", nuevoArticulo.Categoria.Id);
                accesoBD.setParametros("@ImagenUrl", nuevoArticulo.ImagenUrl);
                accesoBD.setParametros("@Precio", nuevoArticulo.Precio);

                accesoBD.ejecutarLectura();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                accesoBD.cerrarConexion();
            }







        }

      

    }
}
