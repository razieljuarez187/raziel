using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de detalles de notas de taller
    /// </summary>
    public class DetalleNotaTallerBO : DetalleDocumentoBaseBO {
        #region Atributos
        private int? cantidadSurtida;
        private int? cantidadCancelada;
        private int? cantidadReservada;
        private int? cantidadDevuelta;
        private ArticuloBO articuloCore;
        private decimal? precioArticuloCore;
        private decimal? costoArticuloCore;
        private decimal? costoPromedioDolares;
        private decimal? porcentajeImpuesto;
        private int? empresaLiderReservaId;
        private int? sucursalLiderReservaId;
        private AlmacenBO almacenReserva;
        private int? tipoRemisionId;
        private int? monedaLiderOriginalId;
        private decimal? precioArticuloOriginal;
        private decimal? precioArticuloCoreOriginal;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public int? CantidadSurtida {
            set { this.cantidadSurtida = value; }
            get { return this.cantidadSurtida; }
        }
        public int? CantidadCancelada {
            set { this.cantidadCancelada = value; }
            get { return this.cantidadCancelada; }
        }
        public int? CantidadReservada {
            set { this.cantidadReservada = value; }
            get { return this.cantidadReservada; }
        }
        public int? CantidadDevuelta {
            set { this.cantidadDevuelta = value; }
            get { return this.cantidadDevuelta; }
        }
        public int? CantidadReal {
            get { return this.Cantidad - (this.cantidadDevuelta - this.CantidadCancelada); }
        }
        public ArticuloBO ArticuloCore {
            set { this.articuloCore = value; }
            get { return this.articuloCore; }
        }
        public decimal? PrecioArticuloCore {
            set { this.precioArticuloCore = value; }
            get { return this.precioArticuloCore; }
        }
        public decimal? CostoArticuloCore {
            set { this.costoArticuloCore = value; }
            get { return this.costoArticuloCore; }
        }
        public decimal? CostoPromedioDolares {
            set { this.costoPromedioDolares = value; }
            get { return this.costoPromedioDolares; }
        }
        public decimal? PorcentajeImpuesto {
            set { this.porcentajeImpuesto = value; }
            get { return this.porcentajeImpuesto; }
        }
        public int? EmpresaLiderReservaId {
            set { this.empresaLiderReservaId = value; }
            get { return this.empresaLiderReservaId; }
        }
        public int? SucursalLiderReservaId {
            set { this.sucursalLiderReservaId = value; }
            get { return this.sucursalLiderReservaId; }
        }
        public AlmacenBO AlmacenReserva {
            set { this.almacenReserva = value; }
            get { return this.almacenReserva; }
        }
        public int? TipoRemisionId {
            set { this.tipoRemisionId = value; }
            get { return this.tipoRemisionId; }
        }
        public int? MonedaLiderOriginalId {
            set { this.monedaLiderOriginalId = value; }
            get { return this.monedaLiderOriginalId; }
        }
        public decimal? PrecioArticuloOriginal {
            set { this.precioArticuloOriginal = value; }
            get { return this.precioArticuloOriginal; }
        }
        public decimal? PrecioArticuloCoreOriginal {
            set { this.precioArticuloCoreOriginal = value; }
            get { return this.precioArticuloCoreOriginal; }
        }
        public bool TieneCores {
            get {
                if (this.articuloCore != null && this.articuloCore.Id != null && this.articuloCore.Id != 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos
    }
}
