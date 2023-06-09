﻿using Inventory.Core.Response;
using Inventory.Core.ViewModel;
using Inventory.Repository.Model;


namespace Inventory.Services.IServices
{
    public interface ICatalogServices
    {
        Task<ResultResponse<IEnumerable<CatalogDTO>>> GetAll();
        Task<ResultResponse<CatalogDTO>> GetById(int id);
        Task<ResultResponse<IEnumerable<CatalogDTO>>> SearchCatalog(string filter);
        Task<ResultResponse<CatalogDTO>> CreateCatalog(CatalogEditDTO dto);
        Task<ResultResponse<CatalogDTO>> UpdateCatalog(int id, CatalogEditDTO dto);
        Task<ResultResponse<CatalogDTO>> DeleteCatalog(int id);
    }
}
