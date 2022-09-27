using Application.Activities;
using Application.Comments;
using Application.Profiles;
using Domain;
using Profile = AutoMapper.Profile;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUsername = null;
        
        CreateMap<Activity, Activity>();
        
        CreateMap<Activity, ActivityDTO>()
            .ForMember(h => h.HostUsername,
                opt
                    => opt.MapFrom(a => a.Attendees.FirstOrDefault(
                        x => x.IsHost)!.AppUser.UserName));
        
        CreateMap<ActivityAttendee, AtendeeDTO>()
            .ForMember(d => d.DisplayName,
                opt
                    => opt.MapFrom(a => a.AppUser.DisplayName))
            .ForMember(d => d.Username,
                opt
                    => opt.MapFrom(a => a.AppUser.UserName))
            .ForMember(i => i.Image, 
                opt 
                    => opt.MapFrom(p => p.AppUser.Photos!.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(d => d.FollowersCount,
                opt
                    => opt.MapFrom(s => s.AppUser.Followers!.Count))
            .ForMember(d => d.FollowingCount,
                opt
                    => opt.MapFrom(s => s.AppUser.Followings!.Count))
            .ForMember(d => d.Following,
                o =>
                    o.MapFrom(s => s.AppUser.Followers!.Any(x => x.Observer.UserName == currentUsername)));

        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(i => i.Image,
                opt
                    => opt.MapFrom(p => p.Photos!.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(d => d.FollowersCount,
                opt
                    => opt.MapFrom(s => s.Followers!.Count))
            .ForMember(d => d.FollowingCount,
                opt
                    => opt.MapFrom(s => s.Followings!.Count))
            .ForMember(d => d.Following,
                o =>
                    o.MapFrom(s => s.Followers!.Any(x => x.Observer.UserName == currentUsername)));
        
        CreateMap<Comment, CommentDTO>()
            .ForMember(d => d.DisplayName,
                opt
                    => opt.MapFrom(a => a.Author.DisplayName))
            .ForMember(d => d.Username,
                opt
                    => opt.MapFrom(a => a.Author.UserName))
            .ForMember(i => i.Image, 
                opt 
                    => opt.MapFrom(p => p.Author.Photos!.FirstOrDefault(x => x.IsMain)!.Url));

        CreateMap<ActivityAttendee, UserActivityDto>()
            .ForMember(d => d.Id,
                o => o.MapFrom(s => s.Activity.Id))
            .ForMember(d => d.Date,
                o => o.MapFrom(s => s.Activity.Date))
            .ForMember(d => d.Title,
                o => o.MapFrom(s => s.Activity.Title))
            .ForMember(d => d.Category,
                o => o.MapFrom(s => s.Activity.Category))
            .ForMember(d => d.HostUsername,
                o => o.MapFrom(s =>
                    s.Activity.Attendees.FirstOrDefault(x => x.IsHost)!.AppUser.UserName));


    }
}