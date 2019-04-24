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
    /// Acceso a Datos para Consultar registros de TipoPedidoConsultar
    /// </summary>
    internal class TransportistaConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de TransportistaConsultarDAO en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de transportistas</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            #region Validar parámetos
            TransportistaBO transportista = null;
            if (catalogoBase is TransportistaBO)
                transportista = (TransportistaBO)catalogoBase;
            string mensajeError = String.Empty;
            if (transportista == null)
                mensajeError += " , Transportista";
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
            sCmd.Append(" SELECT TransportistaID, Clave, Nombre, FUA, Activo ");
            sCmd.Append(" FROM ref_CatTransportistas");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (transportista.Id.HasValue) {
                sWhere.Append(" AND TransportistaID = @valor_TransportistaID");
                Utileria.AgregarParametro(sqlCmd, "valor_TransportistaID", transportista.Id, System.Data.DbType.Int32);
            }
            if (!String.IsNullOrWhiteSpace(transportista.NombreCorto)) {
                sWhere.Append(" AND Clave LIKE @valor_Clave");
                Utileria.AgregarParametro(sqlCmd, "valor_Clave", transportista.NombreCorto, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(transportista.Nombre)) {
                sWhere.Append(" AND Nombre LIKE @valor_Nombre");
                Utileria.AgregarParametro(sqlCmd, "valor_Nombre", transportista.Nombre, System.Data.DbType.String);
            }
            if (transportista.Auditoria != null) {
                if (transportista.Auditoria.FUA.HasValue) {
                    sWhere.Append(" AND FUA = @valor_FUA");
                    Utileria.AgregarParametro(sqlCmd, "valor_FUA", transportista.Auditoria.FUA, System.Data.DbType.DateTime);
                }
            }
            if (transportista.Activo.HasValue) {
                sWhere.Append(" AND Activo = @valor_Activo");
                Utileria.AgregarParametro(sqlCmd, "valor_Activo", transportista.Activo, System.Data.DbType.Boolean);
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
                sqlAdapter.Fill(ds, "Transportistas");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstTransportistas = new List<CatalogoBaseBO>();
            TransportistaBO transportistaBO = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                transportistaBO = new TransportistaBO();
                transportista.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("TransportistaID"))
                    transportistaBO.Id = (Int32)Convert.ChangeType(row["TransportistaID"], typeof(Int32));
                if (!row.IsNull("Nombre"))
                    transportistaBO.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
                if (!row.IsNull("Clave"))
                    transportistaBO.NombreCorto = (string)Convert.ChangeType(row["Clave"], typeof(string));
                if (!row.IsNull("FUA"))
                    transportistaBO.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FUA"], typeof(DateTime));
                if (!row.IsNull("Activo"))
                    transportistaBO.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
                #endregion /ConfiguracionesReglas

                lstTransportistas.Add(transportistaBO);
            }
            return lstTransportistas;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
