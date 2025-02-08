using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using System.Collections;
using System.Linq;

namespace Demo.PL.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<EmpViewModel, Employee>().ReverseMap();
            CreateMap<DepartmentViewModel, Department>().ReverseMap();

        }
    }
}
