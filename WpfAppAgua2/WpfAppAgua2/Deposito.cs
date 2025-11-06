using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppAgua
{
    internal class Deposito : INotifyPropertyChanged
    {
        /*
        //public event EventHandler Cantidad_Changed; ahroa este me sobra ya que uso INotifyPropertyChanged
        //public event EventHandler DepositoVacio;
        */
        // Implementación de INotifyPropertyChanged para notificar cambios en las propiedades de manera automática
        public event PropertyChangedEventHandler PropertyChanged;

        public Deposito() //constructor
        { 
            Cantidad = 0;   
        }

       
        private int cantidad; 

        
        public int Cantidad 
        {
            get
            {
                return cantidad; 
            }
            set 
            {
                cantidad = (value > 10) ? 10 : value; ; 
                
                OnPrpertyChanged();
                /*
                if (cantidad == 0) 
                    DepositoVacio.Invoke(this, new EventArgs());
              */
            }
        }
       
        public void Llenar(int valor)
        {
            Cantidad += valor; 
            
        }

        protected void OnPrpertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
