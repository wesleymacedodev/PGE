using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;
using PGE.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PGE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentoController : Controller
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;
        private readonly IProcessoRepository _processoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly string _filePath;

        public DocumentoController(IDocumentoRepository documentoRepository, IMapper mapper, ILoginRepository loginRepository, IConfiguration configuration, IProcessoRepository processoRepository, IPessoaRepository pessoaRepository)
        {
            _documentoRepository = documentoRepository;
            _mapper = mapper;
            _loginRepository = loginRepository;
            _processoRepository = processoRepository;
            _filePath = configuration["FileSettings:FilePath"];
            _processoRepository = processoRepository;
            _pessoaRepository = pessoaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoDTO>>> SelecionarDocumentos()
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin
            var documentos = await _documentoRepository.SelecionarTodos();
            var documentosDTO = _mapper.Map<IEnumerable<DocumentoDTO>>(documentos);
            return Ok(documentosDTO);
        }

        [HttpGet("Download/{id}")]
        public async Task<ActionResult> BaixarDocumento(int id)
        {

            var documento = await _documentoRepository.SelecionarPelaPK(id);
            
            if (documento == null)
            {
                return NotFound("Documento não encontrado!");
            }

            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            var pessoa = await _pessoaRepository.SelecionarPelaPK(usuarioLogin.PessoaId);
            var processo = await _processoRepository.SelecionarPelaPK(documento.ProcessoId);
            if (pessoa.Oab == null && processo.ParteId != pessoa.Id)
            {
                return BadRequest("Você não consegue baixar esse arquivo!");
            }

            var filePath = documento.Caminho;

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Arquivo não encontrado no servidor!");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoDTO>> SelecionarProcesso(int id)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            var documento = await _documentoRepository.SelecionarPelaPK(id);

            if (documento == null)
            {
                return NotFound("Documento não encontrado!");
            }

            var documentoDTO = _mapper.Map<DocumentoDTO>(documento);

            return Ok(documentoDTO);
        }

        [HttpGet("Processo/{id}")]
        public async Task<ActionResult<IEnumerable<DocumentoDTO>>> SelecionarDocumentoProcesso(int id)
        {
            var documentos = await _documentoRepository.SelecionarTodos();
            var documentosDTO = _mapper.Map<IEnumerable<DocumentoDTO>>(documentos);

            var filtredDTO = new List<DocumentoDTO>();
            foreach (DocumentoDTO documento in documentosDTO)
            {
                if(documento.ProcessoId == id)
                {
                    filtredDTO.Add(documento);
                }
            }

            return Ok(filtredDTO);
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarDocumento([FromForm] int processoId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo selecionado");

            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var path = Path.Combine(_filePath, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var documentoDTO = new DocumentoDTO
            {
                Nome = fileName,
                Caminho = path,
                Extensao = fileExtension,
                ProcessoId = processoId
            };

            Documento documento = _mapper.Map<Documento>(documentoDTO);

            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            var processo = await _processoRepository.SelecionarPelaPK(documento.ProcessoId);

            if (processo.ResponsavelId != usuarioLogin.PessoaId && processo.ParteId != usuarioLogin.PessoaId)
            {
                return BadRequest("Você não é resposavel e nem faz parte desse processo!");
            }

            _documentoRepository.Incluir(documento);
            if (await _documentoRepository.Salvar())
            {
                return Ok("Documento cadastrado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao cadastrar o documento!");
        }

        [HttpPut]
        public async Task<ActionResult> AlterarDocumento(DocumentoDTO documentoDTO)
        {
            // Permissão de admin
            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            if (!usuarioLogin.Admin)
            {
                return BadRequest("Usuario não autorizado!");
            }
            // Permissão de admin

            Documento documento = _mapper.Map<Documento>(documentoDTO);
            _documentoRepository.Alterar(documento);
            if (await _documentoRepository.Salvar())
            {
                return Ok("Documento alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar o documento!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirDocumento(int id)
        {
            var documento = await _documentoRepository.SelecionarPelaPK(id);

            if (documento == null)
            {
                return NotFound("Documento não encontrado!");
            }

            var userId = int.Parse(User.FindFirst("id").Value);
            var usuarioLogin = await _loginRepository.SelecionarPelaPK(userId);
            var processo = await _processoRepository.SelecionarPelaPK(documento.ProcessoId);
            

            if (processo.ResponsavelId != usuarioLogin.PessoaId && processo.ParteId != usuarioLogin.PessoaId)
            {
                return BadRequest("Você não é proprietario desse documento!");
            }

            var filePath = documento.Caminho;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _documentoRepository.Excluir(documento);
            if (await _documentoRepository.Salvar())
            {
                return Ok("Documento excluído com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir o Documento!");
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats.officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
    };
        }
    }
}
