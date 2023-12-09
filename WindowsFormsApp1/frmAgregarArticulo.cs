using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using datos;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

namespace WindowsFormsApp1
{
    public partial class frmAgregarArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAgregarArticulo()
        {
            InitializeComponent();
        }
        public frmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private bool soloNumeros(string cadena)
        {
            decimal number;
            return decimal.TryParse(cadena,out number);
        }
        private bool validarPrecio()
        {
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("Debes cargar el Precio");
                return true;
            }

            if (!(soloNumeros(txtPrecio.Text))) 
            {
                MessageBox.Show("Solo se puede ingresar números");
                return true;
            }
            return false;

           
        }
        private bool validarNombre()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debes colocar un nombre al artículo");
                return true;
            }
            return false;    
        }
        private bool validarCategoria()
        {
            if (cboCategoria.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione una categoria.");
                return true;
            }
            return false;
        }

         


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null) 
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                
                if (validarNombre())
                    return;
                articulo.Nombre = txtNombre.Text;                
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagenUrl.Text;
                
                if(validarPrecio())
                    return;               
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                
                if( articulo.Id != 0) 
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Se modifico correctamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Se agregó correctamente");
                }
                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["carpetaImagen"] + archivo.SafeFileName);


                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            

        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }


            }
            catch (Exception ex) 
            {
                MessageBox.Show("Algo salío mal, intentemos de nuevo");
            }
            
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxAgregarArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxAgregarArticulo.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if ( archivo.ShowDialog() == DialogResult.OK ) 
            { 
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

            }
        }
    }
}
