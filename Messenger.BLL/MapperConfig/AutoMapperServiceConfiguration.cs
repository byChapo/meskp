using AutoMapper;
using Messenger.BLL.DTO;
using Messenger.BLL.Services;
using Messenger.DAL.Models;
using Messenger.DAL.Repository;
using System.Web;

namespace Messenger.BLL.MapperConfig
{
    public static class AutoMapperServiceConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                ConfigureChatMapping(cfg);
                ConfigureMessageMapping(cfg);
                ConfigureUserMapping(cfg);
            });
        }

        private static void ConfigureChatMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ChatDTO, Chat>();
            cfg.CreateMap<Chat, ChatDTO>()
                    .ForMember("AdminId", opt => opt.MapFrom(c => c.Admin.Id));
            cfg.CreateMap<Chat, FullChatDTO>()
                    .ForMember("AdminId", opt => opt.MapFrom(c => c.Admin.Id));
        }

        private static void ConfigureMessageMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Message, MessageDTO>()
                    .ForMember("Author", opt => opt.MapFrom(m => m.Author.GetFullName()))
                    .ForMember("AuthorId", opt => opt.MapFrom(m => m.Author.Id))
                    .ForMember("Type", opt => opt.MapFrom(m => m.Type.Type))
                    .ForMember("ChatId", opt => opt.MapFrom(m => m.Chat.Id));

            cfg.CreateMap<MessageDTO, Message>()
                    .ForMember("Author", opt => opt.MapFrom(m => new User()))
                    .ForMember("Type", opt => opt.MapFrom(m => new MessageType()))
                    .ForMember("Chat", opt => opt.MapFrom(m => new Chat()));
        }

        private static void ConfigureUserMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RegisterDTO, User>();
            cfg.CreateMap<User, UserDTO>();

            cfg.CreateMap<UserDTO, User>();
        }
    }
}
