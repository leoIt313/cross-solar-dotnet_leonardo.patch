using System;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CrossSolar.Tests.Controller
{
    public class AnalyticsControllerTests
    {
        public AnalyticsControllerTests()
        {
            _analyticsController = new AnalyticsController(_analyticsRepository.Object, _panelRepositoryMock.Object);
        }

        private readonly AnalyticsController _analyticsController;

        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();
        private readonly Mock<IAnalyticsRepository> _analyticsRepository = new Mock<IAnalyticsRepository>();

        private readonly string urlParameter = "http://localhost:51064/";

        [Fact]
        public async Task GetAnalyticsByHour()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Concat(urlParameter, "panel/1/2018-02-20T13:34/analytics")),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.SendAsync(request).Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetAnalyticsByDay()
        {

            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Concat(urlParameter, "panel/1/2018-02-20T13:34/analytics/day")),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.SendAsync(request).Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void GetAllAnalyticsByPanel()
        {
            var client = new HttpClient(); 

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Concat(urlParameter, "panel/1/analytics/alldays")),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.SendAsync(request).Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task RegisterAnalytics()
        {
            var panel = new OneHourElectricityModel
            {
                DateTime = DateTime.Now,
                KiloWatt = 35
            };

            var response = new HttpClient().PostAsJsonAsync(string.Concat(urlParameter, "panel/1/analytics"), panel).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
