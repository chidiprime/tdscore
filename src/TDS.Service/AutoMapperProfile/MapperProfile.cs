using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Core.Entities;
using TDS.Core.ViewModel;

namespace TDS.Service.AutoMapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TransactionLedgerDTO, TransactionLedger>()
            .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<TransactionResponseDTO, WalletTransaction>().      
            ReverseMap();
            CreateMap<LedgerResponseDTO, TransactionLedger>().
            ReverseMap();
        }
       
    }
}
