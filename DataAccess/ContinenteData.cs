using DataAccess.Interfaces;
using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ContinenteData : IContinenteData, IDisposable
    {

        private readonly string _conexion;
        public ContinenteData(string conexion)
        {
            _conexion = conexion;
        }

        public Result DeleteContinente(int id)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);
            string insertProcedure = "spDeleteTContinente";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;
            if (id != 0)
            {
                insertCommand.Parameters.AddWithValue("@IdContinente", id);
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
                        res.Message = "La entidad Continente tiene registros que dependen de ella en otra tabla no se pudo eliminar.";
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


        public Continente GetContinente(int id)
        {
            Continente entidad = new Continente();

            Result res = new Result();
            SqlConnection conn = new SqlConnection(_conexion);
            try
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string sql = "";

                sql = "spGetTContinenteByID";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                //manejar llave primaria
                cmd.Parameters.AddWithValue("@IdContinente", id);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if (reader["IdContinente"] != DBNull.Value)
                            entidad.IdContinente = Int32.Parse(reader["IdContinente"].ToString());
                        if (reader["NombreContinente"] != DBNull.Value)
                            entidad.NombreContinente = reader["NombreContinente"].ToString();
                        if (reader["Activo"] != DBNull.Value)
                            entidad.Activo = Boolean.Parse(reader["Activo"].ToString());


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

        public List<Continente> GetContinentes(int page, int numrecords)
        {
            List<Continente> lista = new List<Continente>();

            Result res = new Result();
            SqlConnection conn = new SqlConnection(_conexion);

            try
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string sql = "";

                sql = "spGetListTContinentePaged";
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
                        var it = new Continente();

                        if (reader["IdContinente"] != DBNull.Value)
                            it.IdContinente = Int32.Parse(reader["IdContinente"].ToString());

                        if (reader["NombreContinente"] != DBNull.Value)
                            it.NombreContinente = reader["NombreContinente"].ToString();

                        if (reader["Activo"] != DBNull.Value)
                            it.Activo = Boolean.Parse(reader["Activo"].ToString());


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

        public Result InsertContinente(Continente item)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);
            string insertProcedure = "spInsertTContinente";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;


            insertCommand.Parameters.AddWithValue("@ContinenteNombre", item.NombreContinente);
            insertCommand.Parameters.AddWithValue("@Activo", item.Activo);



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

        public Result UpdateContinente(int id, Continente item)
        {
            Result res = new Result();
            SqlConnection connection = new SqlConnection(_conexion);//Repositorio.ConexionActual;
            string insertProcedure = "spUpdateTContinente";
            SqlCommand insertCommand = new SqlCommand(insertProcedure, connection);
            insertCommand.CommandType = CommandType.StoredProcedure;



            insertCommand.Parameters.AddWithValue("@IdContinente", item.IdContinente);


            insertCommand.Parameters.AddWithValue("@NombreContinente", item.NombreContinente);


            insertCommand.Parameters.AddWithValue("@Activo", item.Activo);



            insertCommand.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
            insertCommand.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                int idUpdate = System.Convert.ToInt32(insertCommand.Parameters["@ReturnValue"].Value);
                if (idUpdate > 0)
                {
                    res.IdInserted = item.IdContinente.ToString();
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

        #region Dispose Methods
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
        #endregion
    }
}
