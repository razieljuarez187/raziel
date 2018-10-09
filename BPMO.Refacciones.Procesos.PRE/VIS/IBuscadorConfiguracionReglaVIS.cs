using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.Procesos.VIS {
    /// <summary>
    /// Vista para el manejo de Consulta de Configuración de Regla
    /// </summary>
    public interface IBuscadorConfiguracionReglaVIS {
        #region Propiedades
        /// <summary>
        /// Obtiene la adscripción actual
        /// </summary>
        AdscripcionBO Adscripcion { get; }
        /// <summary>
        /// Obtiene o establece el identificador de la Configuración
        /// </summary>
        int? Id { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la Empresa
        /// </summary>
        int? EmpresaId { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la Empresa
        /// </summary>
        string NombreEmpresa { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la Sucursal
        /// </summary>
        int? SucursalId { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la Sucursal
        /// </summary>
        string NombreSucursal { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Almacén
        /// </summary>
        int? AlmacenId { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del Almacén
        /// </summary>
        string NombreAlmacen { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Usuario
        /// </summary>
        int? UsuarioId { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del Usuario
        /// </summary>
        string NombreUsuario { get; set; }
        /// <summary>
        /// Obtiene el Tipo de Configuración
        /// </summary>
        ETipoReglaUsuario? TipoRegla { get;  }
        /// <summary>
        /// Obtiene el estatus de la Configuración
        /// </summary>
        bool? Activo { get; }
        /// <summary>
        /// Obtiene o establece un listado de Configuraciones de Reglas
        /// </summary>
        List<ConfiguracionReglaUsuarioBO> ListadoConfiguracionRegla { get; set; }
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
        /// Método a implementar para enviar a Editar la OrdenServicio
        /// </summary>
        /// <param name="OrdenServicioId"></param>
        void EnviarAEditarBO(int Id);
        #endregion
    }
}
