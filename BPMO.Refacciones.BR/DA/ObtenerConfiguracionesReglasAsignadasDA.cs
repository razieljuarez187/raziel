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
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.DA {
    /// <summary>
    /// Acceso a datos para el reporte de Configuraciones asignadas
    /// </summary>
    internal class ObtenerConfiguracionesReglasAsignadasDA {
        #region Métodos
        /// <summary>
        /// Obtiene un DataSet con las configuraciones asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configRegla">Objeto con los parámetros de búsqueda</param>
        /// <returns></returns>
        public DataSet ConsultarConfiguracionesAsignadas(IDataContext dataContext, ConfiguracionReglaUsuarioFiltroBO configRegla) {
            #region Validar parámetos
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
            if (configRegla.ValorInicial.HasValue) {
                sWhere.Append(" AND conf.ValorInicial >= @configuracion_ValorInicialRangoA");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorInicialRangoA", configRegla.ValorInicial, System.Data.DbType.Decimal);
            }
            if (configRegla.ValorInicialFin.HasValue) {
                sWhere.Append(" AND conf.ValorInicial <= @configuracion_ValorInicialRangoB");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorInicialRangoB", configRegla.ValorInicialFin, System.Data.DbType.Decimal);
            }
            if (configRegla.ValorFinal.HasValue) {
                sWhere.Append(" AND conf.ValorFinal >= @configuracion_ValorFinalRangoA");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorFinalRangoA", configRegla.ValorFinal, System.Data.DbType.Decimal);
            }
            if (configRegla.ValorFinalFin.HasValue) {
                sWhere.Append(" AND conf.ValorFinal <= @configuracion_ValorFinalRangoB");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorFinalRangoB", configRegla.ValorFinalFin, System.Data.DbType.Decimal);
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
                sqlAdapter.Fill(ds, "ConfiguracionesReglasAsignadas");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            return ds;
        }
        #endregion /Métodos
    }
}
