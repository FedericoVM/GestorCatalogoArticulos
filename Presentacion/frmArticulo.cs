using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Presentacion
{
    public partial class frmArticulo : Form
    {
        Articulo articulo = null;
        public frmArticulo()
        {
            InitializeComponent();
        }

        public frmArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void btnSalirArticulo_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmArticulo_Load(object sender, EventArgs e)
        {

            cargarComboBox();

            try
            {
                if (articulo != null)

                {
                    
                    txtCodArticulo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtDescripcion.Text = articulo.Descripcion;
                    comboBoxMarca.SelectedValue = articulo.Marca.Id;
                    comboBoxCategoria.SelectedValue = articulo.Categoria.Id;
                    descativarInputs();
                    btnAceptar.Visible = false;
                }
                else
                {
                    articulo = new Articulo();
                    activarInputs(); 
                
                }
             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
         
        }

        private void cargarImagen(string url)
        {
            try
            {
                pictureBoxArticulo.Load(url);
            }
            catch (Exception)
            {
                pictureBoxArticulo.Load("https://img.freepik.com/vector-premium/vector-icono-imagen-predeterminado-pagina-imagen-faltante-diseno-sitio-web-o-aplicacion-movil-no-hay-foto-disponible_87543-11093.jpg");
            }
        }

         
        private void activarInputs()
        {
            txtCodArticulo.Enabled = true;
            txtNombre.Enabled = true;
            txtPrecio.Enabled = true;
            txtUrlImagen.Enabled = true;
            txtDescripcion.Enabled = true;
            comboBoxCategoria.Enabled = true;
            comboBoxMarca.Enabled = true;
        }

        private void descativarInputs()
        {
            txtCodArticulo.Enabled = false;
            txtNombre.Enabled = false;
            txtPrecio.Enabled = false;
            txtUrlImagen.Enabled = false;
            txtDescripcion.Enabled = false;
            comboBoxCategoria.Enabled = false;
            comboBoxMarca.Enabled = false;
        }

        private void activarBtnAgregar()
        {
            if (formatoCodigoArticulo() && formatoNombre() && formatoPrecio())
            {
                btnAceptar.Enabled = true;
            }
           
        }

        private void agregarNuevoArticulo(Articulo articulo)
        {

            ArticuloNegocio negocio = new ArticuloNegocio(); 

            try
            {
                articulo.Codigo = txtCodArticulo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Marca = (Marca)comboBoxMarca.SelectedItem;
                articulo.Categoria = (Categoria)comboBoxCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;
                if (articulo.ImagenUrl == "")
                {
                    articulo.ImagenUrl = "https://img.freepik.com/vector-premium/vector-icono-imagen-predeterminado-pagina-imagen-faltante-diseno-sitio-web-o-aplicacion-movil-no-hay-foto-disponible_87543-11093.jpg";
                }
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Descripcion = txtDescripcion.Text;
                negocio.agregarArticulo(articulo);
                MessageBox.Show("Se agrego correctamente el articulo. Actualice la lista para ver los cambios");

                this.Close();

              

               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            agregarNuevoArticulo(articulo);
            this.Close();
        }

        private void cargarComboBox()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                comboBoxCategoria.DataSource = negocio.listaCategoria();
                comboBoxCategoria.ValueMember = "Id";
                comboBoxCategoria.DisplayMember = "Descripcion";

                comboBoxMarca.DataSource = negocio.listaMarcas();
                comboBoxMarca.ValueMember = "Id";
                comboBoxMarca.DisplayMember = "Descripcion";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool formatoCodigoArticulo()
        {
            string codigo = txtCodArticulo.Text;
            string patronCodigo = @"^[A-Za-z]\d{2}$";

            Regex regexCodigo = new Regex(patronCodigo);
 
            if (!(regexCodigo.IsMatch(codigo)))
            {
                labelValidacionCodigo.Text = "Error! Formato incorrecto";
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                comboBoxCategoria.Enabled = false;
                comboBoxMarca.Enabled = false;
                txtPrecio.Enabled = false;
                txtUrlImagen.Enabled = false;
                btnAceptar.Enabled = false;
                return false;

            } else
            {
                labelValidacionCodigo.Text = "";
                labelValidacionCodigo.Visible = false;
                activarInputs();
                return true;
            }

            
 
        }

        private bool formatoNombre()
        {
            string nombre = txtNombre.Text;
            string patronNombre = @"^\S+(?: \S+)*$";

            Regex regex = new Regex(patronNombre);

            if (!(regex.IsMatch(nombre)))
            {
                labelValidacionNombre.Text = "Error! No puede estar vacio. Debe contener letra/as";
                txtDescripcion.Enabled = false;
                comboBoxCategoria.Enabled = false;
                comboBoxMarca.Enabled = false;
                txtPrecio.Enabled = false;
                txtUrlImagen.Enabled = false;
                btnAceptar.Enabled = false;
                return false;
            } else
            {
                labelValidacionNombre.Text = "";
                labelValidacionNombre.Visible = false;
                activarInputs();
                return true;
            }

       
        }

        private bool formatoPrecio()
        {

            string precio = txtPrecio.Text;
            string patronPrecio = @"^\d+,\d{2,4}$";

            Regex regexPrecio = new Regex(patronPrecio);

            if (!(regexPrecio.IsMatch(precio)))
            {
                labelValidacionPrecio.Text = "Error!Formato incorrecto";
                txtCodArticulo.Enabled = false;
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                comboBoxCategoria.Enabled = false;
                comboBoxMarca.Enabled = false;
                txtUrlImagen.Enabled = false;
                btnAceptar.Enabled = false;
                return false;
            } else
            {
                labelValidacionPrecio.Text = "";
                labelValidacionPrecio.Visible = false;
                activarInputs();
                return true;
            }

        
        }

        private void txtCodArticulo_Leave(object sender, EventArgs e)
        {
            formatoCodigoArticulo();

        }

        private void txtCodArticulo_TextChanged(object sender, EventArgs e)
        {
            formatoCodigoArticulo();
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            formatoNombre();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            formatoNombre();
        }

    
        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            if (formatoPrecio())
            {
                activarBtnAgregar();
            }
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            if (formatoPrecio())
            {
                activarBtnAgregar();
            }
        }
    }
}
