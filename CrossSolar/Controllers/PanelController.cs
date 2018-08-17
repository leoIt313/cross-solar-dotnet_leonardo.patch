using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CrossSolar.Controllers
{
    [Route("api")]
    public class PanelController : Controller
    {
        private readonly IPanelRepository _panelRepository;

        public PanelController(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }

        // POST api/panel
        [HttpPost("[controller]/register")]
        public async Task<IActionResult> Register([FromBody] PanelModel value)
        {
            try
            {
                Regex regexObj = new Regex(@"-?\d{1,2}\.\d{6}");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (!regexObj.Match(value.Latitude.ToString(CultureInfo.InvariantCulture)).Success)
                {
                    return BadRequest(ModelState);
                }

                if (!regexObj.Match(value.Longitude.ToString(CultureInfo.InvariantCulture)).Success)
                {
                    return BadRequest(ModelState);
                }

                var panel = new Panel
                {
                    Latitude = Convert.ToDouble(value.Latitude),
                    Longitude = Convert.ToDouble(value.Longitude),
                    Serial = value.Serial,
                    Brand = value.Brand
                };

                await _panelRepository.InsertAsync(panel);

                return Created($"panel/{panel.Id}", panel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}