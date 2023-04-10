namespace FilmesAPI.Models;

public class Filme
{
	private string _titulo;

	public string Titulo
	{
		get { return _titulo; }
		set { _titulo = value; }
	}

	private string _genero;

	public string Genero
	{
		get { return _genero; }
		set { _genero = value; }
	}

	private int _duracao;

	public int Duracao
	{
		get { return _duracao; }
		set { _duracao = value; }
	}



}
