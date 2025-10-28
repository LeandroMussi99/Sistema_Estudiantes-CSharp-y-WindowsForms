using Data;
using LinqToDB;
using Logica.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Logica
{
    public class LEstudiantes : Librarys
    {
        private List<TextBox> listTextBox;
        private List<Label> listLabel;
        private PictureBox pictureBoxImage;
        private Bitmap _imageBitmap;
        private DataGridView dataGridView;
        private NumericUpDown numericUpDown;
        private Paginador<Estudiante> _paginador;
        private string accion = "insert";
        public bool EsModoUpdate => accion == "update";


        private List<Estudiante> listEstudiante;
        private int idEstudianteSeleccionado = 0;
        public LEstudiantes(List<TextBox> listTextBox, List<Label> listLabel, object[] objetos)
        {
            this.listTextBox = listTextBox;
            this.listLabel = listLabel;
            pictureBoxImage = (PictureBox)objetos[0];
            _imageBitmap = (Bitmap)objetos[1];
            dataGridView = (DataGridView)objetos[2];
            numericUpDown = (NumericUpDown)objetos[3];

            BuscarEstudiante("");
        }


        public void Registrar()
        {

            // VALIDACIONES
            if (listTextBox[0].Text.Equals(""))
            { SetError(0, "El DNI es obligatorio."); return; }

            if (listTextBox[1].Text.Equals(""))
            { SetError(1, "El Nombre es obligatorio."); return; }

            if (listTextBox[2].Text.Equals(""))
            { SetError(2, "El Apellido es obligatorio."); return; }

            if (listTextBox[3].Text.Equals(""))
            { SetError(3, "El Email es obligatorio."); return; }

            if (!textBoxEvent.comprobarFormatoEmail(listTextBox[3].Text))
            {SetError(3, "Formato de Email Incorrecto."); return;}


            //VALIDACIÓN ROBUSTA DEL DNI
            var dniTexto = listTextBox[0].Text.Trim();
            if (!int.TryParse(dniTexto, out var dni) || dni < 1_000_000 || dni > 99_999_999)
            {
                SetError(0, "DNI inválido (7-8 dígitos).");
                return;
            }

            // Imagen (puede ser null y está OK)
            byte[] imageArray = null;
            if (pictureBoxImage.Image != null)
                imageArray = uploadimage.ImageToByte(pictureBoxImage.Image);

            using (var db = new Conexion())
            {
                try
                {
                    // INSERTAR EN LA BASE DE DATOS
                    db.BeginTransaction();

                    var email = (listTextBox[3].Text ?? "").Trim().ToLowerInvariant();

                    //Insertar
                    if (accion == "insert")
                    {
                        // Verificar si ya existe el DNI
                        bool dniExiste = db.EstudianteTabla.Any(u => u.Dni == dni);

                        if (dniExiste)
                        {
                            SetError(0, "DNI ya registrado.");
                            db.RollbackTransaction();
                            return;
                        }

                        // Verificar si ya existe el email
                        bool emailExiste = db.EstudianteTabla.Any(u => u.Email.ToLower() == email);

                        if (emailExiste)
                        {
                            SetError(3, "Email ya registrado.");
                            db.RollbackTransaction();
                            return;
                        }

                        db.Insert(new Estudiante()
                        {
                            Dni = dni,
                            Nombre = listTextBox[1].Text.Trim(),
                            Apellido = listTextBox[2].Text.Trim(),
                            Email = email,
                            Imagen = imageArray
                        });

                        db.CommitTransaction();

                        MessageBox.Show("Estudiante registrado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else  //Actualizar
                    {
                        // Traigo el actual desde la lista cacheada (tenés la imagen previa)
                        var actual = listEstudiante?.FirstOrDefault(x => x.Id == idEstudianteSeleccionado);
                        if (actual == null)
                        {
                            db.RollbackTransaction();
                            MessageBox.Show("No se encontró el estudiante a actualizar.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        bool dniExiste = db.EstudianteTabla.Any(u => u.Dni == dni && u.Id != idEstudianteSeleccionado);
                        if (dniExiste)
                        {
                            SetError(0, "DNI ya registrado.");
                            db.RollbackTransaction();
                            return;
                        }

                        bool emailExiste = db.EstudianteTabla.Any(u => u.Email == email && u.Id != idEstudianteSeleccionado);
                        if (emailExiste)
                        {
                            SetError(3, "Email ya registrado.");
                            db.RollbackTransaction();
                            return;
                        }

                        // Si no cambiaron la imagen (o la limpiaron), mantengo la previa
                        byte[] imagenFinal = imageArray ?? actual.Imagen;

                        // Update por SET encadenado (evita problemas con nulls)
                        db.EstudianteTabla
                          .Where(e => e.Id == idEstudianteSeleccionado)
                          .Set(e => e.Dni, dni)
                          .Set(e => e.Nombre, listTextBox[1].Text.Trim())
                          .Set(e => e.Apellido, listTextBox[2].Text.Trim())
                          .Set(e => e.Email, email)
                          .Set(e => e.Imagen, imagenFinal)
                          .Update();

                        db.CommitTransaction();
                        MessageBox.Show("Estudiante actualizado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Volver a modo insert
                        accion = "insert";
                    }

                    // Restablecer formulario
                    Restablecer();

                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    var oper = accion == "insert" ? "registrar" : "actualizar";
                    MessageBox.Show($"Error al {oper} el estudiante: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
        }

        public void BuscarEstudiante(string campo)
        {
            campo = (campo ?? "").Trim();

            using (var db = new Conexion())
            {
                // base query
                var q = db.EstudianteTabla.AsQueryable();

                // filtro (dni/nombre/apellido/email).
                if (!string.IsNullOrEmpty(campo))
                {
                    bool esNumero = int.TryParse(campo, out var dniBuscado);

                    q = q.Where(e =>
                        (esNumero && e.Dni == dniBuscado) ||
                        e.Dni.ToString().Contains(campo) ||
                        (e.Nombre ?? "").Contains(campo) ||
                        (e.Apellido ?? "").Contains(campo) ||
                        (e.Email ?? "").Contains(campo));
                }

                // Lista base ordenada para paginar
                listEstudiante = q
                    .OrderBy(e => e.Id)
                    .ToList();

                if (listEstudiante.Count > 0)
                {
                    int pageSize = (int)numericUpDown.Value;
                    if (pageSize < 1) pageSize = 1;

                    _paginador = new Paginador<Estudiante>(listEstudiante, listLabel[4], pageSize);

                    // (3) USAR HELPER PARA PINTAR LA PÁGINA
                    int inicio = _paginador.primero();
                    PintarPagina(inicio, pageSize);
                }
                else
                {
                    dataGridView.DataSource = null;
                    listLabel[4].Text = "No se encontraron registros.";
                }

            }

        }

        public void Paginador(string metodo)
        {
            if (_paginador == null || listEstudiante == null) return;

            int inicio = 0;
            switch (metodo)
            {
                case "primero": inicio = _paginador.primero(); break;
                case "anterior": inicio = _paginador.anterior(); break;
                case "siguiente": inicio = _paginador.siguiente(); break;
                case "ultimo": inicio = _paginador.ultimo(); break;
            }

            int pageSize = (int)numericUpDown.Value;
            if (pageSize < 1) pageSize = 1;

            PintarPagina(inicio, pageSize);
        }


        public void registroPaginas()
        {
            // Si todavía no hay lista (app recién carga), trae
            if (listEstudiante == null)
            {
                BuscarEstudiante(""); 
                return;
            }

            int pageSize = (int)numericUpDown.Value;
            if (pageSize < 1) pageSize = 1;

            // Re-crear el paginador con el nuevo tamaño y misma lista
            _paginador = new Paginador<Estudiante>(listEstudiante, listLabel[4], pageSize);

            // Ir a la primera página
            int inicio = _paginador.primero();
            PintarPagina(inicio, pageSize);
        }

        public void GetEstudiante()
        {
            if (dataGridView.CurrentRow == null) return;

            accion = "update";

            
            var id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value);

            // Buscamos el entity completo en la lista cacheada
            var est = listEstudiante.FirstOrDefault(x => x.Id == id);
            if (est == null) return;

            idEstudianteSeleccionado = est.Id;

            listTextBox[0].Text = est.Dni.ToString();
            listTextBox[1].Text = est.Nombre ?? "";
            listTextBox[2].Text = est.Apellido ?? "";
            listTextBox[3].Text = est.Email ?? "";

            // Imagen
            pictureBoxImage.Image?.Dispose(); // libera la anterior si existía
            pictureBoxImage.Image = est.Imagen != null ? ByteArrayToImage(est.Imagen)
                                                       : (_imageBitmap != null ? (Image)_imageBitmap.Clone() : null);
        }

        private void Restablecer()
        {
            // liberar la imagen actual (evita locks si cargaste desde archivo)
            pictureBoxImage.Image?.Dispose();

            // restaurar la imagen por defecto
            pictureBoxImage.Image = _imageBitmap != null ? (Image)_imageBitmap.Clone() : null;

            // limpiar entradas y etiquetas
            foreach (var tb in listTextBox) tb.Clear();
            listLabel[0].Text = "DNI";
            listLabel[1].Text = "Nombre";
            listLabel[2].Text = "Apellido";
            listLabel[3].Text = "Email";
            foreach (var lbl in listLabel) lbl.ForeColor = Color.SteelBlue;
            BuscarEstudiante("");
        }


        public void LimpiarCampos()
        {
            accion = "insert";
            idEstudianteSeleccionado = 0;

            // liberar la imagen actual (si corresponde)
            pictureBoxImage.Image?.Dispose();
            pictureBoxImage.Image = _imageBitmap != null ? (Image)_imageBitmap.Clone() : null;

            // limpiar cajas y labels
            foreach (var tb in listTextBox) tb.Clear();
            listLabel[0].Text = "DNI";
            listLabel[1].Text = "Nombre";
            listLabel[2].Text = "Apellido";
            listLabel[3].Text = "Email";
            foreach (var lbl in listLabel) lbl.ForeColor = Color.SteelBlue;

            // opcional: limpiar selección del grid
            dataGridView.ClearSelection();

            // refrescar lista (si querés mantener la tabla visible actualizada)
            BuscarEstudiante("");
        }

        public void EliminarEstudiante()
        {
            // Debe estar en modo update (alguien cargado con GetEstudiante)
            if (accion != "update" || idEstudianteSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un estudiante (clic en la fila) para eliminar.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Tomo los datos desde los textboxes para el diálogo (coinciden con lo cargado)
            var dni = listTextBox[0].Text.Trim();
            var nombre = listTextBox[1].Text.Trim();
            var apellido = listTextBox[2].Text.Trim();

            var confirm = MessageBox.Show(
                $"¿Eliminar al estudiante {nombre} {apellido} (DNI {dni})?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var db = new Conexion())
                {
                    db.BeginTransaction();

                    var borrados = db.EstudianteTabla
                                     .Where(e => e.Id == idEstudianteSeleccionado)
                                     .Delete();

                    db.CommitTransaction();

                    if (borrados == 0)
                    {
                        MessageBox.Show("El registro ya no existe.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                // Volver a modo inserción y refrescar UI
                LimpiarCampos();          // tu helper que hace accion="insert" y Restablecer()
                MessageBox.Show("Estudiante eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el estudiante: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // =========================================================
        // (3) HELPER: renderiza la página actual en el DataGridView
        // =========================================================
        private void PintarPagina(int inicio, int pageSize)
        {
            if (listEstudiante == null) { dataGridView.DataSource = null; return; }

            var datosPagina = listEstudiante
                .Skip(inicio)
                .Take(pageSize)
                .Select(e => new { e.Id, e.Dni, e.Nombre, e.Apellido, e.Email })
                .ToList();

            dataGridView.DataSource = datosPagina.Count > 0 ? datosPagina : null;

            if (dataGridView.Columns.Count > 0)
                dataGridView.Columns[0].Visible = false; // oculta Id

            dataGridView.ClearSelection();
            accion = "insert";
            idEstudianteSeleccionado = 0;
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            using (var ms = new System.IO.MemoryStream(bytes))
                return Image.FromStream(ms);
        }
        private void SetError(int idx, string msg)
        {
            listLabel[idx].Text = msg;
            listLabel[idx].ForeColor = Color.Red;
            listTextBox[idx].Focus();
        }

    }
}
