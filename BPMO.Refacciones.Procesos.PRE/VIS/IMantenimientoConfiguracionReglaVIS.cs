using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;
using System;

namespace BPMO.Refacciones.Procesos.VIS {
    /// <summary>
    /// Vista para el mantenimiento de Configuración Regla
    /// </summary>
    public interface IMantenimientoConfiguracionReglaVIS {
        #region Propiedades
        /// <summary>
        /// Usuario logueado
        /// </summary>
        UsuarioBO UsuarioSesion { get; }
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
        ETipoReglaUsuario? TipoRegla { get; set; }
        /// <summary>
        /// Obtiene o establece el valor inicial de configuración
        /// </summary>
        decimal? ValorInicial { get; set; }
        /// <summary>
        /// Obtiene o establece el valor final de configuración en un rango
        /// </summary>
        decimal? ValorFinal { get; set; }
        /// <summary>
        /// Obtiene el estatus de la Configuración
        /// </summary>
        bool? Activo { get; set; }
        
        #region DatosBitacora
        /// <summary>
        /// Establece el UsuarioCreacionBitacora
        /// </summary>
        string UsuarioCreacionBitacora { set; }
        /// <summary>
        /// Establece la FechaCreacionBitacora
        /// </summary>
        DateTime? FechaCreacionBitacora { set; }
        /// <summary>
        /// Establece el UsuarioActualizacionBitacora
        /// </summary>
        string UsuarioActualizacionBitacora { set; }
        /// <summary>
        /// Establece la FechaActualizacionBitacora
        /// </summary>
        DateTime? FechaActualizacionBitacora { set; }
        #endregion
        
        /// <summary>
        /// Objeto base de Configuración
        /// </summary>
        object ConfiguracionReglaBase { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la interfaz de usuario para insertar
        /// </summary>
        void PreparaUIInsertar();
        /// <summary>
        /// Prepara la interfaz de usuario para Actualizar
        /// </summary>
        void PreparaUIActualizar();
        /// <summary>
        /// Prepara la interfaz de usuario de detalle
        /// </summary>
        void PreparaUIDetalle();
        /// <summary>
        /// Método de la interfaz a ser implementado
        /// </summary>
        /// <param name="mensaje">mensaje</param>
        /// <param name="tipo">tipo de mensaje</param>
        /// <param name="detalle">detalle del mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}
