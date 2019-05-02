using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Insertar registros de ExcepcionEtapaEmbalajeBO
    /// </summary>
    internal class ConfiguracionEtapaEmbalajeInsertarDAO : IDAOBaseInsertarAuditoria {
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
        /// Inserta un registro ExcepcionEtapaEmbalaje en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los datos a Insertar</param>
        /// <returns>Indica si la operación se efectuó correctamente</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            ConfiguracionEtapaEmbalajeBO configuracion = null;
            if (auditoriaBase is ConfiguracionEtapaEmbalajeBO)
                configuracion = (ConfiguracionEtapaEmbalajeBO)auditoriaBase;
            string msjError = string.Empty;
            if (configuracion == null)
                msjError += " , ConfiguracionRegla";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (configuracion.Empresa == null)
                msjError += " , Empresa";
            if (configuracion.Sucursal == null)
                msjError += " , Sucursal";
            if (configuracion.Almacen == null)
                msjError += " , Almacen";
            if (configuracion.TipoMovimiento == null)
                msjError += " , TipoMovimiento";
            if (configuracion.TipoPedido == null)
                msjError += " , TipoPedidoBO";
            if (!configuracion.Activo.HasValue)
                msjError += " , Activo";
            if (configuracion.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configuracion.Empresa.Id.HasValue)
                msjError += " , Empresa.Id";
            if (!configuracion.Sucursal.Id.HasValue)
                msjError += " , Sucursal.Id";
            if (!configuracion.Almacen.Id.HasValue)
                msjError += " , Almacen.Id";
            if (!configuracion.Auditoria.UC.HasValue)
                msjError += " , Auditoria.UC";
            if (!configuracion.Auditoria.UUA.HasValue)
                msjError += " , Auditoria.UUA";
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
            sCmd.Append(" INSERT INTO eRef_ExcepcionesEtapaEmbalaje (EmpresaId, SucursalId, AlmacenId, TipoMovimiento, TipoPedidoId, ");
            sCmd.Append("   Activo, UC, FC, UA, FA) VALUES(");
            // Empresa
            sValue.Append(", @excepcion_EmpresaId");
            Utileria.AgregarParametro(sqlCmd, "excepcion_EmpresaId", configuracion.Empresa.Id, System.Data.DbType.Byte);
            // Sucursal
            sValue.Append(", @excepcion_SucursalId");
            Utileria.AgregarParametro(sqlCmd, "excepcion_SucursalId", configuracion.Sucursal.Id, System.Data.DbType.Int16);
            // Almacén
            sValue.Append(", @excepcion_AlmacenId");
            Utileria.AgregarParametro(sqlCmd, "excepcion_AlmacenId", configuracion.Almacen.Id, System.Data.DbType.Int32);
            // Tipo de movimiento
            sValue.Append(", @excepcion_TipoMovimiento");
            Utileria.AgregarParametro(sqlCmd, "excepcion_TipoMovimiento", configuracion.TipoPedido.Id, System.Data.DbType.Byte);
            // Tipo de pedido
            sValue.Append(", @excepcion_TipoPedidoId");
            Utileria.AgregarParametro(sqlCmd, "excepcion_TipoPedidoId", configuracion.TipoPedido.Id, System.Data.DbType.Int32);
            // Activo
            sValue.Append(", @excepcion_Activo");
            Utileria.AgregarParametro(sqlCmd, "excepcion_Activo", configuracion.Activo, System.Data.DbType.Boolean);
            // Usuario Creación
            sValue.Append(", @excepcion_UC");
            Utileria.AgregarParametro(sqlCmd, "excepcion_UC", configuracion.Auditoria.UC, System.Data.DbType.Int32);
            // Fecha Creación
            sValue.Append(", getDate() ");
            // Usuario Modificación
            sValue.Append(", @excepcion_UA");
            Utileria.AgregarParametro(sqlCmd, "excepcion_UA", configuracion.Auditoria.UUA, System.Data.DbType.Int32);
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
