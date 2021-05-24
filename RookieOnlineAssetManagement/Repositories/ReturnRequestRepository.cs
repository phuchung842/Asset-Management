﻿using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Repositories
{
    public class ReturnRequestRepository: IReturnRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ReturnRequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ReturnRequestModel> CreateReturnRequestAsync(string assignmentId,string requestedUserId)
        {
            var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(x => x.AssignmentId == assignmentId);
            if (!assignment.State.Equals((int)StateAssignment.Accepted)) return null;
            var requestedUser = await _dbContext.Users.FindAsync(requestedUserId);
            var returnRequest = new ReturnRequest
            {
                AssignmentId = assignment.AssignmentId,
                RequestUserId = requestedUser.Id,
                RequestBy = requestedUser.UserName,
                State = false
            };
            var create = _dbContext.ReturnRequests.Add(returnRequest);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                var returnRequestModel = new ReturnRequestModel
                {
                    AssignmentId = create.Entity.AssignmentId,
                    AssetId = create.Entity.Assignment.AssetId,
                    AssetName = create.Entity.Assignment.AssetName,
                    AcceptedUserId = create.Entity.AcceptedUserId,
                    AcceptedBy = create.Entity.AcceptedBy,
                    RequestUserId = create.Entity.RequestUserId,
                    RequestBy = create.Entity.RequestBy,
                    ReturnedDate = create.Entity.ReturnDate,
                    State = create.Entity.State
                };
                return returnRequestModel;
            }
            return null;
        }
        public async Task<bool> ChangeStateAsync(bool accept, string assignmentId, string acceptedUserId)
        {
            var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(x => x.AssignmentId == assignmentId);
            var returnRequest = await _dbContext.ReturnRequests.FirstOrDefaultAsync(x => x.AssignmentId == assignmentId);
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                if (accept == true)
                {
                    var asset = await _dbContext.Assets.FirstOrDefaultAsync(x => x.AssetId == assignment.AssetId);
                    var acceptedUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == acceptedUserId);
                    asset.State = (short)StateAsset.Avaiable;
                    assignment.State = (int)StateAssignment.Completed;
                    returnRequest.State = true;
                    returnRequest.ReturnDate = DateTime.Now;
                    returnRequest.AcceptedUserId = acceptedUser.Id;
                    returnRequest.AcceptedBy = acceptedUser.UserName;
                }
                else
                {
                    var deleted = _dbContext.ReturnRequests.Remove(returnRequest);
                }
                var result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                return false;
            }
            catch { return false;}
        }
        public async Task<ICollection<ReturnRequestModel>> GetListReturnRequestAsync(ReturnRequestParams returnRequestParams)
        {
            return null;
        }
    }
}