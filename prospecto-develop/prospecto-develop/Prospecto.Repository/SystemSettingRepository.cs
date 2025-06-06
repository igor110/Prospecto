using Dapper;
using Prospecto.IRespository;
using Prospecto.Models.Infos;
using Prospecto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prospecto.Repository
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly IDbConnection _connection;

        public SystemSettingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<SystemSettingInfo>> ListAsync(string keyFilter, int? companyId, int? branchId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ss.*, c.Description AS CompanyName, b.Description AS BranchName");
            sql.AppendLine("FROM systemsetting ss");
            sql.AppendLine("LEFT JOIN companies c ON c.Id = ss.CompanyId");
            sql.AppendLine("LEFT JOIN branches b ON b.Id = ss.BranchId");
            sql.AppendLine("WHERE 1 = 1");

            if (!string.IsNullOrEmpty(keyFilter))
                sql.AppendLine("AND ss.`Key` = @Key");
            if (companyId.HasValue)
                sql.AppendLine("AND ss.CompanyId = @CompanyId");
            if (branchId.HasValue)
                sql.AppendLine("AND ss.BranchId = @BranchId");

            var parametros = new { Key = keyFilter, CompanyId = companyId, BranchId = branchId };

            System.Diagnostics.Debug.WriteLine($"📥 SQL Executado:\n{sql}");
            System.Diagnostics.Debug.WriteLine($"📥 Parâmetros:\nKey={keyFilter}, CompanyId={companyId}, BranchId={branchId}");

            var resultado = await _connection.QueryAsync<SystemSettingInfo>(sql.ToString(), parametros);

            foreach (var item in resultado)
                System.Diagnostics.Debug.WriteLine($"🔄 Parametro retornado: {item.Key} = {item.Value}");

            return resultado;
        }

        public async Task<IEnumerable<SystemSettingInfo>> ListAllAsync(int companyId, int? branchId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ss.*, c.Description AS CompanyName, b.Description AS BranchName");
            sql.AppendLine("FROM systemsetting ss");
            sql.AppendLine("LEFT JOIN companies c ON c.Id = ss.CompanyId");
            sql.AppendLine("LEFT JOIN branches b ON b.Id = ss.BranchId");
            sql.AppendLine("WHERE ss.CompanyId = @CompanyId");

            if (branchId.HasValue)
                sql.AppendLine("AND ss.BranchId = @BranchId");

            var result = await _connection.QueryAsync<SystemSettingInfo>(
                sql.ToString(),
                new { CompanyId = companyId, BranchId = branchId }
            );

            return result;
        }

        public async Task<SystemSettingViewModel> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM systemsetting WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<SystemSettingViewModel>(sql, new { Id = id });
        }

        public async Task CreateAsync(SystemSettingViewModel model)
        {
            var sql = @"
                    INSERT INTO systemsetting (CompanyId, BranchId, `Key`, `Value`, CreatedAt, UpdatedAt)
                    VALUES (@CompanyId, @BranchId, @Key, @Value, @CreatedAt, @UpdatedAt);

            ";
            await _connection.ExecuteAsync(sql, model);
        }

        public async Task<int> UpdateAsync(SystemSettingViewModel model)
        {
            var sql = @"
                UPDATE systemsetting
                SET 
                    `Key` = @Key,
                    `Value` = @Value,
                    CompanyId = @CompanyId,
                    BranchId = @BranchId,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id;
                 ";
            Console.WriteLine("SQL de update será executado:");
            Console.WriteLine(sql);
            Console.WriteLine($"Id={model.Id}, Key={model.Key}, Value={model.Value}, CompanyId={model.CompanyId}, BranchId={model.BranchId}, UpdatedAt={model.UpdatedAt}");

            return await _connection.ExecuteAsync(sql, model);
        }


        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM systemsetting WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
