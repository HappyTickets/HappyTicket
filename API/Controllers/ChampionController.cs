using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ChampionController(IChampionService _championService) : ControllerBase
    {

    }
}
