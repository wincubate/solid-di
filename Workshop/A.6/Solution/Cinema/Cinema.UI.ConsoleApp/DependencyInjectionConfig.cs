using Cinema.DataAccess.Sql;
using Cinema.DataAccess.Xml;
using Cinema.Domain;
using Cinema.Domain.Interfaces;
using Cinema.PresentationLogic;
using System;
using System.Collections.Generic;
using Unity;

namespace Cinema.UI.ConsoleApp
{
    internal class DependencyInjectionConfig : IDisposable
    {
        private readonly IUnityContainer _container;

        public DependencyInjectionConfig()
        {
            _container = new UnityContainer();
        }

        public void Register()
        {
            // UI Layer
            _container
                .RegisterType<IMovieRepository, SqlMovieRepository>()
                //.RegisterFactory<IMovieRepository>(c => new XmlMovieRepository(@"..\..\..\..\..\..\..\02 - Movies.xml"))
                .RegisterType<MovieContext>()
                .RegisterType<ITimeProvider, DefaultTimeProvider>()
                .RegisterType<IMovieService,MovieService>()
                .RegisterType<IUserContext, NullUserContext>()
                ;
        }

        public T Resolve<T>() => _container.Resolve<T>();

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
