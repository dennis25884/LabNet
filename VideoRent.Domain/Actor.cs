using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRent.Domain
{
    public class Actor
    {
        private int actorId;
        private String nombreActor;
        private String apellidosActor;

        public Actor()
        {
            
        }

        public Actor(int actorId, string nombreActor, string apellidosActor)
        {
            this.actorId = actorId;
            this.nombreActor = nombreActor;
            this.apellidosActor = apellidosActor;
        }

        public int ActorId { get => actorId; set => actorId = value; }
        public string NombreActor { get => nombreActor; set => nombreActor = value; }
        public string ApellidosActor { get => apellidosActor; set => apellidosActor = value; }
    }
}
