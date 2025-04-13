using Facultad.Entidades;
using Facultad.Persistencia;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
  
using Microsoft.VisualBasic;


namespace Facultad
{
    public partial class FormInicio : Form
    {
        PersistenciaUtils persistenciaUtils = new PersistenciaUtils();
        Validador validador = new Validador();
        PersistenciaUtils persistencia = new PersistenciaUtils();

        public FormInicio()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1) Validaciones



            // 1.1) Validaciones de integridad de datos

            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            string resultadoValidacion = validador.ValidarCamposNoVacios(usuario, password);
            if (resultadoValidacion != "Campos válidos.")
            {
                MessageBox.Show(resultadoValidacion, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener ejecución si hay error
            }

            // 1.) Validaciones de negocio


            string resultadoLongitudUsuario = validador.ValidarLongitudUsuario(usuario);
            if (resultadoLongitudUsuario != "Usuario válido.")
            {
                MessageBox.Show(resultadoLongitudUsuario, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener ejecución si hay error
            }
            // Validar longitud del usuario


            string resultadoLongitudPassword = validador.ValidarLongitudPassword(password);
            if (resultadoLongitudPassword != "Contraseña válida.")
            {
                MessageBox.Show(resultadoLongitudPassword, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener ejecución si hay error
            }

            // Validar credenciales contra el archivo credenciales.csv
            string resultadoCredenciales = validador.ValidarCredencialesDesdeArchivo(usuario, password);
            if (resultadoCredenciales != "Credenciales válidas.")
            {
                MessageBox.Show(resultadoCredenciales, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener ejecución si las credenciales no son válidas
            }

            // Validar si es la primera vez que inicia sesión
            string resultadoPrimeraVez = validador.ValidarPrimeraVez(usuario);
            if (resultadoPrimeraVez.Contains("Es la primera vez"))
            {
                MessageBox.Show(resultadoPrimeraVez, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Solicitar nueva contraseña
                string nuevaPassword = Microsoft.VisualBasic.Interaction.InputBox(
                    "Por favor, ingresa una nueva contraseña:",
                    "Cambio de Contraseña",
                    "");

                if (string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 6)
                {
                    MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Actualizar la contraseña en el archivo
                persistenciaUtils.ActualizarPassword("credenciales.csv", usuario, nuevaPassword);
                MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            // 1.4) Expira password?
            string resultadoVencimiento = validador.ValidarVencimientoContraseña(usuario);
            if (resultadoVencimiento.Contains("ha expirado"))
            {
                MessageBox.Show(resultadoVencimiento, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            // 2) Redirigir
            this.Hide();
            FormMenu formMenu = new FormMenu();
            formMenu.ShowDialog();
        }

        private List<String> obtenerUsuarios()
        {
            List<String> listado = persistenciaUtils.LeerRegistro("credenciales.csv");

            return listado;
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
