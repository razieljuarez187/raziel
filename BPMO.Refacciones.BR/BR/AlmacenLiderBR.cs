using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Refacciones.DAO;
using BPMO.Patterns.Creational.DataContext;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para AlmacenLider
    /// </summary>
    public class AlmacenLiderBR : IBRBaseCatalogo {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion /Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion /Propiedades

        #region Métodos
        public bool Insertar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        public bool Actualizar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        public bool Borrar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene una lista de Almacenes Líder
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="catalogoBase">Objeto con los criterios de búsqueda</param>
        /// <returns>Lista de objetos que coinciden con los parámetros de búsqueda</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            AlmacenLiderConsultarDAO consultarDAO = new AlmacenLiderConsultarDAO();
            return consultarDAO.Consultar(dataContext, catalogoBase);
        }
        public List<CatalogoBaseBO> ConsultarCompleto(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            throw new NotImplementedException();
        }
        #endregion /Métodos
    }
}
