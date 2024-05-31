using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;
using PGE.Repositories;

namespace PGE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DistribuirController : Controller
    {
        private readonly IDistribuirRepository _distribuirRepository;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IProcessoRepository _processoRepository;

        public DistribuirController(IDistribuirRepository distribuirRepository, IMapper mapper, ILoginRepository loginRepository, IPessoaRepository pessoaRepository, IProcessoRepository processoRepository)
        {
            _distribuirRepository = distribuirRepository;
            _mapper = mapper;
            _loginRepository = loginRepository;
            _pessoaRepository = pessoaRepository;
            _processoRepository = processoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distribuir>>> SelecionarDistribuir()
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin
            var distribuir = await _distribuirRepository.SelecionarTodos();
            var distribuirDTO = _mapper.Map<IEnumerable<DistribuirDTO>>(distribuir);
            return Ok(distribuirDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarDistribuir(int id)
        {

            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var distribuir = await _distribuirRepository.SelecionarPelaPK(id);

            if (distribuir == null)
            {
                return NotFound("Documento não encontrado!");
            }

            var distribuirDTO = _mapper.Map<DistribuirDTO>(distribuir);

            return Ok(distribuirDTO);
        }


        [HttpPost]
        public async Task<ActionResult> CadastrarDistribuir(DistribuirDTO distribuirDTO)
        {

            var distribuir = _mapper.Map<Distribuir>(distribuirDTO);
            _distribuirRepository.Incluir(distribuir);
            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            var processo = await _processoRepository.SelecionarPelaPK(distribuir.ProcessoId);
            if(processo.ResponsavelId != userFind.PessoaId)
            {
                return BadRequest("Você não pode distribuir esse processo!");
            }
            if (processo != null)
            {
                processo.ResponsavelId = distribuirDTO.ResponsavelNovoId;
                _processoRepository.Alterar(processo);
            }

            if (await _distribuirRepository.Salvar() && await _processoRepository.Salvar())
            {
                return Ok("Distribuir cadastrado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao cadastrar o Documento!");
        }

        [HttpPut]
        public async Task<ActionResult> AlterarDistribuir(DistribuirDTO distribuirDTO)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            Distribuir distribuir = _mapper.Map<Distribuir>(distribuirDTO);
            _distribuirRepository.Alterar(distribuir);
            if (await _distribuirRepository.Salvar())
            {
                return Ok("Distribuir alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar o Distribuir!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirDistribuir(int id)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var distribuir = await _distribuirRepository.SelecionarPelaPK(id);

            if (distribuir == null)
            {
                return NotFound("Distribuir não encontrado!");
            }

            _distribuirRepository.Excluir(distribuir);
            if (await _distribuirRepository.Salvar())
            {
                return Ok("Distribuir excluido com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir o Distribuir!");
        }
    }
}
