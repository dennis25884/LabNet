using System.Collections.Generic;
using System;
using Microsoft.Data.SqlClient;
using VideoRent.Domain;

public class PeliculaData
{
    private readonly String connectionString;

    public PeliculaData(String connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Insertar(Pelicula pelicula)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

         
            SqlCommand cmdPelicula = connection.CreateCommand();
            cmdPelicula.CommandText = "sp_PeliculaCreation";
            cmdPelicula.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter parCodPelicula = new SqlParameter("@pelicula_id", System.Data.SqlDbType.Int);
            parCodPelicula.Direction = System.Data.ParameterDirection.Output;
            cmdPelicula.Parameters.Add(parCodPelicula);
            cmdPelicula.Parameters.Add(new SqlParameter("@titulo", pelicula.Titulo));
            cmdPelicula.Parameters.Add(new SqlParameter("@subtitulada", pelicula.Subtitulada));
            cmdPelicula.Parameters.Add(new SqlParameter("@estreno", pelicula.Estreno));
            cmdPelicula.Parameters.Add(new SqlParameter("@genero_Id", pelicula.Genero.GeneroId));

            SqlCommand cmdPeliculaActor = connection.CreateCommand();
            cmdPeliculaActor.CommandText = "InsertPeliculaActor";
            cmdPeliculaActor.CommandType = System.Data.CommandType.StoredProcedure;

            SqlTransaction transaction;

            transaction = connection.BeginTransaction();

            cmdPeliculaActor.Connection = connection;
            cmdPeliculaActor.Transaction = transaction;
            cmdPelicula.Connection = connection;
            cmdPelicula.Transaction = transaction;

            try
            {
            
                cmdPelicula.ExecuteNonQuery();
                pelicula.PeliculaId = Int32.Parse(cmdPelicula.Parameters["@pelicula_id"].Value.ToString());


                List<Actor> actores = pelicula.Actores;
                foreach (Actor actor in actores)
                {
                    cmdPeliculaActor.Parameters.Add(new SqlParameter("@pelicula_id", pelicula.PeliculaId));
                    cmdPeliculaActor.Parameters.Add(new SqlParameter("@actor_id", actor.ActorId));
                    cmdPeliculaActor.ExecuteNonQuery();
                    cmdPeliculaActor.Parameters.Clear();
                }
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw;
            }
        } 
    } 

}