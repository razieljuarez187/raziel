<%@ Page Title="Consultar configuraciones asignadas" Language="C#" MasterPageFile="~/Master.UI/Site.Master" AutoEventWireup="true" CodeBehind="ConfiguracionesReglasAsignadasUI.aspx.cs" Inherits="BPMO.Refacciones.Reportes.ConfiguracionesReglasAsignadasUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Script/jquery.ui.datepicker-es.js"></script>
    <script type="text/javascript">
        function GuardarEstado(boton) {
            var Estado = SHDIVNotificaciones(boton); $('#<%= hdnEstadoDatos.ClientID %>').val(Estado);
        }
        function pageLoad() {
            if ($('#<%= hdnEstadoDatos.ClientID %>').val() == 'false') {
                $('#DatosControles').hide(); var boton = $('#<%=btnSHConsultar.ClientID %>');
                boton.attr('title', 'Mostrar'); boton.parent().css({ 'backgroundColor': 'white',
                    'color': '#5c5e5d'
                });
            }
            if ($('#<%= hdnMostrarReporte.ClientID %>').val() == '0') {
                $('#dContenedor').show();
                $('#BarraHerramientasOS').hide();
                $('#DatosReporte').hide();
            } else {
                $('#dContenedor').hide();
                $('#BarraHerramientasOS').show();
                $('#DatosReporte').show();
            }
            EventTxtBuscar();
        }
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
        function EventTxtBuscar() {
            $("input[type='text']").change(function () {
                switch (this.name) {
                    case $('#<%= txtEmpresa.ClientID %>')[0].name:
                        ValidarTxt(this, $('#<%= hdnEmpresaId.ClientID %>'), $('#<%= ibtnBuscaEmpresa.ClientID %>'));
                        break;
                    case $('#<%= txtSucursal.ClientID %>')[0].name:
                        ValidarTxt(this, $('#<%= hdnSucursalId.ClientID %>'), $('#<%= ibtnBuscaSucursal.ClientID %>'));
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
    <style>
        div#dvReport input[type=text]{padding-right:0px!important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <span style="margin-left: 5px; color: White; font-size: 12px; font-weight: bold;">REPORTES - CONFIGURACIONES ASIGNADAS</span>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 30px;">
            <!-- Barra de herramientas -->
            <div id="BarraHerramientasOS" style="width: 100%; float: right;">
                <div style="float: left; margin-left: 0; width: 400px">
                    <ul id="MenuOpcionesOrden" class="MenuOpciones">
                        <li id="BotonRegresar" class="MenuOpcionesli" onclick="$('#<%=btnRegresar.ClientID %>').click();">REGRESAR</li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="dContenedor">
        <div id="dContenedorTrabajo" style="width: 650px; margin: 15px auto 10px; border-radius: 5px;
            border: 1px solid #cccccc; background-color: #f2f2f2;">
            <div id="dAreaTrabajo" style="width: 620px; height: 22px; background-color: #5c5e5d;
                border-top-left-radius: 5px; border-top-right-radius: 5px; color: white; padding: 4px 0 0 30px;">
                <asp:Label ID="lblArea" runat="server" Text="¿QUE CONFIGURACIONES QUIERE CONSULTAR?" />
                <asp:Image ID="btnSHConsultar" runat="server" title="Ocultar" alt="Boton" onclick=" GuardarEstado(this);"
                    ImageUrl="../Imagenes/Abajo.jpg" Style="cursor: pointer; width: 35px; float: right;
                    height: 20px;" />
            </div>
            <div id="DatosControles">
                <table id="TablaBusqueda" class="trAlinearDerecha" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td style="width: 230px;">EMPRESA</td>
                        <td style="width: .2em"></td>
                        <td style="width: 400px">
                            <asp:TextBox ID="txtEmpresa" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaEmpresa" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaEmpresa_Click" ValidationGroup="requerido" />
                        </td>
                    </tr>
                    <tr>
                        <td>SUCURSAL</td>
                        <td style="width: .2em"></td>
                        <td>
                            <asp:TextBox ID="txtSucursal" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaSucursal" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaSucursal_Click" ValidationGroup="requerido" />
                        </td>
                    </tr>
                    <tr>
                        <td>ALMACÉN</td>
                        <td style="width: .2em">&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtAlmacen" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaAlmacen" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaAlmacen_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>USUARIO</td>
                        <td style="width: .2em">&nbsp;</td>
                        <td style="width: 27em">
                            <asp:TextBox ID="txtUsuario" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaUsuario" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaUsuario_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>TIPO REGLA</td>
                        <td style="width: .2em">&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlTipoRegla" runat="server" CssClass="" Style="width:300px" />
                        </td>
                    </tr>
                    <tr>
                        <td>VALOR INICIAL</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtValorInicialA" runat="server" Width="140px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorInicialA" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorInicialA" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="FormatoValido" Display="Dynamic" />
                            <asp:Label ID="lblValoresIniSeparador" runat="server" Text=" A " />
                            <asp:TextBox ID="txtValorInicialB" runat="server" Width="140px" CssClass="TextoAlto ValFinal" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorInicialB" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorInicialB" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="FormatoValido" Display="Dynamic" />
                            <asp:CompareValidator ID="cvValoresIni" runat="server" ControlToValidate="txtValorInicialA" ControlToCompare="txtValorInicialB"
                                Operator="LessThanEqual" type="Double" CssClass="ColorValidator" Display="Dynamic" ErrorMessage="***" ValidationGroup="FormatoValido" />
                        </td>
                    </tr>
                    <tr>
                        <td>VALOR FINAL</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtValorFinalA" runat="server" Width="140px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorFinalA" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorFinalA" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="FormatoValido" Display="Dynamic" />
                            <asp:Label ID="lblValoresFinSeparador" runat="server" Text=" A " />
                            <asp:TextBox ID="txtValorFinalB" runat="server" Width="140px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revValorFinalB" runat="server" ErrorMessage="**"
                                ControlToValidate="txtValorFinalB" CssClass="ColorValidator" ValidationExpression="^-?[0-9]\d*(\.\d+)?$"
                                ValidationGroup="FormatoValido" Display="Dynamic" />
                            <asp:CompareValidator ID="cvValoresFin" runat="server" ControlToValidate="txtValorFinalA" ControlToCompare="txtValorFinalB"
                                Operator="LessThanEqual" type="Double" ErrorMessage="***" ValidationGroup="FormatoValido" />
                        </td>
                    </tr>
                    <tr>
                        <td>ESTATUS</td>
                        <td style="width: .2em">&nbsp;</td>
                        <td style="width: 27em">
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="" Style="width:200px">
                                <asp:ListItem Value="null">Todos</asp:ListItem>
                                <asp:ListItem Value="true">Activo</asp:ListItem>
                                <asp:ListItem Value="false">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; margin-top: 10px;">
                    <asp:Button ID="btnBuscar" runat="server" Text="BUSCAR" OnClick="btnBuscar_Click"
                        TabIndex="5" ValidationGroup="FormatoValido" CssClass="BotonBuscarCatalogo" Style="margin: 0 273px;" />
                </div>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span><br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        </div>
        <!--SECCIÓN DEL REPORTE-->
        <div id="DatosReporte" style="margin: 23px auto 10px; width: 940px;" class="GroupBody">
            <div id="EncabezadoDatosDelReporte" class="GroupHeader" style="width: 940px; margin: 0;">
                <span style="margin: 3px 15px 0; float: left; font-weight: bold">REPORTE DE CONFIGURACIONES ASIGNADAS</span>
            </div>
            <div id="Div1" style="min-height: 340px; margin-top: 10px">
                <div id="dvReport">
                    <center>
                    <dx:ReportToolbar ID="rptToolBar" runat='server' ShowDefaultButtons='False' ReportViewerID="rptViewer">
                        <Items>
                            <dx:ReportToolbarButton ItemKind='Search' ToolTip="Mostrar la pantalla de busqueda" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind='PrintReport' ToolTip="Imprimir el reporte"/>
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' ToolTip="Primera página"/>
                            <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' ToolTip="Página anterior"/>
                            <dx:ReportToolbarLabel ItemKind='PageLabel' Text="Página"/>
                            <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                            </dx:ReportToolbarComboBox>
                            <dx:ReportToolbarLabel ItemKind='OfLabel' Text="de"/>
                            <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
                            <dx:ReportToolbarButton ItemKind='NextPage' ToolTip="Página siguiente"/>
                            <dx:ReportToolbarButton ItemKind='LastPage' ToolTip="Última página"/>
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind='SaveToDisk' ToolTip="Exportar el reporte y guardarlo en disco" />
                            <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                                <Elements>
                                    <dx:ListElement Value='pdf' />
                                    <dx:ListElement Value='xls' />
                                    <dx:ListElement Value='xlsx' />
                                    <dx:ListElement Value='rtf' />
                                    <dx:ListElement Value='mht' />
                                    <dx:ListElement Value='html' />
                                    <dx:ListElement Value='txt' />
                                    <dx:ListElement Value='csv' />
                                    <dx:ListElement Value='png' />
                                </Elements>
                            </dx:ReportToolbarComboBox>
                        </Items>
                        <Styles>
                            <LabelStyle>
                                <Margins MarginLeft='10px' MarginRight='10px' />
                            </LabelStyle>
                        </Styles>
                    </dx:ReportToolbar>
                    <div style="width: 95%; overflow:scroll;">
                    <dx:ReportViewer ID="rptViewer" runat="server" LoadingPanelText="Cargando..." 
                            onunload="rptViewer_Unload" >
                        <border borderwidth="10px" bordercolor="#CCCCCC"></border>
                    </dx:ReportViewer>
                    </div>
                    </center>
                </div>
                <br />
            </div>
        </div>
        <!--sección de campos ocultos-->
        <asp:HiddenField ID="hdnEmpresaId" runat="server" Value="" />
        <asp:HiddenField ID="hdnSucursalId" runat="server" Value="" />
        <asp:HiddenField ID="hdnAlmacenId" runat="server" Value="" />
        <asp:HiddenField ID="hdnUsuarioId" runat="server" Value="" />
        <asp:HiddenField ID="hdnBuscador" runat="server" Value="" />
        <asp:HiddenField ID="hdnViewUI" runat="server" />
        <asp:HiddenField ID="hdnMostrarReporte" runat="server" Value="0" />
        <asp:HiddenField ID="hdnEstadoDatos" runat="Server" Value="true" />
        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" Style="display: none;" onclick="btnRegresar_Click" />
        <!--Sección de botones ocultos-->
        <asp:Button ID="btnResult" runat="server" Enabled="True" OnClick="btnResult_Click"
            Text="Aplicar resultados" Style="display: none;" />
    </div>
</asp:Content>
