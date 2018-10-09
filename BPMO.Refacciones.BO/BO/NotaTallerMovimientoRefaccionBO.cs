
namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de movimientos de notas de taller
    /// </summary>
    public class NotaTallerMovimientoRefaccionBO {
        #region Atributos
        private NotaTallerBO notaTallerOriginal;
        private NotaTallerBO notaTallerNueva;
        private MovimientoRefaccionBO movimiento;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public NotaTallerBO NotaTallerOriginal {
            set { this.notaTallerOriginal = value; }
            get { return this.notaTallerOriginal; }
        }
        public NotaTallerBO NotaTallerNueva {
            set { this.notaTallerNueva = value; }
            get { return this.notaTallerNueva; }
        }
        public MovimientoRefaccionBO Movimiento {
            set { this.movimiento = value; }
            get { return this.movimiento; }
        }
        #endregion Propiedades

        #region Metodos
        #endregion Metodos
    }
}
