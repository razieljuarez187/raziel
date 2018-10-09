using System;
using BPMO.Basicos.BO;
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto de negocios para ConfiguracionReglaUsuario
    /// </summary>
    public class ConfiguracionReglaUsuarioBO : AuditoriaBaseBO {
        #region Propiedades
        public Int32? Id { get; set; }
        public EmpresaLiderBO Empresa { get; set; }
        public SucursalLiderBO Sucursal { get; set; }
        public AlmacenBO Almacen { get; set; }
        public UsuarioLiderBO Usuario { get; set; }
        public ETipoReglaUsuario? TipoRegla { get; set; }
        public Decimal? ValorInicial { get; set; }
        public Decimal? ValorFinal { get; set; }
        public Boolean? Activo { get; set; }
        #endregion /Propiedades
    }
}
