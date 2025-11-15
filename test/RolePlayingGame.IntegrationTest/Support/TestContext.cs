using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using RolePlayingGame.Api;

namespace RolePlayingGame.IntegrationTest.Support
{
    public class TestContext
    {
        // HttpClient utilizado em TODOS os steps
        public HttpClient HttpClient { get; }

        // Última resposta obtida no cenário
        public HttpResponseMessage? LastResponse { get; set; }

        // Corpo JSON armazenado entre os steps (POST/PUT)
        public string? LastRequestBody { get; set; }

        // Personagens criados nos testes (para GET {id} e battles)
        public List<TestCreatedCharacter> CreatedCharacters { get; } = new();
        public HttpClient Client { get; }


		public TestContext()
		{
			var factory = new WebApplicationFactory<Program>();
			Client = factory.CreateClient();
		}
	}

    public class TestCreatedCharacter
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Job { get; set; }
    }
}
