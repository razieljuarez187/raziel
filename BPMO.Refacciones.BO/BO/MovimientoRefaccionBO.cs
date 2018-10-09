using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de movimientos de refacciones
    /// </summary>
    public class MovimientoRefaccionBO : DocumentoBaseBO {
        #region Atributos
        private int? empresaLiderId;
        private int? sucursalLiderId;
        private AlmacenBO almacen;
        private int? usuarioLiderId;
        private int? conceptoId;
        private int? numeroReferencia;
        private string tipoReferencia;
        private int? monedaLiderId;
        private int? clienteInternoId;
        private bool? esConsigna;
        private string serie;
        private int? rmClienteId;
        private string rmCliente;
        private int? rmReferenciaId;
        private string rmReferencia;
        private int? rmFolio;
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
        public int? ClienteInternoId {
            set { this.clienteInternoId = value; }
            get { return this.clienteInternoId; }
        }
        public bool? EsConsigna {
            set { this.esConsigna = value; }
            get { return this.esConsigna; }
        }
        public string Serie {
            set { this.serie = value; }
            get { return this.serie; }
        }
        public int? RmClienteId {
            set { this.rmClienteId = value; }
            get { return this.rmClienteId; }
        }
        public string RmCliente {
            set { this.rmCliente = value; }
            get { return this.rmCliente; }
        }
        public int? RmReferenciaId {
            set { this.rmReferenciaId = value; }
            get { return this.rmReferenciaId; }
        }
        public string RmReferencia {
            set { this.rmReferencia = value; }
            get { return this.rmReferencia; }
        }
        public int? RmFolio {
            set { this.rmFolio = value; }
            get { return this.rmFolio; }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos
    }
}
