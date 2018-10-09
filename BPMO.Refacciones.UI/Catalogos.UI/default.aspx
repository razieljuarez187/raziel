<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.UI/Site.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="BPMO.Refacciones.Catalogos.UI.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            EfectoHoverSubMenu($('#MenuSecundarioSecundarioli'));
            $("#MenuSecundarioSecundarioli").click(function () {
                ShowHelp();
            });
            $('#divAyuda').draggable();
        });  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Barra de localización -->
    <div id="BarraUbicacion">
    </div>
    <!-- Menú secundario -->
    <div style="height:50px;">
        <ul id="MenuSecundarioInicio" class="MenuSecundario">
            <li id="MenuSecundarioSecundarioli" style="text-align: center; cursor:pointer" class="MenuSecundarioSecundarioli">
                <p class="MenuSecundariolia">BIENVENIDO</p>
            </li>
        </ul>
    </div>
    <!--Contenedor principal de la página de inicio-->
    <div id="CuerpoInicio">
        <!--Notificaciones-->
        <asp:Repeater ID="rptNotificaciones" runat="server">
            <ItemTemplate>
                <!--Cuerpo de la notificación-->
                <div id="CuerpoNotificacion" class='<%# Eval("Tipo") %>'>
                    <img src="" alt="Imagen notificación" style="float: left" />
                    <h4 id="tituloNotificacion">
                        <span id="textoTitulo">
                            <%# Eval("Titulo") %></span>
                        <img id="btnSHCuerpoNotificacion" title="Ocultar" alt="Boton" onclick="SHDIVNotificaciones(this);"
                            class="btnSH" src="../Imagenes/Abajo.jpg" style="float: right; cursor: pointer;
                            width: 51px; height: 35px;" />
                    </h4>
                    <div id="pnlTextoNotificacion">
                        <a href='<%# Eval("Url") %>'>
                            <asp:Image ID="imgNotificacion" runat="server" ImageUrl='<%# Eval("Imagen") %>' AlternateText="Notificación"
                                CssClass="imgNotificacion" />
                        </a><span id="TextoNotificacion">
                            <%# Eval("Notificacion") %></span>
                        <div class="EnlaceNotificacion">
                            <asp:HyperLink ID="hpNotificacion" runat="server" NavigateUrl='<%# Eval("Url") %>'
                                Text='<%# Eval("UrlTitulo") %>' CssClass="hpMasInformacion"></asp:HyperLink>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
