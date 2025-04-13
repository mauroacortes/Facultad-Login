using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facultad.Persistencia
{
    public class PersistenciaUtils
    {
        string archivoCsv = @"C:\Users\mauri\Downloads\ejercicio01_Facultad-01-Login\";
        public List<String> LeerRegistro(String nombreArchivo)
        {
            archivoCsv = archivoCsv + nombreArchivo; // Cambia esta ruta al archivo CSV que deseas leer

            String rutaArchivo = Path.GetFullPath(archivoCsv); // Normaliza la ruta

            List<String> listado = new List<String>();

            try
            {
                using (StreamReader sr = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        listado.Add(linea);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo leer el archivo:");
                Console.WriteLine(e.Message);
            }
            return listado;
        }

        // Método para borrar un registro
        public void BorrarRegistro(string id, String nombreArchivo)
        {
            archivoCsv = archivoCsv + nombreArchivo; // Cambia esta ruta al archivo CSV que deseas leer

            String rutaArchivo = Path.GetFullPath(archivoCsv); // Normaliza la ruta

            try
            {
                // Verificar si el archivo existe
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine("El archivo no existe: " + archivoCsv);
                    return;
                }

                // Leer el archivo y obtener las líneas
                List<string> listado = LeerRegistro(nombreArchivo);

                // Filtrar las líneas que no coinciden con el ID a borrar (comparar solo la primera columna)
                var registrosRestantes = listado.Where(linea =>
                {
                    var campos = linea.Split(';');
                    return campos[0] != id; // Verifica solo el ID (primera columna)
                }).ToList();

                // Sobrescribir el archivo con las líneas restantes
                File.WriteAllLines(archivoCsv, registrosRestantes);

                Console.WriteLine($"Registro con ID {id} borrado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar borrar el registro:");
                Console.WriteLine($"Mensaje: {e.Message}");
                Console.WriteLine($"Pila de errores: {e.StackTrace}");
            }
        }

        // Método para agregar un registro
        public void AgregarRegistro(string nombreArchivo, string nuevoRegistro)
        {
            string archivoCsv = Path.Combine(Directory.GetCurrentDirectory(), "Persistencia", "Datos", nombreArchivo);

            try
            {
                // Verificar si el archivo existe
                if (!File.Exists(archivoCsv))
                {
                    Console.WriteLine("El archivo no existe: " + archivoCsv);
                    return;
                }

                // Abrir el archivo y agregar el nuevo registro
                using (StreamWriter sw = new StreamWriter(archivoCsv, append: true))
                {
                    sw.WriteLine(nuevoRegistro); // Agregar la nueva línea
                }

                Console.WriteLine("Registro agregado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar agregar el registro:");
                Console.WriteLine($"Mensaje: {e.Message}");
                Console.WriteLine($"Pila de errores: {e.StackTrace}");
            }
        }

        public void ActualizarPassword(string nombreArchivo, string usuario, string nuevaPassword)
        {
            string rutaArchivo = Path.Combine(archivoCsv, nombreArchivo);

            try
            {
                // Leer todos los registros del archivo
                List<string> registros = LeerRegistro(nombreArchivo);

                // Crear una lista para almacenar los registros actualizados
                List<string> registrosActualizados = new List<string>();

                foreach (string registro in registros)
                {
                    // Suponiendo que el formato del archivo es "usuario;password;fechaRegistro;ultimaFechaIngreso"
                    string[] partes = registro.Split(';');
                    if (partes.Length >= 4)
                    {
                        string usuarioArchivo = partes[0].Trim();

                        // Si el usuario coincide, actualizar la contraseña
                        if (usuarioArchivo == usuario)
                        {
                            partes[1] = nuevaPassword; // Actualizar el segundo campo (contraseña)
                        }

                        // Reconstruir el registro actualizado y agregarlo a la lista
                        registrosActualizados.Add(string.Join(";", partes));
                    }
                    else
                    {
                        // Si el registro no tiene el formato esperado, agregarlo sin cambios
                        registrosActualizados.Add(registro);
                    }
                }

                // Sobrescribir el archivo con los registros actualizados
                File.WriteAllLines(rutaArchivo, registrosActualizados);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar actualizar la contraseña:");
                Console.WriteLine($"Mensaje: {e.Message}");
                Console.WriteLine($"Pila de errores: {e.StackTrace}");
            }
        }


    }
}
