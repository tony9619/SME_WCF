using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Wcf_SME.Clases
{
    public class Conexion
    {
        SqlConnection con;
        public Conexion()
        {
            try
            {
                
                con = new SqlConnection("Data Source=.;Initial Catalog=sme_db; Integrated Security=true");
            }
            catch (Exception e)
            {

            }
        }
        public SqlConnection abrirCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }
        public SqlConnection cerrarCon()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return con;
        }
    }
}