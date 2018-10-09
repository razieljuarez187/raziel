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
    /// Acceso a datos para consulta de refacciones
    /// </summary>
    internal class RefaccionConsultarDAO : IDAOBaseConsultarCatalogo {
        /// <summary>
        /// Consulta de Refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refacciones que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las Refacciones recuperados por la consulta</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            #region Validar Filtros
            RefaccionBO Refaccion = null;
            if (catalogoBase is RefaccionBO)
                Refaccion = (RefaccionBO)catalogoBase;
            string msjError = string.Empty;
            if (Refaccion == null)
                msjError += " , Refaccion";
            if (dataContext == null)
                msjError += " , datacontext";
            if (Refaccion.Id == null)
                msjError += " , Id ";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
            #endregion

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
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ArticuloId, CveArt, Descripcion, LineaId FROM ref_CatArticulos ");
            StringBuilder sWhere = new StringBuilder();
            #region Refaccion
            if (Refaccion.Id != null) {
                sWhere.Append(" ArticuloId = @ArticuloId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ArticuloId";
                sqlParam.Value = Refaccion.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (Refaccion.NombreCorto != null) {
                sWhere.Append(" AND CveArt = @CveArt");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "CveArt";
                sqlParam.Value = Refaccion.NombreCorto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (Refaccion.Nombre != null) {
                sWhere.Append(" AND Descripcion = @Descripcion");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "Descripcion";
                sqlParam.Value = Refaccion.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (Refaccion.Linea != null && Refaccion.Linea != null) {
                sWhere.Append(" LineaID = @LineaID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "LineaID";
                sqlParam.Value = Refaccion.Linea.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            #endregion Refaccion

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
                sqlAdapter.Fill(ds, "Refaccion");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstCatalogo = new List<CatalogoBaseBO>();
            RefaccionBO RefaccionLider;
            foreach (DataRow row in ds.Tables[0].Rows) {
                RefaccionLider = new RefaccionBO();
                RefaccionLider.Auditoria = new AuditoriaBO();
                RefaccionLider.Linea = new LineaBO();
                if (!row.IsNull("ArticuloId"))
                    RefaccionLider.Id = (int)Convert.ChangeType(row["ArticuloId"], typeof(int));
                if (!row.IsNull("CveArt"))
                    RefaccionLider.NombreCorto = (string)Convert.ChangeType(row["CveArt"], typeof(string));
                if (!row.IsNull("Descripcion"))
                    RefaccionLider.Nombre = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
                if (!row.IsNull("LineaId"))
                    RefaccionLider.Linea.Id = (int)Convert.ChangeType(row["LineaId"], typeof(int));
                lstCatalogo.Add(RefaccionLider);
            }
            return lstCatalogo;
            #endregion
        }
    }
}
