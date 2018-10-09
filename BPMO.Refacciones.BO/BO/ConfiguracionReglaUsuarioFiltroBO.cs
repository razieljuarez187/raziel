using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto de negocios para el objeto filtro de ConfiguracionReglaUsuario
    /// </summary>
    public class ConfiguracionReglaUsuarioFiltroBO : ConfiguracionReglaUsuarioBO {
        #region Propiedades
        public Decimal? ValorInicialFin { get; set; }
        public Decimal? ValorFinalFin { get; set; }
        #endregion /Propiedades
    }
}
