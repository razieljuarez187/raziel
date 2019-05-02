using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.Refacciones.BO {
    /// <summary>
    /// Objeto para el manejo de tipos de pedido
    /// </summary>
    public class TipoPedidoBO : CatalogoBaseBO{
        #region Atributos
        private bool? aplicaTransferencia;
        private bool? aplicaVenta;
        #endregion
        #region Constructores
        #endregion
        #region Propiedades
        public bool? AplicaTransferencia {
            get { return aplicaTransferencia; }
            set { aplicaTransferencia = value; }
        }
        public bool? AplicaVenta {
            get { return aplicaVenta; }
            set { aplicaVenta = value; }
        }
        #endregion
        #region Métodos
        #endregion
    }
}
