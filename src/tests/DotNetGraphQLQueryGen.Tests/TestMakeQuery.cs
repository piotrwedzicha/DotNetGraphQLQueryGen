using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNetGqlClient;
using Generated;
using Xunit;
using Xunit.Abstractions;

namespace CoreData.Model.Tests
{
    public class TestClient : BaseGraphQLClient
    {
        internal string Make<TReturn>(Expression<Func<RootQuery, TReturn>> p)
        {
            return base.MakeQuery(p);
        }
    }

    public class TestTypedQuery
    {
        public TestTypedQuery(ITestOutputHelper testOutputHelper)
        {
        }

        [Fact]
        public void SimpleArgs()
        {
            var client = new TestClient();
            var query = client.Make(q => new {
                Movies = q.Movies(s => new {
                    s.Id,
                }),
            });
            Assert.Equal($@"query BaseGraphQLClient {{
Movies: movies {{
Id: id
}}
}}".Replace("\r\n", "\n"), query);
        }

        [Fact]
        public void SimpleQuery()
        {
            var client = new TestClient();
            var query = client.Make(q => new {
                Actors = q.Actors(s => new {
                    s.Id,
                    DirectorOf = s.DirectorOf(),
                }),
            });
            Assert.Equal($@"query BaseGraphQLClient {{
Actors: actors {{
Id: id
DirectorOf: directorOf {{
Id: id
Name: name
Genre: genre
Released: released
DirectorId: directorId
Rating: rating
}}
}}
}}".Replace("\r\n", "\n"), query);
        }

        [Fact]
        public void TypedClass()
        {
            var client = new TestClient();
            var query = client.Make(q => new MyResult
            {
                Movies = q.Movies(s => new MovieResult
                {
                    Id = s.Id,
                }),
            });
            Assert.Equal($@"query BaseGraphQLClient {{
Movies: movies {{
Id: id
}}
}}".Replace("\r\n", "\n"), query);
        }
    }

    public class MovieResult
    {
        public int Id { get; set; }
    }

    public class MyResult
    {
        public List<MovieResult> Movies { get; set; }
    }
}
