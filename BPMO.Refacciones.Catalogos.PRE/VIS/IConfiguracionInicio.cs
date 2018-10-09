using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;

namespace BPMO.Refacciones.Catalogos.VIS {
    /// <summary>
    /// Vista para el manejo de la vista ConfiguraciónInicio
    /// </summary>
    public interface IConfiguracionInicio {

        #region Propiedades
        /// <summary>
        /// Obtiene la UnidadOperativa
        /// </summary>
        int? UnidadOperativa { get; }
        /// <summary>
        /// Obtiene la Sucursal
        /// </summary>
        int? Sucursal { get; }
        /// <summary>
        /// Obtiene una lista de datos de conexión
        /// </summary>
        List<DatosConexionBO> ListaDatosConexion { get; }
        /// <summary>
        /// Obtiene el usuario
        /// </summary>
        UsuarioBO Usuario { get; }
        /// <summary>
        /// Obtiene o establece la Adscripción
        /// </summary>
        AdscripcionBO Adscripcion { get; set; }
        /// <summary>
        /// Obtiene o establece una lista de adscripciones
        /// </summary>
        List<AdscripcionBO> Adscripciones { get; set; }
        /// <summary>
        /// Obtiene o establece una lista de procesos
        /// </summary>
        List<ProcesoBO> ListaProcesos { get; set; }           
        #endregion

        #region Métodos
        /// <summary>
        /// Método de la interfaz a ser implementado
        /// </summary>
        /// <param name="mensaje">mensaje</param>
        /// <param name="tipo">tipo de mensaje</param>
        /// <param name="detalle">detalle del mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Método de la interfaz a ser implementado
        /// </summary>
        void EnviarAInicio();
        /// <summary>
        /// Método de la interfaz a ser implementado que carga los datos de la adscripción
        /// </summary>
        void CargarDatosAdscripcion();
        #endregion
    }
}
