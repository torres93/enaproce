using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Services;

namespace enaproce
{
    /// <summary>
    /// Descripción breve de servicioweb
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class servicioweb : System.Web.Services.WebService
    {
        public static UtileriasSQL uti;

        [WebMethod]
        public string getFuentes(string modelo)
        {
            //La creacion del objeto de Utilerias que es para la base de datos 
            //La creacion del objeto de StringBuilder para concatenar los datos
            uti = new UtileriasSQL(int.Parse(modelo));
            //Se crea un objeto de SqlDataReader para tomar los datos del procedimiento
            SqlDataReader reader = uti.ExecuteReader("PR_OBTIENE_FUENTE ");
            //Se hace lectura la informacion desde la base de datos
            List<Fuente> pList = new List<Fuente>();
            while (reader.Read())
            {
                Fuente p = new Fuente();
                p.IdFuente = reader["ID_FUENTE"].ToString();
                p.Descripcion = reader["DESCRIPCION"].ToString();

                pList.Add(p);

            }


            string jsonString = JsonConvert.SerializeObject(pList);

            return jsonString;
        }
        [WebMethod]
        public string getActividades(string modelo,string fuente)
        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlDataReader reader = uti.ExecuteReader("PR_OBTIENE_ACT_DESGLOSE '" + fuente + "'");

                List<Actividad_M> pList = new List<Actividad_M>();
                int nivelMaximo = 1;
                while (reader.Read())
                {
                    Actividad_M p = new Actividad_M();
                    p.IdActividadPadre = reader["ID_ACTIVIDAD_PADRE"].ToString();
                    p.IdActividadCompuesta = reader["ID_ACTIVIDAD_COMPUESTA"].ToString();
                    p.IdActividad = reader["ID_ACTIVIDAD"].ToString();
                    p.Descripcion = reader["ACTIVIDAD"].ToString();
                    p.Desglose = reader["DESGLOSE"].ToString();
                    //p.Scian = reader["PRESENTACION"].ToString();
                    int count = p.IdActividadCompuesta.Length - p.IdActividadCompuesta.Replace("|", "").Length + 1;
                    p.Nivel = count.ToString();
                    if (count > nivelMaximo)
                    {
                        nivelMaximo = count - 1;
                    }
                    pList.Add(p);
                    pList[0].NivelMaximo = nivelMaximo.ToString();

                }
                string jsonString = JsonConvert.SerializeObject(pList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public string getEntidad(string modelo, string fuente)
        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlParameter[] paramcollection = new SqlParameter[1];
                paramcollection[0] = new SqlParameter("@ID_FUENTE", "1");
                SqlDataReader reader = uti.ExecuteReader(CommandType.StoredProcedure, "PR_OBTIENE_ENTIDAD", paramcollection);

                List<Entidad_M> pList = new List<Entidad_M>();
                int nivelMaximo = 1;
                while (reader.Read())
                {
                    Entidad_M p = new Entidad_M();
                    p.IdEntidad = reader["ID_ENTIDAD"].ToString();
                    p.Nombre = reader["NOM_ENT"].ToString();
                    pList.Add(p);

                }
                string jsonString = JsonConvert.SerializeObject(pList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public string getVariables(string modelo, string fuente)
        

        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlDataReader reader = uti.ExecuteReader("PR_OBTIENE_VAR_DESGLOSE '" + fuente + "'");

                List<Variable_M> pList = new List<Variable_M>();
                int nivelMaximo = 1;
                while (reader.Read())
                {
                    Variable_M p = new Variable_M();
                    int count = reader["ID_VARIABLE_COMPUESTA"].ToString().Length - reader["ID_VARIABLE_COMPUESTA"].ToString().Replace("|", "").Length + 1;

                    p.IdVariablePadre = reader["ID_VARIABLE_PADRE"].ToString();
                    p.IdVariableCompuesta = reader["ID_VARIABLE_COMPUESTA"].ToString();
                    p.IdVariable = reader["ID_VARIABLE"].ToString();
                    p.Descripcion = reader["VARIABLE"].ToString();
                    p.Tematica = reader["TEMATICA"].ToString();
                    p.Presenta = reader["PRESENTA"].ToString();
                    p.Desglose = reader["DESGLOSE"].ToString();
                    p.Nivel = count.ToString();
                    if (count > nivelMaximo)
                    {
                        nivelMaximo = count;
                    }

                    pList.Add(p);
                    pList[0].NivelMaximo = nivelMaximo.ToString();

                }

                string jsonString = JsonConvert.SerializeObject(pList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public string getAnios(string modelo, string fuente)
        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlParameter[] paramcollection = new SqlParameter[1];
                paramcollection[0] = new SqlParameter("@ID_FUENTE", fuente);
                SqlDataReader reader = uti.ExecuteReader(CommandType.StoredProcedure, "PR_OBTIENE_ANIO", paramcollection);

                List<anios> aniosList = new List<anios>();
                while (reader.Read())
                {
                    anios a = new anios();
                    a.Anio = reader["ANIO"].ToString();
                    a.Actual = reader["ACTUAL"].ToString();
                    aniosList.Add(a);

                }
                string jsonString = JsonConvert.SerializeObject(aniosList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public string getTipoDato(string modelo, string fuente, string variable)
        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlParameter[] paramcollection = new SqlParameter[2];
                paramcollection[0] = new SqlParameter("@ID_FUENTE", fuente);
                paramcollection[1] = new SqlParameter("@ID_VARIABLE_PADRE", variable);
                SqlDataReader reader = uti.ExecuteReader(CommandType.StoredProcedure, "PR_OBTIENE_FUE_VAR_TIPO_DATO", paramcollection);

                List<TipoDatos> TipoDatosList = new List<TipoDatos>();
                int nivelMaximo = 1;
                while (reader.Read())
                {
                    TipoDatos td = new TipoDatos();
                    td.Id_tipo_dato = reader["ID_TIPO_DATO"].ToString();
                    td.Desc = reader["DESCRIPCION"].ToString();
                    TipoDatosList.Add(td);

                }
                string jsonString = JsonConvert.SerializeObject(TipoDatosList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    
        [WebMethod]
        public string getTipoDato(string modelo, string fuente, string variable)
        {
            try
            {
                uti = new UtileriasSQL(int.Parse(modelo));
                SqlParameter[] paramcollection = new SqlParameter[2];
                paramcollection[0] = new SqlParameter("@ID_FUENTE", fuente);
                paramcollection[1] = new SqlParameter("@ID_VARIABLE_PADRE", variable);
                SqlDataReader reader = uti.ExecuteReader(CommandType.StoredProcedure, "PR_OBTIENE_FUE_VAR_TIPO_DATO", paramcollection);

                List<TipoDatos> TipoDatosList = new List<TipoDatos>();
                int nivelMaximo = 1;
                while (reader.Read())
                {
                    TipoDatos td = new TipoDatos();
                    td.Id_tipo_dato = reader["ID_TIPO_DATO"].ToString();
                    td.Desc = reader["DESCRIPCION"].ToString();
                    TipoDatosList.Add(td);

                }
                string jsonString = JsonConvert.SerializeObject(TipoDatosList);

                reader.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Entidad_M
    {
        public string IdEntidad { get; set; }
        public string Nombre { get; set; }
    }

    public class Actividad_M
    {
        public string IdActividadCompuesta { get; set; }
        public string IdActividadPadre { get; set; }
        public string IdActividad { get; set; }
        public string Descripcion { get; set; }
        public string Tematica { get; set; }
        public string Presenta { get; set; }
        public string Desglose { get; set; }
        public string Nivel { get; set; }
        public string NivelMaximo { get; set; }
        public string Scian { get; set; }
    }
    public class Variable_M
    {
        public string IdVariableCompuesta { get; set; }
        public string IdVariablePadre { get; set; }
        public string IdVariable { get; set; }
        public string Descripcion { get; set; }
        public string Tematica { get; set; }
        public string Presenta { get; set; }
        public string Desglose { get; set; }
        public string Nivel { get; set; }
        public string NivelMaximo { get; set; }
    }
}



public class Fuente
{
    public string IdFuente { get; set; }
    public string Descripcion { get; set; }
}
public class anios {
    public string Anio { get; set; }
    public string Actual { get; set; }
}
public class TipoDatos {
    public string Id_tipo_dato { set; get; }
    public string Desc { set; get; }
}