﻿using Inventory.Core.ViewModel;
using Inventory.Core.Common;
using Inventory.Core.Response;
using Inventory.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogServices _catalogServices;

        public CatalogController(ICatalogServices catalogServices)
        {
            _catalogServices = catalogServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CatalogDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCatalog()
        {
            var result = await _catalogServices.GetAll();
            
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CatalogDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCatalog(int id)
        {
            var result = await _catalogServices.GetById(id);

            if (result.Status == ResponseStatus.STATUS_SUCCESS)
                return Ok(result.Data);
            else
                return NotFound(result.Messages);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CatalogDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CatalogByName(string value)
        {
            var result = await _catalogServices.SearchCatalog(value);

            if (result.Status == ResponseStatus.STATUS_SUCCESS)
                return Ok(result.Data);
            else
                return NotFound(result.Messages);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultResponse<CatalogDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCatalog(CatalogEditDTO dto)
        {
            var result = await _catalogServices.CreateCatalog(dto);
            
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ResultResponse<CatalogDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCatalog(int id, CatalogEditDTO dto)
        {
            var result = await _catalogServices.UpdateCatalog(id, dto);

            if (result.Status == ResponseStatus.STATUS_SUCCESS)
                return Ok(result);
            else
                return NotFound(result.Messages);
        }


        [HttpDelete("{id:int}")]

        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCatalog(int id)
        {
            var result = await _catalogServices.DeleteCatalog(id);

            if (result.Status == ResponseStatus.STATUS_SUCCESS)
                return Ok(result.Messages);
            else
                return NotFound(result.Messages);
        }
    }
}
