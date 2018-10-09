using System;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de notas de taller
    /// </summary>
    public class NotaTallerBO : DocumentoBaseBO {
        #region Atributos
        private int? empresaLiderId;
        private int? sucursalLiderId;
        private AlmacenBO almacen;
        private int? areaLiderId;
        private int? mecanicoLiderId;
        private int? usuarioLiderId;
        private int? ordenServicioId;
        private int? movimientoId;
        private int? numeroReferencia;
        private string tipoReferencia;
        private int? clienteLiderId;
        private int? diasCredito;
        private bool? esRescate;
        private int? tipoPedidoId;
        private bool? esDevolucion;
        private int? notaTallerReferencia;
        private bool? estaImpreso;
        private int? kitId;
        private int? monedaLiderId;
        private DateTime? fechaAplicacion;
        private EstatusBO estatus;
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
        public int? AreaLiderId {
            set { this.areaLiderId = value; }
            get { return this.areaLiderId; }
        }
        public int? MecanicoLiderId {
            set { this.mecanicoLiderId = value; }
            get { return this.mecanicoLiderId; }
        }
        public int? UsuarioLiderId {
            set { this.usuarioLiderId = value; }
            get { return this.usuarioLiderId; }
        }
        public int? OrdenServicioId {
            set { this.ordenServicioId = value; }
            get { return this.ordenServicioId; }
        }
        public int? MovimientoId {
            set { this.movimientoId = value; }
            get { return this.movimientoId; }
        }
        public int? NumeroReferencia {
            set { this.numeroReferencia = value; }
            get { return this.numeroReferencia; }
        }
        public string TipoReferencia {
            set { this.tipoReferencia = value; }
            get { return this.tipoReferencia; }
        }
        public int? ClienteLiderId {
            set { this.clienteLiderId = value; }
            get { return this.clienteLiderId; }
        }
        public int? DiasCredito {
            set { this.diasCredito = value; }
            get { return this.diasCredito; }
        }
        public bool? EsRescate {
            set { this.esRescate = value; }
            get { return this.esRescate; }
        }
        public EstatusBO Estatus {
            set { this.estatus = value; }
            get { return this.estatus; }
        }
        public int? TipoPedidoId {
            set { this.tipoPedidoId = value; }
            get { return this.tipoPedidoId; }
        }
        public bool? EsDevolucion {
            set { this.esDevolucion = value; }
            get { return this.esDevolucion; }
        }
        public int? NotaTallerReferencia {
            set { this.notaTallerReferencia = value; }
            get { return this.notaTallerReferencia; }
        }
        public bool? EstaImpreso {
            set { this.estaImpreso = value; }
            get { return this.estaImpreso; }
        }
        public int? KitId {
            set { this.kitId = value; }
            get { return this.kitId; }
        }
        public int? MonedaLiderId {
            set { this.monedaLiderId = value; }
            get { return this.monedaLiderId; }
        }
        public DateTime? FechaAplicacion {
            set { this.fechaAplicacion = value; }
            get { return this.fechaAplicacion; }
        }
        public bool TieneCores {
            get {
                bool tieneCores = false;
                foreach (DetalleNotaTallerBO detalleNotaTaller in this.GetChildren()) {
                    if (detalleNotaTaller.TieneCores)
                        tieneCores = true;
                }
                return tieneCores;
            }
        }
        public bool PuedeDevolverse {
            get {
                bool puedeDevolverse = false;
                foreach (DetalleNotaTallerBO detalleNotaTaller in this.GetChildren()) {
                    if (detalleNotaTaller.CantidadReal > 0)
                        puedeDevolverse = true;
                }
                return puedeDevolverse;
            }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos
    }
}
