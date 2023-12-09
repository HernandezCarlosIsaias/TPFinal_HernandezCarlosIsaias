using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using datos;
using dominio;


namespace WindowsFormsApp1

{
    public partial class frmArticulo : Form

    {
        private List<Articulo> listArticulo;

        public frmArticulo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();     
        }
        private void cargar() 
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                listArticulo = negocio.listar();
                dgvArticulos.DataSource = listArticulo;
                ocultarColumnas();
                cargarImagen(listArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }


        private void DgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }

        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }
        }

        private void agregarArticulo_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo alta = new frmAgregarArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void modificarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if (validarTexto())
                    return;

                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmAgregarArticulo modificar = new frmAgregarArticulo(seleccionado);
                modificar.ShowDialog();
                cargar();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Algo salio mal,volvamos a intentarlo");
            }
        }
        private bool validarTexto()
           
        {


            if (dgvArticulos.CurrentRow == null)
            {
                MessageBox.Show("Debes seleccionar un artículo");
                return true;
            }
          
            return false;
        }
        private void eliminacionFisicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articulo = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                if (validarTexto())
                    return;

                
                DialogResult resultado = MessageBox.Show("¿Queres eliminar este Articulo?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
               
                if(resultado == DialogResult.OK)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    articulo.eliminar(seleccionado.Id);
                    MessageBox.Show("Se Eliminó correctamente");
                    cargar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar");
            }
        }



        private void txtFiltro_TextChanged(object sender, EventArgs e)
       {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                listaFiltrada = listArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper())|| x.Precio.ToString().Contains(filtro)|| x.Id.ToString().Contains(filtro));

            }
            else
            {
                listaFiltrada = listArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }
    }
}
