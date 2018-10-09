using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para consultar Almacenes de refacciones
    /// </summary>
    internal class AlmacenLiderConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Métodos
        /// <summary>
        /// Obtiene una lista de Almacenes de refacciones
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="documentoBase">Objeto que provee los criterios de consulta</param>
        /// <returns></returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO documentoBase) {
            #region Validar parámetos
            AlmacenBO almacen = null;
            if (documentoBase is AlmacenBO)
                almacen = (BO.AlmacenBO)documentoBase;
            string mensajeError = String.Empty;
            if (almacen == null)
                mensajeError += " , Almacen";
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
            sCmd.Append(" SELECT ");
            sCmd.Append("     a.AlmacenId, a.EmpresaId, a.SucursalId, a.Descripcion, a.Activo, a.InterEmpresas, a.AlmacenAlterno, a.Consigna, a.TipoAlmacen ");
            sCmd.Append(" FROM dbo.grl_catAlmacenes a ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (almacen.Id.HasValue) {
                sWhere.Append(" AND a.AlmacenId = @Almacen_Id");
                Utileria.AgregarParametro(sqlCmd, "Almacen_Id", almacen.Id, System.Data.DbType.Int32);
            }
            if (almacen.Sucursal != null && almacen.Sucursal.Id.HasValue) {
                sWhere.Append(" AND a.SucursalId = @Almacen_SucursalId");
                Utileria.AgregarParametro(sqlCmd, "Almacen_SucursalId", almacen.Sucursal.Id, System.Data.DbType.Int16);
            }
            if (!String.IsNullOrWhiteSpace(almacen.Nombre)) {
                sWhere.Append(" AND a.Descripcion LIKE @Almacen_Descripcion");
                Utileria.AgregarParametro(sqlCmd, "Almacen_Descripcion", almacen.Nombre, System.Data.DbType.String);
            }
            if (almacen.Activo.HasValue) {
                sWhere.Append(" AND a.Activo = @Almacen_Activo");
                Utileria.AgregarParametro(sqlCmd, "Almacen_Activo", almacen.Activo, System.Data.DbType.Boolean);
            }
            if (almacen.InterEmpresas.HasValue) {
                sWhere.Append(" AND a.InterEmpresas = @Almacen_InterEmpresas");
                Utileria.AgregarParametro(sqlCmd, "Almacen_InterEmpresas", almacen.InterEmpresas, System.Data.DbType.Boolean);
            }
            if (almacen.AlmacenAlterno.HasValue) {
                sWhere.Append(" AND a.AlmacenAlterno = @Almacen_AlmacenAlterno");
                Utileria.AgregarParametro(sqlCmd, "Almacen_AlmacenAlterno", almacen.AlmacenAlterno, System.Data.DbType.Boolean);
            }
            if (almacen.EsConsigna.HasValue) {
                sWhere.Append(" AND a.Consigna = @Almacen_Consigna");
                Utileria.AgregarParametro(sqlCmd, "Almacen_Consigna", almacen.EsConsigna, System.Data.DbType.Boolean);
            }
            if (!String.IsNullOrWhiteSpace(almacen.TipoAlmacen)) {
                sWhere.Append(" AND a.TipoAlmacen = @Almacen_TipoAlmacen");
                Utileria.AgregarParametro(sqlCmd, "Almacen_TipoAlmacen", almacen.TipoAlmacen, System.Data.DbType.String);
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
                sqlAdapter.Fill(ds, "AlmacenesRefacciones");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstAlmacenes = new List<CatalogoBaseBO>();
            AlmacenBO objAlmacen = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                objAlmacen = new AlmacenBO();
                objAlmacen.Departamento = new DepartamentoBO();
                objAlmacen.Sucursal = new SucursalLiderBO() { Empresa = new EmpresaLiderBO() };
                objAlmacen.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region Almacenes
                if (!row.IsNull("AlmacenId"))
                    objAlmacen.Id = (Int32)Convert.ChangeType(row["AlmacenId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    objAlmacen.Sucursal.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("SucursalId"))
                    objAlmacen.Sucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("Descripcion"))
                    objAlmacen.Nombre = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
                if (!row.IsNull("Activo"))
                    objAlmacen.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("InterEmpresas"))
                    objAlmacen.InterEmpresas = (Boolean)Convert.ChangeType(row["InterEmpresas"], typeof(Boolean));
                if (!row.IsNull("AlmacenAlterno"))
                    objAlmacen.AlmacenAlterno = (Boolean)Convert.ChangeType(row["AlmacenAlterno"], typeof(Boolean));
                if (!row.IsNull("Consigna"))
                    objAlmacen.EsConsigna = (Boolean)Convert.ChangeType(row["Consigna"], typeof(Boolean));
                if (!row.IsNull("TipoAlmacen"))
                    objAlmacen.TipoAlmacen = (String)Convert.ChangeType(row["TipoAlmacen"], typeof(String));
                #endregion /Almacenes

                lstAlmacenes.Add(objAlmacen);
            }
            return lstAlmacenes;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
