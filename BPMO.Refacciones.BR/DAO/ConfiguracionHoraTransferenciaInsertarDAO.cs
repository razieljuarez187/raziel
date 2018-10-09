using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Insertar registros de ConfiguracionHoraTransferenciaInsertar
    /// </summary>
    internal class ConfiguracionHoraTransferenciaInsertarDAO : IDAOBaseInsertarAuditoriaDetalle {
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
        /// Inserta un registro ConfiguracionHoraTransferenciaBO en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="objeto">Objeto que provee los datos a Insertar</param>
        /// <param name="objetoMaestro">Objeto que provee los datos a insertar</param>
        /// <returns>Indica si la operación se efectuó correctamente</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO objeto, AuditoriaBaseBO objetoMaestro) {
            #region Validar Filtros
            ConfiguracionHoraTransferenciaBO config = null;
            ConfiguracionTransferenciaBO confTrans = null;
            if (objeto is ConfiguracionHoraTransferenciaBO)
                config = (ConfiguracionHoraTransferenciaBO)objeto;
            if (objetoMaestro is ConfiguracionTransferenciaBO)
                confTrans = (ConfiguracionTransferenciaBO)objetoMaestro;
            string msjError = string.Empty;
            if (config == null)
                msjError += " , ConfiguracionHoraTransferenciaBO";
            if (confTrans == null)
                msjError += " , ConfiguracionTransferenciaBO";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!confTrans.Id.HasValue)
                msjError += " ConfiguracionTransferenciaId";
            if (config.Lunes == null)
                msjError += " , Lunes";
            if (config.Martes == null)
                msjError += " , Martes";
            if (config.Miercoles == null)
                msjError += " , Miercoles";
            if (config.Jueves == null)
                msjError += " , Jueves";
            if (config.Viernes == null)
                msjError += " , Viernes";
            if (config.Sabado == null)
                msjError += " , Sabado";
            if (config.Domingo == null)
                msjError += " , Domingo";
            if (!config.Activo.HasValue)
                msjError += " , Activo";
            if (confTrans.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!confTrans.Auditoria.UC.HasValue)
                msjError += " , Auditoria.UC";
            if (!confTrans.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UUA";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            #endregion

            #region Conexión a BD
            ManejadorDataContext manejadorDctx = new ManejadorDataContext(dataContext, "LIDER");
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
            sCmd.Append(" INSERT INTO eRef_confHoraTransferencia (ConfiguracionId, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo,");
            sCmd.Append("   Activo, UC, FC, UA, FA) VALUES(");
            // ConfiguracionId
            sValue.Append(", @ConfiguracionId");
            Utileria.AgregarParametro(sqlCmd, "ConfiguracionId", confTrans.Id, DbType.Int32);
            // configuracion_Lunes
            sValue.Append(", @configuracion_Lunes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Lunes", config.Lunes.ToString(), DbType.Time);
            // configuracion_Martes
            sValue.Append(", @configuracion_Martes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Martes", config.Martes.ToString(), DbType.Time);
            // configuracion_Miercoles
            sValue.Append(", @configuracion_Miercoles");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Miercoles", config.Miercoles.ToString(), DbType.Time);
            // configuracion_Jueves
            sValue.Append(", @configuracion_Jueves");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Jueves", config.Jueves.ToString(), DbType.Time);
            // configuracion_Viernes
            sValue.Append(", @configuracion_Viernes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Viernes", config.Viernes.ToString(), DbType.Time);
            // configuracion_Sabado
            sValue.Append(", @configuracion_Sabado");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Sabado", config.Sabado.ToString(), DbType.Time);
            // configuracion_Domingo
            sValue.Append(", @configuracion_Domingo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Domingo", config.Domingo.ToString(), DbType.Time);
            // Activo
            sValue.Append(", @configuracion_Activo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", config.Activo, DbType.Boolean);
            // Usuario Creación
            sValue.Append(", @configuracion_UC");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UC", confTrans.Auditoria.UC, DbType.Int32);
            // Fecha Creación
            sValue.Append(", getDate() ");
            // Usuario Modificación
            sValue.Append(", @configuracion_UA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UA", confTrans.Auditoria.UUA, DbType.Int32);
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
