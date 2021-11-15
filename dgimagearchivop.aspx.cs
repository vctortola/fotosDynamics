/// Autor:          Carlos Juan
/// Fecha:          2012-01-18
/// Descripcion:    Mostrar documento adjunto en pantalla
/// Modificacion:   2012-01-18 - Luis Alberto - Convertir a c#
///                 y obtener datos mediante servicio web
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using EncryptArgs;

public partial class dgimagearchivo2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Obtener parametros
            this.Title = "Documentos Adjuntos";
            //  Dim strImageID As String = Request.QueryString("id")
            decimal tmp;

            try
            {

                string id = string.Empty;
                EncryptedQueryString args = new EncryptedQueryString(Request.QueryString["id"]);
                foreach (var arg in args)
                {

                    id = String.Format("{1}", arg.Key, HttpUtility.HtmlEncode(arg.Value));
                    if (string.IsNullOrEmpty(id))
                    {
                        return;
                    }
                }

                tmp = Convert.ToDecimal(id);

            }
            catch (Exception ex)
            {            
                Response.Write("Problema con el id: " + ex.Message);
                return;
            }
            //Session.Remove("IDARCHIVODIG");

            // Consumir servicio Web
            WSExpediente.Service ws = new WSExpediente.Service();

            List<WSExpediente.Etiquetas> etiquetas = new List<WSExpediente.Etiquetas>();

            // Declarar variables

            byte[] b = null;
            string Nombre = string.Empty;
            string Tipo = string.Empty;

            bool respuesta = false;

            try
           {
                WSExpediente.Carchivo archivo = ws.Obtenerxarchivo(tmp);

                Tipo = archivo.MIME;
                b = archivo.Contenido;
                Nombre = archivo.Nombre;

                respuesta = true;
            }
            catch (Exception ex)
            {
                Response.Write(tmp.ToString());
                Response.Write("La lectura ha fallado: " + ex.Message);

                respuesta = false;
            }

            if (respuesta == true)
            {
                // Mostrar archivo

                if (Tipo.StartsWith("application") == true)
                {
                    // Entregar archivo

                    Stream st = new MemoryStream(b);
                    long largo;
                    
                    largo = st.Length;            
                    Nombre = Nombre.Trim();
                    Nombre = Nombre.Replace(" ", "_");
                    Response.ContentType = Tipo;
                    //Response.ContentType = "text/plain"
                    //Or other you need
                    //
                    if (Tipo != "application/pdf")
                    {
                        Response.AddHeader("Content-Disposition", ("attachment; filename=" + Nombre));
                    }
                    
                   

                    while (largo > 0 && Response.IsClientConnected)
                    {
                        int lengthRead = st.Read(b, 0, Convert.ToInt32(largo));
                        Response.AddHeader("content-length", lengthRead.ToString());
                        Response.OutputStream.Write(b, 0, lengthRead);
                        Response.Flush();
                        largo = (largo - lengthRead);
                    }

                    Response.ContentType = Tipo;
                    Response.Flush();
                    Response.Close();
                    st.Close();

                    Response.End();
                }
                else
                {
                    // Mostrar imagen

                    Response.ContentType = Tipo;

                    Response.BinaryWrite(b);

                    Page.Title = Nombre;
                }

                Response.Write("La imagen fue mostrada exitosamente");
            }

        }
        catch (Exception ex)
        {
            Response.Clear();
            Response.Write("Ha ocurrido un error: " + ex.Message);
        }
    }
}