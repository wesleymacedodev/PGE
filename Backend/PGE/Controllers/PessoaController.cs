using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;
using System.Runtime.CompilerServices;

namespace PGE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PessoaController : Controller
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;

        public PessoaController(IPessoaRepository pessoaRepository, IMapper mapper, ILoginRepository loginRepository)
        {
            _pessoaRepository = pessoaRepository;
            _mapper = mapper;
            _loginRepository = loginRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> SelecionarPessoas() 
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var pessoas = await _pessoaRepository.SelecionarTodos();
            var pessoasDTO = _mapper.Map<IEnumerable<PessoaDTO>>(pessoas);
            return Ok(pessoasDTO);
        }

        [HttpGet("Info")]
        public async Task<ActionResult<Pessoa>> Pessoa()
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userId = await _loginRepository.SelecionarPelaPK(jwtId);
            var pessoa = await _pessoaRepository.SelecionarPelaPK(userId.PessoaId);
            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada!");
            }
            var pessoaDTO = _mapper.Map<PessoaDTO>(pessoa);
            return Ok(pessoaDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarPessoa(int id)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var pessoa = await _pessoaRepository.SelecionarPelaPK(id);

            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada!");
            }

            var pessoaDTO = _mapper.Map<PessoaDTO>(pessoa);

            return Ok(pessoaDTO);
        }


        [HttpPost]
        public async Task<ActionResult> CadastrarPessoa(PessoaDTO pessoaDTO)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            Pessoa pessoa = _mapper.Map <Pessoa>(pessoaDTO);
            _pessoaRepository.Incluir(pessoa);
            if (await _pessoaRepository.Salvar())
            {
                return Ok("Pessoa cadastrada com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao cadastrar a pessoa!");
        }
        [HttpPut]
        public async Task<ActionResult> AlterarPessoa(PessoaDTO pessoaDTO)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            Pessoa pessoa = _mapper.Map<Pessoa>(pessoaDTO);
            _pessoaRepository.Alterar(pessoa);
            if (await _pessoaRepository.Salvar())
            {
                return Ok("Pessoa alterada com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar a pessoa!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirPessoa(int id) 
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var cliente = await _pessoaRepository.SelecionarPelaPK(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado!");
            }

            _pessoaRepository.Excluir(cliente);
            if (await _pessoaRepository.Salvar())
            {
                return Ok("Pessoa excluida com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir a pessoa!");
        }
    }
}
