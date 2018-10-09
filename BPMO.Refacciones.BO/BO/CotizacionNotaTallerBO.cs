using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de cotización de notas de taller
    /// </summary>
    public class CotizacionNotaTallerBO : DocumentoBaseBO {
        #region Atributos
        private int? empresaLiderId;
        private int? sucursalLiderId;
        private int? almacenLiderId;
        private int? areaLiderId;
        private int? mecanicoId;
        private int? usuarioLiderId;
        private int? movimientoId;
        private int? numeroReferencia;
        private string tipoReferencia;
        private int? clienteLiderId;
        private int? diasCredito;
        private bool? esRescate;
        private int? tipoPedidoId;
        private bool? esDevolucion;
        private bool? estaImpreso;
        private int? kitId;
        private int? monedaId;
        private int? ordenServicioId;
        private int? cotizacionOrdenId;
        private int? notaTallerId;
        private DateTime? fechaAutoriza;
        private DateTime? fechaAplica;
        private DateTime? fechaRechaza;
        private DateTime? fechaCaduca;
        private string observaciones;
        private int? statusId;
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public int? EmpresaLiderId {
            set { this.empresaLiderId = value; }
            get { return this.empresaLiderId; }
        }
        public int? SucursalLiderId {
            set { this.sucursalLiderId = value; }
            get { return this.sucursalLiderId; }
        }
        public int? AlmacenLiderId {
            set { this.almacenLiderId = value; }
            get { return this.almacenLiderId; }
        }
        public int? AreaId {
            set { this.areaLiderId = value; }
            get { return this.areaLiderId; }
        }
        public int? MecanicoId {
            set { this.mecanicoId = value; }
            get { return this.mecanicoId; }
        }
        public int? UsuarioLiderId {
            get { return this.usuarioLiderId; }
            set { this.usuarioLiderId = value; }
        }
        public int? MovimientoId {
            get { return this.movimientoId; }
            set { this.movimientoId = value; }
        }
        public int? NumeroReferencia {
            get { return this.numeroReferencia; }
            set { this.numeroReferencia = value; }
        }
        public string TipoReferencia {
            get { return this.tipoReferencia; }
            set { this.tipoReferencia = value; }
        }
        public int? ClienteLiderId {
            set { this.clienteLiderId = value; }
            get { return this.clienteLiderId; }
        }
        public int? DiasCredito {
            get { return this.diasCredito; }
            set { this.diasCredito = value; }
        }
        public bool? EsRescate {
            get { return this.esRescate; }
            set { this.esRescate = value; }
        }
        public int? TipoPedidoId {
            get { return this.tipoPedidoId; }
            set { this.tipoPedidoId = value; }
        }
        public bool? EsDevolucion {
            get { return this.esDevolucion; }
            set { this.esDevolucion = value; }
        }
        public bool? EstaImpreso {
            get { return this.estaImpreso; }
            set { this.estaImpreso = value; }
        }
        public int? KitId {
            get { return this.kitId; }
            set { this.kitId = value; }
        }
        public int? MonedaId {
            set { this.monedaId = value; }
            get { return this.monedaId; }
        }
        public int? OrdenServicioId {
            set { this.ordenServicioId = value; }
            get { return this.ordenServicioId; }
        }
        public int? CotizacionOrdenId {
            set { this.cotizacionOrdenId = value; }
            get { return this.cotizacionOrdenId; }
        }
        public int? NotaTallerId {
            set { this.notaTallerId = value; }
            get { return this.notaTallerId; }
        }
        public DateTime? FechaAutoriza {
            set { this.fechaAutoriza = value; }
            get { return this.fechaAutoriza; }
        }
        public DateTime? FechaAplica {
            set { this.fechaAplica = value; }
            get { return this.fechaAplica; }
        }
        public DateTime? FechaRechaza {
            set { this.fechaRechaza = value; }
            get { return this.fechaRechaza; }
        }
        public DateTime? FechaCaduca {
            set { this.fechaCaduca = value; }
            get { return this.fechaCaduca; }
        }
        public string Observaciones {
            set { this.observaciones = value; }
            get { return this.observaciones; }
        }
        public int? StatusId {
            set { this.statusId = value; }
            get { return this.statusId; }
        }
        public bool TieneCores {
            get { return this.GetChildren().ConvertAll(d => (DetalleCotizacionNotaTallerBO)d).Exists(d => d.TieneCores); }
        } 
        #endregion
        #region Métodos
        #endregion
    }
}
