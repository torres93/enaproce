using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections;


public class Utilerias
   {
        public static UtileriasSQL uti = new UtileriasSQL();
      

        public Utilerias()
        {

        }

        public static bool ValidaSession()
        {

            try
            {
                if (HttpContext.Current.Session["CSUsuarioId"].ToString() != "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }


        public static string regresaSinCaracteresEspeciales(string lsCadena)
        {

            string x1;
            string x2;

            x1 = lsCadena.Replace("¥", "Ñ");
            x2 = x1.Replace("Ð", "Ñ");
            return x2;

        }


        public static DateTime FormateaFecha(object loFecha)
        {
            DateTime ldFecha;
            //ldFecha=Convert.ToDateTime("02/14/2005");

            DateTimeFormatInfo DTFI = new CultureInfo("en-US", true).DateTimeFormat;

            string s = CultureInfo.CurrentCulture.DisplayName.ToString();
            //String.Format("{0:d/M/yyyy HH:mm:ss}", loFecha); 
            try
            {
                ldFecha = Convert.ToDateTime(loFecha, DTFI);
            }
            catch
            {
                ldFecha = DateTime.MinValue;
            }
            return ldFecha;
        }


        public static bool DatoVacio(object loDato)
        {
            if ((loDato.ToString() == string.Empty) || (loDato.ToString().Length == 0) || (loDato.ToString() == null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isFecha(object loValor)
        {
            try
            {
                Convert.ToDateTime(loValor);
                return true;
            }
            catch (Exception Ex)
            {
                // ERROR.AgregarError(Ex, loValor.ToString());
                return false;
            }
        }

        public static bool isNumeric(object loExpresion)
        {
            try
            {
                Convert.ToDecimal(loExpresion);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public static bool isInteger(object loExpresion)
        {
            try
            {
                Convert.ToInt32(loExpresion);
                return true;
            }
            catch
            {

                return false;
            }
        }


        public static string ObtieneFechaSistema()
        {
            return System.DateTime.Today.ToString("dd/MM/yyyy");
        }

        public static string ObtieneFechaSistemaNombreArhivo()
        {
            return System.DateTime.Today.ToString("ddMMyyyy");
        }


        public static bool ValidarContenidoColumna(string lsContenido)
        {
            try
            {
                if ((lsContenido.Trim().ToLower() == string.Empty) || (lsContenido.Trim().Length == 0) || (lsContenido.Trim().ToLower() == null) || (lsContenido.Trim().ToLower() == "0"))
                    return true;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                // ERROR.AgregarError(Ex, "");
                return true;
            }

        }

        public static string CompletaCadena(string lsCadena, int liLongitudComplemento)
        {
            int liLongitud = 0;
            string lsCadenaResult = string.Empty;
            try
            {

                liLongitud = lsCadena.Trim().Length;
                if (liLongitudComplemento == liLongitud)
                {
                    return lsCadena.Trim();
                }
                else
                {

                    for (int c = 0; c < liLongitudComplemento - liLongitud; c++)
                    {
                        lsCadenaResult = lsCadenaResult + "0";
                    }
                }
                return lsCadenaResult + lsCadena.Trim();
            }

            catch
            {
                return lsCadena.Trim();
            }
        }

        public static string obtieneFechaEncabezado()
        {
            string[] mes = new string[12] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            string[] dia = new string[7] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            return "Hoy es " + dia[(int)(DateTime.Now.DayOfWeek)] + ", " + DateTime.Now.Day.ToString() + " de " + mes[DateTime.Now.Month - 1] + " de " + DateTime.Now.Year.ToString() + ".";
        }

        public static int FormateaDatoEntero(string lsDato)
        {
            try
            {
                if (ValidarContenidoColumna(lsDato))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(lsDato);
                }
            }
            catch
            {
                return 0;
            }
        }

        public static string ObtieneHoraSistema()
        {
            string lsHora = string.Empty;
            string lsMinutos = string.Empty;

            lsHora = System.DateTime.Now.Hour.ToString().Trim();
            lsMinutos = System.DateTime.Now.Minute.ToString().Trim();

            try
            {
                if (Convert.ToInt32(lsMinutos) < 10)
                {
                    lsMinutos = "0" + System.DateTime.Now.Minute.ToString().Trim();
                }
                if (Convert.ToInt32(lsHora) < 10)
                {
                    lsHora = "0" + System.DateTime.Now.Hour.ToString().Trim();
                }
                return lsHora + ":" + lsMinutos;
            }
            catch (Exception Ex)
            {
                //  ERROR.AgregarError(Ex, "");
                return string.Empty;
            }
        }

        public static bool LlenaComboboxCategoria(DropDownList ddlControl, string lsQuery, string lsMember, string lstrDisplay)
        {

            DataTable dslControl = new DataTable();

            try
            {
                dslControl = uti.RecuperaTabla(lsQuery);
                DataRow _datRowRenglonPrimerElemento = dslControl.NewRow();
                _datRowRenglonPrimerElemento[lsMember] = -1;
                _datRowRenglonPrimerElemento[lstrDisplay] = "Todas";
                dslControl.Rows.InsertAt(_datRowRenglonPrimerElemento, 0);
                ddlControl.DataSource = dslControl;
                ddlControl.DataValueField = lsMember;
                ddlControl.DataTextField = lstrDisplay;
                ddlControl.DataBind();
                return true;
            }
            catch (Exception Ex)
            {
                //ERROR.AgregarError(Ex, "");
                return false;
            }
        }

        public static bool LlenaCombobox(DropDownList ddlControl, string lsQuery, string lsMember, string lstrDisplay)
        {

            DataTable dslControl = new DataTable(); //
            DataSet ds = new DataSet();

            try
            {
                dslControl = uti.RecuperaTabla(lsQuery);
                DataRow _datRowRenglonPrimerElemento = dslControl.NewRow();
                _datRowRenglonPrimerElemento[lsMember] = -1;
                _datRowRenglonPrimerElemento[lstrDisplay] = "Seleccione";
                dslControl.Rows.InsertAt(_datRowRenglonPrimerElemento, 0);
                ddlControl.DataSource = dslControl;
                ddlControl.DataValueField = lsMember;
                ddlControl.DataTextField = lstrDisplay;
                ddlControl.DataBind();
                return true;
            }
            catch (Exception Ex)
            {
                //ERROR.AgregarError(Ex, "");
                return false;
            }
        }
    
        public static bool LlenaListBox(ListBox lsControl, string lsQuery, string lsMember, string lstrDisplay)
        {

            DataTable dslControl = new DataTable();

            try
            {
                dslControl = uti.RecuperaTabla(lsQuery);
                DataRow _datRowRenglonPrimerElemento = dslControl.NewRow();
                dslControl.Rows.InsertAt(_datRowRenglonPrimerElemento, 0);
                lsControl.DataSource = dslControl;
                lsControl.DataValueField = lsMember;
                lsControl.DataTextField = lstrDisplay;
                lsControl.DataBind();
                return true;
            }
            catch (Exception Ex)
            {
                //ERROR.AgregarError(Ex, "");
                return false;
            }
        }
   }
