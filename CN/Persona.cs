using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CN
{
    public class Persona
    {

        #region Propiedades

        private string _nombreCompleto; //Propiedad FULL de lectura y escritura establece un get y un set para guardar la informacion en la variable privada - propfull

        public string nombreCompleto 
        {
            get { return _nombreCompleto; }
            //set { _nombreCompleto = value; } //se quitó set para agregar a traves de constructores
        }

        private DateTime _fechaNacimiento;

        public DateTime fechaNacimiento
        {
            get { return _fechaNacimiento; }
            //set { _fechaNacimiento = value; }
        }

        public int edad // Propiedad de solo lectura tiene un get pero no un set para la variable
        {
            get 
            {
                DateTime hoy = DateTime.Today;
                    int edad = hoy.Year - fechaNacimiento.Year;

                if (hoy < fechaNacimiento.AddYears(edad)) // se agrego if para calcular edad tomando en cuenta la fecha de nacimiento.
                {
                    edad--;
                }
                return edad;
            }
        }

        public string direccion { get; set; } //Propiedad de implementacion automatica - prop - disponible en nuevas versiones de C#

        #endregion

        #region Constructores

        public Persona(string _nombreCompleto, string _direccion, DateTime _fechaNacimiento)
        {
            this._nombreCompleto = _nombreCompleto;
            this.direccion = _direccion; //notar que no lleva guion bajo despues de this, esto es porque no es una propiedad full
            this._fechaNacimiento = _fechaNacimiento;
        }

        #endregion

        #region Metodos y Funciones

        public virtual string descripcion() // virtual se agrego para ayudar a sobreescribir la funcion
        {
            string resultado = string.Format("Nombre: {0} - Direccion: {1} - Fecha Nacimiento: {2} - Edad: {3}",
                nombreCompleto, direccion, fechaNacimiento, edad);

            return resultado; //utilizado para imprimir la informacion del objeto
        
        }

        #endregion

    }
}
