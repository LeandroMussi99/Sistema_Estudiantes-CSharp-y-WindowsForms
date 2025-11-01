using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estudiantes
{
    public partial class Form1 : Form
    {
        private LEstudiantes estudiante;

        // textos base de los labels
        private const string TXT_DNI = "DNI";
        private const string TXT_NOMBRE = "Nombre";
        private const string TXT_APELLIDO = "Apellido";
        private const string TXT_EMAIL = "Email";

        public Form1()
        {
            InitializeComponent();

            var listTextBox = new List<TextBox> { textBoxDni, textBoxNombre, textBoxApellido, textBoxEmail };
            var listLabel = new List<Label> { labelDni, labelNombre, labelApellido, labelEmail, labelPaginas };

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 50;
            numericUpDown1.Value = 5;
            Object[] objects = { pictureBoxImage, Properties.Resources.logo, dataGridView1, numericUpDown1 };

            estudiante = new LEstudiantes(listTextBox, listLabel, objects);

            this.Load += Form1_Load;
            this.Resize += (s, e) => CentrarPanelIzquierdo();
            this.splitContainer1.Panel1.Resize += (s, e) => CentrarPanelIzquierdo();

            textBoxDni.TextChanged += textBoxDni_TextChanged;
            textBoxDni.KeyPress += textBoxDni_KeyPress;
            textBoxDni.Leave += textBoxDni_Leave;

            textBoxNombre.TextChanged += textBoxNombre_TextChanged;
            textBoxNombre.KeyPress += textBoxNombre_KeyPress;
            textBoxNombre.Leave += textBoxNombre_Leave;

            textBoxApellido.TextChanged += textBoxApellido_TextChanged;
            textBoxApellido.KeyPress += textBoxApellido_KeyPress;
            textBoxApellido.Leave += textBoxApellido_Leave;

            textBoxEmail.TextChanged += textBoxEmail_TextChanged;
            textBoxEmail.Leave += textBoxEmail_Leave;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // forzamos a que el contenido de arriba NO esté dockeado
            tableLeft.Dock = DockStyle.None;     
            tableLeft.AutoSize = true;

            
            tableBotonesLeft.Dock = DockStyle.Bottom;

            // primer centrado
            CentrarPanelIzquierdo();

            // que el dgv no se achique ridículo si hay mucho ancho
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void CentrarPanelIzquierdo()
        {
            // panel izquierdo del split
            var panel = this.splitContainer1.Panel1;

            // el contenido de arriba (titulo + img + campos)
            var contenido = this.tableLeft;

            // la botonera de abajo
            var botones = this.tableBotonesLeft;

            // alto que queda sin contar los botones
            int altoDisponible = panel.ClientSize.Height - botones.Height;

            // alto real del contenido
            int altoContenido = contenido.Height;

            // calculamos top para centrar
            int nuevoTop = (altoDisponible - altoContenido) / 2;
            if (nuevoTop < 10) nuevoTop = 10;

            contenido.Top = nuevoTop;

            // centrado horizontal dentro del panel
            int nuevoLeft = (panel.ClientSize.Width - contenido.Width) / 2;
            if (nuevoLeft < 10) nuevoLeft = 10;

            contenido.Left = nuevoLeft;
        }

        // ===============================
        //  HELPERs de labels
        // ===============================

        private void PintarLabel(Label lbl, string mensaje, bool ok, string textoBase)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
            {
                lbl.Text = textoBase;
                lbl.ForeColor = Color.SteelBlue;
                return;
            }

            lbl.Text = mensaje;
            lbl.ForeColor = ok ? Color.Green : Color.Red;
        }


        private void pictureBoxImage_Click(object sender, EventArgs e)
        {
            estudiante.uploadimage.CargarImagen(pictureBoxImage);
        }

        private void textBoxDni_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDni.Text))
            {
                labelDni.Text = "DNI";
                labelDni.ForeColor = Color.SteelBlue;
            }
        }

        private void textBoxDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.numberKeyPress(e);
        }

        private void textBoxDni_Leave(object sender, EventArgs e)
        {
            var (ok, msg) = estudiante.ValidarCampo("dni", textBoxDni.Text, estudiante.EstudianteSeleccionadoId);
            PintarLabel(labelDni, msg, ok, "DNI");
        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                labelNombre.Text = "Nombre";
                labelNombre.ForeColor = Color.SteelBlue;
            }

        }

        private void textBoxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }

        private void textBoxNombre_Leave(object sender, EventArgs e)
        {
            var (ok, msg) = estudiante.ValidarCampo("nombre", textBoxNombre.Text, estudiante.EstudianteSeleccionadoId);
            PintarLabel(labelNombre, msg, ok, "Nombre");
        }

        private void textBoxApellido_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxApellido.Text))
            {
                labelApellido.Text = "Apellido";
                labelApellido.ForeColor = Color.SteelBlue;
            }
        }

        private void textBoxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }


        private void textBoxApellido_Leave(object sender, EventArgs e)
        {
            var (ok, msg) = estudiante.ValidarCampo("apellido", textBoxApellido.Text, estudiante.EstudianteSeleccionadoId);
            PintarLabel(labelApellido, msg, ok, "Apellido");
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                labelEmail.Text = "Email";
                labelEmail.ForeColor = Color.SteelBlue;
            }
        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {
            var (ok, msg) = estudiante.ValidarCampo("email", textBoxEmail.Text, estudiante.EstudianteSeleccionadoId);
            PintarLabel(labelEmail, msg, ok, "Email");
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            estudiante.Registrar();
            if (!estudiante.EsModoUpdate)
                buttonAgregar.Text = "Agregar";
        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            estudiante.BuscarEstudiante(textBoxBuscar.Text);
        }

        private void buttonPrimero_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("primero");
        }

        private void buttonAnterior_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("anterior");
        }

        private void buttonSiguiente_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("siguiente");
        }

        private void buttonUltimo_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("ultimo");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            estudiante.registroPaginas();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || e.RowIndex < 0) return;
            estudiante.GetEstudiante();
            buttonAgregar.Text = "Actualizar";
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;
            estudiante.GetEstudiante();
            buttonAgregar.Text = "Actualizar";
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            estudiante.LimpiarCampos();
            buttonAgregar.Text = "Agregar";
        }

        private void buttonBorrar_Click(object sender, EventArgs e)
        {
            estudiante.EliminarEstudiante();
            buttonAgregar.Text = "Agregar"; 
        }

    }
}
