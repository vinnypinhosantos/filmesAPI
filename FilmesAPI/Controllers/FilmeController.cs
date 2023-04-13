﻿using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessátios para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    /// <summary>
    /// Recupera filmes cadastrados no banco de dados
    /// </summary>
    /// <param name="skip">A partir de qual filme quer ser recuperado</param>
    /// <param name="take">Número de filmes a serem recuperados</param>
    /// <returns>IEnumerable ReadFilmeDto </returns>
    /// <response code="200">Caso a operação seja feita com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip=0, [FromQuery] int take=50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Recupera um filme cadastrado a partir de seu ID
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a operação seja feita com sucesso</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            return NotFound();
        }
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza todos os dados de um filme
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <param name="filmeDto">Objeto com os campos necessátios para atualização de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a operação seja feita com sucesso</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            return NotFound();
        }
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza um filme a partir de um dado
    /// </summary>
    /// <param name="id">Identificador Único do filme</param>
    /// <param name="patch">Objeto com os campos necessátios para atualização de um filme por PATCH</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a operação seja realizada com sucesso</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) { return NotFound(); }
        
        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme a partir de um id
    /// </summary>
    /// <param name="id">Identificador únido do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a operação seja realizada com sucesso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) { return NotFound(); }
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
