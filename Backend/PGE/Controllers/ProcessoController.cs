using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;
using PGE.Repositories;
using PGE.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace PGE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProcessoController : Controller
    {
        private readonly IProcessoRepository _processoRepository;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;
        private readonly IPessoaRepository _pessoaRepository;

        public ProcessoController(IProcessoRepository processoRepository, IMapper mapper, ILoginRepository loginRepository, IPessoaRepository pessoaRepository)
        {
            _processoRepository = processoRepository;
            _mapper = mapper;
            _loginRepository = loginRepository;
            _pessoaRepository = pessoaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processo>>> SelecionarProcessos()
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var processos = await _processoRepository.SelecionarTodos();
            var processoDTO = _mapper.Map<IEnumerable<ProcessoDTO>>(processos);
            return Ok(processoDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarProcesso(int id)
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            var processo = await _processoRepository.SelecionarPelaPK(id);

            if (processo == null)
            {
                return NotFound("Processo não encontrado!");
            }

            var processoDTO = _mapper.Map<ProcessoDTO>(processo);

            return Ok(processoDTO);
        }

        [HttpGet("List")]
        public async Task<ActionResult> ListarProcessos()
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            var pessoa = await _pessoaRepository.SelecionarPelaPK(userFind.PessoaId);
            var processos = await _processoRepository.SelecionarTodos();
            var processoDTO = _mapper.Map<IEnumerable<ProcessoDTO>>(processos);
            var processosDTO = new List<ProcessoDTO>();
            
            foreach (ProcessoDTO process in processoDTO)
            {
                if (userFind.PessoaId == process.ParteId)
                {
                    processosDTO.Add(process);
                }  else if (pessoa.Oab != null) {
                    processosDTO.Add(process);
                }
            };
            return Ok(processosDTO);

        }


        [HttpPost]
        public async Task<ActionResult> CadastrarProcesso(ProcessoDTO processoDTO)
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            Pessoa pessoa = await _pessoaRepository.SelecionarPelaPK(userFind.PessoaId);
            if (pessoa.Oab == null) {
                return BadRequest("Você não pode cadastrar um processo!");
            }
            processoDTO.ResponsavelId = pessoa.Id;
            Processo processo = _mapper.Map<Processo>(processoDTO);
            _processoRepository.Incluir(processo);
            if (await _processoRepository.Salvar())
            {
                return Ok("Processo cadastrado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao cadastrar o processo!");
        }
        [HttpPut]
        public async Task<ActionResult> AlterarProcesso(ProcessoDTO processoDTO)
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            Processo processo = _mapper.Map<Processo>(processoDTO);
            if (userFind.PessoaId != processo.ResponsavelId)
            {
                return BadRequest("Você não pode alterar esse processo!");
            }
            _processoRepository.Alterar(processo);
            if (await _processoRepository.Salvar())
            {
                return Ok("Processo alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar o processo!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirProcesso(int id)
        {
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            Pessoa pessoa = await _pessoaRepository.SelecionarPelaPK(userFind.PessoaId);
            var processo = await _processoRepository.SelecionarPelaPK(id);
            if (processo.ResponsavelId != userFind.PessoaId)
            {
                return BadRequest("Você não é dono desse processo!");
            }

            if (processo == null)
            {
                return NotFound("Processo não encontrado!");
            }

            _processoRepository.Excluir(processo);
            if (await _processoRepository.Salvar())
            {
                return Ok("Processo excluido com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir o processo!");
        }
    }
}
