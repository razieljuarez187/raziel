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
    /// Acceso a Datos para Consultar registros de Naturalezas
    /// </summary>
    internal class NaturalezasConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Naturalezas</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            #region Validar parámetos
            NaturalezasBO naturalezaMov = null;
            if (catalogoBase is NaturalezasBO)
                naturalezaMov = (NaturalezasBO)catalogoBase;
            string mensajeError = String.Empty;
            if (naturalezaMov == null)
                mensajeError += " , Naturalezas";
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
            sCmd.Append(" SELECT NaturalezaMovId, Clave, Nombre, UC, FC, UUA, FUA ");
            sCmd.Append(" FROM grl_catNaturalezasMov");
            StringBuilder sWhere = new StringBuilder();
            #region Valores

            naturalezaMov.Auditoria = new AuditoriaBO();
            naturalezaMov.Auditoria.UC = -1;
            naturalezaMov.Auditoria.UUA = -1;
            naturalezaMov.Auditoria.FC = DateTime.Parse("2015-08-22 18:38:25.870");
            naturalezaMov.Auditoria.FUA = DateTime.Parse("2015-08-22 18:38:25.870");

            if (naturalezaMov.Id.HasValue) {
                sWhere.Append(" AND NaturalezaMovId = @valor_NaturalezaMovId");
                Utileria.AgregarParametro(sqlCmd, "valor_NaturalezaMovId", naturalezaMov.Id, System.Data.DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(naturalezaMov.NombreCorto)) {
                sWhere.Append(" AND Clave LIKE @valor_Clave");
                Utileria.AgregarParametro(sqlCmd, "valor_Clave", naturalezaMov.NombreCorto, System.Data.DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(naturalezaMov.Nombre)) {
                sWhere.Append(" AND Nombre LIKE @valor_Nombre");
                Utileria.AgregarParametro(sqlCmd, "valor_Nombre", naturalezaMov.Nombre, System.Data.DbType.String);
            }
            if (naturalezaMov.Auditoria != null) {
                if (naturalezaMov.Auditoria.UC.HasValue) {
                    sWhere.Append(" AND UC = @valor_UC");
                    Utileria.AgregarParametro(sqlCmd, "valor_UC", naturalezaMov.Auditoria.UC, System.Data.DbType.Int32);
                }
                if (naturalezaMov.Auditoria.FC.HasValue) {
                    sWhere.Append(" AND FC = @valor_FC");
                    Utileria.AgregarParametro(sqlCmd, "valor_FC", naturalezaMov.Auditoria.FC, System.Data.DbType.DateTime);
                }
                if (naturalezaMov.Auditoria.UUA.HasValue) {
                    sWhere.Append(" AND UUA = @valor_UUA");
                    Utileria.AgregarParametro(sqlCmd, "valor_UUA", naturalezaMov.Auditoria.UUA, System.Data.DbType.Int32);
                }
                if (naturalezaMov.Auditoria.FUA.HasValue) {
                    sWhere.Append(" AND FUA = @valor_FUA");
                    Utileria.AgregarParametro(sqlCmd, "valor_FUA", naturalezaMov.Auditoria.FUA, System.Data.DbType.DateTime);
                }
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
                sqlAdapter.Fill(ds, "Naturalezas");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstNaturalezas = new List<CatalogoBaseBO>();
            NaturalezasBO naturalezas = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                naturalezas = new NaturalezasBO();
                naturalezas.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region Naturalezas
                if (!row.IsNull("NaturalezaMovId"))
                    naturalezas.Id = (Int32)Convert.ChangeType(row["NaturalezaMovId"], typeof(Int32));
                if (!row.IsNull("Nombre"))
                    naturalezas.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
                if (!row.IsNull("Clave"))
                    naturalezas.NombreCorto = (string)Convert.ChangeType(row["Clave"], typeof(string));
                if (!row.IsNull("UC"))
                    naturalezas.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    naturalezas.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UUA"))
                    naturalezas.Auditoria.UUA = (Int32)Convert.ChangeType(row["UUA"], typeof(Int32));
                if (!row.IsNull("FUA"))
                    naturalezas.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FUA"], typeof(DateTime));
                #endregion /Naturalezas

                lstNaturalezas.Add(naturalezas);
            }
            return lstNaturalezas;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
