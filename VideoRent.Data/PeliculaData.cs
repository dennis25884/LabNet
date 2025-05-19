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
    public async Task<List<Pelicula>> GetPeliculasPorTitulo(string titulo)
    {
        var peliculas = new Dictionary<int, Pelicula>();

        string query = @"
    SELECT p.pelicula_id, p.titulo,
    p.subtitulada, p.estreno,
    g.genero_id, g.nombre_genero,
    a.actor_id, a.nombre_actor, a.apellidos_actor
    FROM Pelicula p
    JOIN Genero g ON p.genero_id = g.genero_id
    JOIN PeliculaActor pa ON p.pelicula_id = pa.pelicula_id
    JOIN Actor a ON a.actor_id = pa.actor_id
    WHERE p.titulo LIKE @title";

        await using var connection = new SqlConnection(this.connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@title", $"%{titulo}%"); // Corregida la interpolación

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int peliculaId = reader.GetInt32(reader.GetOrdinal("pelicula_id"));
            if (!peliculas.TryGetValue(peliculaId, out var pelicula))
            {
                pelicula = new Pelicula
                {
                    PeliculaId = peliculaId,
                    Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                    Subtitulada = reader.GetBoolean(reader.GetOrdinal("subtitulada")),
                    Estreno = reader.GetBoolean(reader.GetOrdinal("estreno")),
                    Genero = new Genero
                    {
                        GeneroId = reader.GetInt32(reader.GetOrdinal("genero_id")),
                        NombreGenero = reader.GetString(reader.GetOrdinal("nombre_genero"))
                    },
                    Actores = new List<Actor>()
                };
                peliculas.Add(peliculaId, pelicula);
            }

            var actorId = reader.GetInt32(reader.GetOrdinal("actor_id"));
            if (actorId > 0)
            {
                var actor = new Actor
                {
                    ActorId = actorId,
                    NombreActor = reader.GetString(reader.GetOrdinal("nombre_actor")),
                    ApellidosActor = reader.GetString(reader.GetOrdinal("apellidos_actor"))
                };
                pelicula.Actores.Add(actor);
            }
        }

        return peliculas.Values.ToList();
    }

}