﻿using AutoMapper;
using Inventory.Core.ViewModel;
using Inventory.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Mapping
{
    public class DTOtoEntity : Profile
    {
        public DTOtoEntity()
        {
            CreateMap<CatalogDTO, Catalog>();
            CreateMap<CatalogEditDTO, Catalog>();

            CreateMap<TeamDTO, Team>();
            CreateMap<TeamEditDTO, Team>();
            CreateMap<TeamWithMembersDTO, Team>();

            CreateMap<AppUserDTO, AppUser>();
            CreateMap<AppUserEditDTO, AppUser>();

            CreateMap<ItemDTO, Item>();
            CreateMap<ItemDetailDTO, Item>();
            CreateMap<ItemEditDTO, Item>();

            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderDetailCreateDTO, OrderDetail>();

            CreateMap<ExportCreateDTO, Export>();
            CreateMap<ExportDetailCreateDTO, ExportDetail>();

            CreateMap<ReceiptCreateDTO, Receipt>();
            CreateMap<ReceiptDetailCreateDTO, ReceiptDetail>();
            
            CreateMap<TicketCreateDTO, Ticket>();
            CreateMap<TicketDetailCreateDTO, TicketDetail>();
        }
    }
}
