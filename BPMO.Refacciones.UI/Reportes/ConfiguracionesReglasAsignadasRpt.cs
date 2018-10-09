using DevExpress.XtraReports.UI;

namespace BPMO.Refacciones.Reportes {
    /// <summary>
    /// Reporte para el manejo de la productividad del técnico
    /// </summary>
    public partial class ConfiguracionesReglasAsignadasRpt : DevExpress.XtraReports.UI.XtraReport {
        #region Métodos
        /// <summary>
        /// Método constructor del reporte para la productividad del técnico
        /// </summary>
        public ConfiguracionesReglasAsignadasRpt() {
            try {
                InitializeComponent();
                this.EnlazarControles();
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Enlaza los controles del reporte con la información contenida en la fuente de datos
        /// </summary>
        private void EnlazarControles() {
            #region Configuraciones del reporte
            this.Detail.SortFields.Add(new GroupField("Empresa", XRColumnSortOrder.Ascending));
            this.Detail.SortFields.Add(new GroupField("Sucursal", XRColumnSortOrder.Ascending));
            this.Detail.SortFields.Add(new GroupField("Almacen", XRColumnSortOrder.Ascending));
            this.Detail.SortFields.Add(new GroupField("ValorInicial", XRColumnSortOrder.Ascending));
            #region Grupos
            this.gpEmpresa.GroupFields.Add(new GroupField("EmpresaId"));
            this.xrSucursal.DataBindings.Add("Text", DataSource, "Empresa");
            this.gpSucursal.GroupFields.Add(new GroupField("SucursalId"));
            this.xrSucursal.DataBindings.Add("Text", DataSource, "Sucursal");
            this.gpAlmacen.GroupFields.Add(new GroupField("AlmacenId"));
            this.xrAlmacen.DataBindings.Add("Text", DataSource, "Almacen");
            #endregion
            #endregion
            #region Detalles del reporte
            this.xrConfiguracionID.DataBindings.Add("Text", DataSource, "ConfiguracionReglaId");
            this.xrUsuario.DataBindings.Add("Text", DataSource, "UsuarioNombre");
            this.xrValorInicial.DataBindings.Add("Text", DataSource, "ValorInicial", "{0: #,0.00}");
            this.xrValorFinal.DataBindings.Add("Text", DataSource, "ValorFinal", "{0: #,0.00}");
            #endregion
            #region Footers
            
            #endregion
        }
        #endregion
        /// <summary>
        /// Enlaza contenido al Label
        /// </summary>
        /// <param name="xrlbl">XRLabel al cual se va enlazar la información</param>
        /// <param name="dataMember">Nombre del campo a enlazar</param>
        private void LabelDataBinding(XRLabel xrlbl, string dataMember, bool esFormatoMoneda = true) {
            xrlbl.DataBindings.Add("Text", DataSource, dataMember);
            xrlbl.Summary.Func = SummaryFunc.Sum;
            xrlbl.Summary.Running = SummaryRunning.Group;
            xrlbl.Summary.FormatString = esFormatoMoneda ? "{0: $#,0.00}" : "{0: #,0.00}";
        }
    }
}
