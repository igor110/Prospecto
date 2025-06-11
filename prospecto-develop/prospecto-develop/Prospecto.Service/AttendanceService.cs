using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prospecto.Models.DTO;
using Prospecto.Models.Enums;
using Prospecto.Models.Extensions;
using Prospecto.Models.Infos;
using Prospecto.Models.Result;
using Prospecto.Models.ViewModel;
using Prospecto.Repository;
using Prospecto.Respository.Interface;
using Prospecto.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prospecto.Service
{
    public class AttendanceService : AbstractServiceBase<AttendanceDTO, AttendanceInfo, IAttendanceRepository>, IAttendanceService
    {        
        private readonly IUserService _userService;

        public AttendanceService(
            IUserService userService,
            IAttendanceRepository repository,
            IMapper mapper) : base(repository, mapper)
        {            
            _userService = userService;
        }

        public IList<AttendanceViewModel> ListByFilters(AttendanceFiltersViewModel filters)
        {
            return _repository
                .GetQuery(x => x.Id > 0)
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.Branch)
                .WhereIf(filters.CompanyId > 0, x => x.CompanyId == filters.CompanyId)
                .WhereIf(filters.BranchId > 0, x => x.BranchId == filters.BranchId)
                .WhereIf(filters.UserId > 0, x => x.UserId == filters.UserId)
                .WhereIf(filters.Status > 0, x => x.Status == filters.Status)
                .WhereIf(filters.BeginDate.HasValue && filters.TypeDate == 1, x => x.DateRegistred >= filters.BeginDate.Value && x.DateRegistred <= filters.EndDate.Value)
                .WhereIf(filters.BeginDate.HasValue && filters.TypeDate == 2, x => x.DateReturn >= filters.BeginDate.Value && x.DateReturn <= filters.EndDate.Value)
                .WhereIf(filters.BeginDate.HasValue && filters.TypeDate == 3, x => x.DateClosed >= filters.BeginDate.Value && x.DateClosed <= filters.EndDate.Value)
                .Select(x => x.AsAttendanceViewMode()).ToList();
        }

        public AttendanceViewModel GetWithRelations(int id)
        {
            return _repository
                .GetQuery(x => x.Id == id)
                .Include(x => x.Client)
                .Select(x => x.AsAttendanceViewMode()).FirstOrDefault();
        }

        public async Task<bool> ImportAttendance(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            using var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var cont = 1;
            bool header = false;

            var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            using var begin = _repository.BeginTransaction();
            try
            {
                if (dataSet.Tables.Count > 0)
                {
                    var dtData = dataSet.Tables[0];

                    if (dtData.Columns[0].ToString().ToUpper().Equals("CLIENTE")
                        && dtData.Columns[1].ToString().ToUpper().Equals("TELEFONE")
                        && dtData.Columns[2].ToString().ToUpper().Equals("PRODUTO")
                        && dtData.Columns[3].ToString().ToUpper().Equals("VALOR")
                        && dtData.Columns[4].ToString().ToUpper().Equals("DATA RETORNO")
                        && dtData.Columns[5].ToString().ToUpper().Equals("OBSERVAÇÃO")
                        && dtData.Columns[6].ToString().ToUpper().Equals("IDCONSULTOR"))
                        header = true;
                    else
                        throw new Exception("Arquivo invalido");

                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        cont++;
                        if (string.IsNullOrEmpty(dtData.Rows[i][0].ToString()))
                            throw new Exception("Cliente da linha " + cont + " não econtrado!");

                        if (string.IsNullOrEmpty(dtData.Rows[i][1].ToString()))
                            throw new Exception("Telefone da linha " + cont + " não econtrado!");

                        if (string.IsNullOrEmpty(dtData.Rows[i][2].ToString()))
                            throw new Exception("Produto da linha " + cont + " não econtrado!");

                        if (string.IsNullOrEmpty(dtData.Rows[i][3].ToString()))
                            throw new Exception("Valor da linha " + cont + " não econtrado!");

                        if (string.IsNullOrEmpty(dtData.Rows[i][4].ToString()))
                            throw new Exception("Data do retorno " + cont + " não econtrado!");

                        if (string.IsNullOrEmpty(dtData.Rows[i][6].ToString()))
                            throw new Exception("Id do consultor " + cont + " não econtrado!");

                        var telefoneFormat = dtData.Rows[i][1].ToString();
                        if (telefoneFormat.Length < 10 && telefoneFormat.Length > 11) throw new Exception("Telefone da linha " + cont + " não está no formato esperado!");

                        if (telefoneFormat.Length == 10)
                            telefoneFormat = "(" + telefoneFormat.Substring(0, 2) + ")" + telefoneFormat.Substring(2, 4) + "-" + telefoneFormat.Substring(5);

                        if (telefoneFormat.Length == 11)
                            telefoneFormat = "(" + telefoneFormat.Substring(0, 2) + ")" + telefoneFormat.Substring(2, 5) + "-" + telefoneFormat.Substring(6);

                        var userId = Convert.ToInt32(dtData.Rows[i][6].ToString());

                        var result = _userService.Get(userId).Result;
                        if (result.Success && result.Value != null)
                        {
                            var user = result.Value;


                            var attendanceDto = new AttendanceDTO
                            {
                                NameClient = dtData.Rows[i][0].ToString(),
                                Telephone = telefoneFormat,
                                NameProduct = dtData.Rows[i][2].ToString(),
                                Value = Convert.ToDecimal(dtData.Rows[i][3].ToString()),
                                DateReturn = Convert.ToDateTime(dtData.Rows[i][4].ToString()),
                                Observation = dtData.Rows[i][5].ToString(),
                                DateRegistred = DateTime.Now,
                                Status = StatusAttendancesEnum.OPEN,
                                UserId = user.Id,
                                CompanyId = user.CompanyId.Value,
                                BranchId = user.BranchId,
                                ReschedulingOrigin = ReschedulingOriginEnum.IMPORT
                            };

                            await Insert(attendanceDto);
                        }
                        else
                        {
                            throw new Exception("Consultor da linha " + cont + " não encontrado!");
                        }
                    }
                }

                begin.Commit();
                return true;

            }
            catch (Exception ex)
            {
                begin.Rollback();
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Reschedule(int idAttendace, DateTime date, TimeSpan? time = null)
        {
            var obj = GetWithRelations(idAttendace);

            // Atualiza status do atendimento original para RESCHEDULED
            var dto = _mapper.Map<AttendanceDTO>(obj);
            dto.Status = StatusAttendancesEnum.RESCHEDULED;
            dto.ClientId = null;
            if (dto.BranchId == 0) dto.BranchId = null;

            await Update(idAttendace, dto);

            // Cria novo atendimento com status OPEN e nova data/hora
            var dtoNew = dto;
            dtoNew.Id = 0; // Força inserção de novo registro
            dtoNew.Status = StatusAttendancesEnum.OPEN;

            if (time.HasValue)
                dtoNew.DateReturn = date.Date.Add(time.Value);
            else
                dtoNew.DateReturn = date;

            dtoNew.ReschedulingOrigin = 0;

            if (dtoNew.BranchId == 0) dtoNew.BranchId = null;

            await Insert(dtoNew);
            return true;
        }



        public IList<NotificationViewModel> GetPendingNotifications(int userId)
        {
            var now = DateTime.Now;

            return _repository
                .GetQuery(x =>
                    x.NotifyAt != null &&
                    x.NotifyAt <= now &&
                    x.UserId == userId &&
                    x.DateReturn > now)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    Title = "Retorno Agendado",
                    Message = $"Cliente: {x.NameClient} - Retorno em {x.DateReturn:dd/MM/yyyy HH:mm}",
                    Date = x.NotifyAt.Value
                })
                .ToList();
        }


        public SalesChartDataViewModel SaleByConsultant(SalesChartDataLabelFiltersViewModel filters)
        {
            var salesChartObj = new SalesChartDataViewModel();

            filters.BeginDate = DateTime.Now;
            filters.EndDate = DateTime.Now.AddMonths(-4);
            string[] arr2 = Array.Empty<string>();
            decimal[] data = Array.Empty<decimal>();

            var lstSalesChartLabel = new List<SalesChartDataLabelViewModel>();
            var users = _userService.ListByFilters(new UserFiltersViewModel
            {
                CompanyId = filters.CompanyId ?? 0,
                BranchId = filters.BranchId ?? 0,
                Id = filters.UserId ?? 0
            });

            for (int i = 0; i < 4; i++)
            {
                var date = DateTime.Now.AddMonths(-3 + i);
                filters.BeginDate = new DateTime(date.Year, date.Month, 1);
                filters.EndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
                string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(date.Month).ToLower();
                arr2 = arr2.Append(mesExtenso).ToArray();
            }

            foreach (var user in users)
            {
                for (int i = 0; i < 4; i++)
                {
                    var date = DateTime.Now.AddMonths(-3 + i);
                    filters.BeginDate = new DateTime(date.Year, date.Month, 1);
                    filters.EndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);

                    data = data.Append(_repository
                        .GetQuery(x => x.Id > 0)
                        .WhereIf(filters.CompanyId > 0, x => x.CompanyId == filters.CompanyId)
                        .WhereIf(filters.BranchId > 0, x => x.BranchId == filters.BranchId)
                        .Where(x => x.StatusOrder == StatusOrderEnum.GAIN)
                        .Where(x => x.DateClosed >= filters.BeginDate && x.DateClosed <= filters.EndDate)
                        .Where(x => x.UserId == user.Id).Sum(x => x.ValueClosed)).ToArray();
                }

                lstSalesChartLabel.Add(
                    new SalesChartDataLabelViewModel
                    {
                        label = user.Name,
                        backgroundColor = user.Color,
                        borderColor = user.Color,
                        pointColor = user.Color, //"rgba(210, 214, 222, 1)",
                        pointStrokeColor = "#c1c7d1",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = user.Color,
                        data = data
                    }
                );

                data = Array.Empty<decimal>();
            }

            salesChartObj.labels = arr2;
            salesChartObj.datasets = lstSalesChartLabel;

            return salesChartObj;
        }

        public IList<RankingByConsultantViewModel> RankingByConsultant(RankingByConsultantFiltersViewModel filters)
        {
            var query = _repository
                        .GetQuery(x => x.Id > 0)
                        .Include(x => x.Branch)
                        .Include(x => x.User)
                        .WhereIf(filters.CompanyId > 0, x => x.CompanyId == filters.CompanyId)
                        .WhereIf(filters.BranchId > 0, x => x.BranchId == filters.BranchId)
                        .Where(x => x.StatusOrder == StatusOrderEnum.GAIN)
                        .Where(x => x.DateClosed >= filters.RakingBeginDate && x.DateClosed <= filters.RakingEndDate);
            var list = new List<RankingByConsultantViewModel>();

            foreach (var item in query)
            {
                var sales = new RankingByConsultantViewModel
                {
                    BranchName = item.Branch?.Description,
                    BranchId = item.BranchId ?? 0,
                    Meta = item.Branch?.SalesGoal ?? 0,
                    ConsultantId = item.UserId,
                    Consultant = item.User?.Name,
                    ValueClosed = item.ValueClosed,
                    Color = item.User?.Color
                };

                list.Add(sales);
            }
            return list;
        }

        public IList<NotificationViewModel> ListNotifications()
        {
            var now = DateTime.Now;

            return _repository
                .GetQuery(x =>
                    x.NotifyAt != null &&
                    x.NotifyAt <= now &&
                    x.DateReturn > now)
                .Include(x => x.Client)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    Title = "Retorno Agendado",
                    Message = $"Cliente: {x.Client.Name} - Retorno em {x.DateReturn:dd/MM/yyyy HH:mm}",
                    Date = x.NotifyAt.Value
                })
                .ToList();
        }



        public override async Task<ResultContent> Delete(int id)
        {

            try
            {
                var obj = _repository.GetQuery(id).FirstOrDefault();                
                _repository.Delete(obj);
                return Result.Success<object>(null);
            }
            catch (Exception erro)
            {
                throw;
            }
        }

        public IList<ScheduleServiceViewModel> ScheduledService(AttendanceFiltersViewModel filters)
        {
            var query = _repository
                        .GetQuery(x => x.Id > 0)
                        .Include(x => x.Branch)
                        .Include(x => x.User)
                        .Where(x => (x.DateReturn >= filters.BeginDate && x.DateReturn <= filters.EndDate) && x.Status == StatusAttendancesEnum.OPEN)
                        .ToList()
                        .GroupBy(x => x.UserId);

            
            List<ScheduleServiceViewModel> lstSecheduleService = new();
            if (query.Any())
            {
                foreach (var item in query)
                {
                    lstSecheduleService.Add(new ScheduleServiceViewModel
                    {
                        User = item.First().User.Name,
                        AmountService = item.Count()
                    });
                }
            }

            return lstSecheduleService;
        }

        public void UpdateStatusKanban(int id, int newStatus)
        {
            var atendimentoViewModel = GetWithRelations(id);
            if (atendimentoViewModel == null)
                throw new Exception("Atendimento não encontrado");

            atendimentoViewModel.StatusKanban = newStatus;

            var atendimentoDTO = _mapper.Map<AttendanceDTO>(atendimentoViewModel);
            Update(id, atendimentoDTO);
        }
    }
}
