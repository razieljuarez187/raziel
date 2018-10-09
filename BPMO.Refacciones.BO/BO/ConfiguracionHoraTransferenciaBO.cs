using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de horas para transferencias al día
    /// </summary>
    public class ConfiguracionHoraTransferenciaBO : AuditoriaBaseBO {
        #region Atributos
        private int? id;
        private TimeSpan? lunes;
        private TimeSpan? martes;
        private TimeSpan? miercoles;
        private TimeSpan? jueves;
        private TimeSpan? viernes;
        private TimeSpan? sabado;
        private TimeSpan? domingo;
        private bool? activo;

        #endregion
        #region Constructores
        #endregion
        #region Propiedades

        public int? Id {
            set { this.id = value; }
            get { return this.id; }
        }
        public TimeSpan? Lunes {
            set { this.lunes = value; }
            get { return this.lunes; }
        }
        public TimeSpan? Martes {
            set { this.martes = value; }
            get { return this.martes; }
        }
        public TimeSpan? Miercoles {
            set { this.miercoles = value; }
            get { return this.miercoles; }
        }
        public TimeSpan? Jueves {
            get { return this.jueves; }
            set { this.jueves = value; }
        }
        public TimeSpan? Viernes {
            get { return this.viernes; }
            set { this.viernes = value; }
        }
        public TimeSpan? Sabado {
            get { return this.sabado; }
            set { this.sabado = value; }
        }
        public TimeSpan? Domingo {
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
