using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using BPMO.Primitivos.Utilerias;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.DA {
    /// <summary>
    /// Acceso a Datos para obtener cantidad disponible de artículos en una Cotización de Nota de Taller
    /// </summary>
    internal class ObtenerPrecioRefaccionActualDA {
        #region Atributos
        #endregion

        #region Propiedades
        #endregion

        #region Métodos

        internal decimal Consultar(IDataContext dataContext, ExistenciaAlmacenRefaccionesBO refaccion, ClienteBO cliente, TipoClienteBO tipoCliente,
        DireccionClienteBO direccionCliente, Int32 tipoPedido, Boolean esContado) {
            decimal result = 0;
            #region Validar Filtros
            string msjError = string.Empty;
            if (refaccion == null)
                msjError += " , ExistenciaAlmacenRefacciones";
            if (refaccion.EmpresaLiderId == null)
                msjError += " , EmpresaLiderId";
            if (refaccion.SucursalLiderId == null)
                msjError += " , SucursalLiderId";
            if (refaccion.Almacen == null)
                msjError += " , Almacen";            
            if (refaccion.Articulo == null)
                msjError += " , Articulo";
            if (refaccion.Precio == null)
                msjError += " , Precio";
            if (refaccion.Linea == null)
                msjError += " , Linea";
            if (refaccion.SubGrupoId == null)
                msjError += " , SubGrupoId";
            if (refaccion.Moneda == null)
                msjError += " , Moneda";
            if (cliente == null)
                msjError += " , Cliente";            
            if (tipoCliente == null)
                msjError += " , TipoCliente";
            if (direccionCliente == null)
                msjError += " , DireccionCliente";
            if (msjError.Length > 0)
                throw new ArgumentNullException("Los siguientes campos no pueden ser vacios " + msjError.Substring(2));
            if (refaccion.Almacen.Id == null)
                msjError += " , Almacen.Id";
            if (refaccion.Articulo.Id == null)
                msjError += " , Articulo.Id";
            if (refaccion.Linea.Id == null)
                msjError += " , Linea.Id";
            if (refaccion.Moneda.Id == null)
                msjError += " , Moneda.Id";
            if (cliente.Id == null)
                msjError += " , Cliente.Id";
            if (tipoCliente.Id == null)
                msjError += " , TipoCliente.Id";
            if (direccionCliente.Id == null)
                msjError += " , DireccionCliente.Id";
            if (msjError.Length > 0)
                throw new ArgumentNullException("Los siguientes campos no pueden ser vacios " + msjError.Substring(2));
            #endregion
            
            #region Conexión a BD
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                manejadorDctx.RegresaProveedorInicial(dataContext);
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sqlCmd.CommandText = "ref_spObtenerPrecio";
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "EmpresaId";
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.Value = refaccion.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "SucursalId";
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.Value = refaccion.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "AlmacenId";
            sqlParam.Value = refaccion.Almacen.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ArticuloId";
            sqlParam.Value = refaccion.Articulo.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Precio";
            sqlParam.Value = refaccion.Precio;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Currency;
            sqlCmd.Parameters.Add(sqlParam); 

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "LineaId";
            sqlParam.Value = refaccion.Linea.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "SubgrupoId";
            sqlParam.Value = refaccion.SubGrupoId;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MonedaId";
            sqlParam.Value = refaccion.Moneda.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ClienteId";
            sqlParam.Value = cliente.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "TipoClienteId";
            sqlParam.Value = tipoCliente.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DireccionId";
            sqlParam.Value = direccionCliente.Id;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "TipoPedidoId";
            sqlParam.Value = tipoPedido;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Contado";
            sqlParam.Value = esContado;
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Base";
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.DbType = DbType.String;
            sqlParam.Size = 1;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "PrecioCalculado";
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.DbType = DbType.Currency;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "PrecioOri";
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.DbType = DbType.Currency;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MonedaRetID";
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion

            #region Ejecución Sentecia SQL
            try {
                sqlCmd.ExecuteNonQuery();
                if (sqlCmd.Parameters["PrecioCalculado"].Value != DBNull.Value) {
                    result = (decimal)Convert.ChangeType(sqlCmd.Parameters["PrecioCalculado"].Value, typeof(decimal));
                }
            } catch (Exception ex) {
                throw ex;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
            #endregion

            return result;
        }
        #endregion
    }
}
