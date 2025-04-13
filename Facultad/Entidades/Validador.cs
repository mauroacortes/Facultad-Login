using Facultad.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facultad.Entidades
{
    internal class Validador
    {
        public string ValidarCredenciales(string usuario, string password)
        {
            if (usuario.Length < 6)
            {
                return "El usuario debe tener al menos 6 caracteres.";
            }
            if (password.Length < 6)
            {
                return "La contraseña debe tener al menos 6 caracteres.";
            }
            return "Credenciales válidas.";
        }

        public string ValidarCambioPassword(string password, string nuevoPassword)
        {
            if (password == nuevoPassword)
            {
                return "La nueva contraseña no puede ser igual a la anterior.";
            }
            if (nuevoPassword.Length < 6)
            {
                return "La nueva contraseña debe tener al menos 6 caracteres.";
            }
            return "Cambio de contraseña válido.";
        }

        public string ValidarExpiracionPassword(DateTime fechaUltimoCambio)
        {
            DateTime fechaActual = DateTime.Now;
            TimeSpan diferencia = fechaActual - fechaUltimoCambio;
            if (diferencia.Days > 90)
            {
                return "La contraseña ha expirado. Debe cambiarla.";
            }
            return "La contraseña es válida.";
        }

        public string ValidarPrimeraVez(string usuario)
        {
            // Aquí podrías implementar la lógica para verificar si es la primera vez que el usuario inicia sesión
            // Por ejemplo, podrías consultar una base de datos o un archivo de configuración
            // En este caso, simplemente devolveremos un mensaje de ejemplo
            PersistenciaUtils persistenciaUtils = new PersistenciaUtils();
            List<String> listado = persistenciaUtils.LeerRegistro("credenciales");
            // Verificar si el usuario existe en el archivo
            foreach (string registro in listado)
            {
                // Formato esperado: "usuario;contraseña;último ingreso;expiración de la contraseña"
                string[] partes = registro.Split(';');
                if (partes.Length >= 3)
                {
                    string usuarioArchivo = partes[0].Trim();
                    string ultimoIngreso = partes[2].Trim();

                    if (usuario == usuarioArchivo)
                    {
                        if (string.IsNullOrWhiteSpace(ultimoIngreso))
                        {
                            return "Es la primera vez que inicias sesión. Por favor, cambia tu contraseña.";
                        }
                        else
                        {
                            return "El usuario ya ha iniciado sesión anteriormente.";
                        }
                    }
                }
            }

            return "Usuario no encontrado.";

         }

        public string ValidarLongitudUsuario(string usuario)
        {
            if (usuario.Length < 6)
            {
                return "El usuario debe tener al menos 6 caracteres.";
            }
            return "Usuario válido.";
        }

        public string ValidarLongitudPassword(string password)
        {
            if (password.Length < 6)
            {
                return "La contraseña debe tener al menos 6 caracteres.";
            }
            return "Contraseña válida.";
        }

        public string ValidarCamposNoVacios(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return "El campo de usuario no puede estar vacío.";
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return "El campo de contraseña no puede estar vacío.";
            }
            return "Campos válidos.";
        }

        public string ValidarCredencialesDesdeArchivo(string usuario, string password)
        {
            // Instancia de PersistenciaUtils para leer el archivo
            PersistenciaUtils persistenciaUtils = new PersistenciaUtils();
            List<string> registros = persistenciaUtils.LeerRegistro("credenciales.csv");

            // Verificar si el usuario y la contraseña coinciden con algún registro
            foreach (string registro in registros)
            {
                // Suponiendo que el formato del archivo es "usuario,password"
                string[] partes = registro.Split(';');
                if (partes.Length >= 2)
                {
                    string usuarioArchivo = partes[0].Trim();
                    string passwordArchivo = partes[1].Trim();

                    if (usuario == usuarioArchivo && password == passwordArchivo)
                    {
                        return "Credenciales válidas.";
                    }
                }
            }

            return "Usuario o contraseña incorrectos.";
        }

        public string ValidarVencimientoContraseña(string usuario)
        {
            // Instancia de PersistenciaUtils para leer el archivo
            PersistenciaUtils persistenciaUtils = new PersistenciaUtils();
            List<string> registros = persistenciaUtils.LeerRegistro("credenciales.csv");

            // Verificar si el usuario existe en el archivo y si su contraseña ha expirado
            foreach (string registro in registros)
            {
                // Formato esperado: "usuario;contraseña;último ingreso;vencimiento de la contraseña"
                string[] partes = registro.Split(';');
                if (partes.Length >= 4)
                {
                    string usuarioArchivo = partes[0].Trim();
                    string fechaVencimiento = partes[3].Trim();

                    if (usuario == usuarioArchivo)
                    {
                        // Intentar convertir la fecha de vencimiento a DateTime
                        if (DateTime.TryParse(fechaVencimiento, out DateTime fechaVencimientoDate))
                        {
                            if (fechaVencimientoDate < DateTime.Now)
                            {
                                return "La contraseña ha expirado. Debe cambiarla.";
                            }
                            else
                            {
                                return "La contraseña es válida.";
                            }
                        }
                        else
                        {
                            return "El formato de la fecha de vencimiento es inválido.";
                        }
                    }
                }
            }

            return "Usuario no encontrado.";
        }


    }
}
