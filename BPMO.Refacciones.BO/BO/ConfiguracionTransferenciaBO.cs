using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de configuraciones de transferencias
    /// </summary>
    public class ConfiguracionTransferenciaBO : AuditoriaBaseBO {
        #region Atributos
        private bool? activo;
        private int? id;
        private int? maximoArticulosLinea;
        private int? maximoLineas;
        private AlmacenBO almacen;
        private SucursalLiderBO sucursal;
        private EmpresaLiderBO empresa;
        private ConfiguracionCantidadTransferenciaBO configuracionCantidadTransferencia;
        private ConfiguracionHoraTransferenciaBO configuracionHoraTransferencia;
        private TipoPedidoBO tipoPedido;
        private List<NivelABCBO> nivelesABC;
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public bool? Activo {
            get { return this.activo; }
            set { this.activo = value; }
        }
        public int? Id {
            get { return this.id; }
            set { this.id = value; }
        }
        public int? MaximoArticulosLinea {
            get { return this.maximoArticulosLinea; }
            set { this.maximoArticulosLinea = value; }
        }
        public int? MaximoLineas {
            get { return this.maximoLineas; }
            set { this.maximoLineas = value; }
        }
        public AlmacenBO Almacen {
            get { return this.almacen; }
            set { this.almacen = value; }
        }
        public SucursalLiderBO Sucursal {
            get { return this.sucursal; }
            set { this.sucursal = value; }
        }
        public EmpresaLiderBO Empresa {
            get { return this.empresa; }
            set { this.empresa = value; }
        }
        public ConfiguracionCantidadTransferenciaBO ConfiguracionCantidadTransferencia {
            get { return this.configuracionCantidadTransferencia; }
            set { this.configuracionCantidadTransferencia = value; }
        }
        public ConfiguracionHoraTransferenciaBO ConfiguracionHoraTransferencia {
            get { return this.configuracionHoraTransferencia; }
            set { this.configuracionHoraTransferencia = value; }
        }
        public TipoPedidoBO TipoPedido {
            get { return this.tipoPedido; }
            set { this.tipoPedido = value; }
        }
        public List<NivelABCBO> NivelesABC {
            get { return this.nivelesABC; }
            set { this.nivelesABC = value; }
        }
        #endregion
        #region Métodos
        #endregion
    }
}
