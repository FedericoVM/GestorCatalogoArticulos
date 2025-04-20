using dominio;
using negocio;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;


namespace Presentacion
{
    public partial class frmArticulo : Form
    {
        Articulo articulo = null;
        private bool editarArticulo = false;
        private OpenFileDialog archivo = null;

        public bool EditarArticulo
        {
            get { return editarArticulo; }
            set
            {
                editarArticulo = value;
            }
        }

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
                    if (EditarArticulo == true)
                    {
                        btnAceptar.Text = "Aceptar cambios";
                        btnAgregarImagen.Enabled = true;
                        cargarInputs(articulo);
                        activarInputs();
                        

                    } else
                    {
                        cargarInputs(articulo);
                        descativarInputs();
                        btnAceptar.Visible = false;
                    }
                
                }
                else
                {
                    articulo = new Articulo();
                    btnAgregarImagen.Enabled = true;
                    activarInputs(); 
                }
             

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
         
        }

        private void cargarInputs(Articulo articulo)
        {
            txtCodArticulo.Text = articulo.Codigo;
            txtNombre.Text = articulo.Nombre;
            txtUrlImagen.Text = articulo.ImagenUrl;
            cargarImagen(articulo.ImagenUrl);
            txtPrecio.Text = articulo.Precio.ToString();
            txtDescripcion.Text = articulo.Descripcion;
            comboBoxMarca.SelectedValue = articulo.Marca.Id;
            comboBoxCategoria.SelectedValue = articulo.Categoria.Id;
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

        private void agregarNuevoArticulo(Articulo articulo)
        {
            ArticuloNegocio negocio = new ArticuloNegocio(); 

            try
            {
                if (!formatoCodigoArticulo()) 
                {
                    return;
                } 
                articulo.Codigo = txtCodArticulo.Text.ToUpper();
                articulo.Nombre = txtNombre.Text;
                if (articulo.Nombre == "")
                {
                    labelValidacionNombre.Text = "El campo nombre no puede estar vacio";
                    return;
                }
                else
                {
                    labelValidacionNombre.Text = "";
                }
                articulo.Marca = (Marca)comboBoxMarca.SelectedItem;
                articulo.Categoria = (Categoria)comboBoxCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;
                if (articulo.ImagenUrl == "")
                {
                    articulo.ImagenUrl = "https://img.freepik.com/vector-premium/vector-icono-imagen-predeterminado-pagina-imagen-faltante-diseno-sitio-web-o-aplicacion-movil-no-hay-foto-disponible_87543-11093.jpg";
                }
                decimal numDecimal;
                decimal.TryParse(txtPrecio.Text, out numDecimal);
                if (!formatoPrecio())
                {
                    return;  
                }

                articulo.Precio = numDecimal;

                articulo.Descripcion = txtDescripcion.Text;

                if (formatoCodigoArticulo() && formatoPrecio())
                {
                    if (editarArticulo == true)
                    {
                        negocio.modificar(articulo);
                        System.Windows.MessageBox.Show("Se agregaron los cambios correctamente", "Proceso exitoso");
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        if (negocio.ExisteArticuloConCodigo(articulo.Codigo))
                        {
                            System.Windows.MessageBox.Show("Ya existe un articulo con ese codigo");
                        }
                        else
                        {
                            negocio.agregarArticulo(articulo);
                            this.DialogResult = DialogResult.OK;
                            System.Windows.MessageBox.Show("Se agrego correctamente el articulo", "Proceso exitoso");
                        }

                    }

                    if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP:")))
                    {
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                    }



                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
                agregarNuevoArticulo(articulo);
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

                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private bool formatoCodigoArticulo()
        {
            string codigo = txtCodArticulo.Text;
            string patronCodigo = @"^[A-Za-z]{1,2}\d{2,3}$";

            Regex regexCodigo = new Regex(patronCodigo);

            if(codigo is null || codigo == "")
            {
                labelValidacionCodigo.Text = "El campo codigo no puede estar vacio";
                return false;
            }
            else
            {
                labelValidacionCodigo.Text = "";
            }

            if (!(regexCodigo.IsMatch(codigo)))
            {
                labelValidacionCodigo.Text = "formato incorrecto";
                return false;

            } else
            {
                labelValidacionCodigo.Visible = false;
                return true;
            }

            
 
        }

        private bool formatoPrecio()
        {

            string precio = txtPrecio.Text;
            string patronPrecio = @"^(?:\d{1,3}(?:[ .]\d{3})*|\d+)(?:,\d{1,4})?$";

            Regex regexPrecio = new Regex(patronPrecio);

            if (precio is null || precio == "")
            {
                labelValidacionPrecio.Text = "El campo precio no puede estar vacio";
                return false;
            }
            else
            {
                labelValidacionPrecio.Text = "";
            }

            if (!(regexPrecio.IsMatch(precio)))
            {
                labelValidacionPrecio.Text = "El formato es incorrecto";
                return false;
            } else
            {
                labelValidacionPrecio.Text = "";
                return true;
            }
        
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void btnAgregarImagen_Click_1(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;| png|*.png;|wep|*.wep";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }

    }
}
