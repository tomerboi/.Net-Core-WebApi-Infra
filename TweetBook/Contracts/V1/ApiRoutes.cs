using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook.Contracts
{
    public static class ApiRoutes
    {
        public const string Version = "v1";
        public const string Base = "api";

        public const string Root = "/" + Base + "/" + Version;
        public static class Posts
        {
            public const string GetAll = Root + "/posts";
            public const string Get = Root + "/posts/{id}";
            public const string Update = Root + "/posts/{id}";
            public const string Delete = Root + "/posts/{id}";
            public const string Create = Root + "/posts";
        }
    }
}
