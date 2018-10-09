using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    class ConfiguracionHoraTransferenciaActualizarDAO : IDAOBaseActualizarAuditoriaDetalle {
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
        /// Inserta un objeto ConfiguracionHoraTransferenciaBO en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <param name="objetoMaestro">Objeto padre que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro) {
            #region Validar Filtros
            ConfiguracionHoraTransferenciaBO config = null;
            if (auditoriaBase is ConfiguracionHoraTransferenciaBO)
                config = (ConfiguracionHoraTransferenciaBO )auditoriaBase;
            ConfiguracionTransferenciaBO confMaestro = null;
            if (objetoMaestro is ConfiguracionTransferenciaBO)
                confMaestro = (ConfiguracionTransferenciaBO)objetoMaestro;
            string msjError = string.Empty;
            if (config == null)
                msjError += " , ConfiguracionHora";
            if (confMaestro == null)
                msjError += " , ConfiguracionTransferencia";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!config.Id.HasValue)
                msjError += " , Id";
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
            if (config.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!config.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UUA";
            if (!config.Auditoria.FUA.HasValue)
                msjError += " , Auditoria.FUA";
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
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sSet = new StringBuilder();
            StringBuilder sWhere = new StringBuilder();
            sCmd.Append(" UPDATE eRef_confHoraTransferencia SET ");
            // Lunes
            sSet.Append(", Lunes = @configuracion_Lunes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Lunes", config.Lunes.ToString(), DbType.Time);
            // Martes
            sSet.Append(", Martes = @configuracion_Martes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Martes", config.Martes.ToString(), DbType.Time);
            // Miercoles
            sSet.Append(", Miercoles = @configuracion_Miercoles");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Miercoles", config.Miercoles.ToString(), DbType.Time);
            // Jueves
            sSet.Append(", Jueves = @configuracion_Jueves");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Jueves", config.Jueves.ToString(), DbType.Time);
            // Viernes
            sSet.Append(", Viernes = @configuracion_Viernes");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Viernes", config.Viernes.ToString(), DbType.Time);
            // Sabado
            sSet.Append(", Sabado = @configuracion_Sabado");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Sabado", config.Sabado.ToString(), DbType.Time);
            // Domingo
            sSet.Append(", Domingo = @configuracion_Domingo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Domingo", config.Domingo.ToString(), DbType.Time);
            // Activo
            sSet.Append(", Activo = @configuracion_Activo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", config.Activo, DbType.Boolean);
            // Usuario Modificación
            sSet.Append(", UA = @configuracion_UA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UA", config.Auditoria.UUA, DbType.Int32);
            // Fecha Modificación
            sSet.Append(", FA = getDate() ");
            // WHERE
            // Id
            sWhere.Append(" AND ConfiguracionHoraId = @ConfiguracionHora_Id");
            Utileria.AgregarParametro(sqlCmd, "ConfiguracionHora_Id", config.Id, DbType.Int32);
            // FA
            sWhere.Append(" AND FA = @ConfiguracionHora_FA");
            Utileria.AgregarParametro(sqlCmd, "ConfiguracionHora_FA", config.Auditoria.FUA, DbType.DateTime);

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
