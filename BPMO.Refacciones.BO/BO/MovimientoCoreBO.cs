using System;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de movimientos de cores
    /// </summary>
    public class MovimientoCoreBO : AuditoriaBaseBO {
        #region Atributos
        private int? empresaLiderId;
        private int? sucursalLiderId;
        private AlmacenBO almacen;
        private int? usuarioLiderId;
        private int? conceptoId;
        private ArticuloBO recon;
        private ArticuloBO core;
        private int? cantidad;
        private decimal? costo;
        private decimal? costoDolares;
        private decimal? precio;
        private int? numeroReferencia;
        private string tipoReferencia;
        private int? monedaLiderId;
        private decimal? tipoCambio;
        private DateTime? fecha;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public int? EmpresaLiderId {
            set { this.empresaLiderId = value; }
            get { return this.empresaLiderId; }
        }
        public int? SucursalLiderId {
            set { this.sucursalLiderId = value; }
            get { return this.sucursalLiderId; }
        }
        public AlmacenBO Almacen {
            set { this.almacen = value; }
            get { return this.almacen; }
        }
        public int? UsuarioLiderId {
            set { this.usuarioLiderId = value; }
            get { return this.usuarioLiderId; }
        }
        public int? ConceptoId {
            set { this.conceptoId = value; }
            get { return this.conceptoId; }
        }
        public ArticuloBO Recon {
            set { this.recon = value; }
            get { return this.recon; }
        }
        public ArticuloBO Core {
            set { this.core = value; }
            get { return this.core; }
        }
        public int? Cantidad {
            set { this.cantidad = value; }
            get { return this.cantidad; }
        }
        public decimal? Costo {
            set { this.costo = value; }
            get { return this.costo; }
        }
        public decimal? CostoDolares {
            set { this.costoDolares = value; }
            get { return this.costoDolares; }
        }
        public decimal? Precio {
            set { this.precio = value; }
            get { return this.precio; }
        }
        public int? NumeroReferencia {
            set { this.numeroReferencia = value; }
            get { return this.numeroReferencia; }
        }
        public string TipoReferencia {
            set { this.tipoReferencia = value; }
            get { return this.tipoReferencia; }
        }
        public int? MonedaLiderId {
            set { this.monedaLiderId = value; }
            get { return this.monedaLiderId; }
        }
        public decimal? TipoCambio {
            set { this.tipoCambio = value; }
            get { return this.tipoCambio; }
        }
        public DateTime? Fecha {
            set { this.fecha = value; }
            get { return this.fecha; }
        }
        #endregion Propiedades

        #region Métodos
        #endregion Métodos
    }
}
