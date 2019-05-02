using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Consultar registros de ConfiguracionTransferencia
    /// </summary>
    internal class ConfiguracionEtapaEmbalajeConsultarDAO : IDAOBaseConsultarAuditoria {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ExcepcionEtapaEmbalaje en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de excepciones para etapas de embalaje</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetos
            ConfiguracionEtapaEmbalajeBO exepcion = null;
            if (auditoriaBase is ConfiguracionEtapaEmbalajeBO)
                exepcion = (ConfiguracionEtapaEmbalajeBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (exepcion == null)
                mensajeError += " , ConfiguracionEtapaEmbalaje";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2));
            #endregion Validar parámetros

            #region Conexión a BD
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                throw;
            }
            #endregion Conexión a BD

            #region Armado de Sentencia SQL
            StringBuilder sCmd = new StringBuilder();
            // TODO: Adecual a [eRef_ExcepcionesEtapaEmbalaje]
            sCmd.Append(" SELECT conf.ConfiguracionId, conf.EmpresaId, emp.RazonSocial AS Empresa, conf.SucursalId, suc.Sucursal, conf.AlmacenId, ");
            sCmd.Append(" 	alm.Descripcion AS Almacen, conf.TipoMovimiento, tp.TipoPedidoID, tp.TipoPedido AS NombreTipoPedido, ");
            sCmd.Append("   conf.Activo, conf.UC, conf.FC, conf.UA, conf.FA ");
            sCmd.Append(" FROM eRef_confEtapaEmbalaje conf ");
            sCmd.Append("   INNER JOIN grl_catEmpresas emp ON (emp.EmpresaId = conf.EmpresaId) ");
            sCmd.Append("   INNER JOIN grl_catSucursales suc ON (suc.EmpresaId = conf.EmpresaId AND suc.SucursalId = conf.SucursalId) ");
            sCmd.Append("   INNER JOIN grl_catAlmacenes alm ON (alm.EmpresaId = conf.EmpresaId AND alm.SucursalId = conf.SucursalId AND alm.AlmacenId = conf.AlmacenId) ");
            sCmd.Append("   INNER JOIN ref_TiposPedido tp ON (tp.TipoPedidoID = conf.TipoPedidoId) ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (exepcion.Id.HasValue) {
                sWhere.Append(" AND conf.ConfiguracionId = @configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Id", exepcion.Id, System.Data.DbType.Int32);
            }
            if (exepcion.Empresa != null && exepcion.Empresa.Id.HasValue) {
                sWhere.Append(" AND conf.EmpresaId = @configuracion_EmpresaId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", exepcion.Empresa.Id, System.Data.DbType.Byte);
            }
            if (exepcion.Sucursal != null && exepcion.Sucursal.Id.HasValue) {
                sWhere.Append(" AND conf.SucursalId = @configuracion_SucursalId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", exepcion.Sucursal.Id, System.Data.DbType.Int16);
            }
            if (exepcion.Almacen != null && exepcion.Almacen.Id.HasValue) {
                sWhere.Append(" AND conf.AlmacenId = @configuracion_AlmacenId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", exepcion.Almacen.Id, System.Data.DbType.Int32);
            }
            if (exepcion.TipoMovimiento != null) {
                sWhere.Append(" AND conf.TipoMovimiento = @configuracion_TipoMovimiento");
                Utileria.AgregarParametro(sqlCmd, "configuracion_TipoMovimiento", exepcion.TipoMovimiento, System.Data.DbType.Byte);
            }
            if (exepcion.TipoPedido != null && exepcion.TipoPedido.Id.HasValue) {
                sWhere.Append(" AND conf.TipoTransferenciaId = @configuracion_TipoPedidoID");
                Utileria.AgregarParametro(sqlCmd, "configuracion_TipoPedidoID", exepcion.TipoPedido.Id, System.Data.DbType.Int32);
            }
            if (exepcion.Activo.HasValue) {
                sWhere.Append(" AND conf.Activo = @configuracion_Activo");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", exepcion.Activo, System.Data.DbType.Boolean);
            }
            #endregion Valores

            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ConfiguracionEmbalaje");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstConfiguraciones = new List<AuditoriaBaseBO>();
            ConfiguracionEtapaEmbalajeBO configuracionBO = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionBO = new ConfiguracionEtapaEmbalajeBO();
                configuracionBO.Empresa = new EmpresaLiderBO();
                configuracionBO.Sucursal = new SucursalLiderBO();
                configuracionBO.Almacen = new AlmacenBO();
                configuracionBO.TipoPedido = new TipoPedidoBO();
                configuracionBO.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionId"))
                    configuracionBO.Id = (Int32)Convert.ChangeType(row["ConfiguracionId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    configuracionBO.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("Empresa"))
                    configuracionBO.Empresa.Nombre = (String)Convert.ChangeType(row["Empresa"], typeof(String));
                if (!row.IsNull("SucursalId"))
                    configuracionBO.Sucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("Sucursal"))
                    configuracionBO.Sucursal.Nombre = (String)Convert.ChangeType(row["Sucursal"], typeof(String));
                if (!row.IsNull("AlmacenId"))
                    configuracionBO.Almacen.Id = (Int32)Convert.ChangeType(row["AlmacenId"], typeof(Int32));
                if (!row.IsNull("Almacen"))
                    configuracionBO.Almacen.Nombre = (String)Convert.ChangeType(row["Almacen"], typeof(String));
                if (!row.IsNull("TipoMovimiento"))
                    configuracionBO.TipoMovimiento = (ETipoMovimiento)Convert.ChangeType(row["TipoMovimiento"], typeof(ETipoMovimiento));
                if (!row.IsNull("TipoPedidoID"))
                    configuracionBO.TipoPedido.Id = (Int32)Convert.ChangeType(row["TipoPedidoID"], typeof(Int32));
                if (!row.IsNull("NombreTipoPedido"))
                    configuracionBO.TipoPedido.Nombre = (String)Convert.ChangeType(row["NombreTipoPedido"], typeof(String));
                if (!row.IsNull("Activo"))
                    configuracionBO.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("UC"))
                    configuracionBO.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    configuracionBO.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UA"))
                    configuracionBO.Auditoria.UUA = (Int32)Convert.ChangeType(row["UA"], typeof(Int32));
                if (!row.IsNull("FA"))
                    configuracionBO.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FA"], typeof(DateTime));
                #endregion /ConfiguracionesReglas

                lstConfiguraciones.Add(configuracionBO);
            }
            return lstConfiguraciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
