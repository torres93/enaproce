using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class UtileriasSQL
{

    public static string ConnectionStringLocalTransaction;

    public UtileriasSQL(int idconexion)
    {
        ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["Conexion" + idconexion].ConnectionString;
    }

    public UtileriasSQL()
    {

        ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["conexionsql"].ConnectionString;

    }


    //      Hashtable to store cached parameters
    private Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

    public SqlConnection ObtieneCadenaConexion()
    {
        SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
        if (conn.State != ConnectionState.Open)
        {
            conn.Open();
        }
        return conn;
    }

    public SqlTransaction ObtieneTransaccion()
    {
        using (SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction))
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlTransaction _lsqlTransaction;
            _lsqlTransaction = conn.BeginTransaction();

            return _lsqlTransaction;
        }

    }

    public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
    }

    public int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

 

    //Con parametros agregado 10/11/2010
    public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction))
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
    }


    // <ExecuteNonQuery>
    // ExecuteNonQuery()
    // RECIBE PARAMETROS:SI
    // REGRESA VALOR    :SI
    // Descripcion: Ejecuta una consulta  en base al parametro strSql.
    // Regresa el numero de filas afectadas.
    // </summary>
    public int ExecuteNonQuery(string strSql)
    {
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction))
        {
            PrepareCommand(cmd, conn, null, CommandType.Text, strSql, null);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
    }

    public SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }
    public SqlDataReader ExecuteReader(string strSql, string idFuente)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            PrepareCommand(cmd, conn, null, CommandType.Text, strSql, null);
            cmd.Parameters.Add(
                new SqlParameter("@ID_FUENTE ", idFuente));
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }

    public SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch (Exception Ex)
        {
            conn.Close();
            throw;
        }
    }
    public SqlDataReader ExecuteReader(string strSql)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            PrepareCommand(cmd, conn, null, CommandType.Text, strSql, null);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }


    public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }


    // <RecuperaTabla>
    // RecuperaTabla()
    // RECIBE PARAMETROS:SI
    // REGRESA VALOR    :NO
    // Descripcion: Llena un objeto de tipo Datatable en base a la variable  cmdText.:
    // cmdText: Variable de tipo string que contiene la consulta que se va ejecutar en el servidor para cargar  el datatable.
    // </summary>
    public DataTable RecuperaTabla(string cmdText)
    {
        SqlConnection miConexion = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(cmdText, miConexion);
            adapter.Fill(ds);
            return ds.Tables[0];
        }       
        catch (Exception Ex)
        {
            return null;
        }
        finally
        {
            miConexion.Close();
            miConexion.Dispose();
        }

    }

    public DataTable RecuperaTabla(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            cmd.Parameters.Clear();
            conn.Close();
            return ds.Tables[0];
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }

    public DataTable RecuperaTabla(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            cmd.Parameters.Clear();
            conn.Close();
            return ds.Tables[0];
        }
        catch (Exception Ex)
        {

            conn.Close();
            throw;
        }
    }

    public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return (val == null) ? "0" : val;
        }
    }

    public object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
       

        using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
        {
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return (val == null) ? "0" : val;
        }
    }
    public object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        object val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }
    // <ExecuteScalar>
    // ExecuteScalar()
    // RECIBE PARAMETROS:SI
    // REGRESA VALOR    :SI
    // Descripcion: Ejecuta una consulta  en base al parametro valor.
    // Regresa la primera fila del registro encontrado por medio de  la variable valor, si no se encontro registro entonces regresa un "0".
    // </summary>
    public string ExecuteScalar(string valor)
    {
        object pos = null;
        SqlConnection miConexion = new SqlConnection(ConnectionStringLocalTransaction);
        try
        {
            SqlCommand miComando = new SqlCommand(valor, miConexion);
            miConexion.Open();
            valor = "0";
            if (miComando.ExecuteScalar() != null)
            {
                pos = miComando.ExecuteScalar();
                valor = pos.ToString();
            }
        }
        catch (Exception Ex)
        {

            valor = "0";
        }
        finally
        {
            miConexion.Close();
            miConexion.Dispose();
        }
        return valor;
    }

    public void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
    {
        parmCache[cacheKey] = commandParameters;
    }

    public SqlParameter[] GetCachedParameters(string cacheKey)
    {
        SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
        if (cachedParms == null)
        {
            return null;
        }
        SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];
        for (int i = 0, j = cachedParms.Length; i < j; i++)
        {
            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
        }
        return clonedParms;
    }

    private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
        {
            conn.Open();
        }
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
        {
            cmd.Transaction = trans;
        }
        
        cmd.CommandType = cmdType;
        if (cmdParms != null)
        {
            foreach (SqlParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
            }
        }
    }

}
