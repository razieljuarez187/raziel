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
    /// Acceso a Datos para consultar Empresas
    /// </summary>
    internal class EmpresaLiderConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Métodos
        /// <summary>
        /// Obtiene una lista de Empresas refacciones
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="documentoBase">Objeto que provee los criterios de consulta</param>
        /// <returns></returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO documentoBase) {
            #region Validar parámetos
            EmpresaLiderBO empresa = null;
            if (documentoBase is EmpresaLiderBO)
                empresa = (EmpresaLiderBO)documentoBase;
            string mensajeError = String.Empty;
            if (empresa == null)
                mensajeError += " , Empresa";
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
            sCmd.Append("     e.EmpresaId, e.NombreCorto, e.RazonSocial, e.RFC, e.CURP, e.Direccion, e.Ciudad, e.Poblacion, e.Estado, ");
            sCmd.Append("     e.CodPost, e.Telefono, e.Email, e.Colonia ");
            sCmd.Append(" FROM dbo.grl_catEmpresas e ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (empresa.Id.HasValue) {
                sWhere.Append(" AND e.EmpresaId = @Empresa_Id");
                Utileria.AgregarParametro(sqlCmd, "Empresa_Id", empresa.Id, System.Data.DbType.Int32);
            }
            if (!String.IsNullOrWhiteSpace(empresa.NombreCorto)) {
                sWhere.Append(" AND e.NombreCorto LIKE @Empresa_NombreCorto");
                Utileria.AgregarParametro(sqlCmd, "Empresa_NombreCorto", empresa.NombreCorto, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(empresa.Nombre)) {
                sWhere.Append(" AND e.RazonSocial LIKE @Empresa_RazonSocial");
                Utileria.AgregarParametro(sqlCmd, "Empresa_RazonSocial", empresa.Nombre, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(empresa.RFC)) {
                sWhere.Append(" AND e.RFC = @Empresa_RFC");
                Utileria.AgregarParametro(sqlCmd, "Empresa_RFC", empresa.RFC, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(empresa.CURP)) {
                sWhere.Append(" AND e.CURP = @Empresa_CURP");
                Utileria.AgregarParametro(sqlCmd, "Empresa_CURP", empresa.CURP, System.Data.DbType.String);
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
                sqlAdapter.Fill(ds, "EmpresasRefacciones");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstEmpresas = new List<CatalogoBaseBO>();
            EmpresaLiderBO objEmpresa = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                objEmpresa = new EmpresaLiderBO();
                objEmpresa.Direccion = new DireccionSucursalBO() {
                    Ubicacion = new UbicacionBO() {
                        Pais = new PaisBO(), Estado = new EstadoBO(), Ciudad = new CiudadBO(), Municipio = new MunicipioBO()
                    }
                };
                objEmpresa.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region Empresas
                if (!row.IsNull("EmpresaId"))
                    objEmpresa.Id = (Int32)Convert.ChangeType(row["EmpresaId"], typeof(Int32));
                if (!row.IsNull("NombreCorto"))
                    objEmpresa.NombreCorto = (String)Convert.ChangeType(row["NombreCorto"], typeof(String));
                if (!row.IsNull("RazonSocial"))
                    objEmpresa.Nombre = (String)Convert.ChangeType(row["RazonSocial"], typeof(String));
                if (!row.IsNull("RFC"))
                    objEmpresa.RFC = (String)Convert.ChangeType(row["RFC"], typeof(String));
                if (!row.IsNull("CURP"))
                    objEmpresa.CURP = (String)Convert.ChangeType(row["CURP"], typeof(String));
                if (!row.IsNull("Direccion"))
                    objEmpresa.Direccion.Calle = (String)Convert.ChangeType(row["Direccion"], typeof(String));
                if (!row.IsNull("Estado"))
                    objEmpresa.Direccion.Ubicacion.Estado.Id = (Int32)Convert.ChangeType(row["Estado"], typeof(Int32));
                if (!row.IsNull("Ciudad"))
                    objEmpresa.Direccion.Ubicacion.Ciudad.Id = (Int32)Convert.ChangeType(row["Ciudad"], typeof(Int32));
                if (!row.IsNull("Poblacion"))
                    objEmpresa.Direccion.Ubicacion.Municipio.Id = (Int32)Convert.ChangeType(row["Poblacion"], typeof(Int32));
                if (!row.IsNull("Colonia"))
                    objEmpresa.Direccion.Colonia = (String)Convert.ChangeType(row["Colonia"], typeof(String));
                if (!row.IsNull("CodPost"))
                    objEmpresa.Direccion.CodigoPostal = (String)Convert.ChangeType(row["CodPost"], typeof(String));
                if (!row.IsNull("Telefono"))
                    objEmpresa.Direccion.Telefono = (String)Convert.ChangeType(row["Telefono"], typeof(String));
                if (!row.IsNull("Email"))
                    objEmpresa.Email = (String)Convert.ChangeType(row["Email"], typeof(String));
                #endregion /Empresas

                lstEmpresas.Add(objEmpresa);
            }
            return lstEmpresas;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
