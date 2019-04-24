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
    /// Acceso a Datos para Consultar registros de ConfiguracionTransferencia
    /// </summary>
    class ConfiguracionTransferenciaConsultarDAO : IDAOBaseConsultarAuditoria {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ExcepcionEtapaEmbalaje en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de excepciones para etapas de embalaje</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetos
            ExcepcionEtapaEmbalajeBO exepcion = null;
            if (auditoriaBase is ExcepcionEtapaEmbalajeBO)
                exepcion = (ExcepcionEtapaEmbalajeBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (exepcion == null)
                mensajeError += " , ExcepcionEtapaEmbalaje";
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
            // TODO: Adecual a [eRef_ExcepcionesEtapaEmbalaje]
            sCmd.Append(" SELECT ex.ExcepcionId, ex.EmpresaId, emp.RazonSocial AS Empresa, ex.SucursalId, suc.Sucursal, ex.AlmacenId, ");
            sCmd.Append(" 	alm.Descripcion AS Almacen, ex.TipoMovimiento, tp.TipoPedidoID, tp.TipoPedido AS NombreTipoPedido, ");
            sCmd.Append("   ex.Activo, ex.UC, ex.FC, ex.UA, ex.FA ");
            sCmd.Append(" FROM eRef_ExcepcionesEtapaEmbalaje ex ");
            sCmd.Append("   INNER JOIN grl_catEmpresas emp ON (emp.EmpresaId = ex.EmpresaId) ");
            sCmd.Append("   INNER JOIN grl_catSucursales suc ON (suc.EmpresaId = ex.EmpresaId AND suc.SucursalId = ex.SucursalId) ");
            sCmd.Append("   INNER JOIN grl_catAlmacenes alm ON (alm.EmpresaId = ex.EmpresaId AND alm.SucursalId = ex.SucursalId AND alm.AlmacenId = ex.AlmacenId) ");
            sCmd.Append("   INNER JOIN ref_TiposPedido tp ON (tp.TipoPedidoID = ex.TipoPedidoId) ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (exepcion.Id.HasValue) {
                sWhere.Append(" AND ex.ExcepcionId = @excepcion_Id");
                Utileria.AgregarParametro(sqlCmd, "excepcion_Id", exepcion.Id, System.Data.DbType.Int32);
            }
            if (exepcion.Empresa != null && exepcion.Empresa.Id.HasValue) {
                sWhere.Append(" AND ex.EmpresaId = @excepcion_EmpresaId");
                Utileria.AgregarParametro(sqlCmd, "excepcion_EmpresaId", exepcion.Empresa.Id, System.Data.DbType.Byte);
            }
            if (exepcion.Sucursal != null && exepcion.Sucursal.Id.HasValue) {
                sWhere.Append(" AND ex.SucursalId = @excepcion_SucursalId");
                Utileria.AgregarParametro(sqlCmd, "excepcion_SucursalId", exepcion.Sucursal.Id, System.Data.DbType.Int16);
            }
            if (exepcion.Almacen != null && exepcion.Almacen.Id.HasValue) {
                sWhere.Append(" AND ex.AlmacenId = @excepcion_AlmacenId");
                Utileria.AgregarParametro(sqlCmd, "excepcion_AlmacenId", exepcion.Almacen.Id, System.Data.DbType.Int32);
            }
            if (exepcion.TipoMovimiento != null) {
                sWhere.Append(" AND ex.TipoMovimiento = @excepcion_TipoMovimiento");
                Utileria.AgregarParametro(sqlCmd, "excepcion_TipoMovimiento", exepcion.TipoMovimiento, System.Data.DbType.Byte);
            }
            if (exepcion.TipoPedido != null && exepcion.TipoPedido.Id.HasValue) {
                sWhere.Append(" AND ex.TipoTransferenciaId = @excepcion_TipoPedidoID");
                Utileria.AgregarParametro(sqlCmd, "excepcion_TipoPedidoID", exepcion.TipoPedido.Id, System.Data.DbType.Int32);
            }
            if (exepcion.Activo.HasValue) {
                sWhere.Append(" AND ex.Activo = @excepcion_Activo");
                Utileria.AgregarParametro(sqlCmd, "excepcion_Activo", exepcion.Activo, System.Data.DbType.Boolean);
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
                sqlAdapter.Fill(ds, "ExcepcionesEmbalaje");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstExcepciones = new List<AuditoriaBaseBO>();
            ExcepcionEtapaEmbalajeBO excepcionBO = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                excepcionBO = new ExcepcionEtapaEmbalajeBO();
                excepcionBO.Empresa = new EmpresaLiderBO();
                excepcionBO.Sucursal = new SucursalLiderBO();
                excepcionBO.Almacen = new AlmacenBO();
                excepcionBO.TipoPedido = new TipoPedidoBO();
                excepcionBO.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionId"))
                    excepcionBO.Id = (Int32)Convert.ChangeType(row["ConfiguracionId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    excepcionBO.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("Empresa"))
                    excepcionBO.Empresa.Nombre = (String)Convert.ChangeType(row["Empresa"], typeof(String));
                if (!row.IsNull("SucursalId"))
                    excepcionBO.Sucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("Sucursal"))
                    excepcionBO.Sucursal.Nombre = (String)Convert.ChangeType(row["Sucursal"], typeof(String));
                if (!row.IsNull("AlmacenId"))
                    excepcionBO.Almacen.Id = (Int32)Convert.ChangeType(row["AlmacenId"], typeof(Int32));
                if (!row.IsNull("Almacen"))
                    excepcionBO.Almacen.Nombre = (String)Convert.ChangeType(row["Almacen"], typeof(String));
                if (!row.IsNull("TipoMovimiento"))
                    excepcionBO.TipoMovimiento = (ETipoMovimiento)Convert.ChangeType(row["TipoMovimiento"], typeof(ETipoMovimiento));
                if (!row.IsNull("TipoPedidoID"))
                    excepcionBO.TipoPedido.Id = (Int32)Convert.ChangeType(row["TipoPedidoID"], typeof(Int32));
                if (!row.IsNull("NombreTipoPedido"))
                    excepcionBO.TipoPedido.Nombre = (String)Convert.ChangeType(row["NombreTipoPedido"], typeof(String));
                if (!row.IsNull("Activo"))
                    excepcionBO.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("UC"))
                    excepcionBO.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    excepcionBO.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UA"))
                    excepcionBO.Auditoria.UUA = (Int32)Convert.ChangeType(row["UA"], typeof(Int32));
                if (!row.IsNull("FA"))
                    excepcionBO.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FA"], typeof(DateTime));
                #endregion /ConfiguracionesReglas

                lstExcepciones.Add(excepcionBO);
            }
            return lstExcepciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
