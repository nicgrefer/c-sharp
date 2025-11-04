using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfAppEventosPersonalizados
{
    internal class Caldera
    {
        public event EventHandler<CalderaArgumentosEvento> 
            CambioTemperatura;
        public event EventHandler<CalderaArgumentosEvento> 
            CambioEstado;
        public event EventHandler StopCaldera;
        internal enum Estados : int
        {
            Correcto = 0,
            Alerta = 1,
            Peligro = 2
        }

        private int temperatura;
        public int Temperatura
        {
            private get { return temperatura; }
            set
            {
                temperatura = value;
                if (temperatura > 99) 
                  { StopCaldera?.Invoke(this, EventArgs.Empty); }
                if (temperatura < 60) 
                  { Estado = Estados.Correcto; }
                else if (temperatura < 80) 
                  { Estado = Estados.Alerta; }
                else 
                  { Estado = Estados.Peligro; }
                CambioTemperatura?.Invoke
                    (this, new CalderaArgumentosEvento(
                        temperatura, DateTime.Now, estado));
            }
        }

        private Estados estado;
        public Estados Estado
        {
            get { return estado; }
            private set
            {
                if (value != Estado)
                {
                    estado = value;
                    CambioEstado?.Invoke
                        (this, new CalderaArgumentosEvento(
                            temperatura, DateTime.Now, estado));
                }
            }
        }
    }

    internal class CalderaArgumentosEvento : EventArgs
    {
        public CalderaArgumentosEvento(int t, DateTime h, Caldera.Estados estado)
        {
            GradosC = t;
            Hora = h;
            EstadoCaldera = estado;
        }
        public int GradosC { get; set; }
        public DateTime Hora { get; }
        public Caldera.Estados EstadoCaldera { get; set; }
    }

}
