using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto de negocios para UsuarioLider
    /// </summary>
    public class UsuarioLiderBO : CatalogoBaseBO {
        #region Propiedades
        public EmpleadoBO Empleado { get; set; }
        public DepartamentoBO Deptartamento { get; set; }
        #endregion
    }
}
