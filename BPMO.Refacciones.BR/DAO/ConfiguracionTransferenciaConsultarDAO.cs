using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Consultar registros de ConfiguracionTransferencia
    /// </summary>
    class ConfiguracionTransferenciaConsultarDAO : IDAOBaseConsultarAuditoria {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetos
            ConfiguracionTransferenciaBO configRegla = null;
            if (auditoriaBase is ConfiguracionTransferenciaBO)
                configRegla = (ConfiguracionTransferenciaBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (configRegla == null)
                mensajeError += " , ConfiguracionTransferencia";
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
            sCmd.Append(" SELECT conf.ConfiguracionId, conf.EmpresaId, emp.RazonSocial AS Empresa, conf.SucursalId, suc.Sucursal, conf.AlmacenId, ");
            sCmd.Append(" 	alm.Descripcion AS Almacen,  tp.TipoPedidoID, tp.TipoPedido AS NombreTipoPedido, ");
            sCmd.Append("   conf.MaximoArticulosLinea, conf.MaximoLineas, conf.Activo, conf.UC, conf.FC, conf.UA, conf.FA ");
            sCmd.Append(" FROM eRef_confTransferencia conf ");
            sCmd.Append("   INNER JOIN grl_catEmpresas emp ON (emp.EmpresaId = conf.EmpresaId) ");
            sCmd.Append("   INNER JOIN grl_catSucursales suc ON (suc.EmpresaId = conf.EmpresaId AND suc.SucursalId = conf.SucursalId) ");
            sCmd.Append("   INNER JOIN grl_catAlmacenes alm ON (alm.EmpresaId = conf.EmpresaId AND alm.SucursalId = conf.SucursalId AND alm.AlmacenId = conf.AlmacenId) ");
            sCmd.Append("   INNER JOIN ref_TiposPedido tp ON (tp.TipoPedidoID = conf.TipoTransferenciaId) ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (configRegla.Id.HasValue) {
                sWhere.Append(" AND conf.ConfiguracionId = @configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configRegla.Id, System.Data.DbType.Int32);
            }
            if (configRegla.Empresa != null && configRegla.Empresa.Id.HasValue) {
                sWhere.Append(" AND conf.EmpresaId = @configuracion_EmpresaId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", configRegla.Empresa.Id, System.Data.DbType.Byte);
            }
            if (configRegla.Sucursal != null && configRegla.Sucursal.Id.HasValue) {
                sWhere.Append(" AND conf.SucursalId = @configuracion_SucursalId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", configRegla.Sucursal.Id, System.Data.DbType.Int16);
            }
            if (configRegla.Almacen != null && configRegla.Almacen.Id.HasValue) {
                sWhere.Append(" AND conf.AlmacenId = @configuracion_AlmacenId");
                Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", configRegla.Almacen.Id, System.Data.DbType.Int32);
            }
            if (configRegla.TipoPedido != null && configRegla.TipoPedido.Id.HasValue) {
                sWhere.Append(" AND conf.TipoTransferenciaId = @configuracion_TipoPedidoID");
                Utileria.AgregarParametro(sqlCmd, "configuracion_TipoPedidoID", configRegla.TipoPedido.Id, System.Data.DbType.Int32);
            }
            if (configRegla.Activo.HasValue) {
                sWhere.Append(" AND conf.Activo = @configuracion_Activo");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", configRegla.Activo, System.Data.DbType.Boolean);
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
                sqlAdapter.Fill(ds, "ConfiguracionesReglas");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstConfiguraciones = new List<AuditoriaBaseBO>();
            ConfiguracionTransferenciaBO configuracionRegla = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionRegla = new ConfiguracionTransferenciaBO();
                configuracionRegla.Empresa = new EmpresaLiderBO();
                configuracionRegla.Sucursal = new SucursalLiderBO();
                configuracionRegla.Almacen = new AlmacenBO();
                configuracionRegla.TipoPedido = new TipoPedidoBO();
                configuracionRegla.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionId"))
                    configuracionRegla.Id = (Int32)Convert.ChangeType(row["ConfiguracionId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    configuracionRegla.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("Empresa"))
                    configuracionRegla.Empresa.Nombre = (String)Convert.ChangeType(row["Empresa"], typeof(String));
                if (!row.IsNull("SucursalId"))
                    configuracionRegla.Sucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("Sucursal"))
                    configuracionRegla.Sucursal.Nombre = (String)Convert.ChangeType(row["Sucursal"], typeof(String));
                if (!row.IsNull("AlmacenId"))
                    configuracionRegla.Almacen.Id = (Int32)Convert.ChangeType(row["AlmacenId"], typeof(Int32));
                if (!row.IsNull("Almacen"))
                    configuracionRegla.Almacen.Nombre = (String)Convert.ChangeType(row["Almacen"], typeof(String));
                if (!row.IsNull("TipoPedidoID"))
                    configuracionRegla.TipoPedido.Id = (Int32)Convert.ChangeType(row["TipoPedidoID"], typeof(Int32));
                if (!row.IsNull("NombreTipoPedido"))
                    configuracionRegla.TipoPedido.Nombre = (String)Convert.ChangeType(row["NombreTipoPedido"], typeof(String));
                if (!row.IsNull("MaximoArticulosLinea"))
                    configuracionRegla.MaximoArticulosLinea = (Int32)Convert.ChangeType(row["MaximoArticulosLinea"], typeof(Int32));
                if (!row.IsNull("MaximoLineas"))
                    configuracionRegla.MaximoLineas = (Int32)Convert.ChangeType(row["MaximoLineas"], typeof(Int32));
                if (!row.IsNull("Activo"))
                    configuracionRegla.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("UC"))
                    configuracionRegla.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    configuracionRegla.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UA"))
                    configuracionRegla.Auditoria.UUA = (Int32)Convert.ChangeType(row["UA"], typeof(Int32));
                if (!row.IsNull("FA"))
                    configuracionRegla.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FA"], typeof(DateTime));
                #endregion /ConfiguracionesReglas

                lstConfiguraciones.Add(configuracionRegla);
            }
            return lstConfiguraciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
