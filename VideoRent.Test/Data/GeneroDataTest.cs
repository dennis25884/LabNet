namespace VideoRent.Test;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using VideoRent.Data;
using VideoRent.Domain;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
[TestFixture]
public class GeneroDataTest
{
    private String connectionString;

    [SetUp]
    public void Setup()
    {
        //connectionString = ConfigurationManager.ConnectionStrings["VideoConnection"].ConnectionString.ToString();
        this.connectionString = "Data Source=AMENA\\SQLEXPRESS;Initial Catalog=videorent;User ID=rentinguser;Password=rentingUSER;Encrypt=False;Trust Server Certificate=True";
    }

    [Test]
    public async Task GetGeneros()
    {
        //Assert

       using (var connection = new SqlConnection(this.connectionString))
        {
            await connection.OpenAsync();
            // Clean up existing test data
            using (var deleteCommand = new SqlCommand("DELETE FROM PeliculaActor", connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }
            using (var deleteCommand = new SqlCommand("DELETE FROM Pelicula", connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }
            using (var deleteCommand = new SqlCommand("DELETE FROM Genero", connection))
            {
                await deleteCommand.ExecuteNonQueryAsync();
            }

            // Insert test data
            using (var insertCommand1 = new SqlCommand("INSERT INTO Genero (nombre_genero) VALUES ('Action'), ('Comedia')", connection))
            {
                await insertCommand1.ExecuteNonQueryAsync();
            }
        }
        GeneroData generoData = new GeneroData(this.connectionString);
        //Act
        IEnumerable<Genero> generos = await generoData.GetGeneros();
        Assert.That(generos.Count(), Is.EqualTo(2));
    }
}
