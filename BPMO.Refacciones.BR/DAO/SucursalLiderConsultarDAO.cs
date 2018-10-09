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
    /// Acceso a Datos para consultar Sucursales de refacciones
    /// </summary>
    internal class SucursalLiderConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Métodos
        /// <summary>
        /// Obtiene una lista de Sucursales de refacciones
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="documentoBase">Objeto que provee los criterios de consulta</param>
        /// <returns></returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO documentoBase) {
            #region Validar parámetos
            SucursalLiderBO sucursal = null;
            if (documentoBase is SucursalLiderBO)
                sucursal = (SucursalLiderBO)documentoBase;
            string mensajeError = String.Empty;
            if (sucursal == null)
                mensajeError += " , Sucursal";
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
            sCmd.Append("     s.EmpresaId, s.SucursalId, s.NombreCorto, s.Sucursal, s.EstadoId, s.CiudadId, s.Direccion, s.Colonia, s.CP, s.Telefono1, ");
            sCmd.Append("     s.Fax, s.Matriz, s.Telefono2, s.MonedaNal, s.MonedaExt, s.Activo, s.OracleUOID, s.SUCURSALORACLE_ID ");
            sCmd.Append(" FROM dbo.grl_catSucursales s ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (sucursal.Id.HasValue) {
                sWhere.Append(" AND s.SucursalId = @Sucursal_Id");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_Id", sucursal.Id, System.Data.DbType.Int32);
            }
            if (sucursal.Empresa != null && sucursal.Empresa.Id.HasValue) {
                sWhere.Append(" AND s.EmpresaId = @Sucursal_EmpresaId");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_EmpresaId", sucursal.Empresa.Id, System.Data.DbType.Int16);
            }
            if (!String.IsNullOrWhiteSpace(sucursal.NombreCorto)) {
                sWhere.Append(" AND s.NombreCorto LIKE @Sucursal_NombreCorto");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_NombreCorto", sucursal.NombreCorto, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(sucursal.Nombre)) {
                sWhere.Append(" AND s.Sucursal LIKE @Sucursal_Descripcion");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_Descripcion", sucursal.Nombre, System.Data.DbType.String);
            }
            if (sucursal.Matriz.HasValue) {
                sWhere.Append(" AND s.Matriz = @Sucursal_Matriz");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_Matriz", sucursal.Matriz, System.Data.DbType.Boolean);
            }
            if (sucursal.Activo.HasValue) {
                sWhere.Append(" AND s.Activo = @Sucursal_Activo");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_Activo", sucursal.Activo, System.Data.DbType.Boolean);
            }
            if (sucursal.UnidadOperativa != null && sucursal.UnidadOperativa.Id.HasValue) {
                sWhere.Append(" AND s.OracleUOID = @Sucursal_OracleUOID");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_OracleUOID", sucursal.UnidadOperativa.Id, System.Data.DbType.Int32);
            }
            if (sucursal.SucursalOracle != null && sucursal.SucursalOracle.Id.HasValue) {
                sWhere.Append(" AND s.SUCURSALORACLE_ID = @Sucursal_SUCURSALORACLE_ID");
                Utileria.AgregarParametro(sqlCmd, "Sucursal_SUCURSALORACLE_ID", sucursal.SucursalOracle.Id, System.Data.DbType.Int32);
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
                sqlAdapter.Fill(ds, "SucursalesRefacciones");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstSucursales = new List<CatalogoBaseBO>();
            SucursalLiderBO objSucursal = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                objSucursal = new SucursalLiderBO();
                objSucursal.Empresa = new EmpresaLiderBO();
                objSucursal.Direccion = new DireccionSucursalBO() { Ubicacion = new UbicacionBO() { 
                    Pais = new PaisBO(), Estado = new EstadoBO(), Ciudad = new CiudadBO(), Municipio = new MunicipioBO() } };
                objSucursal.MonedaNacional = new MonedaBO();
                objSucursal.MonedaExtranjera = new MonedaBO();
                objSucursal.UnidadOperativa = new UnidadOperativaBO();
                objSucursal.SucursalOracle = new SucursalBO();
                objSucursal.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region Sucursales
                if (!row.IsNull("SucursalId"))
                    objSucursal.Id = (Int32)Convert.ChangeType(row["SucursalId"], typeof(Int32));
                if (!row.IsNull("EmpresaId"))
                    objSucursal.Empresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("NombreCorto"))
                    objSucursal.NombreCorto = (String)Convert.ChangeType(row["NombreCorto"], typeof(String));
                if (!row.IsNull("Sucursal"))
                    objSucursal.Nombre = (String)Convert.ChangeType(row["Sucursal"], typeof(String));
                if (!row.IsNull("EstadoId"))
                    objSucursal.Direccion.Ubicacion.Estado.Id = (Int32)Convert.ChangeType(row["EstadoId"], typeof(Int32));
                if (!row.IsNull("CiudadId"))
                    objSucursal.Direccion.Ubicacion.Ciudad.Id = (Int32)Convert.ChangeType(row["CiudadId"], typeof(Int32));
                if (!row.IsNull("Direccion")) {
                    string _dir = (String)Convert.ChangeType(row["Direccion"], typeof(String));
                    string[] _dirs = _dir.Split('|');
                    if (_dirs.Length > 0)
                        objSucursal.Direccion.Calle = _dirs[0];
                    if (_dirs.Length > 1)
                        objSucursal.Direccion.NumExt = _dirs[1];
                    if (_dirs.Length > 2)
                        objSucursal.Direccion.Cruzamientos = _dirs[2];
                }
                if (!row.IsNull("Colonia"))
                    objSucursal.Direccion.Colonia = (String)Convert.ChangeType(row["Colonia"], typeof(String));
                if (!row.IsNull("CP"))
                    objSucursal.Direccion.CodigoPostal = (String)Convert.ChangeType(row["CP"], typeof(String));
                if (!row.IsNull("Telefono1"))
                    objSucursal.Direccion.Telefono = (String)Convert.ChangeType(row["Telefono1"], typeof(String));
                if (!row.IsNull("Telefono2"))
                    objSucursal.Telefono2 = (String)Convert.ChangeType(row["Telefono2"], typeof(String));
                if (!row.IsNull("Fax"))
                    objSucursal.Fax = (String)Convert.ChangeType(row["Fax"], typeof(String));
                if (!row.IsNull("Matriz"))
                    objSucursal.Matriz = (Boolean)Convert.ChangeType(row["Matriz"], typeof(Boolean));
                if (!row.IsNull("MonedaNal"))
                    objSucursal.MonedaNacional.Id= (Int32)Convert.ChangeType(row["MonedaNal"], typeof(Int32));
                if (!row.IsNull("MonedaExt"))
                    objSucursal.MonedaExtranjera.Id = (Int32)Convert.ChangeType(row["MonedaExt"], typeof(Int32));
                if (!row.IsNull("Activo"))
                    objSucursal.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("OracleUOID"))
                    objSucursal.UnidadOperativa.Id = (Int32)Convert.ChangeType(row["OracleUOID"], typeof(Int32));
                if (!row.IsNull("SUCURSALORACLE_ID"))
                    objSucursal.SucursalOracle.Id = (Int32)Convert.ChangeType(row["SUCURSALORACLE_ID"], typeof(Int32));
                #endregion /Sucursales

                lstSucursales.Add(objSucursal);
            }
            return lstSucursales;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
