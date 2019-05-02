using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.Procesos.VIS {
    /// <summary>
    /// Vista para el buscador de excepciones de etapa de embalaje
    /// </summary>
    public interface IBuscadorConfiguracionEtapaEmbalajeVIS {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la excepción
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
        /// Obtiene o establece el tipo de movimiento
        /// </summary>
        ETipoMovimiento? TipoMovimiento { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Pedido
        /// </summary>
        int? TipoPedidoId { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del Pedido
        /// </summary>
        string NombreTipoPedido { get; set; }
        /// <summary>
        /// Obtiene el estatus de la Configuración
        /// </summary>
        bool? Activo { get; }
        /// <summary>
        /// Obtiene o establece un listado de excepciones
        /// </summary>
        List<ConfiguracionEtapaEmbalajeBO> ListadoConfiguracionesEmbalaje { get; set; }
        #endregion Propiedades
        #region Métodos
        /// <summary>
        /// Método de la interfaz a ser implementado
        /// </summary>
        /// <param name="mensaje">mensaje</param>
        /// <param name="tipo">tipo de mensaje</param>
        /// <param name="detalle">detalle del mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Método a implementar para enviar a el registro seleccionado
        /// </summary>
        /// <param name="Id"></param>
        void EnviarAEditarBO(int Id);
        #endregion Métodos
    }
}
