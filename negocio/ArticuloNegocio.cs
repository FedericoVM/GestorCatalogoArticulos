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
                    decimal num = (decimal)accesoDB.Lector["Precio"];
                    art.Precio = Math.Floor(num);

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

        public void eliminarArticulo(int idArticulo) 
        {
            BaseDatos baseDatos = new BaseDatos();

            try
            {
                baseDatos.setConsulta("Delete from ARTICULOS where ARTICULOS.Id = @Id");
                baseDatos.setParametros("@Id", idArticulo);
                baseDatos.ejecutarLectura();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                baseDatos.cerrarConexion();
            }
        
        }

        public void modificar(Articulo articulo)
        {
            BaseDatos baseDatos = new BaseDatos();
            try
            {
                baseDatos.setConsulta("Update ARTICULOS set Codigo = @codigo,Nombre=@nombre,Descripcion=@descripcion,IdMarca=@idMarca,IdCategoria=@idCategoria,ImagenUrl=@imagenUrl, Precio=@precio Where ARTICULOS.Id = @id");
                baseDatos.setParametros("@codigo",articulo.Codigo);
                baseDatos.setParametros("@nombre",articulo.Nombre);
                baseDatos.setParametros("@descripcion", articulo.Descripcion);
                baseDatos.setParametros("@idMarca",articulo.Marca.Id);
                baseDatos.setParametros("@idCategoria", articulo.Categoria.Id);
                baseDatos.setParametros("@imagenUrl",articulo.ImagenUrl);
                baseDatos.setParametros("@precio", articulo.Precio);
                baseDatos.setParametros("@id", articulo.Id);

                baseDatos.ejecutarLectura();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                baseDatos.cerrarConexion();
            }
        }

        public List<Articulo> filtrar(string nombre, string marca, string categoria,decimal precioMinimo, decimal precioMaximo)
        {
            BaseDatos accesoBD = new BaseDatos();

            List<Articulo> articulos = new List<Articulo>();

            try
            {
                accesoBD.setConsulta(@"
                SELECT 
                    A.Codigo, 
                    A.Nombre, 
                    A.Id, 
                    A.Precio, 
                    A.Descripcion, 
                    M.Id AS MarcaId, 
                    M.Descripcion AS NombreMarca, 
                    C.Id AS CategoriaId, 
                    C.Descripcion AS NombreCategoria, 
                    A.ImagenUrl 
                FROM 
                    ARTICULOS A, 
                    MARCAS M, 
                    CATEGORIAS C 
                WHERE 
                    A.IdCategoria = C.Id 
                    AND A.IdMarca = M.Id 
                    AND (A.Nombre COLLATE Latin1_General_CI_AI LIKE '%' + @nombre + '%' OR @nombre = '')
                    AND (M.Descripcion LIKE '%' + @marca + '%' OR @marca = '')
                    AND (C.Descripcion LIKE '%' + @categoria + '%' OR @categoria = '')
                    AND ( (@precioMinimo = 0.00 AND @precioMaximo = 0.00) 
                        OR (A.Precio >= @precioMinimo AND A.Precio <= @precioMaximo) 
                        );");
               
                accesoBD.setParametros("@nombre", nombre);
                accesoBD.setParametros("@marca",marca);
                accesoBD.setParametros("@categoria",categoria);
                accesoBD.setParametros("@precioMinimo", precioMinimo);
                accesoBD.setParametros("@precioMaximo", precioMaximo);
                accesoBD.ejecutarConsulta();
                


                while (accesoBD.Lector.Read())
                {
                    Articulo art = new Articulo();
                    art.Id = (int)accesoBD.Lector["Id"];
                    art.Codigo = (string)accesoBD.Lector["Codigo"];
                    art.Nombre = (string)accesoBD.Lector["Nombre"];
                    art.Descripcion = (string)accesoBD.Lector["Descripcion"];
                    if (!(accesoBD.Lector["ImagenUrl"] is DBNull))
                    {
                        art.ImagenUrl = (string)accesoBD.Lector["ImagenUrl"];
                    }

                    art.Marca = new Marca();
                    art.Marca.Id = (int)accesoBD.Lector["MarcaId"];
                    art.Marca.Descripcion = (string)accesoBD.Lector["NombreMarca"];
                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)accesoBD.Lector["CategoriaId"];
                    art.Categoria.Descripcion = (string)accesoBD.Lector["NombreCategoria"];
                    decimal num = (decimal)accesoBD.Lector["Precio"];
                    art.Precio = Math.Floor(num);

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
                accesoBD.cerrarConexion();
            }

        }

        public List<Articulo> buscarSoloNombre(string nombre)
        {
            BaseDatos baseDatos = new BaseDatos();

            List<Articulo> articulos = new List<Articulo>();

            try
            {
                baseDatos.setConsulta("Select Codigo, Nombre,A.Id,Precio ,A.Descripcion,M.Id MarcaId, M.Descripcion NombreMarca,C.Id CategoriaId,C.Descripcion NombreCategoria, ImagenUrl from ARTICULOS A, MARCAS M, CATEGORIAS C Where A.IdCategoria = C.Id AND A.IdMarca = M.Id AND Nombre COLLATE Latin1_General_CI_AI LIKE '%' + @nombre + '%' ");
                baseDatos.setParametros("@nombre", nombre);
                baseDatos.ejecutarConsulta();

                while (baseDatos.Lector.Read())
                {
                    Articulo art = new Articulo();
                    art.Id = (int)baseDatos.Lector["Id"];
                    art.Codigo = (string)baseDatos.Lector["Codigo"];
                    art.Nombre = (string)baseDatos.Lector["Nombre"];
                    art.Descripcion = (string)baseDatos.Lector["Descripcion"];
                    if (!(baseDatos.Lector["ImagenUrl"] is DBNull))
                    {
                        art.ImagenUrl = (string)baseDatos.Lector["ImagenUrl"];
                    }

                    art.Marca = new Marca();
                    art.Marca.Id = (int)baseDatos.Lector["MarcaId"];
                    art.Marca.Descripcion = (string)baseDatos.Lector["NombreMarca"];
                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)baseDatos.Lector["CategoriaId"];
                    art.Categoria.Descripcion = (string)baseDatos.Lector["NombreCategoria"];
                    decimal num = (decimal)baseDatos.Lector["Precio"];
                    art.Precio = Math.Floor(num);

                    articulos.Add(art);

                }

                return articulos;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ExisteArticuloConCodigo(string codigo)
        {
            BaseDatos accesoBD = new BaseDatos();
            try
            {
                accesoBD.setConsulta("SELECT COUNT(*) FROM ARTICULOS WHERE Codigo = @Codigo");
                accesoBD.setParametros("@Codigo", codigo);
                accesoBD.ejecutarConsulta();

                if (accesoBD.Lector.Read())
                {
                    int cantidad = (int)accesoBD.Lector[0];
                    return cantidad > 0;
                }

                return false;
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
