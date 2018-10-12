<%@ Page Title="" Language="C#" MasterPageFile="~/Master.UI/Site.Master" AutoEventWireup="true"
    CodeBehind="MantenimientoConfiguracionTransferenciaUI.aspx.cs" Inherits="BPMO.Refacciones.UI.Procesos.UI.MantenimientoConfiguracionTransferenciaUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .trAlinearDerecha td
        {
            padding: 5px 1px;
        }
        .HeaderGrid
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function BtnBuscar(guid, catalogo) {
            var width = "720px";
            if (catalogo.startsWith("EmpresaLider"))
                width = '660px';
            else if (catalogo.startsWith("SucursalLider"))
                width = '740px';
            $.BuscadorWeb({
                xml: catalogo,
                guid: guid,
                features: {
                    dialogWidth: width,
                    dialogHeight: '350px',
                    center: 'yes',
                    help: 'no',
                    maximize: '0',
                    minimize: 'no'
                },
                btnSender: $("#<%=btnResult.ClientID %>")
            });
        }
        function DeshabilitarMenus() {
            if ($('#<%=hdnTipoAccion.ClientID %>').val() == 'INSERTAR') {
                $('#MenuOpcionesCatalogos > li').addClass('SubMenuCatalogosDeshabilitado');
                $('#ConsultarCatalogo').removeClass('MenuSecundarioSeleccionado').css('backgroundColor', 'transparent').children().css('color', 'black');
                $('#imgConsultaCatalogo').css('display', 'none');
                $('#RegistrarCatalogo').addClass('MenuSecundarioSeleccionado').css('backgroundColor', '#8C6239').children().css('color', 'white');
                $('#imgRegistroCatalogo').css('display', 'block');
            } else {
                if ($('#<%= btnEditar.ClientID %>').is(':disabled')) {
                    $('#MenuOpcionesCatalogos > li.SubMenuSeleccionado').removeClass().addClass('MenuOpcionesli');
                    $('#BotonEditar').addClass('SubMenuSeleccionado').addClass('ImagenFondoGralCatalogos');
                }
                if ($('#<%= btnCancelar.ClientID %>').is(':disabled')) {
                    $('#MenuOpcionesCatalogos > li.SubMenuSeleccionado').removeClass().addClass('MenuOpcionesli');
                    $('#BotonConfiguracion').addClass('SubMenuSeleccionado').addClass('ImagenFondoGralCatalogos');
                }
            }
        }
        function pageLoad() {
            DeshabilitarMenus();
            EstilosMenuOpcionesCatalogos();
            $('#MenuOpcionesCatalogos > li:not(.SubMenuCatalogosDeshabilitado)').click(function () {
                $('#MenuOpcionesCatalogos > li.SubMenuSeleccionado').removeClass().addClass('MenuOpcionesli');
                $(this).addClass('SubMenuSeleccionado').addClass('ImagenFondoGralCatalogos');
                switch (this.id) {
                    case 'BotonConfiguracion':
                        $('#<%= btnCancelar.ClientID %>').click();
                        break;
                    case 'BotonEditar':
                        $('#<%= btnEditar.ClientID %>').click();
                        break;
                }
            });
            $('#divAyuda').draggable();
            EventTxtBuscar();
            MenuTapSelect();
        }
        function EventTxtBuscar() {
            $("input[type='text']").change(function () {
                switch (this.name) {
                    case $('#<%= txtEmpresa.ClientID %>')[0].name:
                        if ($(this).val().length == 0) {
                            $('#<%= hdnEmpresaId.ClientID %>').val("");
                            $('#<%= hdnSucursalId.ClientID %>').val("");
                            $('#<%= txtSucursal.ClientID %>').val("");
                            $('#<%= hdnAlmacenId.ClientID %>').val("");
                            $('#<%= txtAlmacen.ClientID %>').val("");
                        } else {
                            $('#<%= hdnBuscador.ClientID %>').val("1");
                            $('#<%= ibtnBuscaEmpresa.ClientID %>').click();
                        }
                        break;
                    case $('#<%= txtSucursal.ClientID %>')[0].name:
                        if ($(this).val().length == 0) {
                            $('#<%= hdnSucursalId.ClientID %>').val("");
                            $('#<%= hdnAlmacenId.ClientID %>').val("");
                            $('#<%= txtAlmacen.ClientID %>').val("");
                        } else {
                            $('#<%= hdnBuscador.ClientID %>').val("1");
                            $('#<%= ibtnBuscaSucursal.ClientID %>').click();
                        }
                        break;
                    case $('#<%= txtAlmacen.ClientID %>')[0].name:
                        ValidarTxt(this, $('#<%= hdnAlmacenId.ClientID %>'), $('#<%= ibtnBuscaAlmacen.ClientID %>'));
                        break;
                    case $('#<%= txtTipoPedido.ClientID %>')[0].name:
                        ValidarTxt(this, $('#<%= hdnTipoPedidoId.ClientID %>'), $('#<%= ibtnBuscaTipoPedido.ClientID %>'));
                        break;
                }
            });
        }
        function ValidarTxt(txtCampo, hdnId, btnAEjecutar) {
            if ($(txtCampo).val().length == 0) {
                $(hdnId).val("");
            } else {
                $('#<%= hdnBuscador.ClientID %>').val("1");
                $(btnAEjecutar).click();
            }
        }
        function MenuTapSelect() {
            var tapSelect = $("#<%=hdnMenuTaps.ClientID %>").val();
            $("ul#ulTaps >li").removeClass("MenuOrdenSeleccionado");
            $("div#detalleOS > fieldset > div").hide();
            if (tapSelect == "Cantidad") {
                $("#liCantidad").addClass("MenuOrdenSeleccionado");
                $("#divCantidad").show();
            } else if (tapSelect == "Hora") {
                $("#liHora").addClass("MenuOrdenSeleccionado");
                $("#divHora").show();
            } else if (tapSelect == "NivelABC") {
                $("#liNivelABC").addClass("MenuOrdenSeleccionado");
                $("#divNivelABC").show();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server" Style="margin-left: 5px; color: White;
                font-size: 12px; font-weight: bold;">PROCESOS - REGISTRAR REGLA</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 100px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" style="background-color: #8C6239;" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultarConfiguracion" runat="server" Width="100%" NavigateUrl="~/Procesos.UI/BuscadorConfiguracionTransferenciasUI.aspx"
                        ForeColor="White">CONSULTAR<img id="imgConsultaCatalogo" src="../Imagenes/SelectorBlanco.png" alt="selección" 
                        style="float:right; margin-right:10px"/> </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlRegistroConfiguracion" runat="server" NavigateUrl="~/Procesos.UI/MantenimientoConfiguracionTransferenciaUI.aspx">
                    REGISTRAR<img id="imgRegistroCatalogo" src="../Imagenes/SelectorBlanco.png" alt="selección" 
                    style="float:right; margin-right:10px; display:none"/></asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientasOS" style="width: 831px; float: right;">
                <div style="float: left; margin-left: 0; width: 410px">
                    <ul id="MenuOpcionesCatalogos" class="MenuOpciones">
                        <li id="BotonConfiguracion" class="MenuOpcionesli">
                            <div style="font-size: x-small;">
                                CONFIGURACIÓN</div>
                        </li>
                        <li id="BotonEditar" class="MenuOpcionesli">EDITAR</li>
                    </ul>
                </div>
                <div style="float: right; margin-left: 0; width: 340px">
                    <table id="tNumeroCatalogo" runat="server" style="width: 90%; height: 30px; margin-top: 0px;
                        float: left;">
                        <tr>
                            <td style="padding-top: 4px;">
                                <label style="float: left; margin: 3px 3px 0 0;">
                                    # DE CONFIGURACIÓN</label>
                                <asp:Label ID="lblId" runat="server" Width="70px" CssClass="textBoxDisabled" Height="20px"
                                    Style="text-align: right; color: #E9581B; font-weight: bold; border-radius: 5px;"></asp:Label>
                            </td>
                            <td style="border-left: 1px solid #ccc; border-right: 1px solid #ccc; text-align: center;
                                width: 35px">
                                <asp:Button ID="btnInfoBitacora" runat="Server" CssClass="BotonInfoBitacora" OnClientClick="MostrarInformacionBitacora(this); return false;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 311px; float: right; margin-top: 0; height: 50px;">
                <input id="btnNuevaConsulta" type="button" value="NUEVA CONSULTA" class="BotonNuevaConsulta"
                    onclick="window.location='../Procesos.UI/BuscadorConfiguracionTransferenciasUI.aspx'" />
            </div>
        </div>
        <div id="InformacionBItacora" class="InformacionBitacora">
            <div id="ContenidoInformacionBItacora" class="ContenidoInformacionBitacora">
                <div id="EncabezadoBitacora" style="width: 215px; height: 22px; background-color: #5c5e5d;
                    border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;">
                    <asp:Label ID="lblArea" runat="server" Style="margin-left: 5px; font-size: 14px;
                        font-weight: bold;" Text="CONFIGURACIÓN"></asp:Label>
                </div>
                <div id="DatosControlesInformacionBitacora">
                    <table id="TablaBusqueda" style="width: 200px; margin-top: 10px; margin-left: 10px;
                        font-weight: bold;">
                        <tr>
                            <td>
                                CREADO POR<br />
                                <asp:TextBox ID="txtCreadoBitacora" runat="server" CssClass="textBoxDisabled" ReadOnly="true"
                                    Width="185px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                FECHA DE CREACIÓN<br />
                                <asp:TextBox ID="txtFechaCreacionBitacora" runat="server" CssClass="textBoxDisabled"
                                    ReadOnly="true" Width="185px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ACTUALIZACIÓN POR<br />
                                <asp:TextBox ID="txtActualizadoBitacora" runat="server" CssClass="textBoxDisabled"
                                    ReadOnly="true" Width="185px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ULTIMA ACTUALIZACIÓN<br />
                                <asp:TextBox ID="txtFechaActualizacionBitacora" runat="server" CssClass="textBoxDisabled"
                                    ReadOnly="true" Width="185px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="DatosCatalogo" class="GroupBody" style="margin: 20px auto;">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader" style="width: 940px;">
                <span style="margin: 4px 0 0 10px; float: left; font-weight: bold; color: Black;">DATOS
                    DE LA CONFIGURACIÓN DE TRANSFERENCIA</span>
                <div style="float: right; position: relative; right: -1px; top: -20px; width: 290px"
                    class="GroupHeaderOpciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click"
                        ValidationGroup="Requeridos" CssClass="BotonAceptar" Style="float: right; margin: 10px 10px 10px 5px"
                        OnClientClick="ValidatePage('Configuracion');" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                        CssClass="BotonCancelar" Style="float: right; margin: 10px 5px 10px 10px" />
                </div>
            </div>
            <div id="ControlesDatos" style="min-height: 120px; margin-top: 10px;">
                <table style="width: 98%; margin-top: 10px;" class="trAlinearDerecha">
                    <tr>
                        <td style="width: 30%;">
                            *EMPRESA
                        </td>
                        <td style="width: .2em">
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmpresa" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaEmpresa" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaEmpresa_Click" ValidationGroup="requerido" />
                            <asp:RequiredFieldValidator ID="rfvUnidad" runat="server" ControlToValidate="txtEmpresa"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *SUCURSAL
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSucursal" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaSucursal" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaSucursal_Click" ValidationGroup="requerido" />
                            <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" ControlToValidate="txtSucursal"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *ALMACÉN
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAlmacen" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaAlmacen" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaAlmacen_Click" />
                            <asp:RequiredFieldValidator ID="rfvTaller" runat="server" ControlToValidate="txtAlmacen"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *TIPO
                        </td>
                        <td style="width: .2em">
                            &nbsp;
                        </td>
                        <td style="width: 27em">
                            <asp:TextBox ID="txtTipoPedido" runat="server" Width="300px" CssClass="TextoAlto"
                                ValidationGroup="requerido" CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaTipoPedido" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaTipoPedido_Click" />
                            <asp:RequiredFieldValidator ID="rfvTipoPedido" runat="server" ControlToValidate="txtTipoPedido"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *Articulos por item
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaximoArticulosLinea" runat="server" Width="140px" CssClass="TextoAlto"
                                ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revMaximoArticulosLinea" runat="server" ErrorMessage="**"
                                ControlToValidate="txtMaximoArticulosLinea" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                ValidationGroup="requeridos" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="rfvMaximoArticulosLinea" runat="server" ControlToValidate="txtMaximoArticulosLinea"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *Cantidad de items
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaximoLineas" runat="server" Width="140px" CssClass="TextoAlto"
                                ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revMaximoLineas" runat="server" ErrorMessage="**"
                                ControlToValidate="txtMaximoLineas" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                ValidationGroup="requeridos" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="rfvMaximoLineas" runat="server" ControlToValidate="txtMaximoLineas"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trEstatus" runat="server">
                        <td>
                            *Estatus
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="rbtnActivo" runat="server" Text="Activo" GroupName="Estatus" />
                            <asp:RadioButton ID="rbtnInactivo" runat="server" Text="Inactivo" GroupName="Estatus" />
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; text-align: right; height: 35px;">
                    <asp:Button ID="btnEditar" runat="server" OnClick="btnEditar_Click" Text="Editar"
                        CssClass="BotonEditar" Style="float: right; margin-right: 10px;" />
                </div>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
                <div id="ContenidoDatosOrden" style="width: 940px; margin: 40px auto; background-color: White">
                    <ul id="ulTaps">
                        <li id="liCantidad" class="SubMenuOrden">
                            <asp:LinkButton ID="lbtnCantidad" runat="server" Font-Size="Small" Font-Underline="False"
                                OnClick="lbtnCantidad_Click">
                            Cantidad por día
                            </asp:LinkButton>
                        </li>
                        <li id="liHora" class="SubMenuOrden">
                            <asp:LinkButton ID="lbtnHora" runat="server" Font-Size="Small" Font-Underline="False"
                                Style="float: left; margin-left: 3px" OnClick="lbtnHora_Click">
                            Horario por día
                            </asp:LinkButton>
                        </li>
                        <li id="liNivelABC" class="SubMenuOrden">
                            <asp:LinkButton ID="lbtnNivelABC" runat="server" Font-Size="Small" Font-Underline="False"
                                OnClick="lbtnNivelABC_Click">
                            Configuración de Nivel ABC
                            </asp:LinkButton>
                        </li>
                    </ul>
                    <div id="detalleOS" class="GroupBody">
                        <fieldset style="margin-top: 0; border-top: none;">
                            <div id="divCantidad" style="width: 94%; margin: 15px; padding: 15px;">
                                <asp:Panel ID="pnlCantidad" runat="server">
                                    <table style="width: 98%; margin-top: 10px;">
                                        <tr id="trCantidadActivolbl" runat="server">
                                            <td colspan="6">
                                            </td>
                                            <td align="right">
                                                Activo
                                                <asp:CheckBox ID="chkCantidadActivo" runat="server" TextAlign="Left" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr id="trCantidadSemanalbl" runat="server">
                                            <td>
                                                *Lunes
                                            </td>
                                            <td>
                                                *Martes
                                            </td>
                                            <td>
                                                *Miercoles
                                            </td>
                                            <td>
                                                *Jueves
                                            </td>
                                            <td>
                                                *Viernes
                                            </td>
                                            <td>
                                                *Sabado
                                            </td>
                                            <td>
                                                *Domingo
                                            </td>
                                        </tr>
                                        <tr id="trCantidadSemana" runat="server">
                                            <td>
                                                <asp:TextBox ID="txtCantidadLunes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadLunes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadLunes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadLunes" runat="server" ControlToValidate="txtCantidadLunes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadMartes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadMartes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadMartes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadMartes" runat="server" ControlToValidate="txtCantidadMartes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadMiercoles" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadMiercoles" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadMiercoles" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadMiercoles" runat="server" ControlToValidate="txtCantidadMiercoles"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadJueves" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadJueves" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadJueves" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadJueves" runat="server" ControlToValidate="txtCantidadJueves"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadViernes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadViernes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadViernes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadViernes" runat="server" ControlToValidate="txtCantidadViernes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadSabado" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadSabado" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadSabado" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadSabado" runat="server" ControlToValidate="txtCantidadSabado"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCantidadDomingo" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revCantidadDomingo" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtCantidadDomingo" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCantidadDomingo" runat="server" ControlToValidate="txtCantidadDomingo"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div id="divHora" style="width: 94%; margin: 15px; padding: 15px;">
                                <asp:Panel ID="pnlHora" runat="server">
                                    <table style="width: 98%; margin-top: 10px;">
                                        <tr id="trHoraActivolbl" runat="server">
                                            <td colspan="6">
                                            </td>
                                            <td align="right">
                                                Activo
                                                <asp:CheckBox ID="chkHoraActivo" runat="server" TextAlign="Left" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr id="trHoraSemanalbl" runat="server">
                                            <td>
                                                *Lunes
                                            </td>
                                            <td>
                                                *Martes
                                            </td>
                                            <td>
                                                *Miercoles
                                            </td>
                                            <td>
                                                *Jueves
                                            </td>
                                            <td>
                                                *Viernes
                                            </td>
                                            <td>
                                                *Sabado
                                            </td>
                                            <td>
                                                *Domingo
                                            </td>
                                        </tr>
                                        <tr id="trHoraSemana" runat="server">
                                            <td>
                                                <asp:TextBox ID="txtHoraLunes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraLunes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraLunes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraLunes" runat="server" ControlToValidate="txtHoraLunes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraMartes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraMartes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraMartes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraMartes" runat="server" ControlToValidate="txtHoraMartes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraMiercoles" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraMiercoles" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraMiercoles" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraMiercoles" runat="server" ControlToValidate="txtHoraMiercoles"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraJueves" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraJueves" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraJueves" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraJueves" runat="server" ControlToValidate="txtHoraJueves"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraViernes" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraViernes" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraViernes" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraViernes" runat="server" ControlToValidate="txtHoraViernes"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraSabado" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraSabado" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraSabado" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraSabado" runat="server" ControlToValidate="txtHoraSabado"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraDomingo" runat="server" Width="100px" CssClass="TextoAlto"
                                                    ValidationGroup="requerido" CausesValidation="true" MaxLength="50" TextMode="Time"
                                                    format="HH:mm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revHoraDomingo" runat="server" ErrorMessage="**"
                                                    ControlToValidate="txtHoraDomingo" CssClass="ColorValidator" ValidationExpression="^\d*\.?\d+$"
                                                    ValidationGroup="requeridos" Display="Dynamic" />
                                                <asp:RequiredFieldValidator ID="rfvCHoraDomingo" runat="server" ControlToValidate="txtHoraDomingo"
                                                    ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div id="divNivelABC" style="width: 94%; margin: 15px; padding: 15px;">
                                <asp:Panel ID="pnlNivelABC" runat="server">
                                    <table style="width: 98%; margin-top: 10px;">
                                        <tr id="trNivelABCLabel" runat="server">
                                            <td style="width: 40%; margin-top: 10px;" align="right">
                                                <asp:ListBox ID="lbNivelABC" runat="server" Height="150px" Width="50%" SelectionMode="Multiple"
                                                    DataTextField="Nombre"></asp:ListBox>
                                            </td>
                                            <td style="width: 5%; text-align: center; height: 100%; vertical-align:middle;">
                                                <asp:ImageButton ID="imgBtnAgregar" runat="server" ImageUrl="~/Imagenes/imgSlideDerecha.png"
                                                                OnClick="btnAgregar_Click" ImageAlign="Bottom" ToolTip="Agregar" />
                                                <br /><br />
                                                <asp:ImageButton ID="imgBtnQuitar" runat="server" ImageUrl="~/Imagenes/imgSlideIzquierda.png"
                                                                OnClick="btnQuitar_Click" ToolTip="Quitar" />
                                            </td>
                                            <td style="width: 40%; margin-top: 10px;">
                                                <asp:ListBox ID="lbNivelABCRel" runat="server" Height="150px" Width="50%" SelectionMode="Multiple"
                                                    DataTextField="Nombre"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
        <!--Sección de campos ocultos-->
        <asp:HiddenField ID="hdnMenuTaps" runat="server" />
        <asp:HiddenField ID="hdnTipoAccion" runat="server" Value="INSERTAR" />
        <asp:HiddenField ID="hdnEmpresaId" runat="server" Value="" />
        <asp:HiddenField ID="hdnSucursalId" runat="server" Value="" />
        <asp:HiddenField ID="hdnAlmacenId" runat="server" Value="" />
        <asp:HiddenField ID="hdnTipoPedidoId" runat="server" Value="" />
        <asp:HiddenField ID="hdnBuscador" runat="server" Value="" />
        <!--Sección de botones ocultos-->
        <asp:Button ID="btnResult" runat="server" Enabled="True" OnClick="btnResult_Click"
            Text="Aplicar resultados" Style="display: none;" />
    </div>
</asp:Content>
