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

        //
        /*DEPTO MUNI*/
        public System.Data.DataSet lista_deptos_muni(int id_muni)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select d.id_depto depto, d.nombre, m.id_municipio from tb_departamento d " +
                    "join municipio m on d.id_depto = (select id_depto from municipio where id_municipio=@id_muni)";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@id_muni", id_muni);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        /*DEPTO MUNI*/


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

        public System.Data.DataSet ListaOpciones_sub_menu(string sub_menu, int id_perfil)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select p.id_perfil,p.nombre,op.id_opcion,op.opcion,op.titulo,op.url,op.es_submenu,op.sub_menu,op.icono from  tb_opciones_perfil op_p inner join tb_opciones op on op.id_opcion=op_p.id_opcion inner join tb_perfil p on p.id_perfil=op_p.id_perfil where op.sub_menu='" + sub_menu + "' and p.id_perfil="+id_perfil;

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
                    "set valor = UPPER('" + valor + "') where upper(parametro) = upper('" + parametro + "')";

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
        public string Buscar_descripcion_grado(string cadena)
        {
            string res;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select CASE  WHEN NOMBRE = '" + cadena + "' THEN '1' WHEN NOMBRE <>'" + cadena + "' THEN '0' END from tb_grados WHERE NOMBRE='" + cadena + "'";
                res = cmd.ExecuteScalar().ToString();
                con.cerrarCon();
                return res;
            }
            catch
            {
                return "0";
            }

        }// Busca un grado por la descripcion


        public string obtener_abrv_grado(int id_grado)
        {
            string res;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select abreviatura from tb_grados where id_grado=" + id_grado;
                res = cmd.ExecuteScalar().ToString();
                con.cerrarCon();
                return res;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
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

        public System.Data.DataSet Lista_grados_secciones_grados(int id_grado,int id_turno)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select s.id_seccion,s.nombre,a.id_aula from tb_aula a inner join tb_secciones s " +
                "on a.id_seccion=s.id_seccion where a.id_grado=" + id_grado + " and a.id_turno="+id_turno+" order by s.nombre asc";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public int capacidad_aula(int id_grado, int id_turno, int id_seccion)
        {
            try
            {
                int res;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select capacidad - (select count(id_matricula) from tb_matricula where id_grado=" + id_grado + " and id_turno=" + id_turno + " and id_seccion=" + id_seccion + " " +
                    "and estado='V')  from tb_aula where id_grado=" + id_grado + " and id_turno=" + id_turno + " and id_seccion=" + id_seccion + " and id_anio=(" +
                    "select id_anio from tb_anio_escolar where tipo='M' and activo='S')";
                res = int.Parse(cmd.ExecuteScalar().ToString());
                con.cerrarCon();
                return res;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public System.Data.DataSet listado_anios()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            string query = "select id_anio, Concat(anio,'-',tipo) anio from tb_anio_escolar where activo='S'";
            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }
        public string Obtener_anio_x_id(int id_anio)
        {
            string res;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select anio from tb_anio_escolar where id_anio=" + id_anio;
                res = cmd.ExecuteScalar().ToString();
                con.cerrarCon();
                return res;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
        public string Obtener_anio_x_nombre(string nombre)
        {
            string res;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select id_anio from tb_anio_escolar where anio='" + nombre + "'";
                res = cmd.ExecuteScalar().ToString();
                con.cerrarCon();
                return res;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
        public int obtener_Aula(int id_grado, int id_seccion, int id_turno)
        {

            int res;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select * from tb_aula where id_grado = " + id_grado + " and id_seccion = " + id_seccion + " and id_turno = " + id_turno + "";
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
                cm.CommandText = "select  CONCAT(gra.id_grado , '-' ,gra.nombre) as 'Grado', abreviatura as 'Abreviatura', " +
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

        public System.Data.DataSet listado_general_grados(int id_turno, int id_anio)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select * from tb_grados where id_grado not in (select id_grado from tb_aula where id_turno=" + id_turno + " and id_anio=" + id_anio + ")";

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public System.Data.DataSet lista_grados_sin_filtros()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select * from tb_grados";

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public string InsertarGrado(string nombre, int id_ciclo, string usu_gra, string estado, string abreviatura)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_grados(nombre,id_ciclo,usuario_gra,fecha_gra,estado,abreviatura)" +
                            "values (UPPER('" + nombre + "')," + id_ciclo + ",UPPER('" + usu_gra + "'),SYSDATETIME(),UPPER('" + estado + "'),UPPER('" + abreviatura + "'))";


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

        public string validarAbreviaturaGrado(string cadena)
        {
            string res;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select CASE  WHEN abreviatura = '" + cadena + "' THEN '1' WHEN abreviatura <>'" + cadena + "' THEN '0' END from tb_grados WHERE abreviatura='" + cadena + "'";
                res = cmd.ExecuteScalar().ToString();
                con.cerrarCon();
                return res;
            }
            catch
            {
                return "0";
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

        public string validarTurnos(string nombre , string hi, string hf)
        {
            string retorno = "";
            try
            {
                int res_turno;
                SqlCommand cmd_turno = new SqlCommand();
                cmd_turno.Connection = con.abrirCon();
                cmd_turno.CommandText = "select count(*) from tb_turno where UPPER(descripcion)=UPPER('" + nombre + "')";
                res_turno = int.Parse(cmd_turno.ExecuteScalar().ToString());
                con.cerrarCon();

                int res_hi;
                SqlCommand cmd_hi = new SqlCommand();
                cmd_hi.Connection = con.abrirCon();
                cmd_hi.CommandText = "select count(*) from tb_turno where cast('" + hi + "' as time) between cast(hora_inicio as time) and cast(hora_fin as time)";
                res_hi = int.Parse(cmd_hi.ExecuteScalar().ToString());
                con.cerrarCon();

                int res_fi;
                SqlCommand cmd_fi = new SqlCommand();
                cmd_fi.Connection = con.abrirCon();
                cmd_fi.CommandText = "select count(*) from tb_turno where cast('" + hf + "' as time) between cast(hora_inicio as time) and cast(hora_fin as time)";
                res_fi = int.Parse(cmd_fi.ExecuteScalar().ToString());
                con.cerrarCon();

                if(res_turno==0 && res_hi==0 && res_fi ==0)
                {
                    retorno= "0";
                }
                else if(res_turno> 0 || res_hi>0 || res_fi>0)
                {

                    if (res_turno > 0)
                    {
                        retorno ="El turno ya existe";
                    }
                    else if(res_hi > 0) { retorno = "La hora de inicio: " + hi + " es invalida, el horario lo ocupa otro turno"; }
                    else if(res_fi >0) { retorno = "La hora de finalizacion: " + hf + " es invalida, el horario lo ocupa otro turno";  }
                }

                return retorno;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }  

         
        }

        public int InsertarTurno(string descripcion, string hora_ini, string hora_fin, string usua_gra, DateTime fecha,  string estado)
        {
            int resp = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "INSERT into tb_turno (descripcion,hora_inicio,hora_fin,usuario_gra,fecha_gra,estado)" +
                                    "values(UPPER(@des),@hi,@hf,UPPER(@usu_gra),@fecha_gra,UPPER(@est))";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@des", descripcion);
                cm.Parameters.AddWithValue("@hi", hora_ini);
                cm.Parameters.AddWithValue("@hf", hora_fin);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha);
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

        public int ActualizarTurno(int cod, string descripcion, string hora_ini, string hora_fin, string ul_usu, DateTime fum, string estado)
        {
            int resp = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "UPDATE tb_turno set descripcion=UPPER(@des),hora_inicio=@hi,hora_fin=@hf, " +
                            "ultimo_usuario=UPPER(@ult_usua),fum=@fum,estado=UPPER(@est) where id_turno=@cod";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@cod", cod);
                cm.Parameters.AddWithValue("@des", descripcion);
                cm.Parameters.AddWithValue("@hi", hora_ini);
                cm.Parameters.AddWithValue("@hf", hora_fin);
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

        public string nombre_usuario_perfil(string usuario)
        {
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select case  when usu.id_perfil = 0 then (select UPPER(usuario) from tb_usuarios where usuario =usu.usuario) " +
                "when usu.id_perfil = 1 then (select CONCAT(UPPER(nombres),' ',UPPER(apellidos)) from tb_representante where dui=usu.usuario) " +
                "when usu.id_perfil = 2 then (select concat(UPPER(nombre1),' ' , UPPER(apellido1)) from tb_alumno where carnet= usu.usuario) " +
                "when usu.id_perfil = 3 then (select concat(UPPER(nombre1),' ' , UPPER(apellido1)) from tb_docente where autentica= usu.usuario) " +
                "end nombre from tb_usuarios usu where usu.usuario='"+usuario+"'";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
        }

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

        public string NuevoPerfil(string nombre,string estado,string usuario_gra)
        {
            string res = "0";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_perfil(nombre,estado,fecha_gra,usuario_gra) " +
                    "values (upper('"+nombre+"'),upper('"+estado+"'),SYSDATETIME(),UPPER('"+usuario_gra+"'))";
                
                res=cm.ExecuteNonQuery().ToString();
             
                con.cerrarCon();
                return res;

            }
            catch (Exception e)
            {
                res = e.Message.ToString();

            }

            return res;
        }
        public string ActualizarPerfil(int cod_perfil, string nombre, string estado, string ultimo_usuario)
        {
            string res = "0";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "update tb_perfil set nombre=UPPER('" + nombre + "'),estado=UPPER('" + estado + "'), fum=sysdatetime(), " +
                    "ultimo_usuario=UPPER('" + ultimo_usuario + "') where id_perfil=" + cod_perfil;

                res = cm.ExecuteNonQuery().ToString();

                con.cerrarCon();

                return res;

            }
            catch (Exception e)
            {
                res = e.Message.ToString();

            }

            return res;

        }
            public string validar_descripcion_perfil(string nombre)
            {
                string respuesta;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "select count(*) from tb_perfil where UPPER(nombre)=UPPER('" + nombre + "')";

                respuesta = cmd.ExecuteScalar().ToString();

                con.cerrarCon();
                return respuesta;
            }
        
        // *********************** FIN ENPOINTS DE PERFILES ************************************//

        // NP07 ***********************  ENPOINTS DE CICLOS ************************************//

        public string validarDescripcionCiclos(string descripcion)
        {
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "Select count(*) from tb_ciclos where nombre='"+descripcion+"'";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
        }

        public int InsertarCiclo(string nombre, string usua_gra, DateTime fecha_gra)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT INTO tb_ciclos(nombre,usuario_gra,fecha_gra)" +
                    "values(UPPER(@nom),UPPER(@usu_gra),@fecha_gra)";
                cm.Parameters.AddWithValue("@nom", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
               
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


        public int ActualizarCiclo(int id, string nombre, string ult_us, DateTime fum)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "UPDATE tb_ciclos set nombre=UPPER(@nom),ultimo_usuario=UPPER(@ult_us),fum=@fum where id_ciclo=@id";
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
            string query = "select id_ciclo Codifo, nombre Nombre, usuario_gra as 'Usuario Grabacion', ultimo_usuario as 'Ultimo Usuario', fum [Ultima Modificacion] from tb_ciclos";
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

        public int InsertarSeccion(string nombre, string usua_gra, DateTime fecha_gra, string estado)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_secciones (nombre,usuario_gra,fecha_gra,estado)" +
                    "values (UPPER(@nombre),UPPER(@usu_gra),@fech_gra,UPPER(@est))";
                cm.Parameters.AddWithValue("@nombre", nombre);
                cm.Parameters.AddWithValue("@usu_gra", usua_gra);
                cm.Parameters.AddWithValue("@fech_gra", fecha_gra);
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


        public string ActualizarSeccion(int id, string nombre, string ult_us, DateTime fum, string estado)
        {
            string r = "0";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "UPDATE tb_secciones set nombre=UPPER(@nombre),ultimo_usuario=UPPER(@ul_usu),fum=@fum,estado=UPPER(@est) where id_seccion=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@ul_usu", ult_us);
                cmd.Parameters.AddWithValue("@fum", fum);
                cmd.Parameters.AddWithValue("@est", estado);

                r=cmd.ExecuteNonQuery().ToString();

                return r;
            }
            catch (Exception e)
            {
                r = e.Message.ToString();
            }

            return r;
        }

        public string validar_descripcion_seccion( string nombre)
        {
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select count(*) from tb_secciones where UPPER(nombre)=UPPER('"+nombre+"')";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
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

        public System.Data.DataSet MostrarSecciones_Aulas(int id_grado, int id_turno, int id_anio)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                SqlCommand cm = new SqlCommand("select * from tb_secciones where id_seccion not in (select id_seccion from tb_aula where id_grado='" + id_grado + "' " +
                    "and id_turno='" + id_turno + "' and id_anio='" + id_anio + "')", con.abrirCon());
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
                    "values(UPPER(@nom),UPPER(@usu_gra),@fecha_gra)";
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

        public string validar_duplicidad_materia(string cadena)
        {
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select count(*) from tb_materia where UPPER(nombre)=UPPER('"+cadena+"')";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
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
                cm.CommandText = "UPDATE tb_materia set nombre=UPPER(@nom),ultimo_usuario=UPPER(@ult_us),fum=@fum where id_materia=@id";
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



            string rep1,
            string rep2,
            string rep3,

            /******************* alumno INFO *********************************************/
            string alum_n1, string alum_n2, string alum_gen, string alum_ape1, string alum_ape2,
            string alumn_mail, string alum_alergia, string alum_sangre, string alum_enfermo, string alum_enfermedad, string alum_fecha_n,
            int id_ult_grado, int ult_anio_alum, string alum_ult_inst, string alum_ult_status, string isss,

            /****************************REPRESENTANTE 1 INFO********************************************/
            string rep1_name, string rep1_ape, string rep1_tel1, string rep1_tel2, string rep1_dui, string rep1_mail, string rep1_vive_alum,
            int rep1_depto, int rep1_mun, string rep1_domicilio, string rep1_trabaja, string rep1_parentesto, string rep1_empleo, string rep1_cargo, string rep1_telefono3, string rep1_dir_lab,
            string otro_parentesco_rep1, string principal_rep1,

              ///****************************REPRESENTANTE 2 INFO********************************************/
              string rep2_name, string rep2_ape, string rep2_tel1, string rep2_tel2, string rep2_dui, string rep2_mail, string rep2_vive_alum,
            int rep2_depto, int rep2_mun, string rep2_domicilio, string rep2_trabaja, string rep2_parentesto, string rep2_empleo, string rep2_cargo, string rep2_telefono3, string rep2_dir_lab,
            string otro_parentesco_rep2, string principal_rep2,

             ///**************************** REPRESENTANTE 3 INFO ********************************************/

             string rep3_name, string rep3_ape, string rep3_tel1, string rep3_tel2, string rep3_dui, string rep3_mail, string rep3_vive_alum,
            int rep3_depto, int rep3_mun, string rep3_domicilio, string rep3_trabaja, string rep3_parentesto, string rep3_empleo, string rep3_cargo, string rep3_telefono3, string rep3_dir_lab,
            string otro_parentesco_rep3, string principal_rep3,

             ///************ INFO DE MATRICULA ***********************************/
             ///
             int id_aula, int id_turno, int id_grado, int id_seccion,
            string usuario_operacion

            )
        {



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
                    ",SYSDATETIME(),'" + isss + "')";
                res_alum = cm_alumno.ExecuteNonQuery();



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
                        "values('" + rep1_name + "','" + rep1_ape + "','" + rep1_tel1 + "','" + rep1_tel2 + "','" + rep1_dui + "','" + rep1_mail + "','" + alum_fecha_n + "'," +
                        "" + rep1_depto + "," + rep1_mun + ",'" + rep1_domicilio + "','" + rep1_trabaja + "','" + rep1_empleo + "','" + rep1_cargo + "'," +
                        "'" + rep1_telefono3 + "','" + rep1_dir_lab + "','dbo',SYSDATETIME())";

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
                    cmd_rep1_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario,principal) " +
                        "values('" + carnet_alumno + "','" + dui_rep1 + "','" + rep1_vive_alum + "','" + rep1 + "','" + otro_parentesco_rep1 + "',SYSDATETIME(),'" + usuario_operacion + "','" + principal_rep1 + "')";
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
                        "" + rep2_depto + "," + rep2_mun + ",'" + rep2_domicilio + "','" + rep2_trabaja + "','" + rep2_empleo + "','" + rep2_cargo + "'," +
                        "'" + rep2_telefono3 + "','" + rep2_dir_lab + "','" + usuario_operacion + "',SYSDATETIME())";

                    cmd_rep2.ExecuteNonQuery();

                    string dui_rep2 = "";
                    SqlCommand cmd_dui_rep2 = new SqlCommand();
                    cmd_dui_rep2.Connection = con.abrirCon();
                    cmd_dui_rep2.CommandText = "SELECT dui FROM tb_representante where id_representante = (select max(id_representante) from tb_representante)";
                    dui_rep2 = cmd_dui_rep2.ExecuteScalar().ToString();

                    con.cerrarCon();

                    SqlCommand cmd_rep2_alum = new SqlCommand();
                    cmd_rep2_alum.Connection = con.abrirCon();
                    cmd_rep2_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario,principal) " +
                        "values('" + carnet_alumno + "','" + dui_rep2 + "','" + rep2_vive_alum + "','" + rep2 + "','" + otro_parentesco_rep2 + "',SYSDATETIME(),'" + usuario_operacion + "','" + principal_rep2 + "')";
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
                        "'" + rep3_telefono3 + "','" + rep3_dir_lab + "','" + usuario_operacion + "',SYSDATETIME())";

                    cmd_rep3.ExecuteNonQuery();

                    string dui_rep3 = "";
                    SqlCommand cmd_dui_rep3 = new SqlCommand();
                    cmd_dui_rep3.Connection = con.abrirCon();
                    cmd_dui_rep3.CommandText = "SELECT dui FROM tb_representante where id_representante = (select max(id_representante) from tb_representante)";
                    dui_rep3 = cmd_dui_rep3.ExecuteScalar().ToString();

                    con.cerrarCon();

                    SqlCommand cmd_rep2_alum = new SqlCommand();
                    cmd_rep2_alum.Connection = con.abrirCon();
                    cmd_rep2_alum.CommandText = "insert into tb_alumno_rep (id_alumno,id_representante,vive_con_alumno,parentesco,otro,fecha_gra,ultimo_usuario,principal) " +
                        "values('" + carnet_alumno + "','" + dui_rep3 + "','" + rep3_vive_alum + "','" + rep3 + "','" + otro_parentesco_rep3 + "',SYSDATETIME(),'" + usuario_operacion + "','" + principal_rep3 + "')";
                    cmd_rep2_alum.ExecuteNonQuery();

                }// fin REP3

                // OBTENIENDO EL ANIO ESCOLAR VIGENTE E INSERTANDO LAS MATRICULAS 
                int anio_vigente = 0;
                SqlCommand cmd_anio_vigente = new SqlCommand();
                cmd_anio_vigente.Connection = con.abrirCon();
                cmd_anio_vigente.CommandText = "select id_anio from tb_anio_escolar where activo='S' and tipo='M'";
                anio_vigente = int.Parse(cmd_anio_vigente.ExecuteScalar().ToString());

                SqlCommand cmd_alum_matricula = new SqlCommand();
                cmd_alum_matricula.Connection = con.abrirCon();
                cmd_alum_matricula.CommandText = "insert into tb_matricula (alumno,id_aula,usuario_gra,fecha_gra,anio,id_turno,id_grado,id_seccion,estado) " +
                    "values('" + carnet_alumno + "'," + id_aula + ",'" + usuario_operacion + "',SYSDATETIME()," + anio_vigente + "," + id_turno + "," + id_grado + "," + id_seccion + ",'V')";
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
                cm.CommandText = "select id_docente Codigo , CONCAT(nombre1,' ',nombre2,' ',apellido1,' ',apellido2,' ',casada) Nombre, " +
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

        public System.Data.DataSet obtener_anio_escolar_vigente()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "Select * from  tb_anio_escolar where activo='S' and tipo='E'";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public string obtener_anio_matricula()
        {
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select id_anio from tb_anio_escolar where tipo='M' and activo='S'";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
        }

        /*aula*/

        public string ingresar_aula(string codigo, int id_grado, int id_seccion, int id_turno, int id_anio, int capacidad, string usuario, string estado)
        {
            int respuesta = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "insert into tb_aula (codigo,id_grado,id_seccion,id_anio,id_turno,capacidad,usuario_gra,fecha_gra,estado) " +
                    "values('" + codigo + "'," + id_grado + "," + id_seccion + "," + id_anio + "," + id_turno + "," + capacidad + ",UPPER('" + usuario + "'),SYSDATETIME(),'" + estado + "')";

                respuesta = cmd.ExecuteNonQuery();
                if (respuesta == 1)
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
                return e.Message.ToString();
            }
        }

        public System.Data.DataSet Buscar_Aula_xCod(int id_aula)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "Select * from  tb_aula where id_aula=" + id_aula;
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public string editar_aula(int codigo, int id_grado, int id_seccion, int id_turno, int id_anio, int capacidad, string usuario, string estado)
        {
            int respuesta = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.abrirCon();
                cmd.CommandText = "update tb_aula set id_turno=" + id_turno + ",id_grado=" + id_grado + ",id_seccion=" + id_seccion + ", " +
                    "id_anio=" + id_anio + ", capacidad=" + capacidad + ", ultimo_usuario= UPPER('" + usuario + "'), fum=SYSDATETIME(), estado = '" + estado + "' where id_aula=" + codigo + "";

                respuesta = cmd.ExecuteNonQuery();
                if (respuesta == 1)
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
                return e.Message.ToString();
            }
        }

        public System.Data.DataSet listado_aulas()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select id_aula as 'Id. Aula',codigo as 'Abreviatura',gr.nombre as 'Grado',sec.nombre " +
                    "as 'Seccion',tr.descripcion as 'Turno',ae.anio as 'Anio',capacidad,au.usuario_gra as 'Usuario Creacion',au.fecha_gra " +
                    "as 'Fecha Creacion', " +
                    "au.ultimo_usuario as 'Usuario Modificacion',au.fum as 'Fecha Modificacion', " +
                    "au.estado from tb_aula au " +
                    "inner join tb_grados gr on gr.id_grado=au.id_grado " +
                    "inner join tb_secciones sec on sec.id_seccion=au.id_seccion " +
                    "inner join tb_turno tr on tr.id_turno=au.id_turno " +
                    "inner join tb_anio_escolar ae on ae.id_anio=au.id_anio";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        /*****************************ASISTENCIA******************************************/
        public System.Data.DataSet coordinaciones(string usuario)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select doc.id_docente as cod_docente,doc.nombre1,doc.nombre2,doc.apellido1,doc.apellido2,au.id_aula as id_aula,au.codigo as cod_aula, " +
                    "gr.nombre as grado,sec.nombre as seccion,tur.descripcion as turno from tb_coordinador co inner join tb_docente doc " +
                    "on doc.id_docente=co.id_docente inner join tb_aula au on au.id_aula=co.id_aula " +
                    "inner join tb_grados gr on gr.id_grado=au.id_grado inner join tb_secciones sec " +
                    "on sec.id_seccion=au.id_seccion inner join tb_turno tur on tur.id_turno=au.id_turno " +
                    "where doc.autentica='"+usuario+"'";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public System.Data.DataSet alumnos_x_aula(int id_aula)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select ma.alumno carnet, concat(alm.apellido1, ' ',alm.apellido2, ' ', alm.nombre1, ' ' , alm.nombre2) nombre " +
                    "from tb_matricula ma inner join tb_alumno alm " +
                    "on alm.carnet=ma.alumno where ma.id_aula=" + id_aula + " and ma.estado='V' order by alm.apellido1 asc";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public System.Data.DataSet info_aula(int id_aula)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select doc.id_docente as cod_docente,doc.nombre1,doc.nombre2,doc.apellido1,doc.apellido2,au.id_aula as id_aula,au.codigo as cod_aula, " +
                    "gr.nombre as grado,sec.nombre as seccion,tur.descripcion as turno from tb_coordinador co inner join tb_docente doc " +
                    "on doc.id_docente=co.id_docente inner join tb_aula au on au.id_aula=co.id_aula " +
                    "inner join tb_grados gr on gr.id_grado=au.id_grado inner join tb_secciones sec " +
                    "on sec.id_seccion=au.id_seccion inner join tb_turno tur on tur.id_turno=au.id_turno " +
                    "where co.id_aula=" + id_aula;
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        /*********************************************************************************/


        /*COORDINADORES*/

        public string InsertarCoordinadores(int aula, int docente, string usu_gra, DateTime fecha_gra, string ult_usua, DateTime fum)
        {

            string res = "";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "INSERT into tb_coordinador (id_aula,id_docente,usuario_gra,fecha_gra,ultimo_usuario,fum)" +
                                                 "values (@aula,@docente,@usuario_gra,@fecha_gra,@ult_usua,@fum)";
                cm.Parameters.AddWithValue("@aula", aula);
                cm.Parameters.AddWithValue("@docente", docente);
                cm.Parameters.AddWithValue("@usuario_gra", usu_gra);
                cm.Parameters.AddWithValue("@fecha_gra", fecha_gra);
                cm.Parameters.AddWithValue("@ult_usua", ult_usua);
                cm.Parameters.AddWithValue("@fum", fum);

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



        public string EliminarCoor_Aula(int aula)
        {
            string res = "";
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "delete tb_coordinador from tb_coordinador c " +
                    "inner join tb_docente d on c.id_docente = d.id_docente " +
                    "where id_aula = @id_aula";
                cm.Parameters.AddWithValue("@id_aula", aula);

                cm.ExecuteNonQuery();
                res = "Eliminado";
                con.cerrarCon();
            }
            catch (Exception e)
            {
                res = "Error " + e.Message.ToString();
            }

            return res;
        }


        public System.Data.DataSet MostrarCoodinadores()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select a.id_aula, a.codigo Codigo, concat(g.nombre,' ',s.nombre) Grado,concat(d.nombre1,' ', d.nombre2,' ',d.apellido1,' ',d.apellido2) Docente," +
                    "c.usuario_gra Usuario, c.fecha_gra Fecha from tb_aula a inner join tb_coordinador c on a.id_aula = c.id_aula inner join  tb_docente d " +
                    "on d.id_docente = c.id_docente inner join tb_secciones s on s.id_seccion=a.id_seccion inner join tb_grados g on g.id_grado=a.id_grado";
                cm.Connection = con.abrirCon();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public int BuscarDocenteTurnoManana(int cod_Profe)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select d.id_docente, d.nombre1 from tb_docente d " +
                    "inner join tb_coordinador c on d.id_docente=c.id_docente inner join tb_aula a " +
                    "on a.id_aula=c.id_aula where a.id_turno =1 and d.id_docente=@id_profe";
                cm.Parameters.AddWithValue("@id_profe", cod_Profe);

                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    res = 1;
                }
                con.cerrarCon();
            }
            catch (Exception e)
            {
            }

            return res;
        }

        public int BuscarDocenteTurnoTarde(int cod_Profe)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "select d.id_docente, d.nombre1 from tb_docente d " +
                    "inner join tb_coordinador c on d.id_docente=c.id_docente inner join tb_aula a " +
                    "on a.id_aula=c.id_aula where a.id_turno =2 and d.id_docente=@id_profe";
                cm.Parameters.AddWithValue("@id_profe", cod_Profe);

                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    res = 1;
                }

                con.cerrarCon();
            }
            catch (Exception e)
            {
            }

            return res;
        }
        /* FIN COORDINADORES*/

        /*AULA*/


        public System.Data.DataSet MostrarAula(int id_turno)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select a.id_aula, a.codigo, concat(g.nombre,' ', s.nombre) Nombre " +
                    "from tb_grados g inner join tb_aula a on g.id_grado=a.id_grado " +
                    "inner join tb_secciones s on s.id_seccion = a.id_seccion " +
                    "where a.id_aula not in (select id_aula from tb_coordinador) and a.id_turno = @id_turno";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@id_turno", id_turno);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;
        }

        public System.Data.DataSet BuscarAulaCodigo(int codigo)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.CommandText = "select a.id_aula, d.id_docente,concat(g.nombre,' ',s.nombre) Grado, " +
                    "concat(d.nombre1,' ', d.nombre2,' ',d.apellido1,' ',d.apellido2) Docente " +
                    "from tb_aula a inner join tb_coordinador c on a.id_aula = c.id_aula " +
                    "inner join tb_docente d on d.id_docente = c.id_docente " +
                    "inner join tb_grados g on g.id_grado=a.id_grado " +
                    "inner join tb_secciones s on s.id_seccion = a.id_seccion where a.id_aula = @cod";
                cm.Connection = con.abrirCon();
                cm.Parameters.AddWithValue("@cod", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(ds);
            }
            catch (Exception e)
            {

            }
            return ds;

            /*FIN AULA*/

        }

        ///////////////// asistencia ////////////////////////


        public string encabezado_Asistencia(string usuario_operacion,string fecha)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_asistencia (fecha,usuario_gra,estado) values('"+fecha+"','"+ usuario_operacion + "','P')";

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
        ////////////////////////////////////////////////////

        public string assitencia_aula(string carnet,string estado,string permiso, int id_aula,string justificacion)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_asistencia_aula (id_asistencia,alumno,id_aula,estado,permiso,justificacion) " +
                    "values((select max(id_asistencia) from tb_asistencia),'"+carnet+"',"+id_aula+",'"+estado+"','"+permiso+"','"+justificacion+"')";

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

        public string validar_asistencia_aula(string fecha)
        {
            
            string respuesta;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.abrirCon();
            cmd.CommandText = "select COUNT(*) from tb_asistencia " +
                "where fecha='"+fecha+"'";

            respuesta = cmd.ExecuteScalar().ToString();

            con.cerrarCon();
            return respuesta;
        }

        public System.Data.DataSet lista_aula_por_dia(int id_aula, string fecha)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select * from tb_asistencia_aula where id_asistencia = (select id_asistencia from tb_asistencia where id_aula="+id_aula+" and fecha='"+fecha+"')";

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }


        public System.Data.DataSet alumnos_inasistencia_aula(int id_aula)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            string query = "select al.id_asistencia,asis.fecha, al.id_aula,aula.codigo, al.alumno, " +
                "CONCAT(alum.nombre1,' ',alum.apellido1) as nombre_alumno, " +
                "rep.dui,concat(rep.nombres,rep.apellidos ) as nombre_representante, rep.telefono2 " +
                "from tb_asistencia_aula al " +
                "inner  join tb_asistencia asis " +
                "on asis.id_asistencia=al.id_asistencia " +
                "inner join tb_alumno alum " +
                "on alum.carnet=al.alumno " +
                "inner join tb_alumno_rep rep_al " +
                "on rep_al.id_alumno=alum.carnet and rep_al.principal='S' " +
                "inner join tb_representante rep " +
                "on rep.dui=rep_al.id_representante " +
                "inner join tb_aula aula " +
                "on aula.id_aula=al.id_aula " +
                "where al.estado='I' and al.permiso='N' and al.id_asistencia=(select max(id_asistencia) from tb_asistencia " +
                "where estado='P' and al.id_aula="+id_aula+")";

            SqlDataAdapter ad = new SqlDataAdapter(query, con.abrirCon());
            ad.Fill(ds);
            con.cerrarCon();
            return ds;
        }

        public string notificacion_inasistencia(string alumno,string representante,int id_aula,string enviada,string sid,string catch_,string telefono1,string telefono2)
        {
            int res = 0;
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = con.abrirCon();
                cm.CommandText = "insert into tb_notificacion_inasistencia(id_asistencia,id_alumno,id_representante,id_aula,enviada,SID_,catch_,telefono1,telefno2) " +
                    "values((select  distinct(id_asistencia) from tb_asistencia_aula where id_asistencia=(select max(id_asistencia) from tb_asistencia where id_aula="+id_aula+")),'" + alumno+"','"+representante+"',"+id_aula+",'"+enviada+"','"+sid+"','"+catch_+"','"+telefono1+"','"+telefono2+"')";

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


    }

    }
