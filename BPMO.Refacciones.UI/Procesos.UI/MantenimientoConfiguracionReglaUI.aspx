<%@ Page Title="" Language="C#" MasterPageFile="~/Master.UI/Site.Master" AutoEventWireup="true" CodeBehind="MantenimientoConfiguracionReglaUI.aspx.cs" Inherits="BPMO.Refacciones.UI.Procesos.UI.MantenimientoConfiguracionReglaUI" %>
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
                    case $('#<%= txtUsuario.ClientID %>')[0].name:
                        ValidarTxt(this, $('#<%= hdnUsuarioId.ClientID %>'), $('#<%= ibtnBuscaUsuario.ClientID %>'));
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
                    <asp:HyperLink ID="hlConsultarConfiguracion" runat="server" Width="100%" NavigateUrl="~/Procesos.UI/BuscadorConfiguracionReglaUI.aspx"
                        ForeColor="White">CONSULTAR<img id="imgConsultaCatalogo" src="../Imagenes/SelectorBlanco.png" alt="selección" 
                        style="float:right; margin-right:10px"/> </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlRegistroConfiguracion" runat="server" NavigateUrl="~/Procesos.UI/MantenimientoConfiguracionReglaUI.aspx">
                    REGISTRAR<img id="imgRegistroCatalogo" src="../Imagenes/SelectorBlanco.png" alt="selección" 
                    style="float:right; margin-right:10px; display:none"/></asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientasOS" style="width: 831px; float: right;">
                <div style="float: left; margin-left: 0; width: 410px">
                    <ul id="MenuOpcionesCatalogos" class="MenuOpciones">
                        <li id="BotonConfiguracion" class="MenuOpcionesli">REGLA</li>
                        <li id="BotonEditar" class="MenuOpcionesli">EDITAR</li>
                    </ul>
                </div>
                <div style="float: right; margin-left: 0; width: 340px">
                    <table id="tNumeroCatalogo" runat="server" style="width:90%; height: 30px; margin-top: 0px;
                        float: left;">
                        <tr>
                            <td style="padding-top: 4px;">
                                <label style="float: left; margin: 3px 3px 0 0;">
                                    #NÚMERO DE REGLA</label>
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
                    onclick="window.location='../Procesos.UI/BuscadorConfiguracionReglaUI.aspx'" />
            </div>
        </div>
        <div id="InformacionBItacora" class="InformacionBitacora">
            <div id="ContenidoInformacionBItacora" class="ContenidoInformacionBitacora">
                <div id="EncabezadoBitacora" style="width: 215px; height: 22px; background-color: #5c5e5d;
                    border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;">
                    <asp:Label ID="lblArea" runat="server" Style="margin-left: 5px; font-size: 14px;
                        font-weight: bold;" Text="REGLA"></asp:Label>
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
                <span style="margin: 4px 0 0 10px; float: left; font-weight: bold; color: Black;">DATOS DE LA CONFIGURACIÓN DE REGLA</span>
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
                        <td style="width:30%;">*EMPRESA</td>
                        <td style="width: .2em"></td>
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
                        <td>*SUCURSAL</td>
                        <td></td>
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
                        <td>*ALMACÉN</td>
                        <td></td>
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
                        <td>*USUARIO</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtUsuario" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaUsuario" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaUsuario_Click" />
                            <asp:RequiredFieldValidator ID="rfvEmpleado" runat="server" ControlToValidate="txtUsuario"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>*TIPO</td>
                        <td></td>
                        <td>
                            <asp:DropDownList ID="ddlTipoRegla" runat="server" CssClass="" Style="width:300px" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="ddlTipoRegla"
                                ErrorMessage="*" ValidationGroup="Requeridos" CssClass="ColorValidator" />
                        </td>
                    </tr>
                    <tr>
                        <td>*VALOR</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtValorInicial" runat="server" Width="140px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorInicial" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorInicial" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="Requeridos" Display="Dynamic" />
                            <asp:Label ID="lblValoresSeparador" runat="server" Text=" A " CssClass="ValFinal" />
                            <asp:TextBox ID="txtValorFinal" runat="server" Width="140px" CssClass="TextoAlto ValFinal" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorFinal" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorFinal" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="Requeridos" Display="Dynamic" />
                            <asp:CompareValidator ID="cvValores" runat="server" ControlToValidate="txtValorInicial" ControlToCompare="txtValorFinal"
                                Operator="LessThanEqual" type="Double" ErrorMessage="***" ValidationGroup="Requeridos" />
                        </td>
                    </tr>
                    <tr id="trEstatus" runat="server">
                        <td>*Estatus</td>
                        <td></td>
                        <td>
                            <asp:RadioButton ID="rbtnActivo" runat="server" Text="Activo" GroupName="Estatus" />
                            <asp:RadioButton ID="rbtnInactivo" runat="server" Text="Inactivo" GroupName="Estatus" />
                        </td>
                    </tr>
                </table>
                <div style="width:100%; text-align: right; height: 35px;">
                    <asp:Button ID="btnEditar" runat="server" OnClick="btnEditar_Click" Text="Editar"
                        CssClass="BotonEditar" Style="float: right; margin-right: 10px;" />
                </div>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
        <!--Sección de campos ocultos-->
        <asp:HiddenField ID="hdnTipoAccion" runat="server" Value="INSERTAR" />
        <asp:HiddenField ID="hdnEmpresaId" runat="server" Value="" />
        <asp:HiddenField ID="hdnSucursalId" runat="server" Value="" />
        <asp:HiddenField ID="hdnAlmacenId" runat="server" Value="" />
        <asp:HiddenField ID="hdnUsuarioId" runat="server" Value="" />
        <asp:HiddenField ID="hdnBuscador" runat="server" Value="" />
        <!--Sección de botones ocultos-->
        <asp:Button ID="btnResult" runat="server" Enabled="True" OnClick="btnResult_Click"
            Text="Aplicar resultados" Style="display: none;" />
    </div>
</asp:Content>
