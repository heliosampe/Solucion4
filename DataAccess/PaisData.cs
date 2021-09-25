using DataAccess.Interfaces;
using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
    public class PaisData : IPaisData, IDisposable
    {
        private readonly string _conexion;
        public PaisData(string conexion)
        {
            _conexion = conexion;
        }

        #region Metodos CRUD
        public Result DeletePais(int id)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);
            string insertProcedure = "spDeleteTPais";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;
            if (id != 0)
            {
                insertCommand.Parameters.AddWithValue("@IdPais", id);
            }
            else
            {
                res.Exito = false;
                return res;
            }

            insertCommand.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
            insertCommand.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            insertCommand.Parameters.Add("@NumHijos", System.Data.SqlDbType.Int);
            insertCommand.Parameters["@NumHijos"].Direction = ParameterDirection.Output;
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                int idEliminado = System.Convert.ToInt32(insertCommand.Parameters["@ReturnValue"].Value);
                int Numhijos = System.Convert.ToInt32(insertCommand.Parameters["@NumHijos"].Value);
                if (idEliminado > 0 && Numhijos == 0)
                {
                    res.Exito = true;
                }
                else
                {
                    if (Numhijos > 0)
                        res.Message = "La entidad Pais tiene registros que dependen de ella en otra tabla no se pudo eliminar.";
                    else
                        res.Message = "No se pudo eliminar el registro";

                    res.Exito = false;
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Exito = false;
            }
            finally
            {
                connection.Close();
            }

            return res;
        }


        public Pais GetPais(int id)
        {
            Pais entidad = new Pais();

            Result res = new Result();
            SqlConnection conn = new SqlConnection(_conexion);
            try
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string sql = "";

                sql = "spGetTPaisByID";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                //manejar llave primaria
                cmd.Parameters.AddWithValue("@IdPais", id);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if (reader["IdPais"] != DBNull.Value)
                            entidad.IdPais = Int32.Parse(reader["IdPais"].ToString());
                        if (reader["Nombre"] != DBNull.Value)
                            entidad.NombrePais = reader["Nombre"].ToString();

                        if (reader["ContinenteId"] != DBNull.Value)
                            entidad.IdContinente = Int32.Parse(reader["ContinenteId"].ToString());


                    }
                }

            }
            catch (Exception ex)
            {
                res.Exito = false;
                res.Message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return entidad;
        }

        public List<Pais> GetPaises(int page, int numrecords)
        {
            List<Pais> lista = new List<Pais>();

            Result res = new Result();
            SqlConnection conn = new SqlConnection(_conexion);

            try
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string sql = "";

                sql = "spGetListTPaisPaged";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@pagNo", page);
                cmd.Parameters.AddWithValue("@pageSize", numrecords);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var it = new Pais();

                        if (reader["IdPais"] != DBNull.Value)
                            it.IdPais = Int32.Parse(reader["IdPais"].ToString());
                        if (reader["Nombre"] != DBNull.Value)
                            it.NombrePais = reader["Nombre"].ToString();

                        if (reader["ContinenteId"] != DBNull.Value)
                            it.IdContinente = Int32.Parse(reader["ContinenteId"].ToString());




                        lista.Add(it);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Exito = false;
                res.Message = ex.Message;
            }
            finally
            {
                conn.Close();
            }


            return lista;
        }

        public Result InsertPais(Pais item)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);
            string insertProcedure = "spInsertTPais";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;

            insertCommand.Parameters.AddWithValue("@Nombre", item.NombrePais);

            insertCommand.Parameters.AddWithValue("@IdContinente", item.IdContinente);



            insertCommand.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
            insertCommand.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                int idNuevo = System.Convert.ToInt32(insertCommand.Parameters["@ReturnValue"].Value);
                if (idNuevo > 0)
                {
                    res.IdInserted = idNuevo.ToString();
                    res.Exito = true;
                }
                else
                {
                    res.Exito = false;
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Exito = false;
            }
            finally
            {
                connection.Close();
            }

            return res;
        }

        public Result UpdatePais(int id, Pais item)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);//Repositorio.ConexionActual;
            string insertProcedure = "spUpdateTPais";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;


            insertCommand.Parameters.AddWithValue("@IdPais", id);

            insertCommand.Parameters.AddWithValue("@Nombre", item.NombrePais);
            insertCommand.Parameters.AddWithValue("@IdContinente", item.IdContinente);





            insertCommand.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
            insertCommand.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                int idUpdate = System.Convert.ToInt32(insertCommand.Parameters["@ReturnValue"].Value);
                if (idUpdate > 0)
                {
                    res.IdInserted = item.IdPais.ToString();
                    res.Exito = true;
                }
                else
                {
                    res.Exito = false;
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Exito = false;
            }
            finally
            {
                connection.Close();
            }

            return res;

        }

        #endregion

        //Dispose Methods
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            this.disposed = true;
        }
    }
}
