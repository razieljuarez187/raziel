using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Refacciones.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Consulta del credito de cores
    /// </summary>
    internal class CreditoCoresConsultarDAO : IDAOBaseConsultarAuditoria {
        /// <summary>
        /// Consulta de Credito de Cores
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Credito de Cores que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información del credito de cores recuperados por la consulta</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            CreditoCoresBO creditoCores = null;
            if (auditoriaBase is CreditoCoresBO)
                creditoCores = (CreditoCoresBO)auditoriaBase;
            string msjError = string.Empty;
            if (creditoCores == null)
                msjError += " , CreditoCores";
            if (dataContext == null)
                msjError += " , dataContext";
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
            sCmd.Append(" select EmpresaId,SucursalId,LineaId,ClienteID,DiasFactura,DiasCredito,DiasMargen,Activo from cor_diasCredCtexLin");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (creditoCores.EmpresaId != null) {
                sWhere.Append(" EmpresaId = @EmpresaId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "EmpresaId";
                sqlParam.Value = creditoCores.EmpresaId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.SucursalId != null) {
                sWhere.Append(" AND SucursalId = @SucursalId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "SucursalId";
                sqlParam.Value = creditoCores.SucursalId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.Linea.Id != null) {
                sWhere.Append(" AND LineaId = @LineaId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "LineaId";
                sqlParam.Value = creditoCores.Linea.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.ClienteId != null) {
                sWhere.Append(" AND ClienteID = @ClienteID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ClienteID";
                sqlParam.Value = creditoCores.ClienteId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.Activo != null) {
                sWhere.Append(" AND Activo = @Activo");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "Activo";
                sqlParam.Value = creditoCores.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.DiasCredito != null) {
                sWhere.Append(" DiasCredito = @DiasCredito");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DiasCredito";
                sqlParam.Value = creditoCores.DiasCredito;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.DiasFactura != null) {
                sWhere.Append(" DiasFactura = @DiasFactura");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DiasFactura";
                sqlParam.Value = creditoCores.DiasFactura;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (creditoCores.DiasMargen != null) {
                sWhere.Append(" DiasMargen = @DiasMargen");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DiasMargen";
                sqlParam.Value = creditoCores.DiasMargen;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
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
                sqlAdapter.Fill(ds, "creditoCores");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstCatalogo = new List<AuditoriaBaseBO>();
            CreditoCoresBO creditoCoresLider;
            foreach (DataRow row in ds.Tables[0].Rows) {
                creditoCoresLider = new CreditoCoresBO();
                creditoCoresLider.Sucursal = new SucursalBO();
                creditoCoresLider.Refaccion = new RefaccionBO();
                creditoCoresLider.Linea = new LineaBO();
                // Se regresan datos de Lider
                if (!row.IsNull("EmpresaID"))
                    creditoCoresLider.EmpresaId = (int)Convert.ChangeType(row["EmpresaID"], typeof(int));
                if (!row.IsNull("SucursalId"))
                    creditoCoresLider.SucursalId = (int)Convert.ChangeType(row["SucursalId"], typeof(int));
                if (!row.IsNull("LineaId"))
                    creditoCoresLider.Linea.Id = (int)Convert.ChangeType(row["LineaId"], typeof(int));
                if (!row.IsNull("ClienteID"))
                    creditoCoresLider.ClienteId = (int)Convert.ChangeType(row["ClienteID"], typeof(int));
                if (!row.IsNull("DiasFactura"))
                    creditoCoresLider.DiasFactura = (int)Convert.ChangeType(row["DiasFactura"], typeof(int));
                if (!row.IsNull("DiasCredito"))
                    creditoCoresLider.DiasCredito = (int)Convert.ChangeType(row["DiasCredito"], typeof(int));
                if (!row.IsNull("DiasMargen"))
                    creditoCoresLider.DiasMargen = (int)Convert.ChangeType(row["DiasMargen"], typeof(int));
                if (!row.IsNull("Activo"))
                    creditoCoresLider.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
                lstCatalogo.Add(creditoCoresLider);
            }
            return lstCatalogo;
            #endregion
        }
    }
}
