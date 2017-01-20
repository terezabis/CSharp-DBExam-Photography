using AutoMapper;
using Newtonsoft.Json;
using Photography.Data;
using Photography.Dtos;
using Photography.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photography.JsonImport
{
    class JsonImport
    {
        private static string LensPath = "../../../datasets/lenses.json";
        private static string CamerasPath = "../../../datasets/cameras.json";
        private static string PhotographersPath = "../../../datasets/photographers.json";

        private static string Error = "Error. Invalid data provided";

        static void Main(string[] args)
        {
            //var context = new PhotographyContext();
            //context.Database.Initialize(true);
            UnitOfWork unit = new UnitOfWork();
            ConfigureMapping(unit);
            ImportLens(unit);
            ImportCameras(unit);
            ImportPhotographers(unit);
        }

        private static void ImportPhotographers(UnitOfWork unit)
        {
            string json = File.ReadAllText(PhotographersPath);
            IEnumerable<PhotographerDto> photographerDtos = JsonConvert.DeserializeObject<IEnumerable<PhotographerDto>>(json);
            foreach (var photographerDto in photographerDtos)
            {
                
                Photographer photographer = Mapper.Map<Photographer>(photographerDto);
                try
                {
                    unit.Photographers.Add(photographer);
                    unit.Commit();
                    Console.WriteLine($"Successfully imported {photographer.FirstName} {photographer.LastName} | Lenses: {photographer.Lenses.Count}");
                }
                catch (DbEntityValidationException dbex)
                {
                    unit.Photographers.Remove(photographer);
                    Console.WriteLine(Error);
                }
            }

        }

        private static void ImportCameras(UnitOfWork unit)
        {
            string json = File.ReadAllText(CamerasPath);
            IEnumerable<CameraDto> cameraDtos = JsonConvert.DeserializeObject<IEnumerable<CameraDto>>(json);
            foreach (var cameraDto in cameraDtos)
            {
                if (cameraDto.Type == null || cameraDto.Make == null || cameraDto.Model == null || cameraDto.MinISO < 100)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                Camera camera = Mapper.Map<Camera>(cameraDto);

                if (camera.Type == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                unit.Cameras.Add(camera);
                unit.Commit();
                Console.WriteLine($"Successfully imported {cameraDto.Type} {camera.Make} {camera.Model}");
            }
        }



        private static void ImportLens(UnitOfWork unit)
        {
            string json = File.ReadAllText(LensPath);
            IEnumerable<LenDto> lenDtos = JsonConvert.DeserializeObject<IEnumerable<LenDto>>(json);
            foreach (var lenDto in lenDtos)
            {

                Len len = Mapper.Map<Len>(lenDto);
                unit.Lens.Add(len);
                unit.Commit();
                Console.WriteLine($"Successfully imported {len.Make} {len.FocalLength}mm f{len.MaxAperture}");
            }
        }

        private static void ConfigureMapping(UnitOfWork unit)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<LenDto, Len>();

                config.CreateMap<CameraDto, Camera>()
                                          .ForMember(cameraDto => cameraDto.Type,
                                     configurationExpression => configurationExpression
                               .MapFrom(camera1 => camera1.Type));

                config.CreateMap<PhotographerDto, Photographer>()
                    .ForMember(photog => photog.Lenses,
                        expression => expression
                            .MapFrom(dto => unit.Lens
                                .GetAll(lens => lens.Id == dto.Lenses.ToList().First())));
            });
        }

    }
}
