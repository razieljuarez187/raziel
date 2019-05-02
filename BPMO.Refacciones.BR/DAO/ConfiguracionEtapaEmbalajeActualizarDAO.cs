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
    /// Acceso a Datos para editar registros de ConfiguracionEtapaEmbalaje
    /// </summary>
    internal class ConfiguracionEtapaEmbalajeActualizarDAO : IDAOBaseActualizarAuditoria {
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
        /// Actualiza los datos de ConfiguracionEtapaEmbalaje en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los datos a almacenar</param>
        /// <returns>Verdadero si la actualización se realizó con éxito</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            ConfiguracionEtapaEmbalajeBO configuracion = null;
            if (auditoriaBase is ConfiguracionEtapaEmbalajeBO)
                configuracion = (ConfiguracionEtapaEmbalajeBO)auditoriaBase;
            string msjError = string.Empty;
            if (configuracion == null)
                msjError += " , ExcepcionEtapaEmbalaje";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configuracion.Id.HasValue)
                msjError += " , Id";
            if (configuracion.Empresa == null)
                msjError += " , Empresa";
            if (configuracion.Sucursal == null)
                msjError += " , Sucursal";
            if (configuracion.Almacen == null)
                msjError += " , Almacen";
            if (configuracion.TipoMovimiento == null)
                msjError += " , TipoMovimiento";
            if (configuracion.TipoPedido == null)
                msjError += " , TipoPedido";
            if (configuracion.Auditoria == null)
                msjError += " , Auditoria";
            if (!configuracion.Activo.HasValue)
                msjError += " , Activo";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configuracion.Empresa.Id.HasValue)
                msjError += " , Empresa.Id";
            if (!configuracion.Sucursal.Id.HasValue)
                msjError += " , Sucursal.Id";
            if (!configuracion.Almacen.Id.HasValue)
                msjError += " , Almacen.Id";
            if (configuracion.TipoPedido.Id == null)
                msjError += " , TipoPedidoId";
            if (!configuracion.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UUA";
            if (!configuracion.Auditoria.FUA.HasValue)
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
            sCmd.Append(" UPDATE eRef_confEtapaEmbalaje SET ");
            // Empresa
            sSet.Append(", EmpresaId = @configuracion_EmpresaId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_EmpresaId", configuracion.Empresa.Id, DbType.Byte);
            // Sucursal
            sSet.Append(", SucursalId = @configuracion_SucursalId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_SucursalId", configuracion.Sucursal.Id, DbType.Int16);
            // Almacén
            sSet.Append(", AlmacenId = @configuracion_AlmacenId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_AlmacenId", configuracion.Almacen.Id, DbType.Int32);
            // TipoMovimiento
            sSet.Append(", TipoMovimiento = @configuracion_TipoMovimiento");
            Utileria.AgregarParametro(sqlCmd, "configuracion_TipoMovimiento", configuracion.TipoMovimiento, DbType.Int16);
            // TipoPedido
            sSet.Append(", TipoPedidoId = @configuracion_TipoPedidoId");
            Utileria.AgregarParametro(sqlCmd, "configuracion_TipoPedidoId", configuracion.TipoPedido.Id, DbType.Int32);
            // Activo
            sSet.Append(", Activo = @configuracion_Activo");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", configuracion.Activo, DbType.Boolean);
            // Usuario Modificación
            sSet.Append(", UA = @configuracion_UA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_UA", configuracion.Auditoria.UUA, DbType.Int32);
            // Fecha Modificación
            sSet.Append(", FA = getDate() ");

            // WHERE
            // Id
            sWhere.Append(" AND ConfiguracionId = @configuracion_Id");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configuracion.Id, DbType.Int32);
            // Fecha Última Modificación
            sWhere.Append(" AND FA = @configuracion_FA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_FA", configuracion.Auditoria.FUA, DbType.DateTime);

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
