using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;

namespace BPMO.Refacciones.Catalogos.VIS {
    /// <summary>
    /// Vista para el manejo de la MDI
    /// </summary>
    public interface IMDI {

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un listado de datos de conexión para el manejo de la MDI
        /// </summary>
        List<DatosConexionBO> ListadoDatosConexion { get; set; }
        /// <summary>
        /// Obtiene o establece el usuario para el manejo de la MDI
        /// </summary>
        UsuarioBO Usuario { get; set; }
        /// <summary>
        /// Obtiene o establece la adscripción de servicio para el manejo de la MDI
        /// </summary>
        AdscripcionBO Adscripcion { get; set; }
        /// <summary>
        /// Obtiene o establece el ambiente para el manejo de la MDI
        /// </summary>
        string Ambiente { get; set; }
        /// <summary>
        /// Obtiene o establece un listado de procesos para el manejo de la MDI
        /// </summary>
        List<ProcesoBO> ListadoProcesos { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">1: Éxito, 2: Advertencia, 3: Error, 4: Información, 5: Confirmación</param>
        /// <param name="detalle">Desplega el detalle del mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string detalle = null);
        /// <summary>
        /// Carga los procesos a los que el usuario tiene acceso
        /// </summary>
        void CargarProcesos();
        /// <summary>
        /// MenuPredeterminado
        /// </summary>
        void MenuPredeterminado();
        #endregion
    }
}
