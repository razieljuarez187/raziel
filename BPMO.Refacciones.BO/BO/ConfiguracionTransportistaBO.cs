using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de configuraciones de transportistas
    /// </summary>
    public class ConfiguracionTransportistaBO : AuditoriaBaseBO {
        #region Atributos
        private int? longitudGuia;
        private TransportistaBO transportista;
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public int? LongitudGuia {
            get { return longitudGuia; }
            set { longitudGuia = value; }
        }
        public TransportistaBO Transportista {
            get { return transportista; }
            set { transportista = value; }
        }
        #endregion
        #region Métodos
        #endregion
    }
}
