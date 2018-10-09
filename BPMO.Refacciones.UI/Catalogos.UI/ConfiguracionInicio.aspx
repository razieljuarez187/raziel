<%@ Page Title="Configuración Inicio" Language="C#" MasterPageFile="~/Master.UI/Site.Master"
    AutoEventWireup="true" CodeBehind="ConfiguracionInicio.aspx.cs" Inherits="BPMO.Refacciones.Catalogos.UI.PaginaInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Script/Funcionalidad.js" type="text/javascript"></script>
    <script type="text/javascript">
        function PaginaAnterior() {
            window.history.back();
        }
        function pageLoad() {
            SeleccionarBoton($('#<%=btnAceptar.ClientID %>'));
        }       
    </script>
    <div id="PaginaContenido" style="background-image:none; background-color:White;">
        <div id="ContenedorPrincipal_PCI">
            <div id="Contenido_PCI">
                <h1 id="IEncabezado_PCI">
                    ENTRA A TU SUCURSAL</h1>
                <table id="tControles_PCI">
                    <tbody>
                        <tr style="height: 35px">
                            <td class="tdNombre_PCI" style="height: 20px">
                                UNIDAD OPERATIVA
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUnidadOperativa" runat="server" CssClass="tdControl_PCI"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlUnidadOperativa_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr style="height: 35px">
                            <td class="tdNombre_PCI" style="height: 20px">
                                SUCURSAL
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="tdControl_PCI" AutoPostBack="False" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div id="BotonAceptar">
                    <asp:Button ID="btnAceptar" runat="server" Text="ENTRAR" OnClick="btnAceptar_Click"
                        CssClass="BotonComando" Style="margin-left: 240px;" />
                </div>
                <!--Sección de campos ocultos-->
                <asp:HiddenField ID="hdUrl" runat="server" />               
            </div>
        </div>
    </div>
</asp:Content>
