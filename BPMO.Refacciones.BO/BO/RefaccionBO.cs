using System;
using System.Collections.Generic;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de refacciones
    /// </summary>
    public class RefaccionBO : ArticuloBO { 
        #region Atributos
        private LineaBO linea;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public LineaBO Linea {
            get { return this.linea; }
            set { this.linea = value; }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos

    }
}