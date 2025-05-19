using VideoRent.Domain;

namespace VideoRent.Test.Data;
[TestFixture]
public class PeliculaDataTest
{
    private string connectionString;
    [SetUp]
    public void Setup()
    {
        this.connectionString = "Data Source =163.178.173.130;Initial Catalog=VideoRentC37483-I2025; Persist Security Info = True;" +
     "User ID =Lenguajes;Password =lenguajesparaiso2025; Encrypt = False;TrustServerCertificate=True;";
    }

    [Test]
    public void Insertar_ValidPelicula_InsertsSuccessfully()
    {
        //Arrange
        var peliculaData = new PeliculaData(this.connectionString);

        //Datos de prueba
        var genero = new Genero { GeneroId = 1, NombreGenero = "Accion" };
        var actor1 = new Actor { ActorId = 1, NombreActor = "Actor 1", ApellidosActor = "Actor 1" };
        var peliculaToInsert = new Pelicula
        {
            Titulo = "Pelicula de Prueba",
            Subtitulada = true,
            Estreno = false,
            Genero = genero,
            Actores = new List<Actor> { actor1 }
        };
        //act 
        Assert.DoesNotThrow(() => peliculaData.Insertar(peliculaToInsert),
            "No se pudo insertar la pelicula en la base de datos.");
        Assert.That(peliculaToInsert.PeliculaId, Is.GreaterThan(0), "El ID de la pelicula no fue asignado correctamente.");
    }
}