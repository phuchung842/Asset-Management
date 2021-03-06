using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Repositories;
using Xunit;

namespace RookieOnlineAssetManagement.UnitTests.Repositories
{
    public class AssetRepoTest : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;

        public AssetRepoTest(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
            _fixture.CreateDatabase();
        }
        [Fact]
        public async Task CreateAsset_Success()
        {
            // initial mock data
            var dbContext = _fixture.Context;
            var locationId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = categoryId, CategoryName = "Laptop", ShortName = "LA" });
            await dbContext.SaveChangesAsync();
            //
            var assetTest = new AssetRequestModel()
            {
                CategoryId = categoryId,
                LocationId = locationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetTest);
            // test
            Assert.NotNull(assetNew);
        }

        [Fact]
        public async Task UpdateAsset_Success()
        {
            // initial mock data
            var dbContext = _fixture.Context;
            var oldlocationId = Guid.NewGuid().ToString();
            var oldcategoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = oldlocationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = oldcategoryId, CategoryName = "Laptop", ShortName = "LA", NumIncrease = 0 });
            await dbContext.SaveChangesAsync();
            //
            var assetTest = new AssetRequestModel()
            {
                CategoryId = oldcategoryId,
                LocationId = oldlocationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetTest);

            var locationId = Guid.NewGuid().ToString();
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HN" });
            var assetTestNew = new AssetRequestModel()
            {
                AssetId = assetNew.AssetId,
                CategoryId = assetTest.CategoryId,
                LocationId = locationId,
                AssetName = "Asset Test Update",
                InstalledDate = DateTime.Now
            };
            var assetupdate = await assetRepo.UpdateAssetAsync(assetNew.AssetId, assetTestNew);
            Assert.NotNull(assetupdate);
            Assert.True(assetNew.AssetId.Equals(assetupdate.AssetId));
            Assert.True(assetupdate.AssetName.Equals(assetTestNew.AssetName));
            Assert.True(assetupdate.CategoryName.Equals(assetNew.CategoryName));
        }
        [Fact]
        public async Task UpdateAsset_Failed()
        {
            // initial mock data
            var dbContext = _fixture.Context;
            var oldlocationId = Guid.NewGuid().ToString();
            var oldcategoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = oldlocationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = oldcategoryId, CategoryName = "Laptop", ShortName = "LA", NumIncrease = 0 });
            await dbContext.SaveChangesAsync();
            //
            var assetTest = new AssetRequestModel()
            {
                CategoryId = oldcategoryId,
                LocationId = oldlocationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetTest);

            var locationId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HN" });
            var assetTestNew = new AssetRequestModel()
            {
                AssetId = Guid.NewGuid().ToString(),
                LocationId = locationId,
                AssetName = "Asset Test Update",
                InstalledDate = DateTime.Now
            };
            await Assert.ThrowsAsync<Exception>(() => assetRepo.UpdateAssetAsync(assetTestNew.AssetId, assetTestNew));
        }

        [Fact]
        public async Task DeleteAsset_Success()
        {
            var dbContext = _fixture.Context;
            var oldlocationId = Guid.NewGuid().ToString();
            var oldcategoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = oldlocationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = oldcategoryId, CategoryName = "Laptop", ShortName = "LA", NumIncrease = 0 });
            await dbContext.SaveChangesAsync();
            //
            var assetTest = new AssetRequestModel()
            {
                CategoryId = oldcategoryId,
                LocationId = oldlocationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetTest);

            var result = await assetRepo.DeleteAssetAsync(assetNew.AssetId);
            //var asset = await assetRepo.GetAssetByIdAsync(assetNew.AssetId);
            Assert.True(result);
            //Assert.Null(asset);
        }

        [Fact]
        public async Task DeleteAsset_Failed()
        {
            var dbContext = _fixture.Context;
            var oldlocationId = Guid.NewGuid().ToString();
            var oldcategoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = oldlocationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = oldcategoryId, CategoryName = "Laptop", ShortName = "LA", NumIncrease = 0 });
            await dbContext.SaveChangesAsync();
            //
            var assetTest = new AssetRequestModel()
            {
                CategoryId = oldcategoryId,
                LocationId = oldlocationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetTest);

            await Assert.ThrowsAsync<Exception>(() => assetRepo.DeleteAssetAsync(Guid.NewGuid().ToString()));
            //Assert.Null(asset);
        }

        [Fact]
        public async Task GetAssetById_Success()
        {
            var dbContext = _fixture.Context;
            var locationId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = categoryId, CategoryName = "Laptop", ShortName = "LA" });
            await dbContext.SaveChangesAsync();
            var assetcreate = new AssetRequestModel()
            {
                CategoryId = categoryId,
                LocationId = locationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetcreate);

            var asset = await assetRepo.GetAssetByIdAsync(assetNew.AssetId);

            Assert.True(asset.AssetId.Equals(assetNew.AssetId));
            Assert.NotNull(asset);
            Assert.IsType<AssetDetailModel>(asset);
        }
        [Fact]
        public async Task GetAssetById_Failed()
        {
            var dbContext = _fixture.Context;
            var locationId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = categoryId, CategoryName = "Laptop", ShortName = "LA" });
            await dbContext.SaveChangesAsync();
            var assetcreate = new AssetRequestModel()
            {
                CategoryId = categoryId,
                LocationId = locationId,
                AssetName = "Asset Test",
                InstalledDate = new DateTime(),
            };
            // create repo
            var assetRepo = new AssetRepository(dbContext);
            var assetNew = await assetRepo.CreateAssetAsync(assetcreate);

            await Assert.ThrowsAsync<Exception>(() => assetRepo.GetAssetByIdAsync(Guid.NewGuid().ToString()));
            //Assert.Null(asset);
            //Assert.IsNotType<AssetModel>(asset);
        }
        [Fact]
        public async Task GetListHistory_Success()
        {
            var dbContext = _fixture.Context;
            var locationId = Guid.NewGuid().ToString();
            var AssetId = Guid.NewGuid().ToString();
            var Assignid = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            int state = (int)StateAssignment.Accepted;
            var categoryId = Guid.NewGuid().ToString();
            // add mock data
            dbContext.Locations.Add(new Location() { LocationId = locationId, LocationName = "HCM" });
            dbContext.Categories.Add(new Category() { CategoryId = categoryId, CategoryName = "Laptop", ShortName = "LA" });
            await dbContext.SaveChangesAsync();
            var user = new User
            {

                Id = userId,
            };
            var admin = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "AssignedToUser",
                FirstName = "admin",
                LastName = "hcm",
                Gender = true,
                JoinedDate = DateTime.Now,
                IsChange = true,
                StaffCode = "SD00002",
                LocationId = locationId,
            };
            dbContext.Users.Add(admin);
            await dbContext.SaveChangesAsync();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var asset = new Asset
            {
                CategoryId=categoryId,
                AssetId = AssetId,
                AssetName="Test",
            };
            dbContext.Assets.Add(asset);
            var assignment = new Assignment
            {
                UserId = user.Id,
                AssignmentId = Assignid,
                State = state,
                AssetId = asset.AssetId,
                AdminId = admin.Id,
            };
            dbContext.Assignments.Add(assignment);
            await dbContext.SaveChangesAsync();
                var returnRequestModel = new ReturnRequestModel
                {
                    AssignmentId = assignment.AssignmentId,
                    AssetId = AssetId,
                    AssetName = "User",
                    ReturnedDate = DateTime.Now,
                };
            
            var request = new ReturnRequestRepository(dbContext);
            var CreateRequest = await request.CreateReturnRequestAsync(assignment.AssignmentId,userId);         
            
            var assetHistory = new AssetHistoryModel()
            {               
                AssignmentId = CreateRequest.AssignmentId,
                AssignedBy = "admin",
                Date = new DateTime(),
                AssignedTo = "user",                
            };
            var assetRepo = new AssetRepository(dbContext);
            var History = await assetRepo.GetListAssetHistoryAsync(AssetId);
            Assert.NotNull(History);
            Assert.IsType<List<AssetHistoryModel>>(History);
        }
    }
}