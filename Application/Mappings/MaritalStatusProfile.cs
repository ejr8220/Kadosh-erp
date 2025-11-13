using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using AutoMapper;
using Domain.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MaritalStatusProfile : Profile 
    { 
        public MaritalStatusProfile() 
        { 
            CreateMap<MaritalStatusRequestDto, MaritalStatus>(); 
            CreateMap<MaritalStatus, MaritalStatusResponseDto>(); 
        } 
    }

}
