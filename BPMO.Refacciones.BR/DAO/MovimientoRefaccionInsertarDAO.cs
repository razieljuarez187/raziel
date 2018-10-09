using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO
{
    internal class MovimientoRefaccionInsertarDAO : IDAOBaseInsertarDocumento
    {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados
        {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado
        {
            get { return this.ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Metodos
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase)
        {
            #region Validar parametros
            MovimientoRefaccionBO movimiento = null;
            if (documentoBase is MovimientoRefaccionBO)
                movimiento = (MovimientoRefaccionBO)documentoBase;
            string mensajeError = String.Empty;
            if (movimiento == null)
                mensajeError += " , Movimiento";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (movimiento.EmpresaLiderId == null)
                mensajeError += " , Movimiento.EmpresaLiderId";
            if (movimiento.SucursalLiderId == null)
                mensajeError += " , Movimiento.SucursalLiderId";
            if (movimiento.Almacen == null || movimiento.Almacen.Id == null)
                mensajeError += " , Movimiento.Almacen";
            if (movimiento.UsuarioLiderId == null)
                mensajeError += " , Movimiento.UsuarioLiderId";
            if (movimiento.ConceptoId == null)
                mensajeError += " , Movimiento.ConceptoId";
            if (movimiento.NumeroReferencia == null)
                mensajeError += " , Movimiento.NumeroReferencia";
            if (movimiento.TipoReferencia == null)
                mensajeError += " , Movimiento.TipoReferencia";
            if (movimiento.MonedaLiderId == null)
                mensajeError += " , Movimiento.MonedaLiderId";
            if (movimiento.Divisa == null || movimiento.Divisa.TipoCambio == null)
                mensajeError += " , Movimiento.Divisa.TipoCambio";
            if (movimiento.ClienteInternoId == null)
                mensajeError += " , Movimiento.ClienteInternoId";
            if (movimiento.EsConsigna == null)
                mensajeError += " , Movimiento.EsConsigna";
            if (movimiento.Serie == null)
                mensajeError += " , Movimiento.Serie";
            if (movimiento.Folio == null)
                mensajeError += " , Movimiento.Folio";
            if (movimiento.RmClienteId == null)
                mensajeError += " , Movimiento.RmClienteId";
            if (movimiento.RmReferenciaId == null)
                mensajeError += " , Movimiento.RmReferenciaId";
            if (movimiento.RmReferencia == null)
                mensajeError += " , Movimiento.RmReferencia";
            if (movimiento.RmFolio == null)
                mensajeError += " , Movimiento.RmFolio";
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
            sCmd.Append(" INSERT ref_encMovimientos (EmpresaID, AlmacenID, SucursalID, FechGenerado, UsuarioID, ConceptoID, ReferenciaID, TipoReferencia, MonedaID, TipoCambio,");
            sCmd.Append(" ClienteInterno, Consigna, Serie, Folio, rmClienteId, rmCliente, rmReferenciaId, rmReferencia, rmFolio)");
            sCmd.Append(" VALUES(");
            #region Valores
            sValue.Append(", @Movimiento_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_EmpresaId";
            sqlParam.Value = movimiento.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_AlmacenID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_AlmacenID";
            sqlParam.Value = movimiento.Almacen.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_SucursalId";
            sqlParam.Value = movimiento.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", GETDATE()");
            sValue.Append(", @Movimiento_UsuarioID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_UsuarioID";
            sqlParam.Value = movimiento.UsuarioLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_ConceptoID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_ConceptoID";
            sqlParam.Value = movimiento.ConceptoId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_ReferenciaID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_ReferenciaID";
            sqlParam.Value = movimiento.NumeroReferencia;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_TipoReferencia");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_TipoReferencia";
            sqlParam.Value = movimiento.TipoReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_MonedaID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_MonedaID";
            sqlParam.Value = movimiento.MonedaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_TipoCambio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_TipoCambio";
            sqlParam.Value = movimiento.Divisa.TipoCambio;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_ClienteInterno");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_ClienteInterno";
            sqlParam.Value = movimiento.ClienteInternoId;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_Consigna");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_Consigna";
            sqlParam.Value = (bool)movimiento.EsConsigna? 1 : 0;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_Serie");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_Serie";
            sqlParam.Value = movimiento.Serie;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_Folio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_Folio";
            sqlParam.Value = movimiento.Folio;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_rmClienteId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_rmClienteId";
            sqlParam.Value = movimiento.Folio;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_rmCliente");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_rmCliente";
            sqlParam.Value = movimiento.RmCliente;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_rmReferenciaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_rmReferenciaId";
            sqlParam.Value = movimiento.RmReferenciaId;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_rmReferencia");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_rmReferencia";
            sqlParam.Value = movimiento.RmReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_rmFolio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_rmFolio";
            sqlParam.Value = movimiento.RmFolio;
            sqlParam.DbType = DbType.Int32;
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
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
                if (result > 0)
                {
                    object resultId = sqlCmd.Parameters["identity"].Value;
                    if (!(resultId is DBNull))
                        ultimoIdGenerado = Convert.ToInt32(resultId);
                    else
                        ultimoIdGenerado = null;
                }
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
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion Ejecución Sentencia SQL
        }
        #endregion Metodos
    }
}
