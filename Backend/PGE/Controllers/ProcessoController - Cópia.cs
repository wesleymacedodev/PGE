using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;

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

        public ProcessoController(IProcessoRepository processoRepository, IMapper mapper, ILoginRepository loginRepository)
        {
            _processoRepository = processoRepository;
            _mapper = mapper;
            _loginRepository = loginRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processo>>> SelecionarProcessos()
        {
            var processos = await _processoRepository.SelecionarTodos();
            var processoDTO = _mapper.Map<IEnumerable<ProcessoDTO>>(processos);
            return Ok(processoDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarProcesso(int id)
        {
            var processo = await _processoRepository.SelecionarPelaPK(id);

            if (processo == null)
            {
                return NotFound("Processo não encontrado!");
            }

            var processoDTO = _mapper.Map<ProcessoDTO>(processo);

            return Ok(processoDTO);
        }


        [HttpPost]
        public async Task<ActionResult> CadastrarProcesso(ProcessoDTO processoDTO)
        {
            Processo processo = _mapper.Map<Processo>(processoDTO);
            _processoRepository.Incluir(processo);
            if (await _processoRepository.Salvar())
            {
                return Ok("Processo cadastrado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao cadastrar o processo!");
        }
        [HttpPut]
        public async Task<ActionResult> AlterarPessoa(ProcessoDTO processoDTO)
        {
            Processo processo = _mapper.Map<Processo>(processoDTO);
            _processoRepository.Alterar(processo);
            if (await _processoRepository.Salvar())
            {
                return Ok("Processo alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar o processo!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirPessoa(int id)
        {

            var processo = await _processoRepository.SelecionarPelaPK(id);

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
