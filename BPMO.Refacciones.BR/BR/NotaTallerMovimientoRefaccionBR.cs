using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de relaciones de notas de taller y movimientos de refacciones
    /// </summary>
    public class NotaTallerMovimientoRefaccionBR {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Metodos
        /// <summary>
        /// Crea un registro de relaciones de movimientos de refacciones y notas de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="movimiento">Relacion de movimiento y nota de taller que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, NotaTallerMovimientoRefaccionBO movimiento, SeguridadBO firma) {
            try {
                NotaTallerMovimientoRefaccionInsertarDAO insertarDAO = new NotaTallerMovimientoRefaccionInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, movimiento);
                registrosAfectados = insertarDAO.RegistrosAfectados;
                return esExito;
            } catch {
                throw;
            }
        }
        #endregion Metodos
    }
}
