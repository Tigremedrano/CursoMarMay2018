using CD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CN
{
    #region Propiedades
    public class Producto // se agrego public para permitir accesso al Windows form  HomeDepot despues de agregar la referncia
    {
        public int idProducto { get; set; }
        public string unidadMed { get; set; }
        public string descripcion { get; set; }
        public float precioUnit { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; }
        #endregion

        #region Constructores
        public Producto(int _idProducto, string _unidadMed, string _descripcion, float _precioUnit, bool _esActivo)
        {
            idProducto = _idProducto;
            unidadMed = _unidadMed;
            descripcion = _descripcion;
            precioUnit = _precioUnit;
            esActivo = _esActivo;

        }
        public Producto(DataRow fila)
        {
            //los nombres en rojo son los nombres de las columnas, el dato que puede cambiar es el inicial pero el rojo debe coincidir
            idProducto = fila.Field<int>("idProducto");
            unidadMed = fila.Field<string>("unidadMed");
            descripcion = fila.Field<string>("descripcion");
            precioUnit = fila.Field<float>("precioUnit");
            //aqui puede fallar porque teniamos money y aqui es float, puede causar error de cannot convert
            esActivo = fila.Field<bool>("esActivo");
            fechaCreacion = fila.Field<DateTime>("fechaCreacion");
        }
        #endregion

        #region Metodos y funciones

        /*public bool guardar()
        {
            return false;

        }*/
        public void guardar()
        {
            /*throw new NotImplementedException();*/
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@unidadMed", unidadMed));
            parametros.Add(new SqlParameter("@descripcion", descripcion));
            parametros.Add(new SqlParameter("@precioUnit", precioUnit));
            parametros.Add(new SqlParameter("@esActivo", esActivo));
            /*manera larga 
            
            SqlParameter parametroIdProducto = new SqlParameter("@unidadMed", unidadMed);
            parametros.Add(parametroIdProducto);
            
            y la opcion con codigo corto es la que muestra abajo
            parametros.Add(new SqlParameter("@unidadMed", unidadMed));
            */

            try
            {
                if(idProducto > 0)
                {
                    //update
                    parametros.Add(new SqlParameter("@idProducto", idProducto));
                    //string nombreSP = "dbo.SPUProductos";
                    /*SqlParameter[] nuevoParametros.ToArray();
                    bool fueCorrecto = (DataBaseHelper)

                    */
                    if (DataBaseHelper.ExecuteNonQuery("dbo.SPUProductos", parametros.ToArray()) == 0)
                    {
                        throw new Exception("No se actualizo el registro");
                    }
                }
                else
                {
                    //insert
                    if (DataBaseHelper.ExecuteNonQuery("dbo.SPIProductos", parametros.ToArray()) == 0)
                    {
                        throw new Exception("No se actualizo el registro");
                    }
                }
            }

            catch (Exception ex)
            {
            #if DEBUG
                throw new Exception(ex.Message);
            #else
                throw new Exception("Ha ocurrido un error con la base de datos");
            #endif
            }
            

        }
        public static void desactivar(int idProducto,bool esActivo = false)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("@idProducto", idProducto));
            parametros.Add(new SqlParameter("@esActivo", esActivo));

            try
            {
                if (DataBaseHelper.ExecuteNonQuery("dbo.SPDProductos", parametros.ToArray()) == 0)
                {
                    throw new Exception("No se desactivo el registro");
                }
            }
            catch (Exception ex)
            {
            #if DEBUG
            throw new Exception(ex.Message);
            #else
            throw new Exception("Ha ocurrido un error con la base de datos");
            #endif
            }
        }

       
        //throw new NotImplementedException();

        
        /*public static bool desactivar(int idProducto)
        {
            return false;

        }*/
        public static Producto buscarPorID(int idProducto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("@idProducto", idProducto));

            DataTable dt = new DataTable();

            try
            {
                DataBaseHelper.Fill(dt, "dbo.SPSProductos", parametros.ToArray());
                Producto resultado = null;

                foreach(DataRow fila in dt.Rows)
                {
                    resultado = new Producto(fila);
                    break;
                }
                if (resultado == null)
                {
                    throw new System.Exception("No se han encontrado coincidencias.");
                }
                return resultado;
            }
            catch (Exception ex)
            {
#if DEBUG
            throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }

        }
        public static List<Producto> traerTodos(bool filtrarSoloActivos = false)
        {

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (filtrarSoloActivos)
            {
                parametros.Add(new SqlParameter("@esActivo", true));
            }

            DataTable dt = new DataTable();

            try
            {
                DataBaseHelper.Fill(dt, "dbo.SPSProductos", parametros.ToArray());

                List<Producto> listado = new List<Producto>();

                foreach (DataRow fila in dt.Rows)
                {
                    listado.Add(new Producto(fila));
                   
                }
                
                return listado;
            }
            catch (Exception ex)
            {
#if DEBUG
            throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }
        }
#endregion
    }
}