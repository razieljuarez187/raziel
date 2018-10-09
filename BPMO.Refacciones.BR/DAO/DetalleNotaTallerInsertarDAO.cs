using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    internal class DetalleNotaTallerInsertarDAO : IDAOBaseInsertarDetalleDocumento {
        #region Atributos
        private int registrosAfectados;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion Propiedades

        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase) {
            #region Validar parametros
            NotaTallerBO notaTaller = null;
            DetalleNotaTallerBO detalleNotaTaller = null;
            if (documentoBase is NotaTallerBO)
                notaTaller = (NotaTallerBO)documentoBase;
            if (detalleDocumentoBase is DetalleNotaTallerBO)
                detalleNotaTaller = (DetalleNotaTallerBO)detalleDocumentoBase;
            string mensajeError = string.Empty;
            if (notaTaller == null)
                mensajeError += " , NotaTaller";
            if (detalleNotaTaller == null)
                mensajeError += " , DetalleNotaTaller";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (notaTaller.EmpresaLiderId == null)
                mensajeError += " , NotaTaller.EmpresaLiderId";
            if (notaTaller.SucursalLiderId == null)
                mensajeError += " , NotaTaller.SucursalLiderId";
            if (notaTaller.Id == null)
                mensajeError += " , NotaTaller.Id";
            if (notaTaller.Almacen == null || notaTaller.Almacen.Id == null)
                mensajeError += " , NotaTaller.Almacen.Id";
            if (detalleNotaTaller.Articulo == null || detalleNotaTaller.Articulo.Id == null)
                mensajeError += " , DetalleNotaTaller.Articulo.Id";
            if (detalleNotaTaller.Cantidad == null)
                mensajeError += " , DetalleNotaTaller.Cantidad";
            if (detalleNotaTaller.CantidadSurtida == null)
                mensajeError += " , DetalleNotaTaller.CantidadSurtida";
            if (detalleNotaTaller.CantidadCancelada == null)
                mensajeError += " , DetalleNotaTaller.CantidadCancelada";
            if (detalleNotaTaller.CantidadReservada == null)
                mensajeError += " , DetalleNotaTaller.CantidadReservada";
            if (detalleNotaTaller.CantidadDevuelta == null)
                mensajeError += " , DetalleNotaTaller.CantidadDevuelta";
            if (detalleNotaTaller.CostoUnitario == null)
                mensajeError += " , DetalleNotaTaller.CostoUnitario";
            if (detalleNotaTaller.PrecioUnitario == null)
                mensajeError += " , DetalleNotaTaller.PrecioUnitario";
            if (detalleNotaTaller.ArticuloCore != null && detalleNotaTaller.ArticuloCore.Id != null) {
                if (detalleNotaTaller.CostoArticuloCore == null)
                    mensajeError += " , DetalleNotaTaller.CostoArticuloCore";
                if (detalleNotaTaller.PrecioArticuloCore == null)
                    mensajeError += " , DetalleNotaTaller.PrecioArticuloCore";
                if (detalleNotaTaller.PrecioArticuloCoreOriginal == null)
                    mensajeError += " , DetalleNotaTaller.PrecioArticuloCoreOriginal";
            }
            if (detalleNotaTaller.PorcentajeImpuesto == null)
                mensajeError += " , DetalleNotaTaller.PorcentajeImpuesto";
            if (detalleNotaTaller.EmpresaLiderReservaId == null)
                mensajeError += " , DetalleNotaTaller.EmpresaLiderReservaId";
            if (detalleNotaTaller.SucursalLiderReservaId == null)
                mensajeError += " , DetalleNotaTaller.SucursalLiderReservaId";
            if (detalleNotaTaller.AlmacenReserva == null || detalleNotaTaller.AlmacenReserva.Id == null)
                mensajeError += " , DetalleNotaTaller.AlmacenReserva.Id";
            if (detalleNotaTaller.MonedaLiderOriginalId == null)
                mensajeError += " , DetalleNotaTaller.MonedaLiderOriginalId";
            if (detalleNotaTaller.PrecioArticuloOriginal == null)
                mensajeError += " , DetalleNotaTaller.PrecioArticuloOriginal";
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
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" INSERT inv_detnotataller (EmpresaId, SucursalId, AlmacenId, NotaTallerId, ArticuloId, CantSol, CantSurt, CantCancel, CantReserv, CantDevuelta,");
            sCmd.Append(" Costo, Precio, CoreId, PrecioCore, CostoCore, PrecioOriCore, iva, EmpReservaId, SucReservaId, AlmReservaId, TipoRemision, MonedaIdOri, PrecioOri)");
            sCmd.Append("  VALUES(");
            #region Valores
            sValue.Append(", @NotaTaller_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_EmpresaId";
            sqlParam.Value = notaTaller.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_SucursalId";
            sqlParam.Value = notaTaller.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_AlmacenId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_AlmacenId";
            sqlParam.Value = notaTaller.Almacen.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Id";
            sqlParam.Value = notaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_ArticuloId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_ArticuloId";
            sqlParam.Value = detalleNotaTaller.Articulo.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_CantSol");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_CantSol";
            sqlParam.Value = detalleNotaTaller.Cantidad;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_CantSurt");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_CantSurt";
            sqlParam.Value = detalleNotaTaller.CantidadSurtida;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_CantCancel");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_CantCancel";
            sqlParam.Value = detalleNotaTaller.CantidadCancelada;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_CantReserv");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_CantReserv";
            sqlParam.Value = detalleNotaTaller.CantidadReservada;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_CantDevuelta");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_CantDevuelta";
            sqlParam.Value = detalleNotaTaller.CantidadDevuelta;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Costo");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Costo";
            sqlParam.Value = detalleNotaTaller.CostoUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Precio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Precio";
            sqlParam.Value = detalleNotaTaller.PrecioUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            if (detalleNotaTaller.ArticuloCore != null && detalleNotaTaller.ArticuloCore.Id != null) {
                sValue.Append(", @NotaTaller_CoreId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_CoreId";
                sqlParam.Value = detalleNotaTaller.ArticuloCore.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
                                
                sValue.Append(", @NotaTaller_PrecioCore");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_PrecioCore";
                sqlParam.Value = detalleNotaTaller.PrecioArticuloCore;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
                                
                sValue.Append(", @NotaTaller_CostoCore");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_CostoCore";
                sqlParam.Value = detalleNotaTaller.CostoArticuloCore;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
                                
                sValue.Append(", @NotaTaller_PrecioOriCore");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_PrecioOriCore";
                sqlParam.Value = detalleNotaTaller.PrecioArticuloCoreOriginal;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL, 0, 0, 0");
            }
            sValue.Append(", @NotaTaller_IVA");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_IVA";
            sqlParam.Value = detalleNotaTaller.PorcentajeImpuesto;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_EmpReservaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_EmpReservaId";
            sqlParam.Value = detalleNotaTaller.EmpresaLiderReservaId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_SucReservaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_SucReservaId";
            sqlParam.Value = detalleNotaTaller.SucursalLiderReservaId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_AlmReservaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_AlmReservaId";
            sqlParam.Value = detalleNotaTaller.AlmacenReserva.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (detalleNotaTaller.TipoRemisionId != null) {
                sValue.Append(", @NotaTaller_TipoRemision");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_TipoRemision";
                sqlParam.Value = detalleNotaTaller.TipoRemisionId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL");
            }
            sValue.Append(", @NotaTaller_MonedaIdOri");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_MonedaIdOri";
            sqlParam.Value = detalleNotaTaller.MonedaLiderOriginalId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_PrecioOri");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_PrecioOri";
            sqlParam.Value = detalleNotaTaller.PrecioArticuloOriginal;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                if (dataContext.CurrentProvider != incomingDataContext)
                    dataContext.SetCurrentProvider(incomingDataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion Ejecución Sentencia SQL
        }


    }
}
