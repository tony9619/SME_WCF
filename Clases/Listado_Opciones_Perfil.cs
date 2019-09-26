using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Wcf_SME.Clases
{
    [DataContract]
    public class Listado_Opciones_Perfil
    {
        [DataMember]
        public int id_perfil;
        [DataMember]
        public string nombre;
        [DataMember]
        public int id_opcion;
        [DataMember]
        public string titulo;
        [DataMember]
        public string url;
        [DataMember]
        public string es_sub_menu;
        [DataMember]
        public string sub_menu;

    }
}