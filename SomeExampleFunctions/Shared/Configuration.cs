using System;

namespace SomeExampleFunctions.Shared
{
    public static class Configuration
    {
        public static string ValueOf(string key)
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        }
    }
}
