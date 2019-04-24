using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO
{
    internal class ExistenciaAlmacenRefaccionesConsultarDAO : IDAOBaseConsultarAuditoria
    {
        #region Atributos
        #endregion Atributos

        #region Propiedades
        #endregion Propiedades

        #region Métodos
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetos
            ExistenciaAlmacenRefaccionesBO existencia = null;
            if (auditoriaBase is ExistenciaAlmacenRefaccionesBO)
                existencia = (ExistenciaAlmacenRefaccionesBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (existencia == null)
                mensajeError += " , Existencia";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (existencia.EmpresaLiderId == null)
                mensajeError += " , Existencia.EmpresaLiderId";
            if (existencia.SucursalLiderId == null)
                mensajeError += " , Existencia.SucursalLiderId";
            if (existencia.Almacen == null || existencia.Almacen.Id == null)
                mensajeError += " , Existencia.Almacen.Id";            
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
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
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rxa.ArtixID AS ID, rxa.EmpresaID AS EmpresaLiderId, rxa.SucursalID AS SucursalLiderId, rxa.AlmacenID AS AlmacenId, rxa.ArticuloID AS ArticuloID, cat.CoreID AS CoreId,");
            sCmd.Append(" rxa.existInicial AS ExistenciaInicial, rxa.acumEntradas AS AcumuladoEntradas, rxa.acumSalidas AS AcumuladoSalidas, rxa.CantConsignas AS CantidadEnConsigna,");
            sCmd.Append(" rxa.CantReservaVirtual AS CantidadReservada, rxa.CostoPromedio AS CostoPromedio, cat.Precio, cat.MonedaId, cat.SubGrupoId, cat.CveArt AS ClaveArticulo, cat.Descripcion AS NombreArticulo");
            sCmd.Append(" FROM ref_artixalmacen rxa INNER JOIN ref_catarticulos cat ON cat.ArticuloID = rxa.ArticuloID");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            sWhere.Append(" AND rxa.EmpresaID = @Existencia_EmpresaID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Existencia_EmpresaID";
            sqlParam.Value = existencia.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sWhere.Append(" AND rxa.SucursalID = @Existencia_SucursalID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Existencia_SucursalID";
            sqlParam.Value = existencia.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sWhere.Append(" AND rxa.AlmacenID = @Existencia_AlmacenID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Existencia_AlmacenID";
            sqlParam.Value = existencia.Almacen.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            if (existencia.Articulo.Id.HasValue) {
                sWhere.Append(" AND rxa.ArticuloID = @Existencia_ArticuloID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "Existencia_ArticuloID";
                sqlParam.Value = existencia.Articulo.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (existencia.Core != null && existencia.Core.Id != null) {
                sWhere.Append(" AND cat.CoreID = @Existencia_CoreID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "Existencia_CoreID";
                sqlParam.Value = existencia.Core.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (!String.IsNullOrWhiteSpace(existencia.Articulo.NombreCorto)) {
                sWhere.Append(" AND cat.CveArt = @Existencia_CveArt");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "Existencia_CveArt";
                sqlParam.Value = existencia.Articulo.NombreCorto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // En caso necesario se añadiran filtros, por el momento, no son requeridos -- GVV -- 28082012
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
                sqlAdapter.Fill(ds, "ExistenciaAlmacenRefacciones");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstExistencias = new List<AuditoriaBaseBO>();
            ExistenciaAlmacenRefaccionesBO existenciaAlmacenRefaccionesResultado = null;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                existenciaAlmacenRefaccionesResultado = new ExistenciaAlmacenRefaccionesBO();
                existenciaAlmacenRefaccionesResultado.Almacen = new AlmacenBO();
                existenciaAlmacenRefaccionesResultado.Articulo = new ArticuloBO();
                existenciaAlmacenRefaccionesResultado.Core = new ArticuloBO();
                existenciaAlmacenRefaccionesResultado.Linea = new LineaBO();
                existenciaAlmacenRefaccionesResultado.Moneda = new MonedaBO();
                #endregion Inicializar BO

                #region ExistenciaAlmacenRefacciones
                if (!row.IsNull("EmpresaLiderId"))
                    existenciaAlmacenRefaccionesResultado.EmpresaLiderId = (int)Convert.ChangeType(row["EmpresaLiderId"], typeof(int));
                if (!row.IsNull("SucursalLiderId"))
                    existenciaAlmacenRefaccionesResultado.SucursalLiderId = (int)Convert.ChangeType(row["SucursalLiderId"], typeof(int));
                if (!row.IsNull("AlmacenId"))
                    existenciaAlmacenRefaccionesResultado.Almacen.Id = (int)Convert.ChangeType(row["AlmacenId"], typeof(int));
                if (!row.IsNull("ArticuloId"))
                    existenciaAlmacenRefaccionesResultado.Articulo.Id = (int)Convert.ChangeType(row["ArticuloId"], typeof(int));
                if (!row.IsNull("CoreId"))
                    existenciaAlmacenRefaccionesResultado.Core.Id = (int)Convert.ChangeType(row["CoreId"], typeof(int));
                if (!row.IsNull("ExistenciaInicial"))
                    existenciaAlmacenRefaccionesResultado.ExistenciaInicial = (int)Convert.ChangeType(row["ExistenciaInicial"], typeof(int));
                if (!row.IsNull("AcumuladoEntradas"))
                    existenciaAlmacenRefaccionesResultado.AcumuladoEntradas = (int)Convert.ChangeType(row["AcumuladoEntradas"], typeof(int));
                if (!row.IsNull("AcumuladoSalidas"))
                    existenciaAlmacenRefaccionesResultado.AcumuladoSalidas = (int)Convert.ChangeType(row["AcumuladoSalidas"], typeof(int));
                if (!row.IsNull("CantidadEnConsigna"))
                    existenciaAlmacenRefaccionesResultado.CantidadEnConsigna = (int)Convert.ChangeType(row["CantidadEnConsigna"], typeof(int));
                if (!row.IsNull("CantidadReservada"))
                    existenciaAlmacenRefaccionesResultado.CantidadReservada = (int)Convert.ChangeType(row["CantidadReservada"], typeof(int));
                if (!row.IsNull("CostoPromedio"))
                    existenciaAlmacenRefaccionesResultado.CostoPromedio = (decimal)Convert.ChangeType(row["CostoPromedio"], typeof(decimal));
                if (!row.IsNull("Precio"))
                    existenciaAlmacenRefaccionesResultado.Precio = (decimal)Convert.ChangeType(row["Precio"], typeof(decimal));
                if (!row.IsNull("MonedaId"))
                    existenciaAlmacenRefaccionesResultado.Moneda.Id = (int)Convert.ChangeType(row["MonedaId"], typeof(int));
                if (!row.IsNull("SubGrupoId"))
                    existenciaAlmacenRefaccionesResultado.SubGrupoId = (int)Convert.ChangeType(row["SubGrupoId"], typeof(int));
                if (!row.IsNull("ClaveArticulo"))
                    existenciaAlmacenRefaccionesResultado.Articulo.NombreCorto = (string)Convert.ChangeType(row["ClaveArticulo"], typeof(string));
                if (!row.IsNull("NombreArticulo"))
                    existenciaAlmacenRefaccionesResultado.Articulo.Nombre = (string)Convert.ChangeType(row["NombreArticulo"], typeof(string));
                #endregion ExistenciaAlmacenRefacciones

                lstExistencias.Add(existenciaAlmacenRefaccionesResultado);
            }
            return lstExistencias;
            #endregion Mapeo DataSet a BO
        }
        #endregion Métodos
    }
}