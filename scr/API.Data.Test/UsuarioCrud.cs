using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Implementation;
using Api.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace API.Data.Test
{
    public class UsuarioCrud : BaseTest, IClassFixture<DbTeste>
    {
        private ServiceProvider _serviceProvider;
        public UsuarioCrud(DbTeste dbTeste)
        {
            _serviceProvider = dbTeste.ServiceProvider;

        }

        [Fact(DisplayName = "CRUD de USUARIO")]
        [Trait("CRUD", "UserEntity")]
        public async Task E_Possivel_realizar_Crud_Usuario()
        {
            using (var context = _serviceProvider.GetService<MyContext>())
            {
                UserImplamentation _repositorio = new UserImplamentation(context);
                UserEntity _entity = new UserEntity
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()

                };
                var _registroCriado = await _repositorio.InsertAsync(_entity);
                Assert.NotNull(_registroCriado);
                Assert.Equal(_entity.Email, _registroCriado.Email);
                Assert.Equal(_entity.Name, _registroCriado.Name);
                Assert.False(_registroCriado.Id == Guid.Empty);


                _entity.Name = Faker.Name.First();
                var registroAtualizado = await _repositorio.UpdatetAsync(_entity);
                Assert.NotNull(registroAtualizado);
                Assert.Equal(_entity.Email, registroAtualizado.Email);
                Assert.Equal(_entity.Name, registroAtualizado.Name);

                var _registroExiste = await _repositorio.ExistAsync(registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectAsync(registroAtualizado.Id);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(registroAtualizado.Email, _registroSelecionado.Email);
                Assert.Equal(registroAtualizado.Name, _registroSelecionado.Name);


                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 1);

                var removeu = await _repositorio.DeleteAsync(_registroSelecionado.Id);
                Assert.True(removeu);

                var usuarioPadrao = await _repositorio.FindByLogin("wilson@wilson.com");
                Assert.NotNull(usuarioPadrao);
                Assert.Equal("wilson@wilson.com", usuarioPadrao.Email);

            }
        }

    }
}