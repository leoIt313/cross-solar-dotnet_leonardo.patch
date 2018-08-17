using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Controllers
{
    [Route("panel")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        private readonly IPanelRepository _panelRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository)
        {
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;
        }

        //GET panelId/XXXX1111YYYY2222/analytics
        [HttpGet("{panelId}/{hour}/[controller]")]
        public async Task<IActionResult> Get([FromRoute] string panelId, DateTime hour)
        {
            try
            {
                var analytics = await _analyticsRepository.Query().Where(x => x.PanelId == panelId && x.DateTime.Date.Equals(hour.Date) && x.DateTime.TimeOfDay.Hours.Equals(hour.TimeOfDay.Hours)).ToListAsyncSafe();

                if (!analytics.Any() || analytics == null)
                {
                    return NotFound();
                }

                var result = new OneHourElectricityListModel
                {
                    OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                    {
                        Id = c.Id,
                        KiloWatt = c.KiloWatt,
                        DateTime = c.DateTime
                    })
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GET panelId/XXXX1111YYYY2222/analytics/day
        [HttpGet("{panelId}/{day}/[controller]/day")]
        public async Task<IActionResult> DayResults([FromRoute] string panelId, DateTime day)
        {
            try
            {
                var analytics = await _analyticsRepository.Query().Where(x => x.PanelId == panelId && x.DateTime.Date.Equals(day.Date)).ToListAsyncSafe();


                if (!analytics.Any() || analytics == null)
                {
                    return NotFound();
                }

                return Ok(new OneDayElectricity().TransformToOneDayModel(day.Date, analytics));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GET panelId/analytics/alldays
        [HttpGet("{panelId}/[controller]/alldays")]
        public async Task<IActionResult> DayResults([FromRoute] string panelId)
        {
            try
            {
                var analytics = await _analyticsRepository.Query().Where(x => x.PanelId == panelId).GroupBy(x => x.DateTime.Date).ToListAsyncSafe();


                if (!analytics.Any() || analytics == null)
                {
                    return NotFound();
                }

                var result = new List<OneDayElectricityModel>();

                var objOneDay = new OneDayElectricity();
                foreach (var item in analytics)
                {
                    result.Add(objOneDay.TransformToOneDayModel(item.Key, item.Select(x => x).ToList()));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //POST panelId/XXXX1111YYYY2222/analytics
        [HttpPost("{panelId}/[controller]")]
        public async Task<IActionResult> Post([FromRoute] string panelId, [FromBody] OneHourElectricityModel value)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var oneHourElectricityContent = new OneHourElectricity
                {
                    PanelId = panelId,
                    KiloWatt = value.KiloWatt,
                    DateTime = value.DateTime
                };

                await _analyticsRepository.InsertAsync(oneHourElectricityContent);

                var result = new OneHourElectricityModel
                {
                    Id = oneHourElectricityContent.Id,
                    KiloWatt = oneHourElectricityContent.KiloWatt,
                    DateTime = oneHourElectricityContent.DateTime
                };

                return Created($"panel/{panelId}/analytics/{result.Id}", result);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}