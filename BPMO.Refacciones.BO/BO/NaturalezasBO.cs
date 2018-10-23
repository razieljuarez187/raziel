using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    ///  Objeto Nivel ABC
    /// </summary>
    public class NaturalezasBO : CatalogoBaseBO {
        #region Atributos
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public string Descripcion {
            get { return string.Format("{0} ({1})", this.Nombre, this.NombreCorto); }
        }
        #endregion
    }
}
