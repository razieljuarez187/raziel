using System;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para modificar un registro ConfiguracionReglaUsuario
    /// </summary>
    internal class ConfiguracionReglaUsuarioActualizarDAO : IDAOBaseActualizarAuditoria {
        #region Atributos
        private int registrosAfectados;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la cantidad de registros afectados despues de realizar la operación.
        /// </summary>
        /// <remarks>Tiene valor solo cuando se utilizan los métodos Insertar,Actualizar y Borrar.</remarks>
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="auditoriaBase"></param>
        /// <returns></returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            ConfiguracionReglaUsuarioBO configRegla = null;
            if (auditoriaBase is ConfiguracionReglaUsuarioBO)
                configRegla = (ConfiguracionReglaUsuarioBO)auditoriaBase;
            string msjError = string.Empty;
            if (configRegla == null)
                msjError += " , ConfiguracionRegla";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configRegla.Id.HasValue)
                msjError += " , Id";
            if (configRegla.Empresa == null)
                msjError += " , Empresa";
            if (configRegla.Sucursal == null)
                msjError += " , Sucursal";
            if (configRegla.Almacen == null)
                msjError += " , Almacen";
            if (configRegla.Usuario == null)
                msjError += " , Usuario";
            if (!configRegla.TipoRegla.HasValue)
                msjError += " , TipoRegla";
            if (!configRegla.ValorInicial.HasValue && !configRegla.ValorFinal.HasValue)
                msjError += " , ValorInicial ó ValorFinal";
            if (!configRegla.Activo.HasValue)
                msjError += " , Activo";
            if (configRegla.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));            
            if (!configRegla.Empresa.Id.HasValue)
                msjError += " , Empresa.Id";
            if (!configRegla.Sucursal.Id.HasValue)
                msjError += " , Sucursal.Id";
            if (!configRegla.Almacen.Id.HasValue)
                msjError += " , Almacen.Id";
            if (!configRegla.Usuario.Id.HasValue)
                msjError += " , Usuario.Id";
            if (!configRegla.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UUA";
            if (!configRegla.Auditoria.FUA.HasValue)
                msjError += " , Auditoria.FUA";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            #endregion

            #region Conexión a BD
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sSet = new StringBuilder();
            StringBuilder sWhere = new StringBuilder();
            sCmd.Append(" UPDATE eRef_confReglasUsuarios SET ");
            // Empresa
            sSet.Append(", EmpresaId = @configuracion_EmpresaId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", configRegla.Empresa.Id, System.Data.DbType.Byte);
            // Sucursal
            sSet.Append(", SucursalId = @configuracion_SucursalId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", configRegla.Sucursal.Id, System.Data.DbType.Int16);
            // Almacén
            sSet.Append(", AlmacenId = @configuracion_AlmacenId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", configRegla.Almacen.Id, System.Data.DbType.Int32);
            // Usuario
            sSet.Append(", UsuarioID = @configuracion_UsuarioID");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UsuarioID", configRegla.Usuario.Id, System.Data.DbType.Int32);
            // TipoRegla
            sSet.Append(", TipoRegla = @configuracion_TipoRegla");
            Utileria.AgregarParametro(sqlCmd, "configuracion_TipoRegla", configRegla.TipoRegla, System.Data.DbType.Byte);
            // ValorInicial
            if (configRegla.ValorInicial.HasValue) {
                sSet.Append(", ValorInicial = @configuracion_ValorInicial");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorInicial", configRegla.ValorInicial, System.Data.DbType.Decimal);
            } else {
                sSet.Append(", ValorInicial = NULL");
            }
            // ValorFinal
            if (configRegla.ValorFinal.HasValue) {
                sSet.Append(", ValorFinal = @configuracion_ValorFinal");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorFinal", configRegla.ValorFinal, System.Data.DbType.Decimal);
            } else {
                sSet.Append(", ValorFinal = NULL");
            }
            // Activo
            sSet.Append(", Activo = @configuracion_Activo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", configRegla.Activo, System.Data.DbType.Boolean);
            // Usuario Modificación
            sSet.Append(", UA = @configuracion_UA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UA", configRegla.Auditoria.UUA, System.Data.DbType.Int32);
            // Fecha Modificación
            sSet.Append(", FA = getDate() ");

            // WHERE
            // Id
            sWhere.Append(" AND ConfiguracionReglaId = @configuracion_Id");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configRegla.Id, System.Data.DbType.Int32);
            // Fecha Última Modificación
            sWhere.Append(" AND FA = @configuracion_Old_FA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Old_FA", configRegla.Auditoria.FUA, System.Data.DbType.DateTime);

            string cmd = sSet.ToString().Trim();
            if (cmd.StartsWith(", "))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al actualizar el registro o fue modificado mientras era editado.");
            else
                return true;
            #endregion
        } 
        #endregion /Métodos
    }
}
