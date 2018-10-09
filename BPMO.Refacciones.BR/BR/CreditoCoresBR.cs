using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Generales.DAO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Generales.BR;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para Acceder a Credito de Cores
    /// </summary>
    public class CreditoCoresBR : IBRBaseAuditoria   {
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
        /// <summary>
        /// Consulta registros de días de crédito de piezas cores en Líder.
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que desea consultar</param>
        /// <returns>Un Listado de Auditoria Base que contiene la información de Credito de cores generada por la consulta</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            try {
                CreditoCoresConsultarDAO consultarDAO = new CreditoCoresConsultarDAO();
                return consultarDAO.Consultar(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Actualiza un registro de Credito de Cores en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Elimina un registro de Credito de Cores en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que desea borrar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta Completo, hasta primer nivel, registros de Credito de Cores en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que desea consultar </param>
        /// <returns>Un List de Auditoria Base contiene la informacion de Credito de Cores y sus relaciones a primer nivel, generada por la consulta</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Crea un registro de Credito de Cores en la base de datos
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">CreditoCores que desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        #endregion Métodos
    }
}
