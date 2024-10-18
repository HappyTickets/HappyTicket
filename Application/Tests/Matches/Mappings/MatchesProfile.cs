using AutoMapper;
using Shared.DTOs.Test.Request;
using System.Text.RegularExpressions;

namespace Application.Tests.Matches.Mappings
{
    internal class MatchesProfile: Profile
    {
        public MatchesProfile()
        {
            CreateMap<Match, CreateTestMatchDto>().ReverseMap();
            CreateMap<Match, UpdateTestMatchDto>().ReverseMap();
        }
    }
}
