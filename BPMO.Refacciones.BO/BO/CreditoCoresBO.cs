using System;
using System.Collections.Generic;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo del credito de cores
    /// </summary>
    public class CreditoCoresBO : AuditoriaBaseBO {  
        #region Atributos
        // Primer nivel son datos que se reciben de Lider.
        private int? empresaId;
        private int? sucursalId;
        private int? clienteId;
        private int? diasFactura;
        private int? diasCredito;
        private int? diasMargen;
        private bool? activo;
        private SucursalBO sucursal;
        private RefaccionBO refaccion;
        private SubcuentaClienteBO subCuentaCliente;
        private LineaBO linea;
        
        #endregion Atributos

        #region Constructores
        #endregion Constructores

        #region Propiedades
        public int? EmpresaId {
            set { this.empresaId = value; }
            get { return this.empresaId; }
        }
        public int? SucursalId {
            set { this.sucursalId = value; }
            get { return this.sucursalId; }
        }
        public int? ClienteId {
            set { this.clienteId = value; }
            get { return this.clienteId; }
        }
        public int? DiasFactura {
            set { this.diasFactura = value; }
            get { return this.diasFactura; }
        }
        public int? DiasCredito {
            set { this.diasCredito = value; }
            get { return this.diasCredito; }
        }
        public int? DiasMargen {
            set { this.diasMargen = value; }
            get { return this.diasMargen; }
        }
        public bool? Activo {
            set { this.activo = value; }
            get { return this.activo; }
        }
        public SucursalBO Sucursal {
            set { this.sucursal = value; }
            get { return this.sucursal; }
        }
        public RefaccionBO Refaccion {
            set { this.refaccion = value; }
            get { return this.refaccion; }
        }
        public SubcuentaClienteBO SubCuentaCliente {
            set { this.subCuentaCliente = value; }
            get { return this.subCuentaCliente; }
        }
        public LineaBO Linea {
            set { this.linea = value; }
            get { return this.linea; }
        }
        #endregion Propiedades


        #region Metodos
        #endregion Metodos

    }
}