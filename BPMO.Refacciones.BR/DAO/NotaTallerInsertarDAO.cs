using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO
{
    internal class NotaTallerInsertarDAO : IDAOBaseInsertarDocumento
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
            NotaTallerBO notaTaller = null; ;
            if (documentoBase is NotaTallerBO)
                notaTaller = (NotaTallerBO)documentoBase;
            string mensajeError = string.Empty;
            if (notaTaller == null)
                mensajeError += " , NotaTaller";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (notaTaller.EmpresaLiderId == null)
                mensajeError += " , NotaTaller.EmpresaLiderId";
            if (notaTaller.SucursalLiderId == null)
                mensajeError += " , NotaTaller.SucursalLiderId";
            if (notaTaller.Almacen == null || notaTaller.Almacen.Id == null)
                mensajeError += " , NotaTaller.Alamacen.Id";
            if (notaTaller.AreaLiderId == null)
                mensajeError += " , NotaTaller.AreaLiderId";
            if (notaTaller.MecanicoLiderId == null)
                mensajeError += " , NotaTaller.MecanicoLiderId";
            if (notaTaller.UsuarioLiderId == null)
                mensajeError += " , NotaTaller.UsuarioLiderId";
            if (notaTaller.OrdenServicioId == null)
                mensajeError += " , NotaTaller.OrdenServicioId";
            if (notaTaller.Estatus == null || notaTaller.Estatus.Id == null)
                mensajeError += " , NotaTaller.Estado.Id";
            if (notaTaller.NumeroReferencia == null)
                mensajeError += " , NotaTaller.NumeroReferencia";
            if (notaTaller.TipoReferencia == null)
                mensajeError += " , NotaTaller.TipoReferencia";
            if (notaTaller.ClienteLiderId == null)
                mensajeError += " , NotaTaller.clienteLiderLiderId";
            if (notaTaller.DiasCredito == null)
                mensajeError += " , NotaTaller.DiasCredito";
            if (notaTaller.Observaciones == null)
                mensajeError += " , NotaTaller.Observaciones";
            if (notaTaller.EsRescate == null)
                mensajeError += " , NotaTaller.EsRescate";
            if (notaTaller.TipoPedidoId == null)
                mensajeError += " , NotaTaller.TipoPedidoId";
            if (notaTaller.EsDevolucion == null)
                mensajeError += " , NotaTaller.EsDevolucion";
            if (notaTaller.EstaImpreso == null)
                mensajeError += " , NotaTaller.EstaImpreso";
            if (notaTaller.MonedaLiderId == null)
                mensajeError += " , NotaTaller.MonedaLiderId";
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
            sCmd.Append(" INSERT inv_encnotataller (EmpresaId, SucursalId, AlmacenId, AreaId, MecanicoId, FCaptura, UsuarioId, OrdenId, Status,");
            sCmd.Append(" referencia, TipoRef, ClienteId, DiasCredito, Observaciones, Rescate, TipoPedidoId, Devolucion, NotaTallerIdref, Impreso, KitID, MonedaIdVta,");
            sCmd.Append(" FechaAplica) VALUES(");
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
            sValue.Append(", @NotaTaller_AreaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_AreaId";
            sqlParam.Value = notaTaller.AreaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_MecanicoId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_MecanicoId";
            sqlParam.Value = notaTaller.MecanicoLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            //Fecha Captura
            sValue.Append(", GETDATE()");
            sValue.Append(", @NotaTaller_UsuarioId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_UsuarioId";
            sqlParam.Value = notaTaller.UsuarioLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_OrdTrabajoId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_OrdTrabajoId";
            sqlParam.Value = notaTaller.OrdenServicioId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_StatusId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_StatusId";
            sqlParam.Value = notaTaller.Estatus.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Referencia");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Referencia";
            sqlParam.Value = notaTaller.NumeroReferencia;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_TipoRef");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_TipoRef";
            sqlParam.Value = notaTaller.TipoReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_ClienteId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_ClienteId";
            sqlParam.Value = notaTaller.ClienteLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_DiasCredito");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_DiasCredito";
            sqlParam.Value = notaTaller.DiasCredito;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Observaciones");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Observaciones";
            sqlParam.Value = notaTaller.Observaciones;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Rescate");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Rescate";
            sqlParam.Value = notaTaller.EsRescate;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_TipoPedidoId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_TipoPedidoId";
            sqlParam.Value = notaTaller.TipoPedidoId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTaller_Devolucion");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Devolucion";
            sqlParam.Value = notaTaller.EsDevolucion;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            if (notaTaller.NotaTallerReferencia != null)
            {
                sValue.Append(", @NotaTaller_NotaTallerIdRef");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_NotaTallerIdRef";
                sqlParam.Value = notaTaller.NotaTallerReferencia;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            else
                sValue.Append(", NULL");
            sValue.Append(", @NotaTaller_Impreso");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_Impreso";
            sqlParam.Value = notaTaller.EstaImpreso;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            if (notaTaller.KitId != null)
            {
                sValue.Append(", @NotaTaller_KitId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_KitId";
                sqlParam.Value = notaTaller.KitId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            else
                sValue.Append(", NULL");
            sValue.Append(", @NotaTaller_MonedaIdVta");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_MonedaIdVta";
            sqlParam.Value = notaTaller.MonedaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (notaTaller.FechaAplicacion != null)
                sValue.Append(", GETDATE()");
            else
                sValue.Append(", NULL");
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
