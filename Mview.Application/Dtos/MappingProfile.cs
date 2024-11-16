using AutoMapper;
using Mview.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LicensePlatesErrorsModel, LicensePlateErrorsDto>()
                .ForMember(to => to.ID, from => from.MapFrom(obj => obj.ID));
            CreateMap<LicensePlateErrorsDto, LicensePlatesErrorsModel>()
              .ForMember(to => to.ID, from => from.MapFrom(obj => obj.ID));

            CreateMap<LicensePlatesErrorsModel, LicensePlateErrorsDto>()
                .ForMember(to => to.ID, from => from.MapFrom(obj => obj.ID))
                .ForMember(to => to.Date, from => from.MapFrom(obj => obj.DateTime1));
           // var x = _mapper.Map<List<LicensePlateErrorsDto>>(licensePlatesObjects);

        }
    }
}
