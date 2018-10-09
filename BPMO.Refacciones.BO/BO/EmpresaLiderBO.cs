using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    public class EmpresaLiderBO : CatalogoBaseBO {
        #region Propiedades
        public string RFC { get; set; }
        public string CURP { get; set; }
        public DireccionSucursalBO Direccion { get; set; }
        public string Email { get; set; }
        #endregion /Propiedades
    }
}
