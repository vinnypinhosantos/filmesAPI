using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{

	private int _id;
	[Key]
	[Required]
	public int Id
	{
		get { return _id; }
		set { _id = value; }
	}


	private string _titulo;
    [Required(ErrorMessage = "O título do filme é obrigatório.")]
    public string Titulo
	{
		get { return _titulo; }
		set { _titulo = value; }
	}

	private string _genero;
    [Required(ErrorMessage = "O gênero do filme é obrigatório.")]
	[MaxLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres.")]
    public string Genero
	{
		get { return _genero; }
		set { _genero = value; }
	}


	private int _duracao;
    [Required]
	[Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
    public int Duracao
	{
		get { return _duracao; }
		set { _duracao = value; }
	}

}
