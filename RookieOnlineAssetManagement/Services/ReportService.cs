﻿using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<bool> ExportReportAsync(string locationId)
        {
            return await _reportRepository.ExportReportAsync(locationId);
        }
        public async Task<ICollection<ReportModel>> GetListReportAsync(string locationId)
        {
            return await _reportRepository.GetListReportAsync(locationId);
        }
    }
}
