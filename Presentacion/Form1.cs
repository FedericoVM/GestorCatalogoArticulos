using System;
using System.Collections.Generic;
using System.Windows.Forms;
using negocio;
using dominio;
using System.Globalization;

namespace Presentacion
{
    public partial class FormPrincipal : Form
    {
        private List<Articulo> articulos;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnCerrarVentana_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
            this.WindowState = FormWindowState.Maximized;
        }


        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            labelHora.Text = DateTime.Now.ToString("hh:mm:ss");
            labelFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            timer.Enabled = true;
            cargar();
            cargarComboBoxFiltro();
            btnBuscar.Enabled = false;
          
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            DialogResult salir = MessageBox.Show("¿Desea salir de la aplicación?","Cuidado", MessageBoxButtons.YesNo,MessageBoxIcon.Warning );
            if (salir == DialogResult.Yes)
            {
                this.Close();
            }
            
        }

        private void btnVer_Click(object sender, EventArgs e)
        {

            if (dgvForm.CurrentRow != null)
            {
                Articulo aux = (Articulo)dgvForm.CurrentRow.DataBoundItem;
                try
                {
                    frmArticulo ventanaArticulo = new frmArticulo(aux);
                    ventanaArticulo.ShowDialog();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Seleccionar un articulo", "¡Error!");
            }

          
           
           

        }

        public void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                articulos = negocio.listarArticulos();
                dgvForm.DataSource = articulos;
                ocultarColumnas();
                CultureInfo culturaAR = new CultureInfo("es-AR");
                dgvForm.Columns["Precio"].DefaultCellStyle.Format = "C2";
                dgvForm.Columns["Precio"].DefaultCellStyle.FormatProvider = culturaAR;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void cargarComboBoxFiltro()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                comboBoxCategoriaBusqueda.DataSource = negocio.listaCategoria();
                comboBoxCategoriaBusqueda.ValueMember = "Id";
                comboBoxCategoriaBusqueda.DisplayMember = "Descripcion";

                comboBoxMarcaBusqueda.DataSource = negocio.listaMarcas();
                comboBoxMarcaBusqueda.ValueMember = "Id";
                comboBoxMarcaBusqueda.DisplayMember = "Descripcion";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        } 

        private void ocultarColumnas()
        {
            dgvForm.Columns["Descripcion"].Visible = false;
            dgvForm.Columns["ImagenUrl"].Visible = false;
            dgvForm.Columns["Categoria"].Visible = false;
            dgvForm.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmArticulo nuevaVentana = new frmArticulo();
            nuevaVentana.ShowDialog();
            if (nuevaVentana.DialogResult == DialogResult.OK)
            {
                cargar();
            }
            
        }

        private void btnActualizarListado_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvForm.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un articulo", "Error");
                return;
            }
            ArticuloNegocio negocio = new ArticuloNegocio();
            DialogResult resultado = MessageBox.Show("¿Deseas borrar el articulo de la base de datos? ", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            try
            {
                if (resultado == DialogResult.Yes)
                {
                    Articulo articulo = (Articulo)dgvForm.CurrentRow.DataBoundItem;
                    negocio.eliminarArticulo(articulo.Id);
                    MessageBox.Show("Se elimino correctamente.", "Eliminar");
                    cargar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }




        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Articulo articuloEditar = new Articulo();
            if (dgvForm.CurrentRow != null)
            {
                try
                {
                    articuloEditar = (Articulo)dgvForm.CurrentRow.DataBoundItem;
                    frmArticulo formArticulo = new frmArticulo(articuloEditar);
                    formArticulo.EditarArticulo = true;
                    formArticulo.ShowDialog();
                    if (formArticulo.DialogResult == DialogResult.OK)
                    {
                        cargar();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            } else
            {
                MessageBox.Show("Debe seleccionar un articulo","Error");
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> listaFiltroAvanzado = new List<Articulo>();

            try
            {
                string nombre = txtNombreBusqueda.Text;
                string marca = comboBoxMarcaBusqueda.SelectedItem.ToString();
                string categoria = comboBoxCategoriaBusqueda.SelectedItem.ToString();
                decimal precioMin = decimal.Parse(txtPrecioMinBusqueda.Text);
                decimal precioMax = decimal.Parse(txtPrecioMaxBusqueda.Text);

                listaFiltroAvanzado = negocio.filtrar(nombre, marca, categoria,precioMin,precioMax);
                dgvForm.DataSource = null;

                if (listaFiltroAvanzado.Count == 0)
                {
                    MessageBox.Show("No se encontraron resultados","Búsqueda");
                } else
                {
                    dgvForm.DataSource = negocio.filtrar(nombre, marca, categoria,precioMin,precioMax);
                    ocultarColumnas();
                    CultureInfo culturaAR = new CultureInfo("es-AR");
                    dgvForm.Columns["Precio"].DefaultCellStyle.Format = "C2";
                    dgvForm.Columns["Precio"].DefaultCellStyle.FormatProvider = culturaAR;
                    btnLimpiarFiltroAvanzado.Visible = true;
                }
             

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            };
           
        }

        private void activarDesactivarInputsFiltroAvanzado(bool booleano)
        {
            txtNombreBusqueda.Enabled = booleano;
            txtPrecioMinBusqueda.Enabled = booleano;
            txtPrecioMaxBusqueda.Enabled = booleano;
            comboBoxCategoriaBusqueda.Enabled = booleano;
            comboBoxMarcaBusqueda.Enabled = booleano;
        }
        private void checkBoxactivarFiltro_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxactivarFiltro.Checked == true) {
                txtBusquedaPrincipal.Enabled = false;
                btnBusquedaSimple.Enabled = false;
                activarDesactivarInputsFiltroAvanzado(true);
                btnBuscar.Enabled = true;
            } else
            {
                txtBusquedaPrincipal.Enabled = true;
                btnBusquedaSimple.Enabled = true;
                activarDesactivarInputsFiltroAvanzado(false);
                btnBuscar.Enabled = false;
            }
        }

        private void btnBusquedaSimple_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            string busqueda = txtBusquedaPrincipal.Text;

            List<Articulo> listaFiltrada = new List<Articulo>();

            try
            {
                dgvForm.DataSource = null;

                if (busqueda.Length == 0 )
                {
                    listaFiltrada = articulos;
                }
                else 
                {
                    listaFiltrada = negocio.buscarSoloNombre(busqueda);

                    if (listaFiltrada.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados", "Búsqueda");
                        listaFiltrada = articulos;
                        dgvForm.DataSource = listaFiltrada;
                        ocultarColumnas();
                    } else
                    {
                        dgvForm.DataSource = listaFiltrada;
                        ocultarColumnas();
                        btnLimpiarFiltroSimple.Visible = true;
                    }
                    
                }

                dgvForm.DataSource = null;
                dgvForm.DataSource = listaFiltrada;
                CultureInfo culturaAR = new CultureInfo("es-AR");
                dgvForm.Columns["Precio"].DefaultCellStyle.Format = "C2";
                dgvForm.Columns["Precio"].DefaultCellStyle.FormatProvider = culturaAR;
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void txtBusquedaPrincipal_Click(object sender, EventArgs e)
        {
            txtBusquedaPrincipal.Text = "";
        }

        private void btnLimpiarFiltroSimple_Click(object sender, EventArgs e)
        {
            txtBusquedaPrincipal.Text = "Buscar por nombre";
            btnLimpiarFiltroSimple.Visible = false;
            cargar();
        }

        private void btnLimpiarFiltroAvanzado_Click(object sender, EventArgs e)
        {
            btnLimpiarFiltroSimple.Visible = false;
            txtNombreBusqueda.Text = "";
            txtPrecioMaxBusqueda.Text = "0.00";
            txtPrecioMinBusqueda.Text = "0.00";
            cargar();
        }
    }

}
