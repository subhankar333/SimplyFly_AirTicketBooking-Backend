using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class SeatDetailServiceTest
    {
        private SeatDetailService _seatDetailService;
        private Mock<IRepository<string, SeatDetail>> _mockSeatDetailRepository;
        private Mock<ILogger<SeatDetailService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockSeatDetailRepository = new Mock<IRepository<string, SeatDetail>>();
            _mockLogger = new Mock<ILogger<SeatDetailService>>();

            _seatDetailService = new SeatDetailService(_mockSeatDetailRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddSeatDetail_Validate_Returns_AddedSeatDetail_Test()
        {
            //arrange
            var seatDetail = new SeatDetail
            {
                SeatNo = "FLI009SE01",
                SeatClass = "Economy"
            };

            _mockSeatDetailRepository.Setup(repo => repo.Add(seatDetail)).ReturnsAsync(seatDetail);

            //act
            var addedSeatDetail = await _seatDetailService.AddSeatDetail(seatDetail);

            //assert
            Assert.That(seatDetail.SeatNo,Is.EqualTo(addedSeatDetail.SeatNo));
        }

        [Test]
        public async Task Remove_SeatDetail_Existing_SeatNo_Test()
        {
            //arrange
            var seatNo = "FLI009SE01";
            var seatDetail = new SeatDetail
            {
                SeatNo = seatNo,
                SeatClass = "Economy"
            };

            _mockSeatDetailRepository.Setup(repo => repo.GetAsync(seatNo)).ReturnsAsync(seatDetail);

            //act
            var result = await _seatDetailService.RemoveSeatDetail(seatDetail.SeatNo);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveSeatDetail_NonExisting_SeatNo_Test()
        {
            // Arrange
            var nonExistingSeatDetailId = "FLI069SE01";
            _mockSeatDetailRepository.Setup(repo => repo.GetAsync(nonExistingSeatDetailId)).ReturnsAsync((SeatDetail)(null));

            // Act
            var result = await _seatDetailService.RemoveSeatDetail(nonExistingSeatDetailId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllSeatDetails_Test()
        {
            //arrange
            var seatDetail1 = new SeatDetail
            {
                SeatNo = "FLI009SE01",
                SeatClass = "Economy"
            };

            var seatDetail2 = new SeatDetail
            {
                SeatNo = "FLI009SE02",
                SeatClass = "Economy"
            };

            var seatDetailsList = new List<SeatDetail>();
            seatDetailsList.Add(seatDetail1);
            seatDetailsList.Add(seatDetail2);

            _mockSeatDetailRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(seatDetailsList);

            // Act
            var result = await _seatDetailService.GetAllSeatDetails();

            // Assert
            Assert.That(result, Is.EqualTo(seatDetailsList));
        }


        [Test]
        public async Task GetByIdSeatDetails_ExistingSeatNo_ReturnsSeatDetail_Test()
        {
            // Arrange
            var seatNo = "FLI009SE01";
            var seatDetail = new SeatDetail
            {
                SeatNo = seatNo,
                SeatClass = "Economy"
            };

            _mockSeatDetailRepository.Setup(repo => repo.GetAsync(seatNo)).ReturnsAsync(seatDetail);

            // Act
            var result = await _seatDetailService.GetSeatDetailById(seatNo);

            // Assert
            Assert.That(result, Is.EqualTo(seatDetail));
        }

        [Test]
        public async Task GetByIdSeatDetails_NonExistingSeatNo_ReturnsNull_Test()
        {
            // Arrange
            var noneExistingSeatNo = "FLI069SE01";
            var seatDetail = new SeatDetail
            {
                SeatNo = noneExistingSeatNo,
                SeatClass = "Economy"
            };

            _mockSeatDetailRepository.Setup(repo => repo.GetAsync(noneExistingSeatNo)).ReturnsAsync((SeatDetail)(null));

            // Act
            var result = await _seatDetailService.GetSeatDetailById(noneExistingSeatNo);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }
    }
}
