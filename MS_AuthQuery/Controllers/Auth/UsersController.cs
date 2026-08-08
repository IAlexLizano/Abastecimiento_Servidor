using AuthApplication.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace MS_AuthQuery.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Información del usuario</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var query = new GetUserByIdQuery { UserId = id };
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Información del usuario</returns>
        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var query = new GetUserByUsernameQuery { Username = username };
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Obtiene todos los usuarios activos
        /// </summary>
        /// <returns>Lista de usuarios activos</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Obtiene todos los usuarios que sean Operarios de Apertura
        /// </summary>
        /// <returns>Lista de usuarios con rol Operario de Apertura</returns>
        [HttpGet("opening-operators")]
        public async Task<IActionResult> GetOpeningOperators()
        {
            var query = new GetUsersByRoleIdQuery { RoleId = UserRoles.OperarioApertura };
            return Ok(await Mediator.Send(query));
        }
    }
}

