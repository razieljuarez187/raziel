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
    /// Acceso a Datos para Consultar registros de ConfiguracionReglaUsuario
    /// </summary>
    internal class ConfiguracionReglaUsuarioConsultarDAO : IDAOBaseConsultarAuditoria {
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
            ConfiguracionReglaUsuarioBO configRegla = null;
            if (auditoriaBase is ConfiguracionReglaUsuarioBO)
                configRegla = (ConfiguracionReglaUsuarioBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (configRegla == null)
                mensajeError += " , ConfiguracionRegla";
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
            sCmd.Append(" SELECT conf.ConfiguracionReglaId, conf.EmpresaId, conf.SucursalId, conf.AlmacenId, conf.UsuarioID, conf.TipoRegla, ");
            sCmd.Append("     conf.ValorInicial, conf.ValorFinal, conf.Activo, conf.UC, conf.FC, conf.UA, conf.FA ");
            sCmd.Append(" FROM eRef_confReglasUsuarios conf ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (configRegla.Id.HasValue) {
                sWhere.Append(" AND conf.ConfiguracionReglaId = @configuracion_Id");
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
            if (configRegla.Usuario != null && configRegla.Usuario.Id.HasValue) {
                sWhere.Append(" AND conf.UsuarioID = @configuracion_UsuarioID");
                Utileria.AgregarParametro(sqlCmd, "configuracion_UsuarioID", configRegla.Usuario.Id, System.Data.DbType.Int32);
            }
            if (configRegla.TipoRegla.HasValue) {
                sWhere.Append(" AND conf.TipoRegla = @configuracion_TipoRegla");
                Utileria.AgregarParametro(sqlCmd, "configuracion_TipoRegla", configRegla.TipoRegla, System.Data.DbType.Byte);
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
            ConfiguracionReglaUsuarioBO configuracionRegla = null;
            
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionRegla.Empresa = new EmpresaLiderBO();
                configuracionRegla.Sucursal = new SucursalLiderBO();
                configuracionRegla.Almacen = new AlmacenBO();
                configuracionRegla.Usuario = new UsuarioLiderBO();
                configuracionRegla.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionReglaId"))
                    configuracionRegla.Id = (Int32)Convert.ChangeType(row["ConfiguracionReglaId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    configuracionRegla.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("SucursalId"))
                    configuracionRegla.Sucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("AlmacenId"))
                    configuracionRegla.Almacen.Id = (Int32)Convert.ChangeType(row["AlmacenId"], typeof(Int32));
                if (!row.IsNull("UsuarioID"))
                    configuracionRegla.Usuario.Id = (Int32)Convert.ChangeType(row["UsuarioID"], typeof(Int32));
                if (!row.IsNull("TipoRegla"))
                    configuracionRegla.TipoRegla = (ETipoReglaUsuario)Convert.ChangeType(row["TipoRegla"], typeof(ETipoReglaUsuario));
                if (!row.IsNull("ValorInicial"))
                    configuracionRegla.ValorInicial = (Decimal)Convert.ChangeType(row["ValorInicial"], typeof(Decimal));
                if (!row.IsNull("ValorFinal"))
                    configuracionRegla.ValorFinal = (Decimal)Convert.ChangeType(row["ValorFinal"], typeof(Decimal));
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
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetos
            ConfiguracionReglaUsuarioBO configRegla = null;
            if (auditoriaBase is ConfiguracionReglaUsuarioBO)
                configRegla = (ConfiguracionReglaUsuarioBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (configRegla == null)
                mensajeError += " , ConfiguracionRegla";
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
            sCmd.Append(" SELECT conf.ConfiguracionReglaId, conf.EmpresaId, emp.RazonSocial AS Empresa, conf.SucursalId, suc.Sucursal, conf.AlmacenId, ");
            sCmd.Append(" 	alm.Descripcion AS Almacen, conf.UsuarioID, usr.Usuario, usr.Nombre AS UsuarioNombre, conf.TipoRegla, conf.ValorInicial, ");
            sCmd.Append("   conf.ValorFinal, conf.Activo, conf.UC, conf.FC, conf.UA, conf.FA ");
            sCmd.Append(" FROM eRef_confReglasUsuarios conf ");
            sCmd.Append("   INNER JOIN grl_catEmpresas emp ON (emp.EmpresaId = conf.EmpresaId) ");
            sCmd.Append("   INNER JOIN grl_catSucursales suc ON (suc.EmpresaId = conf.EmpresaId AND suc.SucursalId = conf.SucursalId) ");
            sCmd.Append("   INNER JOIN grl_catAlmacenes alm ON (alm.EmpresaId = conf.EmpresaId AND alm.SucursalId = conf.SucursalId AND alm.AlmacenId = conf.AlmacenId) ");
            sCmd.Append("   INNER JOIN ssl_CatUsuarios usr ON (usr.UsuarioID = conf.UsuarioID) ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (configRegla.Id.HasValue) {
                sWhere.Append(" AND conf.ConfiguracionReglaId = @configuracion_Id");
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
            if (configRegla.Usuario != null && configRegla.Usuario.Id.HasValue) {
                sWhere.Append(" AND conf.UsuarioID = @configuracion_UsuarioID");
                Utileria.AgregarParametro(sqlCmd, "configuracion_UsuarioID", configRegla.Usuario.Id, System.Data.DbType.Int32);
            }
            if (configRegla.TipoRegla.HasValue) {
                sWhere.Append(" AND conf.TipoRegla = @configuracion_TipoRegla");
                Utileria.AgregarParametro(sqlCmd, "configuracion_TipoRegla", configRegla.TipoRegla, System.Data.DbType.Byte);
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
            ConfiguracionReglaUsuarioBO configuracionRegla = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionRegla = new ConfiguracionReglaUsuarioBO();
                configuracionRegla.Empresa = new EmpresaLiderBO();
                configuracionRegla.Sucursal = new SucursalLiderBO();
                configuracionRegla.Almacen = new AlmacenBO();
                configuracionRegla.Usuario = new UsuarioLiderBO();
                configuracionRegla.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionReglaId"))
                    configuracionRegla.Id = (Int32)Convert.ChangeType(row["ConfiguracionReglaId"], typeof(Int32));
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
                if (!row.IsNull("UsuarioID"))
                    configuracionRegla.Usuario.Id = (Int32)Convert.ChangeType(row["UsuarioID"], typeof(Int32));
                if (!row.IsNull("Usuario"))
                    configuracionRegla.Usuario.NombreCorto = (String)Convert.ChangeType(row["Usuario"], typeof(String));
                if (!row.IsNull("UsuarioNombre"))
                    configuracionRegla.Usuario.Nombre = (String)Convert.ChangeType(row["UsuarioNombre"], typeof(String));
                if (!row.IsNull("TipoRegla"))
                    configuracionRegla.TipoRegla = (ETipoReglaUsuario)Convert.ChangeType(row["TipoRegla"], typeof(byte));
                if (!row.IsNull("ValorInicial"))
                    configuracionRegla.ValorInicial = (Decimal)Convert.ChangeType(row["ValorInicial"], typeof(Decimal));
                if (!row.IsNull("ValorFinal"))
                    configuracionRegla.ValorFinal = (Decimal)Convert.ChangeType(row["ValorFinal"], typeof(Decimal));
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
