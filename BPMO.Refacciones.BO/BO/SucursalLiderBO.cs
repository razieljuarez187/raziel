using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto de negocios para Sucursal Lider
    /// </summary>
    public class SucursalLiderBO : CatalogoBaseBO {
        #region Propiedades
        public EmpresaLiderBO Empresa { get; set; }
        public DireccionSucursalBO Direccion { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        public bool? Matriz { get; set; }
        public MonedaBO MonedaNacional { get; set; }
        public MonedaBO MonedaExtranjera { get; set; }
        public UnidadOperativaBO UnidadOperativa { get; set; }
        public SucursalBO SucursalOracle { get; set; }
        #endregion /Propiedades
    }
}
