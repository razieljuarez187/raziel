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
    /// Acceso a datos para consultar UsuarioLider
    /// </summary>
    internal class UsuarioLiderConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Métodos
        /// <summary>
        /// Obtiene una lista de Sucursales de refacciones
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="documentoBase">Objeto que provee los criterios de consulta</param>
        /// <returns></returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO documentoBase) {
            #region Validar parámetos
            UsuarioLiderBO usuario = null;
            if (documentoBase is UsuarioLiderBO)
                usuario = (UsuarioLiderBO)documentoBase;
            string mensajeError = String.Empty;
            if (usuario == null)
                mensajeError += " , Usuario";
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
            sCmd.Append(" SELECT  ");
            sCmd.Append(" 	   u.UsuarioID, u.Usuario, u.Nombre, u.EmpleadoID, u.DeptoId, u.Status, u.FechaAlta, u.FechaEdicion ");
            sCmd.Append(" FROM dbo.ssl_CatUsuarios u ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (usuario.Id.HasValue) {
                sWhere.Append(" AND u.UsuarioID = @Usuario_Id");
                Utileria.AgregarParametro(sqlCmd, "Usuario_Id", usuario.Id, System.Data.DbType.Int32);
            }
            if (!String.IsNullOrWhiteSpace(usuario.NombreCorto)) {
                sWhere.Append(" AND u.Usuario LIKE @Usuario_Usuario");
                Utileria.AgregarParametro(sqlCmd, "Usuario_Usuario", usuario.NombreCorto, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(usuario.Nombre)) {
                sWhere.Append(" AND u.Nombre LIKE @Usuario_Nombre");
                Utileria.AgregarParametro(sqlCmd, "Usuario_Nombre", usuario.Nombre, System.Data.DbType.String);
            }
            if (usuario.Empleado != null && usuario.Empleado.Id.HasValue) {
                sWhere.Append(" AND u.EmpleadoID = @Usuario_EmpleadoID");
                Utileria.AgregarParametro(sqlCmd, "Usuario_EmpleadoID", usuario.Empleado.Id, System.Data.DbType.Int32);
            }
            if (usuario.Deptartamento != null && usuario.Deptartamento.Id.HasValue) {
                sWhere.Append(" AND u.DeptoId = @Usuario_DeptoId");
                Utileria.AgregarParametro(sqlCmd, "Usuario_DeptoId", usuario.Deptartamento.Id, System.Data.DbType.Byte);
            }
            if (usuario.Activo.HasValue) {
                sWhere.Append(" AND u.Status = @Usuario_Status");
                Utileria.AgregarParametro(sqlCmd, "Usuario_Status", usuario.Activo, System.Data.DbType.Boolean);
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
                sqlAdapter.Fill(ds, "UsuariosRefacciones");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstUsuarios = new List<CatalogoBaseBO>();
            UsuarioLiderBO objUsuario = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                objUsuario = new UsuarioLiderBO();
                objUsuario.Empleado = new EmpleadoBO();
                objUsuario.Deptartamento = new DepartamentoBO();
                objUsuario.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region Usuarios
                if (!row.IsNull("UsuarioID"))
                    objUsuario.Id = (Int32)Convert.ChangeType(row["UsuarioID"], typeof(Int32));
                if (!row.IsNull("Usuario"))
                    objUsuario.NombreCorto = (String)Convert.ChangeType(row["Usuario"], typeof(String));
                if (!row.IsNull("Nombre"))
                    objUsuario.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
                if (!row.IsNull("EmpleadoID"))
                    objUsuario.Empleado.Id = (Int32)Convert.ChangeType(row["EmpleadoID"], typeof(Int32));
                if (!row.IsNull("DeptoId"))
                    objUsuario.Deptartamento.Id = (Int32)Convert.ChangeType(row["DeptoId"], typeof(Int32));
                if (!row.IsNull("Status"))
                    objUsuario.Activo = (Boolean)Convert.ChangeType(row["Status"], typeof(Boolean));
                if (!row.IsNull("FechaAlta"))
                    objUsuario.Auditoria.FC = (DateTime)Convert.ChangeType(row["FechaAlta"], typeof(DateTime));
                if (!row.IsNull("FechaEdicion"))
                    objUsuario.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FechaEdicion"], typeof(DateTime));
                #endregion /Sucursales

                lstUsuarios.Add(objUsuario);
            }
            return lstUsuarios;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
