using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de detalles de movimientos de refacciones
    /// </summary>
    public class DetalleMovimientoRefaccionBO : DetalleDocumentoBaseBO {
        #region Atributos
        private ArticuloBO articuloCore;
        private decimal? costoCore;
        private decimal? precioCore;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        /// <summary>
        /// Objeto Core correspondiente a la refacción Recon
        /// </summary>
        public ArticuloBO ArticuloCore {
            get { return articuloCore; }
            set { articuloCore = value; }
        }
        /// <summary>
        /// Costo del Core
        /// </summary>
        public decimal? CostoCore { 
            get { return costoCore; }
            set { costoCore = value; }
        }
        /// <summary>
        /// Precio del Core
        /// </summary>
        public decimal? PrecioCore {
            get { return precioCore; }
            set { precioCore = value; }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos
    }
}
