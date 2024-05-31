using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGE.DTOs;
using PGE.Interfaces;
using PGE.Models;
using PGE.Repositories;
using PGE.Services;
using System.Security.Cryptography;
using System.Text;

namespace PGE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticate _autheticateService;

        public LoginController(ILoginRepository loginRepository, IMapper mapper, IAuthenticate authenticateService)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
            _autheticateService = authenticateService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserToken>> Incluir(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Dados invalidos");
            }

            var nomeExiste = await _autheticateService.UserExists(loginDTO.Nome);

            if (nomeExiste) 
            {
                return BadRequest("Nome em uso!");
            }
            Login login = _mapper.Map<Login>(loginDTO);

            if (loginDTO.Password != null)
            {
                using var hmac = new HMACSHA512();
                byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                byte[] passwordSalt = hmac.Key;
                login.PasswordHash = passwordHash;
                login.PasswordSalt = passwordSalt;
            }

            _loginRepository.Incluir(login);
            if (!await _loginRepository.Salvar()) {
                return BadRequest("Erro ao cadastrar");
            }

            var token = _autheticateService.GenerateToken(loginDTO.Id, loginDTO.Nome);

            return new UserToken
            {
                Token = token
            };

        }
        [HttpPost]
        public async Task<ActionResult<UserToken>> Selecionar(LoginModel loginModel)
        {
            var existe = await _autheticateService.UserExists(loginModel.Nome);
            if (!existe)
            {
                return Unauthorized("Login não existe.");
            }
            var result = await _autheticateService.AuthenticateAsync(loginModel.Nome, loginModel.Password);
            if (!result)
            {
                return Unauthorized("Nome ou senha invalido.");
            }

            var usuario = await _autheticateService.GetUserByName(loginModel.Nome);

            var token = _autheticateService.GenerateToken(usuario.Id, usuario.Nome);

            return new UserToken {
                Token = token
            };
        }

        [HttpGet("Validate")]
        [Authorize]
        public ActionResult ValidarToken()
        {
            return Ok("Token valido!");
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<ActionResult> AlterarSenha(LoginChangePassword loginChangePassword)
        {
            if (loginChangePassword == null)
            {
                return BadRequest("Dados inválidos");
            }

            var jwtId = int.Parse(User.FindFirst("id").Value);
            var userFind = await _loginRepository.SelecionarPelaPK(jwtId);
            var userName = userFind.Nome;
            var user = await _autheticateService.GetUserByName(userName);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var currentPasswordIsValid = await _autheticateService.AuthenticateAsync(userName, loginChangePassword.CurrentPassword);

            if (!currentPasswordIsValid)
            {
                return Unauthorized("Senha atual inválida");
            }

            using var hmac = new HMACSHA512();
            byte[] newPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginChangePassword.NewPassword));
            byte[] newPasswordSalt = hmac.Key;

            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;

            _loginRepository.Alterar(user);

            if (!await _loginRepository.Salvar())
            {
                return BadRequest("Erro ao alterar a senha");
            }

            return Ok("Senha alterada com sucesso");
        }
    }
}
