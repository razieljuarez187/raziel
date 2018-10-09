using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO
{
    internal class DetalleMovimientoRefaccionInsertarDAO : IDAOBaseInsertarDetalleDocumento
    {
        #region Atributos
        private int registrosAfectados;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados
        {
            get { throw new NotImplementedException(); }
        }
        #endregion Propiedades

        #region Métodos
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase)
        {
            #region Validar parametros
            MovimientoRefaccionBO movimiento = null;
            DetalleMovimientoRefaccionBO detalleMovimiento = null;
            if (documentoBase is MovimientoRefaccionBO)
                movimiento = (MovimientoRefaccionBO)documentoBase;
            if (detalleDocumentoBase is DetalleMovimientoRefaccionBO)
                detalleMovimiento = (DetalleMovimientoRefaccionBO)detalleDocumentoBase;
            string mensajeError = string.Empty;
            if (movimiento == null)
                mensajeError += " , Movimiento";
            if (detalleMovimiento == null)
                mensajeError += " , DetalleMovimiento";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (movimiento.Id == null)
                mensajeError += " , Movimiento.Id";
            if (movimiento.EmpresaLiderId == null)
                mensajeError += " , Movimiento.EmpresaLiderId";
            if (movimiento.SucursalLiderId == null)
                mensajeError += " , Movimiento.SucursalLiderId";
            if (movimiento.Almacen == null || movimiento.Almacen.Id == null)
                mensajeError += " , Movimiento.Almacen.Id";
            if (movimiento.MonedaLiderId == null)
                mensajeError += " , Movimiento.MonedaLiderId";
            if (movimiento.Divisa == null || movimiento.Divisa.TipoCambio == null)
                mensajeError += " , Movimiento.Divisa.TipoCambio";
            if (detalleMovimiento.Articulo == null || detalleMovimiento.Articulo.Id == null)
                mensajeError += " , DetalleMovimiento.Articulo.Id";
            if (detalleMovimiento.Cantidad == null)
                mensajeError += " , DetalleMovimiento.Cantidad";
            if (detalleMovimiento.CostoUnitario == null)
                mensajeError += " , DetalleMovimiento.CostoUnitario";
            if (detalleMovimiento.PrecioUnitario == null)
                mensajeError += " , DetalleMovimiento.PrecioUnitario";
            if (detalleMovimiento.ArticuloCore != null && detalleMovimiento.ArticuloCore.Id != null) {
                // Si tiene CoreID, se validan los importes
                if (detalleMovimiento.CostoCore == null)
                    mensajeError += " , DetalleMovimiento.CostoCore";
                if (detalleMovimiento.PrecioCore == null)
                    mensajeError += " , DetalleMovimiento.PrecioCore";
            }
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            #endregion Validar parametros

            #region Conexión a BD
            if (!dataContext.CheckProviderByName("LIDER"))
                throw new ArgumentNullException("LIDER", "No se ha definido un proveedor de conexiones para la base de datos requerida!!!");
            string incomingDataContext = dataContext.CurrentProvider;
            if (dataContext.CurrentProvider != "LIDER")
                dataContext.SetCurrentProvider("LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try
            {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            }
            catch
            {
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" INSERT ref_detMovimientos (EmpresaId, AlmacenId, SucursalId, MovimientoId, ArticuloId, Cantidad, Costo, Precio, MonedaId, TipoCambio,");
            sCmd.Append("   CoreId, CostoCore, PrecioCore)");
            sCmd.Append("  VALUES(");
            #region Valores
            sValue.Append(", @Movimiento_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_EmpresaId";
            sqlParam.Value = movimiento.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @Movimiento_AlmacenId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_AlmacenId";
            sqlParam.Value = movimiento.Almacen.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @Movimiento_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_SucursalId";
            sqlParam.Value = movimiento.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @Movimiento_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_Id";
            sqlParam.Value = movimiento.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            
            sValue.Append(", @DetalleMovimiento_ArticuloId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleMovimiento_ArticuloId";
            sqlParam.Value = detalleMovimiento.Articulo.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            
            sValue.Append(", @DetalleMovimiento_Cantidad");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleMovimiento_Cantidad";
            sqlParam.Value = detalleMovimiento.Cantidad;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @DetalleMovimiento_Costo");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleMovimiento_Costo";
            sqlParam.Value = detalleMovimiento.CostoUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @DetalleMovimiento_Precio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleMovimiento_Precio";
            sqlParam.Value = detalleMovimiento.PrecioUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @Movimiento_MonedaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_MonedaId";
            sqlParam.Value = movimiento.MonedaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(", @Movimiento_TipoCambio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_TipoCambio";
            sqlParam.Value = movimiento.Divisa.TipoCambio;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            if (detalleMovimiento.ArticuloCore != null && detalleMovimiento.ArticuloCore.Id != null) {
                sValue.Append(", @DetalleMovimiento_CoreId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DetalleMovimiento_CoreId";
                sqlParam.Value = detalleMovimiento.ArticuloCore.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
                
                sValue.Append(", @DetalleMovimiento_CostoCore");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DetalleMovimiento_CostoCore";
                sqlParam.Value = detalleMovimiento.CostoCore;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);

                sValue.Append(", @DetalleMovimiento_PrecioCore");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DetalleMovimiento_PrecioCore";
                sqlParam.Value = detalleMovimiento.PrecioCore;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL, 0, 0");
            }

            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            int result = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                dataContext.CloseConnection(firma);
                if (dataContext.CurrentProvider != incomingDataContext)
                    dataContext.SetCurrentProvider(incomingDataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Ocurrió un error desconocido al insertar el registro!!!");
            else
                return true;
            #endregion Ejecución Sentencia SQL
        }
        #endregion Métodos
    }
}
