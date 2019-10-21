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

        // ***************** CLASE CONEXION *************************//

        Clases.Conexion con = new Clases.Conexion();

        // *************************************************************

        /*
        ***********************************
        * CATEGORIAS
        * NP00 - UTILIDADES
        * NP01 - OPCIONES
        * NP02 - CONFIGURACION
        * NP03 - USUARIOS
        * NP04 - GRADOS
        * NP05 - TURNOS
        * NP06 - PERFILES
        * NP07 - CICLOS
        * NP08 - SECCIONES
        * NP09 - MATERIAS
        * *********************************

        */

        // NP00 ******************* UTILIDADES *******************************************//
        public System.Data.DataSet lista_deptos()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select * from tb_departamento";

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet lista_municipiosx_depto(int id_depto)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select * from municipio where id_depto=" + id_depto;

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        // NP01 ********************** ENPOINT DE OPCIONES *********************************//
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

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;


        }

        public System.Data.DataSet ListaOpciones_sub_menu(string sub_menu)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select p.id_perfil,p.nombre,op.id_opcion,op.opcion,op.titulo,op.url,op.es_submenu,op.sub_menu,op.icono from  tb_opciones_perfil op_p inner join tb_opciones op on op.id_opcion=op_p.id_opcion inner join tb_perfil p on p.id_perfil=op_p.id_perfil where op.sub_menu='" + sub_menu + "'";

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;


        }

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

        public string Agregar_opciones_perfil(int id_perfil, int id_opcion)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_opciones_perfil values (" + id_perfil + "," + id_opcion + ")";

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

        public string eliminar_opciones_perfil(int id_perfil)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "delete from tb_opciones_perfil where id_perfil=" + id_perfil;

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

        public System.Data.DataSet ObtenerIconoOpcion(string page)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                string query = "select titulo,icono from tb_opciones where url like '%" + page + "%'";
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

        //***************************** FIN DE ENPOINTS SE OPCIONES *************************//

        /// NP02 ****************ENPOINTS DE CONFIGURACION *************************************//

        public String Parametros(string parametro_config)
        {
            string texto;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select valor from tb_parametros where parametro='" + parametro_config + "'";

            texto = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return texto;



        }

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
                SqlCommand cm = new SqlCommand("SELECT '" + e.Message.ToString() + "' as 'Error_Message'", con.abrirCon());
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
                SqlCommand cm = new SqlCommand("SELECT * from tb_parametros where parametro='" + valor + "'", con.abrirCon());
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
                    "set valor = '" + valor + "' where parametro = '" + parametro + "'";

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

        // ****************** FIN DE ENPOINTS DE CONFIGURACION *********************************************************//

        // NP03 ************** ENPOINTS DE USUARIOS *****************************************//

        public int login_cliente(string usuario, string clave)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select dbo.FT_SEGURITY_APP('" + usuario + "','" + clave + "')";
            return int.Parse(cmd.ExecuteScalar().ToString());
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
                "on usu.id_perfil=p.id_perfil where usu.usuario='" + usuario + "'";
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
            cmd.CommandText = "update tb_usuarios set activo = '" + estado + "' , id_perfil =" + id_perfil + "  where usuario = '" + usuario + "'";
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
                "EXEC  @RetValue  = dbo.FT_CHANGE_PWD_APP @USUARIO = '" + usuario + "',@CREDENCIAL_ANTIGUA ='" + clave_actual + "' , @CREDENCIAL_NUEVA ='" + clave_nueva + "'; " +
                "SELECT @RetValue";
            res = cmd.ExecuteNonQuery();
            con.abrirCon();
            return res;
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

        //*************** FIN ENDPOINTS DE USUARIOS ***********************************//


        /// NP04 ************* ENPOINTS DE GRADOS *****************************************//

        public System.Data.DataSet Catalogos_grados()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select * from tb_catalogo_grados order by correlativo asc";
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
                "where t.id_turno=" + id_turno + " order by g.id_grado asc";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);

            con.cerrarCon();
            return ds;
        }

        public System.Data.DataSet Lista_grados_secciones_grados(int id_grado)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select s.id_seccion,s.nombre,a.id_aula from tb_aula a inner join tb_secciones s " +
                "on a.id_seccion=s.id_seccion where a.id_grado=" + id_grado + " order by s.nombre asc";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int capacidad_aula(int id_aula)
        {
            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select capacidad from tb_aula where id_aula=" + id_aula;
            res = int.Parse(cmd.ExecuteScalar().ToString());
            con.cerrarCon();
            return res;
        }

        public int obtener_Aula(int id_grado, int id_seccion, int id_turno)
        {

            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select * from tb_aula where id_grado = "+id_grado+" and id_seccion = "+id_seccion+" and id_turno = "+id_turno+"";
            res = int.Parse(cmd.ExecuteScalar().ToString());
            con.cerrarCon();
            return res;
        }

        public System.Data.DataSet ObtenerGrados()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select  CONCAT(gra.id_grado , '-' ,gra.nombre) as 'Grado', " +
                "CONCAT(ci.id_ciclo, '-', ci.nombre) as 'Ciclo', " +
                "gra.usuario_gra as 'Usuario grabacion', " +
                "gra.fecha_gra as 'Fecha grabacion', " +
                "gra.ultimo_usuario as 'Ultimo Usuario', " +
                "gra.fum as 'FUM', gra.estado as 'Estado' " +
                "from tb_grados gra " +
                "inner " +
                "join tb_ciclos ci " +
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
                cm.CommandText = "INSERT into tb_grados(nombre,id_ciclo,usuario_gra,fecha_gra,estado)" +
                            "values (UPPER('" + nombre + "')," + id_ciclo + ",UPPER('" + usu_gra + "'),SYSDATETIME(),UPPER('" + estado + "'))";


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


        public string ActualizarGrado(int cod, string nombre, int id_ciclo, string ul_usu, string fum, string estado)
        {
            int r = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "update tb_grados set nombre=UPPER('" + nombre + "'),id_ciclo=" + id_ciclo + "," +
                    "ultimo_usuario='" + ul_usu + "',fum=SYSDATETIME(),estado=UPPER('" + estado + "') " +
                    "where id_grado=" + cod;


                r = cmd.ExecuteNonQuery();

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

        //************************ FIN ENDPOINTS DE GRADOS *******************************//

        // NP05 *********************** ENPOINTS DE TURNOS ************************************//

        public System.Data.DataSet lista_turnos()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select id_turno,descripcion from tb_turno";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int InsertarTurno(string descripcion, string hora_ini, string hora_fin, string usua_gra, DateTime fecha, string ul_usu, DateTime fum, string estado)
        {
            int resp = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "INSERT into tb_turno (descripcion,hora_inicio,hora_fin,usuario_gra,fecha_gra,ultimo_usuario,fum,estado)" +
                                    "values(@des,@hi,@hf,@usu_gra,@fecha_gra,@ult_usua,@fum,@est)";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@des", descripcion);
                cm.Parameters.AddWithValue("@hi", hora_ini);
                cm.Parameters.AddWithValue("@hf", hora_fin);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha);
                cm.Parameters.AddWithValue("@ult_usua", ul_usu);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.Parameters.AddWithValue("@est", estado);

                cm.ExecuteNonQuery();
                resp = 1;
                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return resp;
        }

        public int ActualizarTurno(int cod, string descripcion, string hora_ini, string hora_fin, string usua_gra, DateTime fecha, string ul_usu, DateTime fum, string estado)
        {
            int resp = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "UPDATE tb_turno set descripcion=@des,hora_inicio=@hi,hora_fin=@hf,usuario_gra=@usu_gra,fecha_gra=@fecha_gra," +
                            "ultimo_usuario=@ult_usua,fum=@fum,estado=@est where id_turno=@cod";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@cod", cod);
                cm.Parameters.AddWithValue("@des", descripcion);
                cm.Parameters.AddWithValue("@hi", hora_ini);
                cm.Parameters.AddWithValue("@hf", hora_fin);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha);
                cm.Parameters.AddWithValue("@ult_usua", ul_usu);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.Parameters.AddWithValue("@est", estado);

                cm.ExecuteNonQuery();
                resp = 1;
                con.cerrarCon();
            }
            catch (Exception e)
            {

            }

            return resp;
        }


        public System.Data.DataSet ListarTurno()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select id_turno Codigo, descripcion Descripcion, hora_inicio [Inicia], hora_fin [Finaliza],fum [Ultima Modificacion],estado Estado from tb_turno";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con.abrirCon());
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public System.Data.DataSet BuscarTurno(int cod)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "SELECT * from tb_turno where id_turno = @cod";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@cod", cod);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        // *********************** FIN DE ENPOINTS DE TURNOS *****************************//

        // NP06 *********************** ENPOINTS DE PERFILES ************************************//

        public int perfil_usuario(string usuario)
        {
            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select id_perfil from tb_usuarios where usuario='" + usuario + "'";
            res = int.Parse(cmd.ExecuteScalar().ToString());
            con.cerrarCon();
            return res;
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

        // *********************** FIN ENPOINTS DE PERFILES ************************************//

        // NP07 ***********************  ENPOINTS DE CICLOS ************************************//

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

        public System.Data.DataSet ListarCiclos()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select id_ciclo Codifo, nombre Nombre, fum [Ultima Modificacion] from tb_ciclos";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con.abrirCon());
                da.Fill(ds);
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

        // *********************** FIN ENPOINTS DE CICLOS ************************************//

        // NP08 ***********************  ENPOINTS DE SECCIONES ************************************//

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

        //************************* FIN ENDPOINTS DE SECCIONES ******************************************//

        //NP09 ************************ ENDPOINTS DE MATERIAS *************************************************//

        public string InsertarMateria(string nombre, string usua_gra, DateTime fecha_gra)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT INTO tb_materia(nombre,usuario_gra,fecha_gra)" +
                    "values(@nom,@usu_gra,@fecha_gra)";
                cm.Parameters.AddWithValue("@nom", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                res = cm.ExecuteNonQuery();

                con.cerrarCon();

                return "1";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }

        public System.Data.DataSet BuscarMateria(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_materia where id_materia = @cod";
                cm.Parameters.AddWithValue("@cod", codigo);


                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }


        public int ActualizarMateria(int id, string nombre, string ult_us, DateTime fum)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "UPDATE tb_materia set nombre=@nom,ultimo_usuario=@ult_us,fum=@fum where id_materia=@id";
                cm.Parameters.AddWithValue("@id", id);
                cm.Parameters.AddWithValue("@nom", nombre);
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

        public System.Data.DataSet MostrarMaterias()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "SELECT * from tb_materia";

                SqlDataAdapter ad = new SqlDataAdapter(cm);
                ad.Fill(ds);

            }
            catch (Exception e)
            {

            }

            return ds;
        }


        public int EliminarMaterias(int codigo)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "DELETE from tb_materia where id_materia=@id";
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


        // ************************************METODO DE MATRICULACION 1A. VEZ*****************************************************************//

        public string matricula_nuevo_ingreso(

            string rep1,string rep2 ,string rep3,

            /******************* alumno INFO *********************************************/
            string alum_n1, string alum_n2, string alum_gen, string alum_ape1, string alum_ape2,
            string alumn_mail, string alum_alergia, string alum_sangre, string alum_enfermo, string alum_enfermedad, string alum_fecha_n,
            int id_ult_grado, int ult_anio_alum, string alum_ult_inst, string alum_ult_status,string isss,

            /****************************REPRESENTANTE 1 INFO********************************************/
            string rep1_name, string rep1_ape, string rep1_tel1, string rep1_tel2, string rep1_dui, string rep1_mail, string rep1_vive_alum,
            int rep1_depto, int rep1_mun, string rep1_domicilio, string rep1_trabaja, string rep1_parentesto, string rep1_empleo, string rep1_cargo, string rep1_telefono3, string rep1_dir_lab,
            string otro_parentesco_rep1,

              ///****************************REPRESENTANTE 2 INFO********************************************/
              string rep2_name, string rep2_ape, string rep2_tel1, string rep2_tel2, string rep2_dui, string rep2_mail, string rep2_vive_alum,
            int rep2_depto, int rep2_mun, string rep2_domicilio, string rep2_trabaja, string rep2_parentesto, string rep2_empleo, string rep2_cargo, string rep2_telefono3, string rep2_dir_lab,
            string otro_parentesco_rep2,

             ///**************************** REPRESENTANTE 3 INFO ********************************************/

             string rep3_name, string rep3_ape, string rep3_tel1, string rep3_tel2, string rep3_dui, string rep3_mail, string rep3_vive_alum,
            int rep3_depto, int rep3_mun, string rep3_domicilio, string rep3_trabaja, string rep3_parentesto, string rep3_empleo, string rep3_cargo, string rep3_telefono3, string rep3_dir_lab,
            string otro_parentesco_rep3,

             ///************ INFO DE MATRICULA ***********************************/
             ///
             int id_aula, int id_turno, int id_grado, int id_seccion,
            string usuario_operacion

            ){


            int res_alum = 0;
            try
            {
                /*INGRESANDO LA INFORMACION DEL ALUMNO A TABLA ALUMNO*/

                SqlCommand cm_alumno = new SqlCommand();
                cm_alumno.Connection = con.abrirCon();
                cm_alumno.CommandText = "insert into tb_alumno (nombre1,nombre2,genero,apellido1,apellido2,email,alergia,tipo_sangre," +
                    "padece_enfermedad,enfermedad,fecha_nacimiento,usuario_gra,fecha_gra,isss) " +
                    "values('" + alum_n1 + "','" + alum_n2 + "','" + alum_gen + "','" + alum_ape1 + "','" + alum_ape2 + "'," +
                    "'" + alumn_mail + "','" + alum_alergia + "','" + alum_sangre + "','" + alum_enfermo + "','" + alum_enfermedad + "','" + alum_fecha_n + "','" + usuario_operacion + "'" +
                    ",SYSDATETIME(),'"+isss+"')";
                res_alum= cm_alumno.ExecuteNonQuery();

                

                string carnet_alumno = ""; // OBTENIEDNO EL CARNET DEL ALUMNO INSERTADO
                SqlCommand cmd_carnet_alumno = new SqlCommand();
                cmd_carnet_alumno.Connection = con.abrirCon();
                cmd_carnet_alumno.CommandText = "SELECT carnet FROM tb_alumno where id_alumno = (select max(id_alumno) from tb_alumno)";
                carnet_alumno = cmd_carnet_alumno.ExecuteScalar().ToString();

                /*INGRESANDO ESCOLARIDAD DEL ALUMNO*/

                SqlCommand cmd_historico_grados = new SqlCommand();
                cmd_historico_grados.Connection = con.abrirCon();
                cmd_historico_grados.CommandText = "insert into tb_historico_grados_alumno (id_grado_catalogo,anio,institucion,estado,fecha_gra,usuario_gra,alumno) " +
                    "values(" + id_ult_grado + ",'" + ult_anio_alum + "','" + alum_ult_inst + "','" + alum_ult_status + "',SYSDATETIME(),'" + usuario_operacion + "','" + carnet_alumno + "')";
                
                cmd_historico_grados.ExecuteNonQuery();

                if (rep1 != "")
                {
                    // SI ESPECIFICO UN REPRESENTANTE INSERTA A TABLA REPRESENTANTE

                    SqlCommand cmd_rep1 = new SqlCommand();
                    cmd_rep1.Connection = con.abrirCon();
                    cmd_rep1.CommandText = "insert into tb_representante(nombres,apellidos,telefono1,telefono2,dui,email,fecha_nacimiento," +
                        "cod_depto,cod_municipio,domicilio,trabaja,empresa,cargo,telefono3,direccion_empresa,usuario_gra,fecha_gra) " +
                        "values('"+rep1_name+"','"+rep1_ape+"','"+rep1_tel1+"','"+rep1_tel2+"','"+rep1_dui+"','"+rep1_mail+"','"+alum_fecha_n+"'," +
                        ""+rep1_depto+","+rep1_mun+",'"+rep1_domicilio+"','"+rep1_trabaja+"','"+rep1_empleo+"','"+rep1_cargo+"'," +
                        "'"+rep1_telefono3+"','"+rep1_dir_lab+"','dbo',SYSDATETIME())";

                    cmd_rep1.ExecuteNonQuery();

                    // INSERTA A TABLA ALUMNO REPRESENTANTE
                    string dui_rep1 = "";
                    SqlCommand cmd_dui_rep1 = new SqlCommand();
                    cmd_dui_rep1.Connection = con.abrirCon();
                    cmd_dui_rep1.CommandText = "SELECT dui FROM tb_representante where id_representante = (select max(id_representante) from tb_representante)";
                    dui_rep1 = cmd_dui_rep1.ExecuteScalar().ToString();

                    con.cerrarCon();

                    SqlCommand cmd_rep1_alum = new SqlCommand();
                    cmd_rep1_alum.Connection = con.abrirCon();
                    cmd_rep1_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario) " +
                        "values('"+carnet_alumno+"','"+dui_rep1+"','"+rep1_vive_alum+"','"+rep1+"','"+ otro_parentesco_rep1 + "',SYSDATETIME(),'"+usuario_operacion+"')";
                    cmd_rep1_alum.ExecuteNonQuery();

                }// fin rep1

                // MISMA LOGICA PARA REP 2 Y REP3
                if (rep2 != "0")
                {

                    SqlCommand cmd_rep2 = new SqlCommand();
                    cmd_rep2.Connection = con.abrirCon();
                    cmd_rep2.CommandText = "insert into tb_representante(nombres,apellidos,telefono1,telefono2,dui,email,fecha_nacimiento," +
                        "cod_depto,cod_municipio,domicilio,trabaja,empresa,cargo,telefono3,direccion_empresa,usuario_gra,fecha_gra) " +
                        "values('" + rep2_name + "','" + rep2_ape + "','" + rep2_tel1 + "','" + rep2_tel2 + "','" + rep2_dui + "','" + rep2_mail + "','" + alum_fecha_n + "'," +
                        ""+ rep2_depto + "," + rep2_mun + ",'" + rep2_domicilio + "','" + rep2_trabaja + "','" + rep2_empleo + "','" + rep2_cargo + "'," +
                        "'" + rep2_telefono3 + "','" + rep2_dir_lab + "','"+usuario_operacion+"',SYSDATETIME())";

                    cmd_rep2.ExecuteNonQuery();

                    string dui_rep2 = "";
                    SqlCommand cmd_dui_rep2 = new SqlCommand();
                    cmd_dui_rep2.Connection = con.abrirCon();
                    cmd_dui_rep2.CommandText = "SELECT dui FROM tb_representante where id_representante = (select max(id_representante) from tb_representante)";
                    dui_rep2 = cmd_dui_rep2.ExecuteScalar().ToString();

                    con.cerrarCon();

                    SqlCommand cmd_rep2_alum = new SqlCommand();
                    cmd_rep2_alum.Connection = con.abrirCon();
                    cmd_rep2_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario) " +
                        "values('" + carnet_alumno + "','" + dui_rep2 + "','" + rep2_vive_alum + "','"+rep2+"','"+ otro_parentesco_rep2 + "',SYSDATETIME(),'"+usuario_operacion+"')";
                    cmd_rep2_alum.ExecuteNonQuery();

                }// fin REP2

                if (rep3 != "0")
                {


                    SqlCommand cmd_rep3 = new SqlCommand();
                    cmd_rep3.Connection = con.abrirCon();
                    cmd_rep3.CommandText = "insert into tb_representante(nombres,apellidos,telefono1,telefono2,dui,email,fecha_nacimiento," +
                        "cod_depto,cod_municipio,domicilio,trabaja,empresa,cargo,telefono3,direccion_empresa,usuario_gra,fecha_gra) " +
                        "values('" + rep3_name + "','" + rep3_ape + "','" + rep3_tel1 + "','" + rep3_tel2 + "','" + rep3_dui + "','" + rep3_mail + "','" + alum_fecha_n + "'," +
                        "" + rep3_depto + "," + rep3_mun + ",'" + rep3_domicilio + "','" + rep3_trabaja + "','" + rep3_empleo + "','" + rep3_cargo + "'," +
                        "'" + rep3_telefono3 + "','" + rep3_dir_lab + "','"+usuario_operacion+"',SYSDATETIME())";

                    cmd_rep3.ExecuteNonQuery();

                    string dui_rep3 = "";
                    SqlCommand cmd_dui_rep3 = new SqlCommand();
                    cmd_dui_rep3.Connection = con.abrirCon();
                    cmd_dui_rep3.CommandText = "SELECT dui FROM tb_representante where id_representante = (select max(id_representante) from tb_representante)";
                    dui_rep3 = cmd_dui_rep3.ExecuteScalar().ToString();

                    con.cerrarCon();

                    SqlCommand cmd_rep2_alum = new SqlCommand();
                    cmd_rep2_alum.Connection = con.abrirCon();
                    cmd_rep2_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario) " +
                        "values('" + carnet_alumno + "','" + dui_rep3 + "','" + rep3_vive_alum + "','" + rep3 + "','" + otro_parentesco_rep3 + "',SYSDATETIME(),'"+usuario_operacion+"')";
                    cmd_rep2_alum.ExecuteNonQuery();

                }// fin REP3

                // OBTENIENDO EL ANIO ESCOLAR VIGENTE E INSERTANDO LAS MATRICULAS 
                int anio_vigente = 0;
                SqlCommand cmd_anio_vigente = new SqlCommand();
                cmd_anio_vigente.Connection = con.abrirCon();
                cmd_anio_vigente.CommandText = "select id_anio from tb_anio_escolar where activo='S'";
                anio_vigente = int.Parse(cmd_anio_vigente.ExecuteScalar().ToString());

                SqlCommand cmd_alum_matricula = new SqlCommand();
                cmd_alum_matricula.Connection = con.abrirCon();
                cmd_alum_matricula.CommandText = "insert into tb_matricula (alumno,id_aula,usuario_gra,fecha_gra,anio,id_turno,id_grado,id_seccion) " +
                    "values('"+carnet_alumno+"',"+id_aula+",'"+usuario_operacion+"',SYSDATETIME(),"+ anio_vigente + ","+id_turno+","+id_grado+","+id_seccion+")";
                cmd_alum_matricula.ExecuteNonQuery();

               
                // SI TODO LO INSERTO RETORNE 1
                return "1";
            }
            catch (Exception e)
            {
                // SI NO EXCEPSION
                return e.Message.ToString();
            }

          
        }

        //************************************** FIN METODO DE MATRICULACION 1A. VEZ *********************************************

        /*MATERIAS CICLO*/

        public System.Data.DataSet ListarMaterias()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select * from tb_materia";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }




        public System.Data.DataSet Get_Materias(int id_materia)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                string query = "select id_materia,nombre,usuario_gra,fecha_gra,ultimo_usuario,fum," +
                    "(select 'S' from tb_materia_ciclo mc where mc.id_materia=mt.id_materia and mc.id_ciclo='" + id_materia + "') marca " +
                    "from tb_materia mt";
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





        public string eliminar_materias_ciclo(int id_ciclo)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "delete from tb_materia_ciclo where id_ciclo=" + id_ciclo;

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


        public string Agregar_materia_ciclo(int id_ciclo, int id_materia)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_materia_ciclo values (" + id_ciclo + "," + id_materia + ")";

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

        public System.Data.DataSet MostrarMateria_Ciclo()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select tc.nombre Ciclo,tm.nombre Materia from tb_ciclos tc inner join " +
                    "tb_materia_ciclo tmc on tc.id_ciclo=tmc.id_ciclo inner join " +
                    "tb_materia tm on tm.id_materia = tmc.id_materia";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;

        }
        /*MATERIAS CICLO*/


        /*DOCENTES*/

        public string InsertarDocente(string nombre1, string nombre2, string ape1, string ape2, string casada, string genero, string estado_civil, DateTime fecha_nac, string tel1, string tel2, string domicilio, int id_dire, string titulo, string especialidad, string usu_gra, DateTime fecha_gra, string ult_usua, DateTime fum, string estado)
        {

            string res = "";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_docente (nombre1,nombre2,apellido1,apellido2,casada,genero,estado_civil,fecha_nacimiento,telefono1,telefono2,domicilio,id_direccion,titulo,especialidad,usuario_gra,fecha_gra,ultimo_usuario,fum,estado)" +
                                                 "values (@nombre1,@nombre2,@ape1,@ape2,@casada,@genero,@estado_civil,@fecha_nac,@telefono1,@telefono2,@domicilio,@id_direccion,@titulo,@especialidad,@usuario_gra,@fecha_gra,@ult_usua,@fum,@estado)";
                cm.Parameters.AddWithValue("@nombre1", nombre1);
                cm.Parameters.AddWithValue("@nombre2", nombre2);
                cm.Parameters.AddWithValue("@ape1", ape1);
                cm.Parameters.AddWithValue("@ape2", ape2);
                cm.Parameters.AddWithValue("@casada", casada);
                cm.Parameters.AddWithValue("@genero", genero);
                cm.Parameters.AddWithValue("@estado_civil", estado_civil);
                cm.Parameters.AddWithValue("@fecha_nac", fecha_nac);
                cm.Parameters.AddWithValue("@telefono1", tel1);
                cm.Parameters.AddWithValue("@telefono2", tel2);
                cm.Parameters.AddWithValue("@domicilio", domicilio);
                cm.Parameters.AddWithValue("@id_direccion", id_dire);
                cm.Parameters.AddWithValue("@titulo", titulo);
                cm.Parameters.AddWithValue("@especialidad", especialidad);
                cm.Parameters.AddWithValue("@usuario_gra", usu_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ult_usua", ult_usua);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.Parameters.AddWithValue("@estado", estado);

                cm.ExecuteNonQuery();
                res = "Insertado";
                con.cerrarCon();
            }
            catch (Exception e)
            {
                res = "Error " + e.Message.ToString();
            }

            return res;
        }


        public string ActualizarDocente(int codigo, string nombre1, string nombre2, string ape1, string ape2, string casada, string genero, string estado_civil, DateTime fecha_nac, string tel1, string tel2, string domicilio, int id_dire, string titulo, string especialidad, DateTime fecha_gra, string ult_usua, DateTime fum, string estado)
        {
            string res = "";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "UPDATE tb_docente set nombre1=@nombre1,nombre2=@nombre2,apellido1=@ape1,apellido2=@ape2,casada=@casada,genero=@genero,estado_civil=@estado_civil,fecha_nacimiento=@fecha_nac," +
                    "telefono1=@telefono1,telefono2=@telefono2,domicilio=@domicilio,id_direccion=@id_direccion,titulo=@titulo,especialidad=@especialidad,fecha_gra=@fecha_gra,ultimo_usuario=@ult_usua,fum=@fum,estado=@estado where id_docente=@codigo";
                cm.Parameters.AddWithValue("@codigo", codigo);
                cm.Parameters.AddWithValue("@nombre1", nombre1);
                cm.Parameters.AddWithValue("@nombre2", nombre2);
                cm.Parameters.AddWithValue("@ape1", ape1);
                cm.Parameters.AddWithValue("@ape2", ape2);
                cm.Parameters.AddWithValue("@casada", casada);
                cm.Parameters.AddWithValue("@genero", genero);
                cm.Parameters.AddWithValue("@estado_civil", estado_civil);
                cm.Parameters.AddWithValue("@fecha_nac", fecha_nac);
                cm.Parameters.AddWithValue("@telefono1", tel1);
                cm.Parameters.AddWithValue("@telefono2", tel2);
                cm.Parameters.AddWithValue("@domicilio", domicilio);
                cm.Parameters.AddWithValue("@id_direccion", id_dire);
                cm.Parameters.AddWithValue("@titulo", titulo);
                cm.Parameters.AddWithValue("@especialidad", especialidad);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ult_usua", ult_usua);
                cm.Parameters.AddWithValue("@fum", fum);
                cm.Parameters.AddWithValue("@estado", estado);

                cm.ExecuteNonQuery();
                res = "Actualizado";
                con.cerrarCon();
            }
            catch (Exception e)
            {
                res = "Error " + e.Message.ToString();
            }

            return res;
        }


        public System.Data.DataSet MostrarDocentes()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select CONCAT(nombre1,' ',nombre2,' ',apellido1,' ',apellido2,' ',casada) Nombre, " +
                "telefono1 Telefono, titulo Titulo, especialidad Especialidad," +
                "fecha_gra Ingreso, fum Modificacion, estado Estado from tb_docente";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;

        }

        public System.Data.DataSet MostrarDocentesCodigo(int cod)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "Select * from  tb_docente where id_docente = @cod;";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@cod", cod);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;

        }
        /*FIN DOCENTES*/

    }
}
