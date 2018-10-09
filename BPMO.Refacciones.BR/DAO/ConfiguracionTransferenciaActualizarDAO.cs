using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using System.Data.Common;
using BPMO.Refacciones.DAO;
using System.Data;
using BPMO.Primitivos.Utilerias;

namespace BPMO.Refacciones.DAO {
    class ConfiguracionTransferenciaActualizarDAO : IDAOBaseActualizarAuditoria {
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
        /// Actualiza los datos de ConfiguracionTransferenciaBO en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los datos a almacenar</param>
        /// <returns>Verdadero si la actualización se realizó con éxito</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            ConfiguracionTransferenciaBO config = null;
            if (auditoriaBase is ConfiguracionTransferenciaBO)
                config = (ConfiguracionTransferenciaBO)auditoriaBase;
            string msjError = string.Empty;
            if (config == null)
                msjError += " , ConfiguracionTransferencia";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!config.Id.HasValue)
                msjError += " , Id";
            if (config.Empresa == null)
                msjError += " , Empresa";
            if (config.Sucursal == null)
                msjError += " , Sucursal";
            if (config.Almacen == null)
                msjError += " , Almacen";
            if (config.TipoPedido == null)
                msjError += " , TipoTransferencia";
            if (config.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (config.MaximoArticulosLinea == null)
                msjError += " , MaximoArticulosLinea";
            if (config.MaximoLineas == null)
                msjError += " , MaximoLineas";
            if (!config.Activo.HasValue)
                msjError += " , Activo";           
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!config.Empresa.Id.HasValue)
                msjError += " , Empresa.Id";
            if (!config.Sucursal.Id.HasValue)
                msjError += " , Sucursal.Id";
            if (!config.Almacen.Id.HasValue)
                msjError += " , Almacen.Id";
            if (config.TipoPedido.Id == null)
                msjError += " , TipoTransferenciaId";
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
            sCmd.Append(" UPDATE eRef_confTransferencia SET ");
            // Empresa
            sSet.Append(", EmpresaId = @configuracion_EmpresaId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", config.Empresa.Id, DbType.Byte);
            // Sucursal
            sSet.Append(", SucursalId = @configuracion_SucursalId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", config.Sucursal.Id, DbType.Int16);
            // Almacén
            sSet.Append(", AlmacenId = @configuracion_AlmacenId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", config.Almacen.Id, DbType.Int32);
            // TipoTranferencia
            sSet.Append(", TipoTransferenciaId = @configuracion_TipoTranferenciaId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_TipoTranferenciaId", config.TipoPedido.Id, DbType.Int32);
            // MaximoArticulosLinea
            sSet.Append(", MaximoArticulosLinea = @configuracion_MaximoArticulosLinea");
            Utileria.AgregarParametro(sqlCmd, "configuracion_MaximoArticulosLinea", config.MaximoArticulosLinea, DbType.Int32);
            // MaximoLineas
            sSet.Append(", MaximoLineas = @configuracion_MaximoLineas");
            Utileria.AgregarParametro(sqlCmd, "configuracion_MaximoLineas", config.MaximoLineas, DbType.Int32);
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
            sWhere.Append(" AND ConfiguracionId = @configuracion_Id");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Id", config.Id, DbType.Int32);
            // Fecha Última Modificación
            sWhere.Append(" AND FA = @configuracion_Old_FA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Old_FA", config.Auditoria.FUA, DbType.DateTime);

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
