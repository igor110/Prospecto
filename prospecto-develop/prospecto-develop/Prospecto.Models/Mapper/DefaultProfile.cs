using AutoMapper;
using Prospecto.Models.DTO;
using Prospecto.Models.Extensions;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;

namespace Prospecto.Models.Mapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            #region USER
            CreateMap<UserInfo, UserInfo>().Ignore(x => x.Id);
            CreateMap<UserDTO, UserInfo>().Ignore(x => x.Id).Ignore(x => x.IsVisible);
            CreateMap<UserViewModel, UserDTO>();
            #endregion

            #region COMPANY
            CreateMap<CompanyInfo, CompanyInfo>().Ignore(x => x.Id);
            CreateMap<CompanyDTO, CompanyInfo>().Ignore(x => x.Id);
            CreateMap<CompanyViewModel, CompanyDTO>();
            #endregion

            #region BRANCH
            CreateMap<BranchInfo, BranchInfo>().Ignore(x => x.Id);
            CreateMap<BranchDTO, BranchInfo>().Ignore(x => x.Id);
            CreateMap<BranchViewModel, BranchDTO>();
            #endregion

            #region ATTENDANCE
            CreateMap<AttendanceInfo, AttendanceInfo>().Ignore(x => x.Id);
            CreateMap<AttendanceDTO, AttendanceInfo>().Ignore(x => x.Id);
            CreateMap<AttendanceInfo, AttendanceDTO>();
            CreateMap<AttendanceViewModel, AttendanceDTO>();
            CreateMap<AttendanceViewModel, AttendanceDTO>().ReverseMap();
            #endregion

            #region CLIENT
            CreateMap<ClientInfo, ClientInfo>().Ignore(x => x.Id);
            CreateMap<ClientDTO, ClientInfo>().Ignore(x => x.Id);
            CreateMap<ClientInfo, ClientDTO>();
            CreateMap<ClientViewModel, ClientDTO>();
            #endregion
        }
    }
}
