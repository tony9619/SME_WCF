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

    }




}
