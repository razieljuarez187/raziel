using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de cantidad de transferencias al día
    /// </summary>
    public class ConfiguracionCantidadTransferenciaBO : AuditoriaBaseBO {
        #region Atributos
        private int? id;
        private int? lunes;
        private int? martes;
        private int? miercoles;
        private int? jueves;
        private int? viernes;
        private int? sabado;
        private int? domingo;
        private bool? activo;
     
        #endregion
        #region Constructores
        #endregion
        #region Propiedades

        public int? Id {
            set { this.id = value; }
            get { return this.id; }
        }
        public int? Lunes {
            set { this.lunes = value; }
            get { return this.lunes; }
        }
        public int? Martes {
            set { this.martes = value; }
            get { return this.martes; }
        }
        public int? Miercoles {
            set { this.miercoles = value; }
            get { return this.miercoles; }
        }
        public int? Jueves {
            get { return this.jueves; }
            set { this.jueves = value; }
        }
        public int? Viernes {
            get { return this.viernes; }
            set { this.viernes = value; }
        }
        public int? Sabado {
            get { return this.sabado; }
            set { this.sabado = value; }
        }
        public int? Domingo {
            get { return this.domingo; }
            set { this.domingo = value; }
        }
        public bool? Activo {
            get { return this.activo; }
            set { this.activo = value; }
        }
        #endregion
        #region Métodos
        #endregion
    }
}
