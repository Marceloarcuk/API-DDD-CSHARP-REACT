using AutoMapper;
using Domain.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IMessage _IMessage;

        public MessageController(IMapper IMapper, IMessage IMessage)
        {
            _IMapper = IMapper;
            _IMessage = IMessage;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<List<Notifies>> Add(MessageViewModel message)
        {
            message.UserId = await RetornarIdUsuarioLogado();
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Add(messageMap);
            return messageMap.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Update")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Update(messageMap);
            return messageMap.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Delete")]
        public async Task<List<Notifies>> Delete(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Delete(messageMap);
            return messageMap.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetEntityById")]
        public async Task<MessageViewModel> GetEntityById(Message message)
        {
            message = await _IMessage.GetEntityById(message.Id);
            var messageMap = _IMapper.Map<MessageViewModel>(message);
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/List")]
        public async Task<List<MessageViewModel>> List()
        {
            var mensagens = await _IMessage.List();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }



        private async Task<string> RetornarIdUsuarioLogado()
        {
            if (User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }

            return string.Empty;

        }


    }
}
/*using AutoMapper;
using Domain.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _iMapper;
        private readonly IMessage _iMessage;
        public MessageController(IMapper iMapper, IMessage iMessage)
        {
            _iMapper = iMapper;
            _iMessage = iMessage;
        }
        
        
        //[AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<List<Notifies>> Add(MessageViewModel message)
        {
            message.IdUser = await RetornarIdUsuarioLogado();
            var messageMap = _iMapper.Map<Message>(message);
            await _iMessage.Add(messageMap);
            return messageMap.Notificacoes;

        }


        //[AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Authorize]
        [Produces("application/json")]
        [HttpPut("/api/Update")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _iMapper.Map<Message>(message);
            await _iMessage.Update(messageMap);
            return messageMap.Notificacoes;
        }

        //[AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Authorize]
        [Produces("application/json")]
        [HttpDelete("/api/Delete")]
        public async Task<List<Notifies>> Delete(MessageViewModel message)
        {
            var messageMap = _iMapper.Map<Message>(message);
            await _iMessage.Delete(messageMap);
            return messageMap.Notificacoes;
        }

        //[AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Authorize]
        [Produces("application/json")]
        [HttpGet("/api/GetEntityById")]
        public async Task<MessageViewModel> GetEntityById(Message message)
        {
            message = await _iMessage.GetEntityById(message.Id);
            var messageMap = _iMapper.Map<MessageViewModel>(message);
            return messageMap;
        }

        //[AllowAnonymous]     // QUALQUER USUARIO PODE ACESSAR
        [Authorize]
        [Produces("application/json")]
        [HttpGet("/api/List")]
        public async Task<List<MessageViewModel>> List()
        {
            var mensagens = await _iMessage.List();
            var messageMap = _iMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }


        private async Task<string> RetornarIdUsuarioLogado()
        {
            if (User != null)
            {
                try
                {
                    var idUser = User.FindFirst("idUser");
                    return idUser.Value;
                } catch (Exception ex)
                {
                    return "1";
                }
                //var idUser = User.FindFirst("idUser");
                //return idUser.Value;
            }

            return string.Empty;
        }
    }
}*/