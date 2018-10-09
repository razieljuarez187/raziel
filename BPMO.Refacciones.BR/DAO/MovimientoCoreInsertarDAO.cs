using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    internal class MovimientoCoreInsertarDAO : IDAOBaseInsertarAuditoria {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Métodos
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar parámetros
            MovimientoCoreBO movimientoCore = null;
            if (auditoriaBase is MovimientoCoreBO)
                movimientoCore = (MovimientoCoreBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (movimientoCore == null)
                mensajeError += " , MovimientoCore";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (movimientoCore.EmpresaLiderId == null)
                mensajeError += " , MovimientoCore.EmpresaLiderId";
            if (movimientoCore.SucursalLiderId == null)
                mensajeError += " , MovimientoCore.SucursalLiderId";
            if (movimientoCore.Almacen == null || movimientoCore.Almacen.Id == null)
                mensajeError += " , MovimientoCore.Almacen";
            if (movimientoCore.UsuarioLiderId == null)
                mensajeError += " , MovimientoCore.UsuarioLiderId";
            if (movimientoCore.ConceptoId == null)
                mensajeError += " , MovimientoCore.ConceptoId";
            if (movimientoCore.Recon == null || movimientoCore.Recon.Id == null)
                mensajeError += " , MovimientoCore.Recon";
            if (movimientoCore.Core == null || movimientoCore.Core.Id == null)
                mensajeError += " , MovimientoCore.Core";
            if (movimientoCore.Cantidad == null)
                mensajeError += " , MovimientoCore.Cantidad";
            if (movimientoCore.Costo == null)
                mensajeError += " , MovimientoCore.Costo";
            if (movimientoCore.CostoDolares == null)
                mensajeError += " , MovimientoCore.CostoDolares";
            if (movimientoCore.Precio == null)
                mensajeError += " , MovimientoCore.Precio";
            if (movimientoCore.NumeroReferencia == null)
                mensajeError += " , MovimientoCore.NumeroReferencia";
            if (movimientoCore.TipoReferencia == null)
                mensajeError += " , MovimientoCore.TipoReferencia";
            if (movimientoCore.MonedaLiderId == null)
                mensajeError += " , MovimientoCore.MonedaLiderId";
            if (movimientoCore.TipoCambio == null)
                mensajeError += " , MovimientoCore.TipoCambio";
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
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" INSERT cor_Movimientos (EmpresaId, SucursalId, AlmacenId, ReferenciaId, TipoReferencia, Fecha, ConceptoId, UsuarioId, ReconId,");
            sCmd.Append(" CoreId, Cantidad, Costo, CostoDL, Precio, MonedaId, TipoCambio)");
            sCmd.Append(" VALUES(");
            #region Valores
            sValue.Append(", @MovimientoCore_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_EmpresaId";
            sqlParam.Value = movimientoCore.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_SucursalId";
            sqlParam.Value = movimientoCore.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_AlmacenID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_AlmacenID";
            sqlParam.Value = movimientoCore.Almacen.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_ReferenciaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_ReferenciaId";
            sqlParam.Value = movimientoCore.NumeroReferencia;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_TipoReferencia");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_TipoReferencia";
            sqlParam.Value = movimientoCore.TipoReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", GETDATE()");
            sValue.Append(", @MovimientoCore_ConceptoId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_ConceptoId";
            sqlParam.Value = movimientoCore.ConceptoId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_UsuarioId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_UsuarioId";
            sqlParam.Value = movimientoCore.UsuarioLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_ReconId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_ReconId";
            sqlParam.Value = movimientoCore.Recon.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_CoreId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_CoreId";
            sqlParam.Value = movimientoCore.Core.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_Cantidad");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_Cantidad";
            sqlParam.Value = movimientoCore.Cantidad;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_Costo");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_Costo";
            sqlParam.Value = movimientoCore.Costo;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_CostoDL");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_CostoDL";
            sqlParam.Value = movimientoCore.CostoDolares;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_Precio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_Precio";
            sqlParam.Value = movimientoCore.Precio;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_MonedaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_MonedaId";
            sqlParam.Value = movimientoCore.MonedaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @MovimientoCore_TipoCambio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "MovimientoCore_TipoCambio";
            sqlParam.Value = movimientoCore.TipoCambio;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            sCmd.Append(" SELECT @identity = SCOPE_IDENTITY()");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "identity";
            sqlParam.DbType = DbType.Int32;
            sqlParam.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
                if (result > 0) {
                    object resultId = sqlCmd.Parameters["identity"].Value;
                    if (!(resultId is DBNull))
                        ultimoIdGenerado = Convert.ToInt32(resultId);
                    else
                        ultimoIdGenerado = null;
                }
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion Ejecución Sentencia SQL
        }
        #endregion Métodos
    }
}
