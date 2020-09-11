﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StandardApi.Constants;
using StandardApi.Constants.Files;
using StandardApi.Core.User.ViewModels;
using StandardApi.Data.Entities.File;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.User.Queries.GetUserAvatar
{
    public class GetUserAvatarQuery : IGetUserAvatarQuery
    {
        private readonly IDistributedCache _cache;
        private readonly IRepository<UserAvatarEntity> _avatarRepository;
        private readonly IRepository<DefaultFileEntity> _fileDefaultRepository;

        public GetUserAvatarQuery(
            IDistributedCache memoryCache,
            IRepository<UserAvatarEntity> avatarRepository,
            IRepository<DefaultFileEntity> fileDefaultRepository)
        {
            _cache = memoryCache;
            _avatarRepository = avatarRepository;
            _fileDefaultRepository = fileDefaultRepository;
        }

        public async Task<UserAvatarViewModel> ExecuteAsync(int userId)
        {
            return await GetAvatarByUserId(userId);
        }

        public async Task<UserAvatarViewModel> ExecuteAndSaveCacheAsync(int userId)
        {
            var result = new UserAvatarViewModel();
            var cacheData = await _cache.GetStringAsync(CacheKey.USER_PHOTO + userId);

            if (cacheData != null)
            {
                result = JsonConvert.DeserializeObject<UserAvatarViewModel>(cacheData);
            }
            else
            {
                result = await GetAvatarByUserId(userId);

                await _cache.SetStringAsync(CacheKey.USER_PHOTO + userId, JsonConvert.SerializeObject(result), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60)
                });
            }

            return result;
        }

        private async Task<UserAvatarViewModel> GetAvatarByUserId(int userId)
        {
            var result = await _avatarRepository.TableNoTracking
                .Where(n => n.UserId == userId)
                .Select(n => new UserAvatarViewModel()
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Filename = n.FileName,
                    FileSize = n.FileSize,
                    FileType = n.FileType,
                    Content = n.FileContent
                })
                .LastOrDefaultAsync();

            if (result == null)
            {
                result = await _fileDefaultRepository.TableNoTracking
                    .Where(n => n.DefaultType == DefaultFileType.USER_AVATAR_DEFAULT)
                    .Select(n => new UserAvatarViewModel()
                    {
                        UserId = 0,
                        Filename = n.FileName,
                        FileSize = n.FileSize,
                        FileType = n.FileType,
                        Content = n.FileContent
                    })
                    .FirstOrDefaultAsync();
            }

            return result;
        }
    }
}
