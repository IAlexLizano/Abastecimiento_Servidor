using AuthApplication.Features.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthCommand.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="command">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="command">Datos del usuario a actualizar</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            command.UserId = id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Desactiva un usuario
        /// </summary>
        /// <param name="id">ID del usuario a desactivar</param>
        /// <returns>Mensaje de confirmación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var command = new DeactivateUserCommand { UserId = id };
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Reactiva un usuario
        /// </summary>
        /// <param name="id">ID del usuario a reactivar</param>
        /// <returns>Mensaje de confirmación</returns>
        [HttpPut("{id}/reactivate")]
        public async Task<IActionResult> ReactivateUser(int id)
        {
            var command = new ReactivateUserCommand { UserId = id };
            return Ok(await Mediator.Send(command));
        }
    }
}
