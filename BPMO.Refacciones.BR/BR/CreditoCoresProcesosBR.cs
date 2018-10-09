using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Generales.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para acceder a los procesos del días de crédito de cores
    /// </summary>
    public class CreditoCoresProcesosBR {
        #region Atributos
        #endregion Atributos

        #region Propiedades
        #endregion Propiedades

        #region Métodos
        /// <summary>
        /// Consulta un registro de Credito de Cores en la base de datos
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que provee el criterio de seleccion para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los creditos cores recuperados de la consulta</returns>
        public List<AuditoriaBaseBO> ConsultarCreditoCores(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            try {
                #region Validar Filtros
                CreditoCoresBO creditoCores = null;
                if (auditoriaBase is CreditoCoresBO)
                    creditoCores = (CreditoCoresBO)auditoriaBase;
                string msjError = string.Empty;
                if (creditoCores == null)
                    msjError += " , CreditoCores";
                if (dataContext == null)
                    msjError += " , dataContext";
                // Es la sucursal de Oracle
                if (creditoCores.Sucursal.Id == null)
                    msjError += " , Sucursal.Id ";
                if (creditoCores.SubCuentaCliente.Id == null)
                    msjError += " , SubCuentaCliente.Id ";
                if (creditoCores.Refaccion.Id == null)
                    msjError += " , Refaccion.Id ";
                if (msjError.Length > 0)
                    throw new ArgumentNullException(msjError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
                #endregion Validar Filtros

                #region Derivaciones
                //  Buscar en Oracle El nombre corto de la Sucursal y ID de la UO
                SucursalBR ConsultaSucOracle = new SucursalBR();
                List<CatalogoBaseBO> lstSucursal = ConsultaSucOracle.Consultar(dataContext, creditoCores.Sucursal);
                if (lstSucursal.Count == 1) {
                    creditoCores.Sucursal = (SucursalBO)lstSucursal[0];
                } else throw new Exception(" No se pudo derivar la SucursalID en Lider");

                //Buscar del nombre corto de la UO para que sirva en la derivacion del sucursalid en lider
                UnidadOperativaBR ConsultaUOOracle = new UnidadOperativaBR();
                List<CatalogoBaseBO> lstUO = ConsultaUOOracle.Consultar(dataContext, creditoCores.Sucursal.UnidadOperativa);
                if (lstUO.Count == 1) {
                    creditoCores.Sucursal.UnidadOperativa = (UnidadOperativaBO)lstUO[0];
                } else throw new Exception(" No se pudo derivar la EmpresaID en lider");

                // Derivacion de SucursalID de Lider 
                CatalogosGeneralesLiderBR Consulta = new CatalogosGeneralesLiderBR();
                creditoCores.Sucursal.Id = Consulta.ConsultarSucursalLider(dataContext, creditoCores.Sucursal);

                //Derivacion de la EmpresaID de Lider
                creditoCores.EmpresaId = Consulta.ConsultaEmpresaLider(dataContext, creditoCores.Sucursal.UnidadOperativa);

                //Derivacion del cliente de Lider
                creditoCores.ClienteId = Consulta.ConsultaNumeroClienteLider(dataContext, creditoCores.SubCuentaCliente);

                // Consulta del la refaccion 
                RefaccionBR ConsultaRefaccion = new RefaccionBR();
                List<CatalogoBaseBO> lstCatalogo = ConsultaRefaccion.Consultar(dataContext, creditoCores.Refaccion);
                if (lstCatalogo.Count == 1) {
                    creditoCores.Refaccion = (RefaccionBO)lstCatalogo[0];
                    creditoCores.Linea = creditoCores.Refaccion.Linea;
                } else throw new Exception(" No Existe refaccionId en lider");
                #endregion Derivaciones

                #region Consulta de configuración
                // Consulta del credito de cores
                CreditoCoresConsultarDAO consultarDAO = new CreditoCoresConsultarDAO();
                return consultarDAO.Consultar(dataContext, creditoCores);
                #endregion Consulta de configuración

            } catch {
                throw;
            }
        }
        #endregion Métodos
    }
}
