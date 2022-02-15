using AutoMapper;
using CikguHub.Api.Model;
using CikguHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CaseRenterDeposit, CaseRenterDepositModel>();
            CreateMap<CaseRenterDepositModel, CaseRenterDeposit>();
        }
    }
}
