using BPMO.Basicos.BO;
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de configuraciones de transferencias
    /// </summary>
    public class ExcepcionEtapaEmbalajeBO : AuditoriaBaseBO {
        #region Atributos
        private int? id;
        private bool? activo;
        private AlmacenBO almacen;
        private SucursalLiderBO sucursal;
        private EmpresaLiderBO empresa;
        private ETipoMovimiento tipoMovimiento;
        private TipoPedidoBO tipoPedido;
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public int? Id {
            get { return id; }
            set { id = value; }
        }
        public bool? Activo {
            get { return activo; }
            set { activo = value; }
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
        public ETipoMovimiento TipoMovimiento {
            get { return tipoMovimiento; }
            set { tipoMovimiento = value; }
        }
        public TipoPedidoBO TipoPedido {
            get { return tipoPedido; }
            set { tipoPedido = value; }
        }
        #endregion
        #region Métodos
        #endregion
    }
}
