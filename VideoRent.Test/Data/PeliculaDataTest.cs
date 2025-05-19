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
        var genero = new Genero { GeneroId = 35, NombreGenero = "Action" };
        var actor1 = new Actor { ActorId = 1, NombreActor = "Tom", ApellidosActor = "Holland" };
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
    [Test]
    public async Task GetPeliculasPorTitulo_ReturnValues()
    {
        var peliculaData = new PeliculaData(this.connectionString);
        List<Pelicula> peliculas = await peliculaData.GetPeliculasPorTitulo("Pe");
        Assert.That(peliculas.Count, Is.GreaterThan(0));
    }
}