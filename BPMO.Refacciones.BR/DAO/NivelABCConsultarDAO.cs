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
    internal class NivelABCConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            #region Validar parámetos
            NivelABCBO configRegla = null;
            if (catalogoBase is NivelABCBO)
                configRegla = (NivelABCBO)catalogoBase;
            string mensajeError = String.Empty;
            if (configRegla == null)
                mensajeError += " , ConfiguracionRegla";
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
            sCmd.Append(" SELECT ClaveABC, Descripcion, NivelABCId ");
            sCmd.Append(" FROM ref_CatNivABC");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (configRegla.Id.HasValue) {
                sWhere.Append(" AND NivelABCId = @valor_NivelABCId");
                Utileria.AgregarParametro(sqlCmd, "valor_NivelABCId", configRegla.Id, System.Data.DbType.Int32);
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
                sqlAdapter.Fill(ds, "TipoPedido");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstNivelABC = new List<CatalogoBaseBO>();
            NivelABCBO nivelABC = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                nivelABC = new NivelABCBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("NivelABCId"))
                    nivelABC.Id = (Int32)Convert.ChangeType(row["NivelABCId"], typeof(Int32));
                if (!row.IsNull("Descripcion"))
                    nivelABC.Nombre = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
                if (!row.IsNull("ClaveABC"))
                    nivelABC.NombreCorto = (string)Convert.ChangeType(row["ClaveABC"], typeof(string));
                #endregion /ConfiguracionesReglas

                lstNivelABC.Add(nivelABC);
            }
            return lstNivelABC;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
