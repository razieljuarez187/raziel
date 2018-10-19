using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;
using System;

namespace BPMO.Refacciones.Procesos.VIS {
    /// <summary>
    /// Vista para el mantenimiento de Configuracion Transferencia
    /// </summary>
    public interface IMantenimientoConfiguracionTransferenciaVIS {
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
        /// tipo pedido
        /// </summary>
        TipoPedidoBO TipoPedidoBO { get; set; }
        /// <summary>
        /// Cantidad Lunes
        /// </summary>
        int? confCantidadLunes { get; set; }
        /// <summary>
        ///  Cantidad Martes
        /// </summary>
        int? confCantidadMartes { get; set; }
        /// <summary>
        /// Cantidad Miercoles
        /// </summary>
        int? confCantidadMiercoles { get; set; }
        /// <summary>
        ///  Cantidad Jueves
        /// </summary>
        int? confCantidadJueves { get; set; }
        /// <summary>
        ///  Cantidad Viernes
        /// </summary>
        int? confCantidadViernes { get; set; }
        /// <summary>
        ///  Cantidad Sabado
        /// </summary>
        int? confCantidadSabado { get; set; }
        /// <summary>
        ///  Cantidad Domingo
        /// </summary>
        int? confCantidadDomingo { get; set; }
        /// <summary>
        ///  Cantidad Activo
        /// </summary>
        bool? confCantidadActivo { get; set; }
        /// <summary>
        /// Hora Lunes
        /// </summary>
        TimeSpan? confHoraLunes { get; set; }
        /// <summary>
        ///  Hora Martes
        /// </summary>
        TimeSpan? confHoraMartes { get; set; }
        /// <summary>
        /// Hora Miercoles
        /// </summary>
        TimeSpan? confHoraMiercoles { get; set; }
        /// <summary>
        ///  Hora Jueves
        /// </summary>
        TimeSpan? confHoraJueves { get; set; }
        /// <summary>
        ///  Hora Viernes
        /// </summary>
        TimeSpan? confHoraViernes { get; set; }
        /// <summary>
        ///  Hora Sabado
        /// </summary>
        TimeSpan? confHoraSabado { get; set; }
        /// <summary>
        ///  Hora Domingo
        /// </summary>
        TimeSpan? confHoraDomingo { get; set; }
        /// <summary>
        ///  Cantidad Activo
        /// </summary>
        bool? confHoraActivo { get; set; }
        /// <summary>
        /// Obtiene la adscripción actual
        /// </summary>
        List<NivelABCBO> NivelABCBO { get; set; }
        /// <summary>
        /// Obtiene la adscripción actual
        /// </summary>
        List<NivelABCBO> ConfNivelABCBO { get; set; }
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
        /// Obtiene o establece el identificador del Pedido
        /// </summary>
        int? TipoPedidoId { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Pedido
        /// </summary>
        string NombreTipoPedido { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del Almacén
        /// </summary>
        int? maximoArticulosLinea { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Usuario
        /// </summary>
        int? maximoLineas { get; set; }
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
        object ConfiguracionTransferenciaBase { get; set; }
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
