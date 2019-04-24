using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de almacenes
    /// </summary>
    public class AlmacenBO : CatalogoBaseBO {
        #region Atributos
        private string tipoAlmacen;
        private DepartamentoBO departamento;
        private bool? esConsigna;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public string TipoAlmacen {
            set { this.tipoAlmacen = value; }
            get { return this.tipoAlmacen; }
        }
        public DepartamentoBO Departamento {
            set { this.departamento = value; }
            get { return this.departamento; }
        }
        public bool? EsConsigna {
            set { this.esConsigna = value; }
            get { return this.esConsigna; }
        }
        public SucursalLiderBO Sucursal { get; set; }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos

    }
}
