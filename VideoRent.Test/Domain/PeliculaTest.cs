using VideoRent.Domain;
using NUnit.Framework;

namespace VideoRent.Test {

[TestFixture]
public class PeliculaTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Pelicula_Constructor_Con_PropiedadesInicializadas()
    {
        // Arrange
        int peliculaIdEsperado = 123;
        string tituloEsperado = "Ladrona de libros";
        bool estrenoEsperado = false;
        bool subtituladaEsperado = false;
        Genero genero = new Genero();
        genero.NombreGenero = "Drama";
        genero.GeneroId = 100;
       List<Actor> actors = new List<Actor>();
        Actor actor = new Actor();
        actor.ActorId = 10;
        actor.ApellidosActor = "Nélisse";
        actor.NombreActor = "Sophie";
        actors.Add(actor);

        // Act
        Pelicula pelicula = new Pelicula(peliculaIdEsperado, tituloEsperado, 
            estrenoEsperado, subtituladaEsperado, genero, actors);

        // Assert
        
        Assert.That(peliculaIdEsperado==pelicula.PeliculaId);
        Assert.That(tituloEsperado==pelicula.Titulo);
        Assert.That(estrenoEsperado==pelicula.Estreno);
        Assert.That(subtituladaEsperado==pelicula.Subtitulada);
        // Todo más assertions para otras propiedades, como genero
    }
}
}
