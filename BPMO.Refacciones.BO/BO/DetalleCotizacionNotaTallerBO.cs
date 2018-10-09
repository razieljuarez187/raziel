using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de detalle de cotización de notas de taller
    /// </summary>
    public class DetalleCotizacionNotaTallerBO : DetalleDocumentoBaseBO {
        #region Atributos
        private int? empresaLiderReservaId;
        private int? sucursalLiderReservaId;
        private int? almacenId;
        private int? cantidadSolicitada;
        private int? cantidadSurtida;
        private decimal? porcentajeImpuesto;
        private int? tipoRemisionId;
        private int? monedaLiderOriginalId;
        private ArticuloBO articuloCore;
        private decimal? precioArticuloCore;
        private decimal? costoArticuloCore;
        private decimal? precioArticuloOriginal;
        private decimal? precioArticuloCoreOriginal;
        #endregion

        #region Constructores
        #endregion
        
        #region Propiedades
        public int? EmpresaLiderReservaId {
            set { this.empresaLiderReservaId = value; }
            get { return this.empresaLiderReservaId; }
        }
        public int? SucursalLiderReservaId {
            set { this.sucursalLiderReservaId = value; }
            get { return this.sucursalLiderReservaId; }
        }
        public int? AlmacenId {
            set { this.almacenId = value; }
            get { return this.almacenId; }
        }
        public int? CantidadSolicitada {
            set { this.cantidadSolicitada = value; }
            get { return this.cantidadSolicitada; }
        }
        public int? CantidadSurtida {
            set { this.cantidadSurtida = value; }
            get { return this.cantidadSurtida; }
        }
        public decimal? PorcentajeImpuesto {
            set { this.porcentajeImpuesto = value; }
            get { return this.porcentajeImpuesto; }
        }
        public int? TipoRemisionId {
            get { return this.tipoRemisionId; }
            set { this.tipoRemisionId = value; }
        }
        public int? MonedaLiderOriginalId {
            get { return this.monedaLiderOriginalId; }
            set { this.monedaLiderOriginalId = value; }
        }
        public ArticuloBO ArticuloCore {
            get { return this.articuloCore; }
            set { this.articuloCore = value; }
        }
        public decimal? CostoArticuloCore {
            set { this.costoArticuloCore = value; }
            get { return this.costoArticuloCore; }
        }
        public decimal? PrecioArticuloCore {
            set { this.precioArticuloCore = value; }
            get { return this.precioArticuloCore; }
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
            get { return (this.articuloCore != null && this.articuloCore.Id != null && this.articuloCore.Id != 0); }
        }
        #endregion
        
        #region Métodos
        #endregion
    }
}
