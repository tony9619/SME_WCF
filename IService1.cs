using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Wcf_SME
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IService1
    {
        

        [OperationContract]
        System.Data.DataSet ListaOpcionesxPerfil(int id_perfil);

        [OperationContract]
        System.Data.DataSet ListaOpciones_sub_menu(string sub_menu);

        [OperationContract]
        String Parametros(string parametro_config);

        [OperationContract]
        int login_cliente(string usuario, string clave);

        [OperationContract]
        System.Data.DataSet Obtener_Genero();

        [OperationContract]
        System.Data.DataSet Catalogos_grados();

        [OperationContract]
        System.Data.DataSet Lista_grados_institucion(int id_turno);

        [OperationContract]
        System.Data.DataSet Lista_grados_secciones_grados(int id_grado);

        [OperationContract]
        int capacidad_aula(int id_aula);

        [OperationContract]
        System.Data.DataSet lista_turnos();

        [OperationContract]
        int perfil_usuario(string usuario);

        [OperationContract]
        System.Data.DataSet lista_usuarios();

        [OperationContract]
        System.Data.DataSet get_usuario_x_user(string usuario);

        [OperationContract]
        System.Data.DataSet get_pefiles();

        [OperationContract]
        int editar_usuario(string usuario, int id_perfil, string estado);

        [OperationContract]
        int actualizar_clave (string usuario, string clave_actual, string clave_nueva);

        /*MANTENIMIENTO CICLOS*/
        [OperationContract]
        int InsertarCiclo(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum);

        [OperationContract]
        System.Data.DataSet BuscarCiclo(int codigo);

        [OperationContract]
        int ActualizarCiclo(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum);

        [OperationContract]
        System.Data.DataSet MostrarCiclos();

        [OperationContract]
        int EliminarCiclos(int codigo);
        /*MANTENIMIENTO CICLOS*/





        /* MANTENIMIENTO GRADOS*/

        [OperationContract]
        System.Data.DataSet ObtenerGrados();

        [OperationContract]
        string InsertarGrado(string nombre, int id_ciclo, string usu_gra,string estado);

        [OperationContract]
        string ActualizarGrado(int cod, string nombre, int id_ciclo, string ul_usu, DateTime fum, string estado);

        [OperationContract]
        System.Data.DataSet BuscarGrado(int codigo);

        [OperationContract]
        int EliminarGrado(int codigo);
        /* MANTENIMIENTO GRADOS*/




        /*MANTENIMIENTO SECCIONES*/
        [OperationContract]
        int InsertarSeccion(string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum);

        [OperationContract]
        System.Data.DataSet BuscarSeccion(int codigo);

        [OperationContract]
        int ActualizarSeccion(int id, string nombre, string usua_gra, DateTime fecha_gra, string ult_us, DateTime fum);

        [OperationContract]
        System.Data.DataSet MostrarSecciones();

        [OperationContract]
        int EliminarSeccion(int codigo);

        /*MANTENIMIENTO SECCIONES*/


        /*MANTENIMIENTO DE PARAMETRIZACION LECTURA Y ESCRITURA*/
        //gmaldonado 7/10/2019
        [OperationContract]
        System.Data.DataSet ListaParametros();

        [OperationContract]
        System.Data.DataSet Busqueda_parametro_valor(string valor);

        [OperationContract]
        string Actualizacion_parametro(string parametro, string valor);

        [OperationContract]
        System.Data.DataSet Get_Opciones(int id_perfil);

        [OperationContract]
        string prueba(string valor);

        [OperationContract]
        string Agregar_opciones_perfil(int id_perfil, int id_opcion);

        [OperationContract]
        string eliminar_opciones_perfil(int id_perfil);

        [OperationContract]
        System.Data.DataSet BuscarPerfil(int codigo);

      

    }

}





