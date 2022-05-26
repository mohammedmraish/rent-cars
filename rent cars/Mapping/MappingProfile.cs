using AutoMapper;
using System.Linq;
using veggga.Controllers.Resources;
using veggga.Core.Models;
using veggga.Models;

namespace veggga.Mapping
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            //Domain to api resource
            CreateMap<Photo, PhotoResource>();
            CreateMap<Make, MakeResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { ContactName = v.ContactName, ContactPhone = v.ContactPhone, ContactEmail = v.ContactEmail }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource { Id = vf.Feature.Id, Name = vf.Feature.Name })))
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => new KeyValuePairResource { Name=v.Model.Make.Name,Id=v.Model.Make.Id}));

           CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr=>vr.Features,opt=>opt.Ignore())
                .AfterMap((v, vr) =>
                {
                    //contact mapping
                    vr.Contact = new ContactResource { ContactName = v.ContactName, ContactPhone = v.ContactPhone, ContactEmail = v.ContactEmail };

                    //features mapping
                    foreach(var f in v.Features)
                       vr.Features.Add(f.FeatureId);
                });


            //api resource to Domain 
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.ContactName))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.ContactEmail))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.ContactPhone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) =>
                {
                    v.Features.Clear();
                    foreach (var f in vr.Features) {
                        v.Features.Add(new VehicleFeature { VehicleId = v.Id, FeatureId = f });
                    }
                });

            
        }
    }
}
