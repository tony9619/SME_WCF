using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Wcf_SME
{
    /*
    ***********************************
    * CATEGORIAS
    * NP00 - UTILES
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

    [ServiceContract]
    public interface IService1
    {
        /* NP00 TODOS LOS ENDPOINTS DE UTILIDADES*/
        [OperationContract]
        System.Data.DataSet lista_deptos();

        [OperationContract]
        System.Data.DataSet lista_municipiosx_depto(int id_depto);
        /***********************************/

        /*TODOS LOS ENDPOINTS DE OPCIONES*/

        /// NP01 *********************** OPCIONES *******************************************//////////

        [OperationContract]
        System.Data.DataSet ListaOpcionesxPerfil(int id_perfil); // OBTIENE TODAS LAS OPCIONES FILTRANDOLAS POR PERFIL

        [OperationContract]
        System.Data.DataSet ListaOpciones_sub_menu(string sub_menu); // OBTIENEN TODAS LAS OPCIONES(SUBOPCIONES) CORRESPONDIENTES A UN SUBMENU

        [OperationContract]
        System.Data.DataSet Get_Opciones(int id_perfil); // OBTIENE TODAS LAS OPCIONES, PERO INDICA POR UNA MARCA 'S' SI EL PERFIL LA POSEE

        [OperationContract]
        string Agregar_opciones_perfil(int id_perfil, int id_opcion); // AGREGA OPCIONES A UN PERFIL

        [OperationContract]
        string eliminar_opciones_perfil(int id_perfil); // ELIMINA LAS OPCIONES PARA UN DETERMINADO PERFIL

        [OperationContract]
        System.Data.DataSet ObtenerIconoOpcion(string page); // obtiene el icono de la pagina

        /// ************* ****************************************************************
        

        
        /// NP02 ****************ENPOINTS DE CONFIGURACION *************************************//


        [OperationContract]
        String Parametros(string parametro_config); // OBTIENE EL VALOR DE UN DETERMINADO PARAMETRO MEDIANTE STRING

        [OperationContract]
        System.Data.DataSet ListaParametros(); // OBTIENE EL LISTADO DE PARAMETROS 

        [OperationContract]
        System.Data.DataSet Busqueda_parametro_valor(string valor); // OBTIENE EL VALOR DE UN PARAMETRO RETORNADO EN DS

        [OperationContract]
        string Actualizacion_parametro(string parametro, string valor); // ACTUALIZA EL VALOR DE UN DETERMINADO PARAMETRO

        // ************** FIN ENDPOINTS DE CONFIGURACION ********************************//


        // NP03 ************** ENPOINTS DE USUARIOS *****************************************//

        [OperationContract]
        int login_cliente(string usuario, string clave); // ENDPOINT QUE ACCESO A LA APLICACION

        [OperationContract]
        System.Data.DataSet lista_usuarios(); // ENDPOINT QUE LISTA TODOS LOS USUARIOS

        [OperationContract]
        System.Data.DataSet get_usuario_x_user(string usuario); // ENPOINT QUE OBTIENE LA INFORMACION DE UN DETERMINADO USUARIO

        [OperationContract]
        int editar_usuario(string usuario, int id_perfil, string estado); // ENPOINT QUE ACTUALIZA LA INFORMACION DE UN USUARIO

        [OperationContract]
        System.Data.DataSet Obtener_Genero(); // ENPOINT QUE OBTIENE EL CATALOGO DE GENEROS EN BD

        [OperationContract]
        int actualizar_clave(string usuario, string clave_actual, string clave_nueva); // ENDPOINT QUE ACTUALIZA LA CONTRASE;A DE UN DETERMINADO USUARIO


        //*************** FIN ENDPOINTS DE USUARIOS ***********************************//


        /// NP04 ************* ENPOINTS DE GRADOS *****************************************//

        [OperationContract]
        System.Data.DataSet Catalogos_grados(); 
        // OBTIENE UN LISTADO DE GRADOS SIN EMBARGO NO DE LA TABLA GRADOS, ES DECIR HAY UN CATALOGO DE GRADOS INDEPENDIENTE
        //POR EJEMPLO EL CATALOGO DE GRADOS PUEDE OBTENER KINDER 4, PERO LA INDTITUCION NO IMPARTE KINDER 4, EN TABLA GRADOS
        // ESTE CATALOGO SIRVE PARA LA MATRICULA

        [OperationContract]
        System.Data.DataSet Lista_grados_institucion(int id_turno); // OBTIENE UN LISTADO DE GRADOS QUE IMPARTE LA INSTITUCION FILTRANDOLA POR TURNOS

        [OperationContract]
        System.Data.DataSet Lista_grados_secciones_grados(int id_grado); // obtiene todas las secciones asociadas a un determinado grado

        [OperationContract]
        int capacidad_aula(int id_aula); // obtiene la capacidad de un aula

        [OperationContract]
        int obtener_Aula(int id_grado, int id_seccion, int id_turno); // obtiene la capacidad de un aula



        [OperationContract]
        System.Data.DataSet ObtenerGrados(); // OBTIENE UN LISTADO DE TODOS LOS GRADOS

        [OperationContract]
        string InsertarGrado(string nombre, int id_ciclo, string usu_gra, string estado); // INGRESA UN NUEVO GRADO

        [OperationContract]
        string ActualizarGrado(int cod, string nombre, int id_ciclo, string ul_usu, string fum, string estado); // ACTUALIZA UN GRADO

        [OperationContract]
        System.Data.DataSet BuscarGrado(int codigo); // BUSCA UN DETERMINADO GRADO

        [OperationContract]
        int EliminarGrado(int codigo); // ELIMINA UN DETERMINADO GRADO


        //************************ FIN ENDPOINTS DE GRADOS *******************************//

        // NP05 *********************** ENPOINTS DE TURNOS ************************************//

        [OperationContract]
        System.Data.DataSet lista_turnos(); // OBTIENE UN LISTADO DE TURNOS

        [OperationContract]
        int InsertarTurno(string descripcion, string hora_ini, string hora_fin, string usua_gra, DateTime fecha, string ul_usu, DateTime fum, string estado);

        [OperationContract]
        int ActualizarTurno(int cod, string descripcion, string hora_ini, string hora_fin, string usua_gra, DateTime fecha, string ul_usu, DateTime fum, string estado);

        [OperationContract]
        System.Data.DataSet ListarTurno();

        [OperationContract]
        System.Data.DataSet BuscarTurno(int cod);



        // *********************** FIN DE ENPOINTS DE TURNOS *****************************//


        // NP06 *********************** ENPOINTS DE PERFILES ************************************//

        [OperationContract]
        int perfil_usuario(string usuario); // OBTIENE EL PERFIL DE UN DETERMINADO USUARIO


        [OperationContract]
        System.Data.DataSet get_pefiles(); // OBTIENE UN LISTADO DE TODOS LOS PERFILES


        [OperationContract]
        System.Data.DataSet BuscarPerfil(int codigo); // RETORNA LA INFORMACION DE UN DETERMINADO PERFIL


        // *********************** FIN ENPOINTS DE PERFILES ************************************//


        // NP07 ***********************  ENPOINTS DE CICLOS ************************************//

        [OperationContract]
        int InsertarCiclo(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum); // INGRESA UN NUEVO CICLO

        [OperationContract]
        System.Data.DataSet BuscarCiclo(int codigo); // BUSCA UN DETERMINADO CICLO

        [OperationContract]
        int ActualizarCiclo(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum); //ACTUALIZA LA INFOR DE UN DETERMINADO CICLO

        [OperationContract]
        System.Data.DataSet MostrarCiclos(); // MUESTRA UN LISTADO DE TODOS LOS CICLOS

        [OperationContract]
        System.Data.DataSet ListarCiclos();


        [OperationContract]
        int EliminarCiclos(int codigo); // ELIMINA UN DETERMINADO CICLO

        // *********************** FIN ENPOINTS DE CICLOS ************************************//

        // NP08 ***********************  ENPOINTS DE SECCIONES ************************************//


        [OperationContract]
        int InsertarSeccion(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum, string estado); // INGRESA UNA NUEVA SECCION


        [OperationContract]
        System.Data.DataSet BuscarSeccion(int codigo); // BUSCCA UNA DETERMINADA SECCION

        [OperationContract]
        int ActualizarSeccion(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum, string estado); // ACTUALIZA UNA DETERMINADA SECCION

        [OperationContract]
        System.Data.DataSet MostrarSecciones(); // MUESTRA UN LISTADO DE TODAS LAS SECCCIONES

        [OperationContract]
        int EliminarSeccion(int codigo);

        // ***********************  FIN DE ENPOINTS DE SECCIONES ************************************//

        //NP09 ************************* ENDPONITS DE MATERIAS *******************************************//
        [OperationContract]
        System.Data.DataSet MostrarMaterias();

        [OperationContract]
        int ActualizarMateria(int id, string nombre, string ult_us, DateTime fum);

        [OperationContract]
        System.Data.DataSet BuscarMateria(int codigo);

        [OperationContract]
        string InsertarMateria(string nombre, string usua_gra, DateTime fecha_gra);
        // ******************************************************************************************//

        //****************************MAtricula**************************************************//

        [OperationContract]
         string matricula_nuevo_ingreso(
            string rep1,string rep2,string rep3,

            /******************* alumno INFO *********************************************/
            string alum_n1, string alum_n2, string alum_gen, string alum_ape1, string alum_ape2,
            string alumn_mail, string alum_alergia, string alum_sangre, string alum_enfermo, string alum_enfermedad, string alum_fecha_n,
            int id_ult_grado, int ult_anio_alum, string alum_ult_inst, string alum_ult_status, string isss,
            /****************************REPRESENTANTE 1********************************************/
            string rep1_name, string rep1_ape, string rep1_tel1, string rep1_tel2, string rep1_dui, string rep1_mail, string rep1_vive_alum,
            int rep1_depto, int rep1_mun, string rep1_domicilio, string rep1_trabaja, string rep1_parentesto, string rep1_empleo, string rep1_cargo, string rep1_telefono3, string rep1_dir_lab,
            string otro_parentesco_rep1,
            ///****************************REPRESENTANTE 2********************************************/
            string rep2_name, string rep2_ape, string rep2_tel1, string rep2_tel2, string rep2_dui, string rep2_mail, string rep2_vive_alum,
            int rep2_depto, int rep2_mun, string rep2_domicilio, string rep2_trabaja, string rep2_parentesto, string rep2_empleo, string rep2_cargo, string rep2_telefono3, string rep2_dir_lab,
            string otro_parentesco_rep2,

            ///****************************REPRESENTANTE 3********************************************/

            string rep3_name, string rep3_ape, string rep3_tel1, string rep3_tel2, string rep3_dui, string rep3_mail, string rep3_vive_alum,
            int rep3_depto, int rep3_mun, string rep3_domicilio, string rep3_trabaja, string rep3_parentesto, string rep3_empleo, string rep3_cargo, string rep3_telefono3, string rep3_dir_lab,
            string otro_parentesco_rep3,

            ///************ MATRICULA ***********************************/
            int id_aula, int id_turno, int id_grado, int id_seccion,
            string usuario_operacion
            );


        //*****************************************FIN MATRICULA 1A VEZ *********************************************//

        /*MATERIAS CICLO*/
        [OperationContract]
        System.Data.DataSet ListarMaterias();

        [OperationContract]
        System.Data.DataSet Get_Materias(int id_materia);

        [OperationContract]
        string eliminar_materias_ciclo(int id_ciclo);

        [OperationContract]
        string Agregar_materia_ciclo(int id_ciclo, int id_materia);

        [OperationContract]
        System.Data.DataSet MostrarMateria_Ciclo();

        /*MATERIAS CICLO*/




        /*DOCENTES*/
        [OperationContract]
        string InsertarDocente(string nombre1, string nombre2, string ape1, string ape2, string casada, string genero, string estado_civil, DateTime fecha_nac, string tel1, string tel2, string domicilio, int id_dire, string titulo, string especialidad, string usu_gra, DateTime fecha_gra, string ult_usua, DateTime fum, string estado);

        [OperationContract]
        string ActualizarDocente(int codigo, string nombre1, string nombre2, string ape1, string ape2, string casada, string genero, string estado_civil, DateTime fecha_nac, string tel1, string tel2, string domicilio, int id_dire, string titulo, string especialidad, DateTime fecha_gra, string ult_usua, DateTime fum, string estado);

        [OperationContract]
        System.Data.DataSet MostrarDocentes();

        [OperationContract]
        System.Data.DataSet MostrarDocentesCodigo(int cod);
        /*FIN DOCENTES*/

    }


}





