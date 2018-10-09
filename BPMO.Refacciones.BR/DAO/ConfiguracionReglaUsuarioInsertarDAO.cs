using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Insertar registros de ConfiguracionReglaUsuario
    /// </summary>
    internal class ConfiguracionReglaUsuarioInsertarDAO : IDAOBaseInsertarAuditoria {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion /Atributos

        #region Propiedades
        /// <summary>
        /// Obtiene la cantidad de registros afectados despues de realizar la operación.
        /// </summary>
        /// <remarks>Tiene valor solo cuando se utilizan los métodos Insertar,Actualizar y Borrar.</remarks>
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        /// <summary>
        /// Obtiene el último identificador generado al insertar.
        /// </summary>
        /// <remarks>Tiene valor solo cuando se utiliza el método Insertar.</remarks>
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion /Propiedades

        #region Métodos
        /// <summary>
        /// Inserta un registro ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los datos a Insertar</param>
        /// <returns>Indica si la operación se efectuó correctamente</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
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
            if (!configRegla.Auditoria.UC.HasValue)
                msjError += " , Auditoria.UC";
            if (!configRegla.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UAA";
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
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" INSERT INTO eRef_confReglasUsuarios (EmpresaId, SucursalId, AlmacenId, UsuarioID, TipoRegla, ValorInicial, ValorFinal,");
            sCmd.Append("   Activo, UC, FC, UA, FA) VALUES(");
            // Empresa
            sValue.Append(", @configuracion_EmpresaId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", configRegla.Empresa.Id, System.Data.DbType.Byte);
            // Sucursal
            sValue.Append(", @configuracion_SucursalId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", configRegla.Sucursal.Id, System.Data.DbType.Int16);
            // Almacén
            sValue.Append(", @configuracion_AlmacenId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", configRegla.Almacen.Id, System.Data.DbType.Int32);
            // Usuario
            sValue.Append(", @configuracion_UsuarioID");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UsuarioID", configRegla.Usuario.Id, System.Data.DbType.Int32);
            // TipoRegla
            sValue.Append(", @configuracion_TipoRegla");
            Utileria.AgregarParametro(sqlCmd, "configuracion_TipoRegla", configRegla.TipoRegla, System.Data.DbType.Byte);
            // ValorInicial
            if (configRegla.ValorInicial.HasValue) {
                sValue.Append(", @configuracion_ValorInicial");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorInicial", configRegla.ValorInicial, System.Data.DbType.Decimal);
            } else {
                sValue.Append(", NULL");
            }
            // ValorFinal
            if (configRegla.ValorFinal.HasValue) {
                sValue.Append(", @configuracion_ValorFinal");
                Utileria.AgregarParametro(sqlCmd, "configuracion_ValorFinal", configRegla.ValorFinal, System.Data.DbType.Decimal);
            } else {
                sValue.Append(", NULL");
            }
            // Activo
            sValue.Append(", @configuracion_Activo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", configRegla.Activo, System.Data.DbType.Boolean);
            // Usuario Creación
            sValue.Append(", @configuracion_UC");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UC", configRegla.Auditoria.UC, System.Data.DbType.Int32);
            // Fecha Creación
            sValue.Append(", getDate() ");
            // Usuario Modificación
            sValue.Append(", @configuracion_UA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UA", configRegla.Auditoria.UUA, System.Data.DbType.Int32);
            // Fecha Modificación
            sValue.Append(", getDate() ");

            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");

            sCmd.Append(" SELECT @identity = SCOPE_IDENTITY()");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "identity";
            sqlParam.DbType = DbType.Int32;
            sqlParam.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
                if (result > 0) {
                    object resultId = sqlCmd.Parameters["identity"].Value;
                    if (!(resultId is DBNull))
                        ultimoIdGenerado = Convert.ToInt32(resultId);
                    else
                        ultimoIdGenerado = null;
                }
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion
        }
        #endregion /Métodos
    }
}
