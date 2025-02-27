using AutoMapper;
using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories_BE.Interfaces;
using Repositories_BE.Repositories;
using Services_BE.Interfaces;
using Services_BE.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Test
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly SWP391G3DbContext _context;

        //setup for repository
        protected readonly Mock<IRatingRepository> _ratingRepositoryMock;

        protected readonly Mock<IRatingService> _ratingServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;

        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new MapperConfigProfile());
                }
                );
            _mapperConfig = mappingConfig.CreateMapper();

            //repository
            _ratingRepositoryMock = new Mock<IRatingRepository>();

            //service
            _ratingServiceMock = new Mock<IRatingService>();
            _currentTimeMock = new Mock<ICurrentTime>();

            var options = new DbContextOptionsBuilder<SWP391G3DbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS; Database=swp391-spring25; Uid=sa; Pwd=12345;TrustServerCertificate=True")
                .Options;
          /*  var options = new DbContextOptionsBuilder<SWP391G3DbContext>()
                .UseSqlServer("Server=localhost; Database=swp391-spring25; Uid=sa; Pwd=12345;TrustServerCertificate=True")
                .Options;*/
            _context =  new SWP391G3DbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow.AddHours(7));

            // _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(0);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
