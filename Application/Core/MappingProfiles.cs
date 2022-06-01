using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();
        CreateMap<Activity, ActivityDTO>()
            .ForMember(h => h.HostUsername,
                opt
                    => opt.MapFrom(a => a.Atendees.FirstOrDefault(
                        x => x.IsHost)!.AppUser.UserName));
        CreateMap<ActivityAtendee, AtendeeDTO>()
            .ForMember(d => d.DisplayName,
                opt
                    => opt.MapFrom(a => a.AppUser.DisplayName))
            .ForMember(d => d.Username,
                opt
                    => opt.MapFrom(a => a.AppUser.UserName))
            .ForMember(i => i.Image, 
                opt 
                    => opt.MapFrom(p => p.AppUser.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(i => i.Image, 
                opt 
                    => opt.MapFrom(p => p.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Comment, CommentDTO>()
            .ForMember(d => d.DisplayName,
                opt
                    => opt.MapFrom(a => a.Author.DisplayName))
            .ForMember(d => d.Username,
                opt
                    => opt.MapFrom(a => a.Author.UserName))
            .ForMember(i => i.Image, 
                opt 
                    => opt.MapFrom(p => p.Author.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        
    }
}