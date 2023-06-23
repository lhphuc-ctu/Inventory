﻿using Inventory.Core.Common;
using Inventory.Core.Extensions;
using Inventory.Core.Response;
using Inventory.Core.ViewModel;
using Inventory.Repository.Model;
using Inventory.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IRedisCacheService _cacheService;
        private const string redisKey = "Item";

        public ItemController(IItemService itemService, IRedisCacheService cacheService)
        {
            _itemService = itemService;
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ItemDetailDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListItem()
        {
            if (_cacheService.TryGetCacheAsync(redisKey,out IEnumerable<ItemDetailDTO> items))
            {
                return Ok(items);
            }
            else
            {
                var result = await _itemService.GetAll();

                if (result.Status == ResponseStatus.STATUS_SUCCESS)
                {
                    await _cacheService.SetCacheAsync(redisKey, result.Data);
                    return Ok(result.Data);
                }

                return NotFound(result.Messages);
            }
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(ItemDetailDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseMessage>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItem(Guid id)
        {
            if (_cacheService.TryGetCacheAsync(redisKey + id, out ItemDetailDTO items))
            {
                return Ok(items);
            }
            else
            {
                var result = await _itemService.GetById(id);

                if (result.Status == ResponseStatus.STATUS_SUCCESS)
                {
                    await _cacheService.SetCacheAsync(redisKey + id, result.Data);
                    return Ok(result.Data);
                }

                return NotFound(result.Messages);
            }
        }

        [HttpPost]
        [Authorize(Roles =InventoryRoles.IM)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ResponseMessage>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ResponseMessage>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateItem(ItemEditDTO item)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState.GetErrorMessages()); }

            var accessToken = await HttpContext.GetAccessToken();

            var result = await _itemService.CreateItem(accessToken, item);

            await _cacheService.RemoveCacheAsync(redisKey);

            return result.Status == ResponseStatus.STATUS_SUCCESS ?
                Created("item/" + result.Data!.Id, result.Messages) : NotFound(result.Messages);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = InventoryRoles.IM)]
        [ProducesResponseType(typeof(ResultResponse<ItemDetailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ResponseMessage>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItem(Guid id, ItemEditDTO item)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState.GetErrorMessages()); }

            var accessToken = await HttpContext.GetAccessToken();

            var result = await _itemService.UpdateItem(accessToken, id, item);

            return result.Status == ResponseStatus.STATUS_SUCCESS ?
                    Ok(result) : NotFound(result.Messages);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = InventoryRoles.IM)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var accessToken = await HttpContext.GetAccessToken();

            var result = await _itemService.DeleteItem(accessToken, id);

            return result.Status == ResponseStatus.STATUS_SUCCESS ?
                    Ok(result.Messages) : NotFound(result.Messages);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(List<ItemDetailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseMessage>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _itemService.SearchByName(name);

            return result.Status == ResponseStatus.STATUS_SUCCESS ? 
                    Ok(result.Data) : NotFound(result.Messages);
        }
    }
}
