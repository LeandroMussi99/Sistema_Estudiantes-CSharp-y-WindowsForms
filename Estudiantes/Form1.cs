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
        public Form1()
        {
            InitializeComponent();

            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBoxDni);
            listTextBox.Add(textBoxNombre);
            listTextBox.Add(textBoxApellido);
            listTextBox.Add(textBoxEmail);
            var listLabel = new List<Label>();
            listLabel.Add(labelDni);
            listLabel.Add(labelNombre);
            listLabel.Add(labelApellido);
            listLabel.Add(labelEmail);
            listLabel.Add(labelPaginas);
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 50;
            numericUpDown1.Value = 5;
            Object[] objects = { pictureBoxImage, Properties.Resources.logo, dataGridView1, numericUpDown1 };
            estudiante = new LEstudiantes(listTextBox, listLabel, objects);
        }



        private void pictureBoxImage_Click(object sender, EventArgs e)
        {
            estudiante.uploadimage.CargarImagen(pictureBoxImage);
        }

        private void textBoxDni_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDni.Text.Equals(""))
            { 
                labelDni.ForeColor = Color.SteelBlue;
            }
            else
            {
                labelDni.ForeColor = Color.Green;
                labelDni.Text = "DNI ✔";
            }   
        }

        private void textBoxDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.numberKeyPress(e);
        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNombre.Text.Equals(""))
            {
                labelNombre.ForeColor = Color.SteelBlue;
            }
            else
            {
                labelNombre.ForeColor = Color.Green;
                labelNombre.Text = "Nombre ✔";
            }

        }

        private void textBoxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }

        private void textBoxApellido_TextChanged(object sender, EventArgs e)
        {
            if (textBoxApellido.Text.Equals(""))
            {
                labelApellido.ForeColor = Color.SteelBlue;
            }
            else
            {
                labelApellido.ForeColor = Color.Green;
                labelApellido.Text = "Apellido ✔";
            }
        }

        private void textBoxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            if (textBoxEmail.Text.Equals(""))
            {
                labelEmail.ForeColor = Color.SteelBlue;
            }
            else
            {
                labelEmail.ForeColor = Color.Green;
                labelEmail.Text = "Email ✔";
            }

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
