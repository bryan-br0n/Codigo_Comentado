//Este bloque importa las bibliotecas necesarias
//para trabajar con colecciones genéricas (List),
//conexiones a bases de datos SQL (SqlConnection, SqlCommand, SqlDataReader), y otras utilidades de C#.
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DatosLayer
{
    public class CustomerRepository //La clase CustomerRepository actúa como un repositorio de clientes,
                                    //encargada de la gestión de datos de la tabla Customers.
    {
        
        public List<Customers> ObtenerTodos()
        {                         //Este método obtiene todos los registros de la tabla Customers.
                                  //Se establece una conexión a la base de datos, se ejecuta una consulta SQL para seleccionar
                                  //todos los campos de la tabla, y luego se lee cada fila del resultado, creando instancias
                                  //de la clase Customers y agregándolas a una lista que se devuelve al final.
            using (var conexion= DataBase.GetSqlConnection()) {
                String selectFrom = "";
                selectFrom = selectFrom + "SELECT [CustomerID] " + "\n";
                selectFrom = selectFrom + "      ,[CompanyName] " + "\n";
                selectFrom = selectFrom + "      ,[ContactName] " + "\n";
                selectFrom = selectFrom + "      ,[ContactTitle] " + "\n";
                selectFrom = selectFrom + "      ,[Address] " + "\n";
                selectFrom = selectFrom + "      ,[City] " + "\n";
                selectFrom = selectFrom + "      ,[Region] " + "\n";
                selectFrom = selectFrom + "      ,[PostalCode] " + "\n";
                selectFrom = selectFrom + "      ,[Country] " + "\n";
                selectFrom = selectFrom + "      ,[Phone] " + "\n";
                selectFrom = selectFrom + "      ,[Fax] " + "\n";
                selectFrom = selectFrom + "  FROM [dbo].[Customers]";

                using (SqlCommand comando = new SqlCommand(selectFrom, conexion)) {
                    SqlDataReader reader = comando.ExecuteReader();
                    List<Customers> Customers = new List<Customers>();

                    while (reader.Read())
                    {
                        var customers = LeerDelDataReader(reader);
                        Customers.Add(customers);
                    }
                    return Customers;
                }
            }
           
        }
        //Este método obtiene un cliente específico por su CustomerID.
        //Se construye y ejecuta una consulta SQL que selecciona el registro cuyo CustomerID
        //coincide con el valor pasado como parámetro.
        public Customers ObtenerPorID(string id) {

            using (var conexion = DataBase.GetSqlConnection()) {
                
                String selectForID = "";
                selectForID = selectForID + "SELECT [CustomerID] " + "\n";
                selectForID = selectForID + "      ,[CompanyName] " + "\n";
                selectForID = selectForID + "      ,[ContactName] " + "\n";
                selectForID = selectForID + "      ,[ContactTitle] " + "\n";
                selectForID = selectForID + "      ,[Address] " + "\n";
                selectForID = selectForID + "      ,[City] " + "\n";
                selectForID = selectForID + "      ,[Region] " + "\n";
                selectForID = selectForID + "      ,[PostalCode] " + "\n";
                selectForID = selectForID + "      ,[Country] " + "\n";
                selectForID = selectForID + "      ,[Phone] " + "\n";
                selectForID = selectForID + "      ,[Fax] " + "\n";
                selectForID = selectForID + "  FROM [dbo].[Customers] " + "\n";
                selectForID = selectForID + $"  Where CustomerID = @customerId";

                using (SqlCommand comando = new SqlCommand(selectForID, conexion))
                {
                    comando.Parameters.AddWithValue("customerId", id);


                    var reader = comando.ExecuteReader();
                    Customers customers = null;
                    //validadmos 
                    if (reader.Read()) {
                        customers = LeerDelDataReader(reader);
                    }
                    return customers;
                }

            }
        }

        //Este método se encarga de mapear los resultados de la consulta SQL a un objeto Customers.
        //Se verifica si algún campo es DBNull (valor nulo en SQL) y se asigna un valor por defecto si es necesario.
        public Customers LeerDelDataReader( SqlDataReader reader) {
          
            Customers customers = new Customers();
            customers.CustomerID = reader["CustomerID"] == DBNull.Value ? " " : (String)reader["CustomerID"];
            customers.CompanyName = reader["CompanyName"] == DBNull.Value ? "" : (String)reader["CompanyName"];
            customers.ContactName = reader["ContactName"] == DBNull.Value ? "" : (String)reader["ContactName"];
            customers.ContactTitle = reader["ContactTitle"] == DBNull.Value ? "" : (String)reader["ContactTitle"];
            customers.Address = reader["Address"] == DBNull.Value ? "" : (String)reader["Address"];
            customers.City = reader["City"] == DBNull.Value ? "" : (String)reader["City"];
            customers.Region = reader["Region"] == DBNull.Value ? "" : (String)reader["Region"];
            customers.PostalCode = reader["PostalCode"] == DBNull.Value ? "" : (String)reader["PostalCode"];
            customers.Country = reader["Country"] == DBNull.Value ? "" : (String)reader["Country"];
            customers.Phone = reader["Phone"] == DBNull.Value ? "" : (String)reader["Phone"];
            customers.Fax = reader["Fax"] == DBNull.Value ? "" : (String)reader["Fax"];
            return customers;
        }
        //Este método inserta un nuevo cliente en la base de datos.
        //Se construye y ejecuta una consulta SQL INSERT INTO con los valores
        //correspondientes del objeto Customers pasado como parámetro.

        public int InsertarCliente(Customers customer) {
            using (var conexion = DataBase.GetSqlConnection()) {
                String insertInto = "";
                insertInto = insertInto + "INSERT INTO [dbo].[Customers] " + "\n";
                insertInto = insertInto + "           ([CustomerID] " + "\n";
                insertInto = insertInto + "           ,[CompanyName] " + "\n";
                insertInto = insertInto + "           ,[ContactName] " + "\n";
                insertInto = insertInto + "           ,[ContactTitle] " + "\n";
                insertInto = insertInto + "           ,[Address] " + "\n";
                insertInto = insertInto + "           ,[City]) " + "\n";
                insertInto = insertInto + "     VALUES " + "\n";
                insertInto = insertInto + "           (@CustomerID " + "\n";
                insertInto = insertInto + "           ,@CompanyName " + "\n";
                insertInto = insertInto + "           ,@ContactName " + "\n";
                insertInto = insertInto + "           ,@ContactTitle " + "\n";
                insertInto = insertInto + "           ,@Address " + "\n";
                insertInto = insertInto + "           ,@City)";

                using (var comando = new SqlCommand( insertInto,conexion )) {
                  int  insertados = parametrosCliente(customer, comando);
                    return insertados;
                }

            }
        }
        //Este método actualiza la información de un cliente en la base de datos
        //Se construye una consulta SQL UPDATE que modifica los campos del cliente que coincida
        //con el CustomerID proporcionado.
        public int ActualizarCliente(Customers customer) {
            using (var conexion = DataBase.GetSqlConnection()) {
                String ActualizarCustomerPorID = "";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "UPDATE [dbo].[Customers] " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "   SET [CustomerID] = @CustomerID " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "      ,[CompanyName] = @CompanyName " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "      ,[ContactName] = @ContactName " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "      ,[ContactTitle] = @ContactTitle " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "      ,[Address] = @Address " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + "      ,[City] = @City " + "\n";
                ActualizarCustomerPorID = ActualizarCustomerPorID + " WHERE CustomerID= @CustomerID";
                using (var comando = new SqlCommand(ActualizarCustomerPorID, conexion)) {

                    int actualizados = parametrosCliente(customer, comando);

                    return actualizados;
                }
            } 
        }

        //Este método asigna los parámetros correspondientes del objeto Customers al
        //comando SQL y ejecuta la consulta Devuelve el número de filas afectadas
        //(ya sea insertadas, actualizadas, o eliminadas).
        public int parametrosCliente(Customers customer, SqlCommand comando) {
            comando.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            comando.Parameters.AddWithValue("CompanyName", customer.CompanyName);
            comando.Parameters.AddWithValue("ContactName", customer.ContactName);
            comando.Parameters.AddWithValue("ContactTitle", customer.ContactName);
            comando.Parameters.AddWithValue("Address", customer.Address);
            comando.Parameters.AddWithValue("City", customer.City);
            var insertados = comando.ExecuteNonQuery();
            return insertados;
        }

        //Este método elimina un cliente de la base de datos
        //Se construye una consulta SQL DELETE para eliminar el registro cuyo
        //CustomerID coincide con el parámetro proporcionado.
        public int EliminarCliente(string id) {
            using (var conexion = DataBase.GetSqlConnection() ){
                String EliminarCliente = "";
                EliminarCliente = EliminarCliente + "DELETE FROM [dbo].[Customers] " + "\n";
                EliminarCliente = EliminarCliente + "      WHERE CustomerID = @CustomerID";
                using (SqlCommand comando = new SqlCommand(EliminarCliente, conexion)) {
                    comando.Parameters.AddWithValue("@CustomerID", id);
                    int elimindos = comando.ExecuteNonQuery();
                    return elimindos;
                }
            }
        }
    }
}
