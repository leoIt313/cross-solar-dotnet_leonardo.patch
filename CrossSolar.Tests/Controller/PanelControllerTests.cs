using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;

namespace CrossSolar.Tests.Controller
{
    public class PanelControllerTests
    {
        public PanelControllerTests()
        {
            _panelController = new PanelController(_panelRepositoryMock.Object);
        }

        private readonly PanelController _panelController;

        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();

        private readonly string urlParameter = "http://localhost:51064/";

        [Fact]
        public async Task Register_ShouldInsertPanel()
        {
            var panel = new PanelModel
            {
                Latitude = -21.051542,
                Longitude = -47.051542,
                Serial = "wfQYyBSBKcRGUEKH",
                Brand = "Brand"
            };

            var response = new HttpClient().PostAsJsonAsync(string.Concat(urlParameter, "api/panel/register"), panel).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_FailSerial()
        {
            var panel = new PanelModel
            {
                Latitude = -21.051542,
                Longitude = -47.051542,
                Serial = "wfQYyBSBKc",
                Brand = "20"
            };

            var response = new HttpClient().PostAsJsonAsync(string.Concat(urlParameter, "api/panel/register"), panel).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_FailLatitude()
        {
            var panel = new PanelModel
            {
                Latitude = -100,
                Longitude = -47.063240,
                Serial = "wfQYyBSBKcRGUEKH",
                Brand = "20"
            };

            var response = new HttpClient().PostAsJsonAsync(string.Concat(urlParameter, "api/panel/register"), panel).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_FailLongitude()
        {
            var panel = new PanelModel
            {
                Latitude = -22.907104,
                Longitude = 190,
                Serial = "wfQYyBSBKcRGUEKH",
                Brand = "20"
            };

            var response = new HttpClient().PostAsJsonAsync(string.Concat(urlParameter, "api/panel/register"), panel).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}