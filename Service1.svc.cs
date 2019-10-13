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

        /*MANTENIMIENTO CICLOS*/

        public int InsertarCiclo(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT INTO tb_ciclos(nombre,usuario_gra,fecha_gra,ultimo_usuario,fum)" +
                    "values(@nom,@usu_gra,@fecha_gra,@ult_us,@fum)";
                cm.Parameters.AddWithValue("@nom", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ult_us", ult_us);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.ExecuteNonQuery();
                res = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }
            return res;
        }

        public System.Data.DataSet BuscarCiclo(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_ciclos where id_ciclo = @cod";
                cm.Parameters.AddWithValue("@cod", codigo);


                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }


        public int ActualizarCiclo(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "UPDATE tb_ciclos set nombre=@nom,usuario_gra=@usu_gra,fecha_gra=@fecha_gra,ultimo_usuario=@ult_us,fum=@fum where id_ciclo=@id";
                cm.Parameters.AddWithValue("@id", id);
                cm.Parameters.AddWithValue("@nom", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ult_us", ult_us);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.ExecuteNonQuery();
                res = 1;

                con.cerrarCon();

            }
            catch (Exception e)
            {

            }


            return res;
        }

        public System.Data.DataSet MostrarCiclos()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_ciclos";

                SqlDataAdapter ad = new SqlDataAdapter(cm);
                ad.Fill(ds);

            }
            catch (Exception e)
            {

            }

            return ds;
        }


        public int EliminarCiclos(int codigo)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "DELETE from tb_ciclos where id_ciclo=@id";
                cm.Parameters.AddWithValue("@id", codigo);
                cm.ExecuteNonQuery();

                res = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return res;
        }
        /*MANTENIMIENTO CICLOS*/




        /* MANTENIMINETO GRADO*/
        public System.Data.DataSet ObtenerGrados()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select  CONCAT(gra.id_grado , '-' ,gra.nombre) as 'Grado', "+
                "CONCAT(ci.id_ciclo, '-', ci.nombre) as 'Ciclo', "+
                "gra.usuario_gra as 'Usuario grabacion', "+
                "gra.fecha_gra as 'Fecha grabacion', "+
                "gra.ultimo_usuario as 'Ultimo Usuario', "+
                "gra.fum as 'FUM', gra.estado as 'Estado' "+
                "from tb_grados gra "+
                "inner "+
                "join tb_ciclos ci "+
                "on gra.id_ciclo = ci.id_ciclo ";

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public string InsertarGrado(string nombre, int id_ciclo, string usu_gra, string estado)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_grados(nombre,id_ciclo,usuario_gra,estado)" +
                            "values (@nombre,@idCiclo,@usua_gra,@estado)";
                cm.Parameters.AddWithValue("@nombre", nombre);
                cm.Parameters.AddWithValue("@idCiclo", id_ciclo);
                cm.Parameters.AddWithValue("@usua_gra", usu_gra);
                cm.Parameters.AddWithValue("@estado", estado);
       
                res=cm.ExecuteNonQuery();

                con.cerrarCon();

                if (res == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                return "Ocurrio un error: "+e.Message.ToString();
            }

           
        }


        public string ActualizarGrado(int cod, string nombre, int id_ciclo, string ul_usu, DateTime fum,string estado)
        {
            int r = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "update tb_grados set nombre='"+nombre+"',id_ciclo="+id_ciclo+"," +
                    "ultimo_usuario='"+ul_usu+"',fum='"+fum+"',estado='"+estado+"' " +
                    "where id_grado="+cod;
                //cmd.Parameters.AddWithValue("@id", cod);
                //cmd.Parameters.AddWithValue("@nombre", nombre);
                //cmd.Parameters.AddWithValue("@idCiclo", id_ciclo);
                //cmd.Parameters.AddWithValue("@ul_usu", ul_usu);
                //cmd.Parameters.AddWithValue("@fum", fum);
                //cmd.Parameters.AddWithValue("@estado", estado);

                r =cmd.ExecuteNonQuery();

                con.cerrarCon();

                if (r == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception e)
            {
                return "Ocurrio un error: " + e.Message.ToString();

            }

      
        }


        public System.Data.DataSet BuscarGrado(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_grados where id_grado=@cod";
                cm.Parameters.AddWithValue("@cod", codigo);

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }

            return ds;
        }

        public int EliminarGrado(int codigo)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "DELETE from tb_grados where id_grado=@id";
                cm.Parameters.AddWithValue("@id", codigo);
                cm.ExecuteNonQuery();

                res = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return res;
        }

        /*    FIN MANTENIMENTO GRADOS    */



        /*MANTENIMIENTO SECCIONES*/
        public int InsertarSeccion(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_secciones (nombre,usuario_gra,fecha_gra,ultimo_usuario,fum)" +
                    "values (@nombre,@usu_gra,@fech_gra,@ul_usua,@fum)";
                cm.Parameters.AddWithValue("@nombre", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fech_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ul_usua", ult_us);
                cm.Parameters.AddWithValue("@fum", fum);

                cm.ExecuteNonQuery();
                res = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return res;
        }


        public System.Data.DataSet BuscarSeccion(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_secciones where id_seccion=@cod";
                cm.Parameters.AddWithValue("@cod", codigo);

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }


        public int ActualizarSeccion(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum)
        {
            int r = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "UPDATE tb_secciones set nombre=@nombre,usuario_gra=@usua_gra,fecha_gra=@fecha_gra,ultimo_usuario=@ul_usu,fum=@fum where id_seccion=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@usua_gra", usua_gra);
                cmd.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cmd.Parameters.AddWithValue("@ul_usu", ult_us);
                cmd.Parameters.AddWithValue("@fum", fum);

                cmd.ExecuteNonQuery();

                r = 1;
            }
            catch (Exception e)
            {

            }

            return r;
        }

        public System.Data.DataSet MostrarSecciones()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                SqlCommand cm = new SqlCommand("SELECT * from tb_secciones", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }

            return ds;
        }


        public int EliminarSeccion(int codigo)
        {
            int resp = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "DELETE from tb_secciones where id_seccion=@cod";
                cmd.Parameters.AddWithValue("@cod", codigo);

                cmd.ExecuteNonQuery();

                resp = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return resp;
        }
        /*FIN MANTENIMIENTO SECCIONES*/


        /*ENDPOINTS DE PARAMETRIZACION*/ //gmaldonado
        public System.Data.DataSet ListaParametros()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand("SELECT * from tb_parametros", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                SqlCommand cm = new SqlCommand("SELECT '"+e.Message.ToString()+"' as 'Error_Message'", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }

            return ds;
        }
        public System.Data.DataSet Busqueda_parametro_valor(string valor)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand("SELECT * from tb_parametros where parametro='"+valor+"'", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                SqlCommand cm = new SqlCommand("SELECT '" + e.Message.ToString() + "' as 'Error_Message'", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }

            return ds;
        }

        public string Actualizacion_parametro(string parametro, string valor)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "update tb_parametros " +
                    "set valor = '"+valor+"' where parametro = '"+parametro+"'";

                res = cm.ExecuteNonQuery();

                con.cerrarCon();

                if (res == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                return "Ocurrio un error: " + e.Message.ToString();
            }
        }//

        public System.Data.DataSet Get_Opciones(int id_perfil)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                string query = "select id_opcion,opcion,titulo,url,es_submenu, " +
                    "sub_menu,ultimo_usuario,fecha_gra,fum,icono, " +
                    "(select 'S' from tb_opciones_perfil pe where pe.id_opcion=op.id_opcion and pe.id_perfil='" + id_perfil + "') marca " +
                    "from tb_opciones op";
                SqlCommand cm = new SqlCommand(query, con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                SqlCommand cm = new SqlCommand("SELECT '" + e.Message.ToString() + "' as 'Error_Message'", con.abrirCon());
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }

            return ds;
        }

        public string prueba(string valor)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into test values ('"+valor+"')";

                res = cm.ExecuteNonQuery();

                con.cerrarCon();

                if (res == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                return "Ocurrio un error: " + e.Message.ToString();
            }
        }//

        public string eliminar_opciones_perfil(int id_perfil)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "delete from tb_opciones_perfil where id_perfil="+id_perfil;

                res = cm.ExecuteNonQuery();

                con.cerrarCon();

                if (res == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                return "Ocurrio un error: " + e.Message.ToString();
            }
        }
        public string Agregar_opciones_perfil(int id_perfil, int id_opcion)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_opciones_perfil values ("+id_perfil+","+id_opcion+")";

                res = cm.ExecuteNonQuery();

                con.cerrarCon();

                if (res == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                return "Ocurrio un error: " + e.Message.ToString();
            }
        }

        public System.Data.DataSet BuscarPerfil(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_perfil where id_perfil = @cod";
                cm.Parameters.AddWithValue("@cod", codigo);


                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        /*MANTENIMIENTO SECCIONES*/
        public int InsertarSeccion(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum, string estado)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_secciones (nombre,usuario_gra,fecha_gra,ultimo_usuario,fum,estado)" +
                    "values (@nombre,@usu_gra,@fech_gra,@ul_usua,@fum,@est)";
                cm.Parameters.AddWithValue("@nombre", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fech_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ul_usua", ult_us);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.Parameters.AddWithValue("@est", estado);

                cm.ExecuteNonQuery();
                res = 1;

                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return res;
        }


        


        public int ActualizarSeccion(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum, string estado)
        {
            int r = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "UPDATE tb_secciones set nombre=@nombre,usuario_gra=@usua_gra,fecha_gra=@fecha_gra,ultimo_usuario=@ul_usu,fum=@fum,estado=@est where id_seccion=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@usua_gra", usua_gra);
                cmd.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cmd.Parameters.AddWithValue("@ul_usu", ult_us);
                cmd.Parameters.AddWithValue("@fum", fum);
                cmd.Parameters.AddWithValue("@est", estado);

                cmd.ExecuteNonQuery();

                r = 1;
            }
            catch (Exception e)
            {

            }

            return r;
        }

       

       

        

    }
}
