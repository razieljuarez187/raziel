<%@ Page Title="Configuracion de Transferencias" Language="C#" MasterPageFile="~/Master.UI/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscadorConfiguracionTransferenciasUI.aspx.cs"
    Inherits="BPMO.Refacciones.UI.BuscadorConfiguracionTransferenciasUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function BtnBuscar(guid, catalogo) {
            var width = "720px";
            if (catalogo.startsWith("EmpresaLider"))
                width = '660px';
            else if (catalogo.startsWith("SucursalLider"))
                width = '740px';
            else if (catalogo.startsWith("TipoPedido"))
                width = '600px';
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
        function GuardarEstado(boton) {
            var Estado = SHDIVNotificaciones(boton);
            $('#<%= hdnViewUI.ClientID %>').val(Estado);
        }
        function pageLoad() {
            if ($('#<%= hdnViewUI.ClientID %>').val() == 'false') {
                $('#DatosControles').hide();
                var boton = $('#<%=hdnViewUI.ClientID %>');
                boton.attr('title', 'Mostrar');
                boton.parent().css({ 'backgroundColor': 'white', 'color': '#5c5e5d' });
            }
            EjecutarBotonBusqueda();
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
        function ValidarCampo(tipo) {
            var correcto = true;
            if (tipo == 1) {
                if ($('#<%= txtEmpresa.ClientID %>').val().length == 0) {
                    MensajeGrowUI("Es necesario que primero seleccione una empresa.", "4");
                    correcto = false;
                }
            } else {
                if ($('#<%= txtSucursal.ClientID %>').val().length == 0) {
                    MensajeGrowUI("Es necesario que primero seleccione una sucursal.", "4");
                    correcto = false;
                }
            }
            return correcto;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <span style="margin-left: 5px; color: White; font-size: 12px; font-weight: bold;">PROCESOS
                - CONFIGURACION DE TRANSFERENCIAS - BUSCAR CONFIGURACIÓN </span>
        </div>
        <div style="height: 110px;">
            <ul id="MenuSecundario">
                <li style="background-color: #E9581B;" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Procesos.UI/BuscadorConfiguracionTransferenciasUI.aspx"
                        ForeColor="White" Width="100%"><img src="../Imagenes/IconoConsultarSeleccionado.png" alt="Icono" class="IconoMenu" /> CONSULTAR <img src="../Imagenes/SelectorBlanco.png" alt="selección" style="float:right; margin-right:10px" /></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroCotizacion" runat="server" NavigateUrl="~/Procesos.UI/MantenimientoConfiguracionTransferenciaUI.aspx" Font-Size=Small> <img src="../Imagenes/IconoNuevo.png" alt="Icono" class="IconoMenu" />NUEVA CONFIGURACIÓN</asp:HyperLink>
                </li>
            </ul>
        </div>
        <div id="dContenedorTrabajo" style="width: 650px; margin: 0 auto 10px; border-radius: 5px;
            border: 1px solid #cccccc; background-color: #f2f2f2;">
            <div id="dAreaTrabajo" style="width: 620px; height: 22px; background-color: #5c5e5d;
                border-top-left-radius: 5px; border-top-right-radius: 5px; color: white; padding: 4px 0 0 30px;">
                <asp:Label ID="lblArea" runat="server" Text="¿QUE CONFIGURACÓN DE TRANSFERENCIA QUIERE CONSULTAR?" />
                <img id="btnSHConsultar" title="Ocultar" alt="Boton" onclick="SHDIVNotificaciones(this);"
                    class="btnSH" src="../Imagenes/Abajo.jpg" style="float: right; cursor: pointer;
                    width: 35px; height: 20px;" />
            </div>
            <div id="DatosControles">
                <table id="TablaBusqueda" class="trAlinearDerecha" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td style="width: 230px;">
                            IDENTIFICADOR
                        </td>
                        <td style="width: .2em">
                        </td>
                        <td style="width: 400px">
                            <asp:TextBox ID="txtId" runat="server" MaxLength="10" Width="300px" CausesValidation="True"
                                CssClass="TextoAlto" ValidationGroup="FormatoValido" Style="text-align: right"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 230px;">
                            EMPRESA
                        </td>
                        <td style="width: .2em">
                        </td>
                        <td style="width: 400px">
                            <asp:TextBox ID="txtEmpresa" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaEmpresa" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaEmpresa_Click" ValidationGroup="requerido" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SUCURSAL
                        </td>
                        <td style="width: .2em">
                        </td>
                        <td>
                            <asp:TextBox ID="txtSucursal" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaSucursal" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaSucursal_Click" ValidationGroup="requerido" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ALMACÉN
                        </td>
                        <td style="width: .2em">
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtAlmacen" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaAlmacen" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaAlmacen_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            TIPO
                        </td>
                        <td style="width: .2em">
                            &nbsp;
                        </td>
                        <td style="width: 27em">
                            <asp:TextBox ID="txtTipoPedido" runat="server" Width="300px" CssClass="TextoAlto" ValidationGroup="requerido"
                                CausesValidation="true" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaTipoPedido" runat="server" ImageUrl="~/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaTipoPedido_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ESTATUS
                        </td>
                        <td style="width: .2em">
                            &nbsp;
                        </td>
                        <td style="width: 27em">
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="" Style="width: 200px">
                                <asp:ListItem Value="null">Todos</asp:ListItem>
                                <asp:ListItem Value="true">Activo</asp:ListItem>
                                <asp:ListItem Value="false">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; margin-top: 10px;">
                    <asp:Button ID="btnBuscar" runat="server" Text="BUSCAR" OnClick="btnBuscar_Click"
                        TabIndex="5" ValidationGroup="FormatoValido" CssClass="BotonComando" Style="margin: 0 273px;" />
                    <asp:Button ID="btnResult" runat="server" Enabled="True" OnClick="btnResult_Click"
                        Text="Aplicar resultados" Style="display: none;" />
                </div>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
        <center>
            <div id="ContenedorGridView" style="width: 98%;">
                <asp:GridView ID="grvConfiguracionesTransferencia" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" Width="100%" CssClass="Grid" PageSize="5" AllowPaging="true"
                    AllowSorting="true" 
                    OnRowDataBound="grvConfiguracionesTransferencia_RowDataBound" 
                    onrowcommand="grvConfiguracionesTransferencia_RowCommand" 
                    onsorting="grvConfiguracionesTransferencia_Sorting" 
                    onpageindexchanging="grvConfiguracionesTransferencia_PageIndexChanging" >
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="ID">
                            <ItemStyle HorizontalAlign="Left" Width="40px" Wrap="False" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Empresa" SortExpression="EMPRESA">
                            <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                            <ItemTemplate>
                                <asp:Label ID="lblEmpresa" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sucursal" SortExpression="SUCURSAL">
                            <ItemStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                            <ItemTemplate>
                                <asp:Label ID="lblSucursal" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Almacen" SortExpression="ALMACEN">
                            <ItemStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                            <ItemTemplate>
                                <asp:Label ID="lblAlmacen" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo" SortExpression="TIPOPEDIDO">
                            <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="True" />
                            <ItemTemplate>
                                <asp:Label ID="lblTipoPedido" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activo" SortExpression="ACTIVO">
                            <ItemStyle HorizontalAlign="Center" Width="40px" Wrap="False" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbDetalle" ImageUrl="~/Imagenes/Detalle.png" runat="server"
                                    CommandName="CMDDETALLES" CommandArgument='<%# Eval("Id")%>' />
                            </ItemTemplate>
                            <ItemStyle Width="16px" Wrap="False" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeader" />
                    <EditRowStyle CssClass="GridAlternatingRow" />
                    <PagerStyle CssClass="GridPager" />
                    <RowStyle CssClass="GridRow" />
                    <FooterStyle CssClass="GridFooter" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                    <EmptyDataTemplate>
                        <b>No existen registros que cumplan con la condición solicitada, favor de corregir.</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </center>
        <asp:HiddenField ID="hdnEmpresaId" runat="server" Value="" />
        <asp:HiddenField ID="hdnSucursalId" runat="server" Value="" />
        <asp:HiddenField ID="hdnAlmacenId" runat="server" Value="" />
        <asp:HiddenField ID="hdnTipoPedidoId" runat="server" Value="" />
        <asp:HiddenField ID="hdnBuscador" runat="server" Value="" />
        <asp:HiddenField ID="hdnViewUI" runat="server" />
    </div>
</asp:Content>
