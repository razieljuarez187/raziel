using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;
using BPMO.Basicos.BO;
using System.Data;

namespace BPMO.Refacciones.Procesos.REPORTESVIS {
    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguracionesAsignadasVIS {
        #region Propiedades
        /// <summary>
        /// Obtiene el usuario que está logueado
        /// </summary>
        UsuarioBO UsuarioSesion { get; }
        /// <summary>
        /// Obtiene la adscripción actual
        /// </summary>
        AdscripcionBO Adscripcion { get; }
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
        ETipoReglaUsuario? TipoRegla { get; }
        /// <summary>
        /// Obtiene el valor inicial de configuración
        /// </summary>
        decimal? ValorInicialA { get; }
        /// Obtiene el valor inicial de configuración
        /// </summary>
        decimal? ValorInicialB { get; }
        /// <summary>
        /// Obtiene el valor final de configuración en un rango
        /// </summary>
        decimal? ValorFinalA { get; }
        /// <summary>
        /// Obtiene el valor final de configuración en un rango
        /// </summary>
        decimal? ValorFinalB { get; }
        /// <summary>
        /// Obtiene el estatus de la Configuración
        /// </summary>
        bool? Activo { get; }
        #endregion /Propiedades

        #region Métodos
        /// <summary>
        /// Genera el reporte de configuraciones asignadas
        /// </summary>
        /// <param name="listaDatos"></param>
        void DesplegarConfiguracionesAsignadas(DataSet listaDatos);
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
