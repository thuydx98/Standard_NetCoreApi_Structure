using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StandardApi.Common.Entities;
using StandardApi.Common.Securities;
using System.Data;
using System.Data.Common;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Entities.File;
using StandardApi.Data.Entities.BackgroundWorker;

namespace StandardApi.Data.Services
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        #region Fields & Constructor
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerFactory _loggerFactory;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor = null, ILoggerFactory loggerFactory = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggerFactory = loggerFactory;
        }
        #endregion

        #region Methods
        public Task<int> ExecuteSqlCommandAsync(string commandText,
            params object[] parameters) => Database.ExecuteSqlRawAsync(commandText, parameters);

        public async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureListAsync(string commandText,
            params object[] parameters)
        {
            using (var cmd = Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    for (var i = 0; i <= parameters.Length - 1; i++)
                    {
                        var p = parameters[i] as DbParameter;
                        if (p == null)
                        {
                            throw new Exception("Not support parameter type");
                        }
                        cmd.Parameters.Add(p);
                    }
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var list = new List<Dictionary<string, object>>();
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        var dataRow = new Dictionary<string, object>();
                        for (var iField = 0; iField < dataReader.FieldCount; iField++)
                        {
                            dataRow.Add(dataReader.GetName(iField),
                                dataReader.IsDBNull(iField) ? null : dataReader[iField]);
                        }
                        list.Add(dataRow);
                    }
                }
                return list;
            }
        }

        public override int SaveChanges()
        {
            var validationErrors = ChangeTracker
                .Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(r => r != ValidationResult.Success)
                .ToArray();

            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine, validationErrors.Select(error => string.Format("Properties {0} Error: {1}", error.MemberNames, error.ErrorMessage)));
                throw new Exception(exceptionMessage);
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry?.Entity is ICreatedEntity createdEntity)
                {
                    createdEntity.CreatedBy = _httpContextAccessor?.HttpContext?.User?.UserName();
                    createdEntity.CreatedAt = DateTime.UtcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                if (entry?.Entity is IUpdatedEntity updatedEntity)
                {
                    updatedEntity.UpdatedBy = _httpContextAccessor?.HttpContext?.User?.UserName();
                    updatedEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            var validationErrors = ChangeTracker
                .Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(r => r != ValidationResult.Success)
                .ToArray();

            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine, validationErrors.Select(error => string.Format("Properties {0} Error: {1}", error.MemberNames, error.ErrorMessage)));
                throw new Exception(exceptionMessage);
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry?.Entity is ICreatedEntity createdEntity)
                {
                    createdEntity.CreatedBy = _httpContextAccessor?.HttpContext?.User?.UserName();
                    createdEntity.CreatedAt = DateTime.UtcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                if (entry?.Entity is IUpdatedEntity updatedEntity)
                {
                    updatedEntity.UpdatedBy = _httpContextAccessor?.HttpContext?.User?.UserName();
                    updatedEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync();
        }
        #endregion

        #region Define Background Worker Model
        public virtual DbSet<HangfireServiceEntity> HangfireServiceEntity { get; set; }
        #endregion

        #region Define File Model
        public virtual DbSet<DefaultFileEntity> DefaultFileEntity { get; set; }
        #endregion

        #region Define User Model
        public virtual DbSet<LogUserStatusEntity> LogUserStatusEntity { get; set; }
        public virtual DbSet<PermissionEntity> PermissionEntity { get; set; }
        public virtual DbSet<RoleEntity> RoleEntity { get; set; }
        public virtual DbSet<UserAvatarEntity> UserAvatarEntity { get; set; }
        public virtual DbSet<UserAddressEntity> UserAddressEntity { get; set; }
        public virtual DbSet<UserEntity> UserEntity { get; set; }
        public virtual DbSet<UserPermissionEntity> UserPermissionEntity { get; set; }
        public virtual DbSet<UserStatusEntity> UserStatusEntity { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Background Worker
            modelBuilder.Entity<HangfireServiceEntity>(entity =>
            {
                entity.ToTable("HangfireService");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.Cron)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Period)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");
            });
            #endregion

            #region File
            modelBuilder.Entity<DefaultFileEntity>(entity =>
            {
                entity.ToTable("DefaultFile");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.DefaultType)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.Title)
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.FileSize)
                    .HasColumnType("varchar(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.FileContent)
                    .IsRequired()
                    .HasColumnType("varbinary(MAX)");

                entity.Property(e => e.FileType)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnType("bit")
                    .HasDefaultValue(true);
            });
            #endregion

            #region User
            modelBuilder.Entity<LogUserStatusEntity>(entity =>
            {
                entity.ToTable("LogUserStatusEntity");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.UserId)
                     .IsRequired()
                     .HasColumnType("int");

                entity.Property(e => e.StatusId)
                     .IsRequired()
                     .HasColumnType("int");

                entity.Property(e => e.ModifiedDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserStatusLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UserStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<PermissionEntity>(entity =>
            {
                entity.ToTable("Permission");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.Permission)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<UserAddressEntity>(entity =>
            {
                entity.ToTable("UserAddress");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.Ward)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.Detail)
                    .HasColumnType("nvarchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.IsDefault)
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserAvatarEntity>(entity =>
            {
                entity.ToTable("UserAvatar");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.FileSize)
                    .HasColumnType("varchar(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.FileName)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.FileContent)
                    .IsRequired()
                    .HasColumnType("varbinary(MAX)");

                entity.Property(e => e.FileType)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Avatars)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.UserCode)
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.MiddleName)
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.CitizenID)
                    .HasColumnType("varchar(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.Gender)
                    .HasColumnType("varchar(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.Birthday)
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasColumnType("varchar(200)")
                    .HasMaxLength(200);

                entity.Property(e => e.RoleId)
                    .HasColumnType("int");

                entity.Property(e => e.StatusId)
                    .HasColumnType("int");

                entity.Property(e => e.Deleted)
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserPermissionEntity>(entity =>
            {
                entity.ToTable("UserOtherPermission");

                entity.HasKey(e => new { e.UserId, e.PermissionId });

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserOtherPermisisons)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Permisison)
                    .WithMany(p => p.UserOtherPermisisons)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserStatusEntity>(entity =>
            {
                entity.ToTable("UserStatus");

                entity.HasIndex(e => e.Id)
                       .HasName("Id");

                entity.Property(e => e.IsBlockAccess)
                    .HasColumnType("bit");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)");
            });
            #endregion
        }
    }
}
