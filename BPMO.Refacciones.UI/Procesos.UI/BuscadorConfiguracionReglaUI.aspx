<%@ Page Title="Buscador Cotizacion de Servicio" Language="C#" MasterPageFile="~/Master.UI/Site.Master" AutoEventWireup="true" CodeBehind="BuscadorConfiguracionReglaUI.aspx.cs" Inherits="BPMO.Refacciones.UI.BuscadorConfiguracionReglaUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="../Script/jquery.ui.datepicker-es.js" type="text/javascript"></script>
     <script type="text/javascript">
         function pageLoad() {
             if ($("#<%=grvConfiguracionesReglas.ClientID %>").length == 1 && $("#<%=grvConfiguracionesReglas.ClientID %>")[0].rows.length == 1 && $("#<%=txtId.ClientID %>").val().length > 0)
                 SetValue("", "", "", "", "", "", "", "", "", true);

             $("#<%=txtId.ClientID %>").change(function () {
                 if (typeof (Page_ClientValidate) == 'function') {
                     Page_ClientValidate("FormatoValido");
                 }
                 if (this.value.length > 0) {
                     if (Page_IsValid)
                         CopyData();
                     else
                         return;
                 } else {
                     //Restablecer Filtros
                     var valores = $("#<%=hdnViewUI.ClientID %>").val();
                     if (valores.length > 0) {
                         var filtros = valores.split("$$");
                         SetValue(filtros[0], filtros[1], filtros[2], filtros[3], filtros[4], filtros[5], filtros[6], filtros[7], filtros[8], false);
                         $("#<%=hdnViewUI.ClientID %>").val("");
                     }
                 }
             });
             $('form').keypress(function (e) {
                 if (e.which == 13)
                     return false;
             });
             $("#<%=txtId.ClientID %>").keypress(function (e) {
                 if (e.which == 13 && this.value.length !== 0) {
                     if (typeof (Page_ClientValidate) == 'function')
                         Page_ClientValidate("FormatoValido");
                     if (Page_IsValid) {
                         CopyData();
                         $('.BotonComando').click();
                     } else {
                         return;
                     }
                 }
             });
             EventTxtBuscar();
         }
         function CopyData() {
             if ($("#<%=hdnViewUI.ClientID %>").val().length > 0)
                 return;
             //Almacenar Filtro y limpiar la UI
             var filtros = $("#<%=hdnSucursalId.ClientID %>").val() + "$$" + $("#<%=txtSucursal.ClientID %>").val() + "$$" +
            $("#<%=hdnAlmacenId.ClientID %>").val() + "$$" + $("#<%=txtAlmacen.ClientID %>").val() + "$$" +
            $("#<%=hdnUsuarioId.ClientID %>").val() + "$$" + $("#<%=txtUsuario.ClientID %>").val() + "$$" + $("#<%=ddlEstatus.ClientID %>").val();
             $("#<%=hdnViewUI.ClientID %>").val(filtros);
             SetValue("", "", "", "", "", "", "", "", "", "", "", true);
         }
         function SetValue(val1, val2, val3, val4, val5, val6, val7, disabled) {
             var sucursal = $("#<%=txtSucursal.ClientID %>").selector;
             var almacen = $("#<%=txtAlmacen.ClientID %>").selector;
             var usuario = $("#<%=txtUsuario.ClientID %>").selector;
             var status = $("#<%=ddlEstatus.ClientID %>").selector;
             $("#<%=hdnSucursalId.ClientID %>").val(val1); $(sucursal).val(val2);
             $("#<%=hdnAlmacenId.ClientID %>").val(val3); $(almacen).val(val4);
             $("#<%=hdnUsuarioId.ClientID %>").val(val5); $(usuario).val(val6); $(status).val(val7);
             if (disabled) {
                 AddClass(sucursal, $("#<%=ibtnBuscaSucursal.ClientID %>").selector);
                 AddClass(almacen, $("#<%=ibtnBuscaAlmacen.ClientID %>").selector);
                 AddClass(usuario, $("#<%=ibtnBuscaUsuario.ClientID %>").selector);
                 $(status).attr("disabled", "true");
             } else {
                 DelClass(sucursal, $("#<%=ibtnBuscaSucursal.ClientID %>").selector);
                 DelClass(almacen, $("#<%=ibtnBuscaAlmacen.ClientID %>").selector);
                 DelClass(usuario, $("#<%=ibtnBuscaUsuario.ClientID %>").selector);
                 $(status).removeAttr("disabled", "true");
             }
         }
         function AddClass(compont, btnImg) {
             $(compont).attr("disabled", "true"); $(compont)._addClass("textBoxDisabled");
             $(btnImg).attr("disabled", "true");
         }
         function DelClass(compont, btnImg) {
             $(compont).removeAttr("disabled"); $(compont)._removeClass("textBoxDisabled");
             $(btnImg).removeAttr("disabled");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <span style="margin-left: 5px; color: White; font-size: 12px; font-weight: bold;">PROCESOS - REGLAS USUARIO - BUSCAR REGLA </span>
        </div>
        <div style="height: 110px;">
            <ul id="MenuSecundario">
                <li style="background-color: #E9581B;" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Procesos.UI/BuscadorConfiguracionReglaUI.aspx"
                        ForeColor="White" Width="100%"><img src="../Imagenes/IconoConsultarSeleccionado.png" alt="Icono" class="IconoMenu" />
                        CONSULTAR <img src="../Imagenes/SelectorBlanco.png" alt="selección" style="float:right; margin-right:10px" /> </asp:HyperLink>
                </li>
                <li>                    
                    <asp:HyperLink ID="hlRegistroCotizacion" runat="server" NavigateUrl="~/Procesos.UI/MantenimientoConfiguracionReglaUI.aspx">
                    <img src="../Imagenes/IconoNuevo.png" alt="Icono" class="IconoMenu" />NUEVA REGLA</asp:HyperLink>
                </li>               
            </ul>
        </div>
        <div id="dContenedorTrabajo" style="width: 650px; margin: 0 auto 10px; border-radius: 5px;
            border: 1px solid #cccccc; background-color: #f2f2f2;">
            <div id="dAreaTrabajo" style="width: 620px; height: 22px; background-color: #5c5e5d;
                border-top-left-radius: 5px; border-top-right-radius: 5px; color: white; padding: 4px 0 0 30px;">
                <asp:Label ID="lblArea" runat="server" Text="¿QUE CONFIGURACÓN DE REGLA QUIERE CONSULTAR?" />
                <img id="btnSHConsultar" title="Ocultar" alt="Boton" onclick="SHDIVNotificaciones(this);"
                    class="btnSH" src="../Imagenes/Abajo.jpg" style="float: right; cursor: pointer;
                    width: 35px; height: 20px;" />
            </div>
            <div>
                <table id="TablaBusqueda" class="trAlinearDerecha" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td style="width: 230px;">ID</td>
                        <td style="width: .2em"></td>
                        <td style="width: 400px">
                            <asp:TextBox ID="txtId" runat="server" MaxLength="10" Width="300px" CausesValidation="True"
                                CssClass="TextoAlto" ValidationGroup="FormatoValido" Style="text-align: right"></asp:TextBox>
                        </td>
                    </tr>
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
        <div id="ContenedorGridView" style="width:98%;">
             <asp:GridView ID="grvConfiguracionesReglas" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" CssClass="Grid"
                PageSize="5" AllowPaging="true" OnRowCommand="grvConfiguracionesReglas_RowCommand" OnPageIndexChanging="grvConfiguracionesReglas_PageIndexChanging"
                AllowSorting="true" OnSorting="grvConfiguracionesReglas_Sorting" OnRowDataBound="grvConfiguracionesReglas_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="ID">
                        <ItemStyle HorizontalAlign="Left" Width="40px" Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Empresa" SortExpression="EMPRESA" Visible="false">
                        <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                        <ItemTemplate>
                            <asp:Label ID="lblEmpresa" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sucursal" SortExpression="SUCURSAL" Visible="false">
                        <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                        <ItemTemplate>
                            <asp:Label ID="lblSucursal" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Almacen" SortExpression="ALMACEN">
                        <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                        <ItemTemplate>
                            <asp:Label ID="lblAlmacen" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Usuario" SortExpression="USUARIO">
                        <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                        <ItemTemplate>
                            <asp:Label ID="lblUsuario" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo" SortExpression="TIPO">
                        <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblTipoRegla" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ValorInicial" HeaderText="Valor" SortExpression="VALOR">
                        <ItemStyle HorizontalAlign="Left" Width="75px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ValorFinal" HeaderText="Final" SortExpression="FINAL">
                        <ItemStyle HorizontalAlign="Left" Width="75px" Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Activo" SortExpression="ACTIVO">
                        <ItemStyle HorizontalAlign="Center" Width="40px" Wrap="False" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActivo" runat="server" Enabled="false"/>
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
        <asp:HiddenField ID="hdnUsuarioId" runat="server" Value="" />
        <asp:HiddenField ID="hdnBuscador" runat="server" Value="" />
        <asp:HiddenField ID="hdnViewUI" runat="server" />
    </div>
</asp:Content>