using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;

namespace Wcf_SME
{
    
    public class Service1 : IService1
    {
       

        Clases.Conexion con = new Clases.Conexion();


        public System.Data.DataSet ListaOpcionesxPerfil(int id_perfil)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select p.id_perfil,p.nombre,op.id_opcion,op.opcion,op.titulo,op.url," +
                "op.es_submenu,op.sub_menu,op.icono " +
                "from  tb_opciones_perfil op_p " +
                "inner join tb_opciones op " +
                "on op.id_opcion=op_p.id_opcion " +
                "inner join tb_perfil p " +
                "on p.id_perfil=op_p.id_perfil " +
                "where op.es_submenu='S' and p.id_perfil=" + id_perfil;

            SqlDataAdapter ad = new SqlDataAdapter(query,con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;

            
        }

        public System.Data.DataSet ListaOpciones_sub_menu(string sub_menu)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select p.id_perfil,p.nombre,op.id_opcion,op.opcion,op.titulo,op.url,op.es_submenu,op.sub_menu,op.icono from  tb_opciones_perfil op_p inner join tb_opciones op on op.id_opcion=op_p.id_opcion inner join tb_perfil p on p.id_perfil=op_p.id_perfil where op.sub_menu='"+sub_menu+"'";

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;

            
        }

        public String Parametros(string parametro_config)
        {
            string texto;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select valor from tb_parametros where parametro='"+ parametro_config + "'";

            texto = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return texto;

            
        
        }

        public int login_cliente(string usuario, string clave)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select dbo.FT_SEGURITY_APP('"+usuario+"','"+clave+"')";
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public System.Data.DataSet Obtener_Genero()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select * from tb_genero";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet Catalogos_grados()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select * from tb_catalogo_grados";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet Lista_grados_institucion(int id_turno)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select  distinct(g.nombre), g.id_grado " +
                "from tb_grados g inner join tb_aula a on g.id_grado=a.id_grado " +
                "inner join tb_secciones s on s.id_seccion=a.id_seccion " +
                "inner join tb_turno t on t.id_turno=a.id_turno " +
                "where t.id_turno="+id_turno+" order by g.id_grado asc";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);

            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet Lista_grados_secciones_grados(int id_grado)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select s.id_seccion,s.nombre,a.id_aula from tb_aula a inner join tb_secciones s " +
                "on a.id_seccion=s.id_seccion where a.id_grado=" + id_grado+" order by s.nombre asc";
            SqlDataAdapter ad = new SqlDataAdapter(query,con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int capacidad_aula(int id_aula)
        {
            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select capacidad from tb_aula where id_aula="+id_aula;
            res= int.Parse(cmd.ExecuteScalar().ToString());
            con.cerrarCon();
            return res;
        }

        public System.Data.DataSet lista_turnos()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select id_turno,descripcion from tb_turno";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int perfil_usuario(string usuario)
        {
            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select id_perfil from tb_usuarios where usuario='"+usuario+"'";
            res = int.Parse(cmd.ExecuteScalar().ToString());
            con.cerrarCon();
            return res;
        }

        public System.Data.DataSet lista_usuarios()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select usu.id_usuario,usu.usuario,usu.activo,usu.id_perfil,p.nombre nombre_perfil " +
                "from tb_usuarios usu " +
                "inner join tb_perfil p " +
                "on usu.id_perfil=p.id_perfil";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet get_usuario_x_user(string usuario)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select usu.id_usuario,usu.usuario,usu.activo,usu.id_perfil,p.nombre nombre_perfil " +
                "from tb_usuarios usu " +
                "inner join tb_perfil p " +
                "on usu.id_perfil=p.id_perfil where usu.usuario='"+usuario+"'";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet get_pefiles()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select * from tb_perfil";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int editar_usuario(string usuario, int id_perfil, string estado)
        {
            int resp = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "update tb_usuarios set activo = '"+estado+"' , id_perfil ="+id_perfil+"  where usuario = '"+usuario+"'";
            resp = cmd.ExecuteNonQuery();
            con.cerrarCon();
            return resp;
        }

        public int actualizar_clave(string usuario, string clave_actual, string clave_nueva)
        {
            int res = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "DECLARE @RetValue INT " +
                "EXEC  @RetValue  = dbo.FT_CHANGE_PWD_APP @USUARIO = '"+usuario+"',@CREDENCIAL_ANTIGUA ='"+clave_actual+"' , @CREDENCIAL_NUEVA ='"+clave_nueva+"'; " +
                "SELECT @RetValue";
            res = cmd.ExecuteNonQuery();
            con.abrirCon();
            return res;
        }
    }
}
