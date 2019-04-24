using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de existencias de refacciones del almacén de refacciones
    /// </summary>
    public class ExistenciaAlmacenRefaccionesBO : AuditoriaBaseBO {
        #region Atributos
        private int? id;
        private ArticuloBO articulo;
        private ArticuloBO core;
        private int? empresaLiderId;
        private int? sucursalLiderId;
        private AlmacenBO almacen;
        private int? existenciaInicial;
        private int? acumuladoEntradas;
        private int? acumuladoSalidas;
        private int? cantidadEnConsigna;
        private int? cantidadReservada;
        private decimal costoPromedio;
        private decimal? precio;
        private LineaBO linea;
        private MonedaBO moneda;
        private int? subGrupoId;
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public int? Id {
            set { this.id = value; }
            get { return this.id; }
        }
        public ArticuloBO Articulo {
            set { this.articulo = value; }
            get { return this.articulo; }
        }
        public ArticuloBO Core {
            set { this.core = value; }
            get { return this.core; }
        }
        public int? EmpresaLiderId {
            set { this.empresaLiderId = value; }
            get { return this.empresaLiderId; }
        }
        public int? SucursalLiderId {
            set { this.sucursalLiderId = value; }
            get { return this.sucursalLiderId; }
        }
        public AlmacenBO Almacen {
            set { this.almacen = value; }
            get { return this.almacen; }
        }
        public int? ExistenciaInicial {
            set { this.existenciaInicial = value; }
            get { return this.existenciaInicial; }
        }
        public int? AcumuladoEntradas {
            set { this.acumuladoEntradas = value; }
            get { return this.acumuladoEntradas; }
        }
        public int? AcumuladoSalidas {
            set { this.acumuladoSalidas = value; }
            get { return this.acumuladoSalidas; }
        }
        public int? CantidadEnConsigna {
            set { this.cantidadEnConsigna = value; }
            get { return this.cantidadEnConsigna; }
        }
        public int? CantidadReservada {
            set { this.cantidadReservada = value; }
            get { return this.cantidadReservada; }
        }
        public decimal CostoPromedio {
            set { this.costoPromedio = value; }
            get { return this.costoPromedio; }
        }
        public int? Disponible {
            get { return this.existenciaInicial + this.acumuladoEntradas - this.acumuladoSalidas - this.cantidadEnConsigna - this.cantidadReservada; }
        }
        public decimal? Precio {
            get { return precio; }
            set { precio = value; }
        }
        public LineaBO Linea {
            get { return linea; }
            set { linea = value; }
        }
        public MonedaBO Moneda {
            get { return moneda; }
            set { moneda = value; }
        }
        public int? SubGrupoId {
            get { return subGrupoId; }
            set { subGrupoId = value; }
        }
        #endregion Propiedades

        #region Métodos
        #endregion Métodos
    }
}
