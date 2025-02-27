using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;

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
            this.WindowState |= FormWindowState.Minimized;
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
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVer_Click(object sender, EventArgs e)
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

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                articulos = negocio.listarArticulos();
                dgvForm.DataSource = articulos;
                ocultarColumnas();

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
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmArticulo nuevaVentana = new frmArticulo();
            nuevaVentana.ShowDialog();
        }

        private void btnActualizarListado_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            DialogResult resultado = MessageBox.Show("¿Deseas borrar el articulo de la base de datos? ", "Eliminar",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            try
            {
                if (resultado == DialogResult.Yes )
                {
                    Articulo articulo = (Articulo)dgvForm.CurrentRow.DataBoundItem;
                    negocio.eliminarArticulo(articulo.Id);
                    MessageBox.Show("Se elimino correctamente. Actualizar la lista para ver los cambios", "Eliminar");
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
        
            try
            {
                articuloEditar = (Articulo)dgvForm.CurrentRow.DataBoundItem;
                frmArticulo formArticulo = new frmArticulo(articuloEditar);
                formArticulo.EditarArticulo = true;
                formArticulo.ShowDialog();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()) ;
            }
        }
    }



}
